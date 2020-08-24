using BaseClients.Architecture;
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

        private readonly Logger logger = LogManager.CreateNullLogger();

        public AbstractCallbackClient(Uri netTcpUri, TimeSpan heartbeat = default, NetTcpBinding binding = null)
            : base(netTcpUri, binding)
        {
            SetInstanceContext();

            Heartbeat = heartbeat < TimeSpan.FromMilliseconds(1000) ? TimeSpan.FromMilliseconds(1000) : heartbeat;

            hearbeatThread = new Thread(new ThreadStart(HeartbeatThread));
            hearbeatThread.Start();
        }

        ~AbstractCallbackClient()
        {
            Dispose(false);
        }

        public event Action<DateTime> Connected;

        public event Action<DateTime> Disconnected;

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

            Logger.Trace("HeartbeatThread exit");
        }

        public TimeSpan Heartbeat { get; private set; }

        public Guid Key { get; } = Guid.NewGuid();

        protected bool Terminate { get; private set; } = false;

        protected new DuplexChannelFactory<T> CreateChannelFactory()
            => new DuplexChannelFactory<T>(context, binding, EndpointAddress);

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposed) return;

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