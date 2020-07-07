using NLog;
using System;
using System.ServiceModel;

namespace BaseClients.Architecture
{
	public interface IClient : IDisposable
	{
		/// <summary>
		/// Endpoint address of the service host
		/// </summary>
		EndpointAddress EndpointAddress { get; }

		Exception LastCaughtException { get; }

		/// <summary>
		/// Logger for debugging / monitoring
		/// </summary>
		ILogger Logger { get; set; }
	}
}