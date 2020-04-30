using NLog;
using System;
using System.ComponentModel;
using System.ServiceModel;

namespace BaseClients.Core
{
	public interface IClient : IDisposable, INotifyPropertyChanged
	{
		/// <summary>
		/// Endpoint address of the service host
		/// </summary>
		EndpointAddress EndpointAddress { get; }

		Exception LastCaughtException { get; }

		/// <summary>
		/// Logger for debugging / monitoring
		/// </summary>
		Logger Logger { get; set; }
	}
}