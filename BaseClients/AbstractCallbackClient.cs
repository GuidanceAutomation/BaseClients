using NLog;
using System;
using System.ServiceModel;
using System.Threading;

namespace BaseClients
{
	public abstract class AbstractCallbackClient<T> : AbstractClient<T>, ICallbackClient
	{
		protected InstanceContext context;

		protected AutoResetEvent heartbeatReset = new AutoResetEvent(false);

		private readonly Guid key = Guid.NewGuid();

		private Thread hearbeatThread;

		private bool isConnected = false;

		private bool isDisposed = false;

		private Logger logger = LogManager.CreateNullLogger();

		private bool terminate = false;

		public AbstractCallbackClient(Uri netTcpUri, NetTcpBinding binding = null)
			: base(netTcpUri, binding)
		{
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

		protected bool Terminate => terminate;

		protected new DuplexChannelFactory<T> CreateChannelFactory()
			=> new DuplexChannelFactory<T>(context, binding, EndpointAddress);

		protected override void Dispose(bool isDisposing)
		{
			if (isDisposed) return;

			terminate = true;
			heartbeatReset.Set();
			hearbeatThread.Join();

			base.Dispose(isDisposing);

			isDisposed = true;
		}

		protected abstract void HeartbeatThread();

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