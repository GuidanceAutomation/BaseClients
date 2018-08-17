using NLog;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Threading;

namespace BaseClients
{
    public abstract class AbstractCallbackClient<T> : ICallbackClient
    {
        protected InstanceContext context;

        protected AutoResetEvent heartbeatReset = new AutoResetEvent(false);

        private readonly Guid key = Guid.NewGuid();

        private NetTcpBinding binding = new NetTcpBinding(SecurityMode.None)
        {
            PortSharingEnabled = true
        };

        private EndpointAddress endpointAddress;

        private Thread hearbeatThread;

        private bool isConnected = false;

        private bool isDisposed = false;

        private Exception lastCaughtException = null;

        private Logger logger = LogManager.CreateNullLogger();

        private bool terminate = false;

        public AbstractCallbackClient(Uri netTcpUri)
        {
            this.endpointAddress = new EndpointAddress(netTcpUri);

            SetInstanceContext();

            hearbeatThread = new Thread(new ThreadStart(HeartbeatThread));
            hearbeatThread.Start();
        }

        ~AbstractCallbackClient()
        {
            Dispose(false);
        }

        public event Action<DateTime> Connected;

        public event Action<DateTime> Disconnected;

        public event PropertyChangedEventHandler PropertyChanged;

        public EndpointAddress EndpointAddress { get { return endpointAddress; } }

        public bool IsConnected
        {
            get { return isConnected; }

            protected set
            {
                if (isConnected != value)
                {
                    isConnected = value;
                    OnNotifyPropertyChanged();

                    Logger.Debug("IsConnected: {0}", value);

                    if (value)
                    {
                        OnConnected(DateTime.Now);
                    }
                    else
                    {
                        OnDisconnected(DateTime.Now);
                    }
                }
            }
        }

        public Guid Key { get { return key; } }

        public Exception LastCaughtException
        {
            get { return lastCaughtException; }

            protected set
            {
                if (lastCaughtException != value)
                {
                    lastCaughtException = value;
                    if (value is EndpointNotFoundException)
                    {
                        Logger.Warn("EndpointNotFoundException - is The server running?");
                    }
                    else
                    {
                        Logger.Error(value);
                    }
                    OnNotifyPropertyChanged();
                }
            }
        }

        public Logger Logger
        {
            get { return logger; }

            set
            {
                if (value == null)
                {
                    value = LogManager.CreateNullLogger();
                }

                logger = value;
                logger.Info("Binding:{0} PortSharing:{1}", binding.Name, binding.PortSharingEnabled);
                logger.Info("Endpoint Address:{0}", endpointAddress);
            }
        }

        protected bool Terminate { get { return terminate; } }

        public void Dispose()
        {
            Dispose(true);
        }

        protected DuplexChannelFactory<T> CreateChannelFactory()
        {
            return new DuplexChannelFactory<T>(context, binding, EndpointAddress);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposed)
            {
                return;
            }

            terminate = true;
            heartbeatReset.Set();
            hearbeatThread.Join();

            isDisposed = true;
        }

        protected ServiceOperationResult HandleClientException(Exception ex)
        {
            LastCaughtException = ex;
            Logger.Error(ex);
            return ServiceOperationResult.FromClientException(ex);
        }

        protected abstract void HeartbeatThread();

        protected void OnNotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected abstract void SetInstanceContext();

        private void OnConnected(DateTime dateTime)
        {
            Action<DateTime> handlers = Connected;
            if (handlers != null)
            {
                foreach (Action<DateTime> handler in handlers.GetInvocationList())
                {
                    handler.BeginInvoke(dateTime, null, null);
                }
            }
        }

        private void OnDisconnected(DateTime dateTime)
        {
            Action<DateTime> handlers = Disconnected;
            if (handlers != null)
            {
                foreach (Action<DateTime> handler in handlers.GetInvocationList())
                {
                    handler.BeginInvoke(dateTime, null, null);
                }
            }
        }
    }
}