using NLog;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceModel;

namespace BaseClients.Core
{
	public abstract class AbstractClient<T> : IClient
	{
		protected readonly NetTcpBinding binding;

		private bool isDisposed = false;

		private Exception lastCaughtException = null;

		private Logger logger = LogManager.CreateNullLogger();

		public AbstractClient(Uri netTcpUri, NetTcpBinding binding = null)
		{
			this.EndpointAddress = new EndpointAddress(netTcpUri);

			if (binding == null)
			{
				binding = new NetTcpBinding(SecurityMode.None) { PortSharingEnabled = true };
			}

			this.binding = binding;
		}

		~AbstractClient()
		{
			Dispose(false);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public EndpointAddress EndpointAddress { get; }

		/// <summary>
		/// Last caught exception handled by the client
		/// </summary>
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
						Logger.Warn("EndpointNotFoundException - is the server running?");
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
				if (value == null) value = LogManager.CreateNullLogger();

				logger = value;
				logger.Info("Binding:{0} PortSharing:{1}", binding.Name, binding.PortSharingEnabled);
				logger.Info("Endpoint Address:{0}", EndpointAddress);
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected ChannelFactory<T> CreateChannelFactory()
		 => new ChannelFactory<T>(binding, EndpointAddress);

		protected virtual void Dispose(bool isDisposing)
		{
			if (isDisposed) return;

			isDisposed = true;
		}

		protected ServiceOperationResult HandleClientException(Exception ex)
		{
			LastCaughtException = ex;
			Logger.Error(ex);
			return ServiceOperationResult.FromClientException(ex);
		}

		protected void OnNotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}