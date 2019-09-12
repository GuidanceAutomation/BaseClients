using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseClients;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ServiceModel;
using NLog;
using System.Runtime.CompilerServices;

namespace BaseClients.DemoApp
{
	public class FooCallbackClient : ICallbackClient
	{
		private Random random = new Random();

		public FooCallbackClient()
		{
		}

		private bool isConnected = false;

		public bool IsConnected
		{
			get { return isConnected; }
			set
			{
				if (isConnected != value)
				{
					isConnected = value;
					OnNotifyPropertyChanged();
				}
			}
		}

		public Guid Key => throw new NotImplementedException();

		public EndpointAddress EndpointAddress => throw new NotImplementedException();

		public Exception LastCaughtException => throw new NotImplementedException();

		public Logger Logger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public event Action<DateTime> Connected;

		public event Action<DateTime> Disconnected;

		public event PropertyChangedEventHandler PropertyChanged;

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		protected void OnNotifyPropertyChanged([CallerMemberName] String propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public void Randomize()
		{
			IsConnected = random.Next(0, 2) > 0.5;
		}
	}
}
