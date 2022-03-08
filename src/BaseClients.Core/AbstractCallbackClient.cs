using BaseClients.Architecture;
using GAAPICommon.Architecture;
using GAAPICommon.Core;
using GAAPICommon.Core.Dtos;
using MoreLinq;
using NLog;
using System;
using System.Linq;
using System.ServiceModel;
using System.Threading;

namespace BaseClients.Core
{
    public abstract class AbstractCallbackClient<T> : AbstractClient<T>, ICallbackClient
    {
        protected InstanceContext context;

        protected AutoResetEvent heartbeatReset = new AutoResetEvent(false);

        private readonly Thread hearbeatThread;

        private bool isConnected = false;

        private bool isDisposed = false;

        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="netTcpUri">.net tcp uri of the server side endpoint.</param>
        /// <param name="heartbeat">Interval between registration (keep-alive) for callbacks.</param>
        public AbstractCallbackClient(Uri netTcpUri, TimeSpan heartbeat = default)
            : base(netTcpUri)
        {
            SetInstanceContext();

            Heartbeat = heartbeat < TimeSpan.FromMilliseconds(1000) 
                ? TimeSpan.FromMilliseconds(1000)
                : heartbeat;

            hearbeatThread = new Thread(new ThreadStart(HeartbeatThread));
            hearbeatThread.Start();
        }

        ~AbstractCallbackClient()
        {
            Dispose(false);
        }

        /// <summary>
        /// Fired whenever a callback client successfully registers with the server.
        /// </summary>
        public event Action<DateTime> Connected;

        /// <summary>
        /// Fired whenever a callback client fails to register or maintain its registration with the server. 
        /// </summary>
        public event Action<DateTime> Disconnected;

        /// <summary>
        /// True if the client has made a successful registration attempt. 
        /// </summary>
        public bool IsConnected
        {
            get { return isConnected; }

            protected set
            {
                if (isConnected != value)
                {
                    isConnected = value;

                    Logger.Debug("IsConnected: {0}", value);

                    if (value)
                        OnConnected(DateTime.Now);
                    else
                        OnDisconnected(DateTime.Now);
                }
            }
        }

        /// <summary>
        /// Abstract implementation of callback registration. 
        /// </summary>
        /// <param name="channel">Channel to use for communication</param>
        /// <param name="key">Unique client identifier</param>
        protected abstract void HandleSubscriptionHeartbeat(T channel, Guid key);

        private void HeartbeatThread()
        {
            Logger.Trace("HeartbeatThread()");

            ChannelFactory<T> channelFactory = CreateChannelFactory();
            T channel = channelFactory.CreateChannel();

            while (!Terminate)
            {
                bool exceptionCaught = false;

                try
                {
                    Logger.Trace("SubscriptionHeartbeat({0})", Key);
                    HandleSubscriptionHeartbeat(channel, Key);
                    IsConnected = true;
                }
                catch (EndpointNotFoundException)
                {
                    Logger.Warn("HeartbeatThread - EndpointNotFoundException. Is the server running?");
                    exceptionCaught = true;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    LastCaughtException = ex;
                    exceptionCaught = true;
                }

                if (exceptionCaught == true)
                {
                    channelFactory.Abort();
                    IsConnected = false;

                    channelFactory = CreateChannelFactory(); // Create a new channel as this one is dead
                    channel = channelFactory.CreateChannel();
                }

                heartbeatReset.WaitOne(Heartbeat);
            }

            Thread.Sleep(2000); // Allow any pending work to complete before closing.
            channelFactory.Close();
            IsConnected = false;
            Logger.Trace("HeartbeatThread exit");
        }

        /// <summary>
        /// The interval between registration events.
        /// </summary>
        public TimeSpan Heartbeat { get; private set; }

        /// <summary>
        /// Instance Id of the client. Used to uniquely identify clients to the server. 
        /// </summary>
        public Guid Key { get; } = Guid.NewGuid();

        /// <summary>
        /// Set true to terminate all threads.
        /// </summary>
        protected bool Terminate { get; private set; } = false;

        /// <summary>
        /// Handles an API call that returns an IServiceCallResult
        /// </summary>
        /// <param name="apiCall">Method the handles the channel call</param>
        protected new IServiceCallResult HandleAPICall(Func<T, ServiceCallResultDto> apiCall)
        {
            try
            {
                using (DuplexChannelFactory<T> channelFactory = CreateChannelFactory())
                {
                    T channel = channelFactory.CreateChannel();
                    ServiceCallResultDto result = apiCall(channel);
                    channelFactory.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                LastCaughtException = ex;
                return ServiceCallResultFactory.FromClientException(ex);
            }
        }

        /// <summary>
        /// Handles an API call that returns a value of type T
        /// </summary>
        /// <typeparam name="T">Dto type to be returned</typeparam>
        /// <param name="apiCall">Method the handles the channel call</param>
        protected new IServiceCallResult<U> HandleAPICall<U>(Func<T, ServiceCallResultDto<U>> apiCall)
        {
            try
            {
                using (DuplexChannelFactory<T> channelFactory = CreateChannelFactory())
                {
                    T channel = channelFactory.CreateChannel();
                    ServiceCallResultDto<U> result = apiCall(channel);
                    channelFactory.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                LastCaughtException = ex;
                return ServiceCallResultFactory<U>.FromClientException(ex);
            }
        }


        private DuplexChannelFactory<T> CreateChannelFactory()
            => new DuplexChannelFactory<T>(context, binding, EndpointAddress);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposed) 
                return;

            Terminate = true;
            heartbeatReset.Set();
            hearbeatThread.Join();

            base.Dispose(isDisposing);

            isDisposed = true;
        }

        protected abstract void SetInstanceContext();

        private void OnConnected(DateTime dateTime)
        {
            Action<DateTime> handlers = Connected;

            handlers?
                .GetInvocationList()
                .Cast<Action<DateTime>>()
                .ForEach(e => e.BeginInvoke(dateTime, null, null));
        }

        private void OnDisconnected(DateTime dateTime)
        {
            Action<DateTime> handlers = Disconnected;

            handlers?
                .GetInvocationList()
                .Cast<Action<DateTime>>()
                .ForEach(e => e.BeginInvoke(dateTime, null, null));
        }
    }
}