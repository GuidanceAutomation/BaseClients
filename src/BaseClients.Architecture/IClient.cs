using NLog;
using System;
using System.ServiceModel;

namespace BaseClients.Architecture
{
    /// <summary>
    /// Clients enabled remote procedure calls to be made on the server which may or may not return a value.
    /// </summary>
    public interface IClient : IDisposable
    {
        /// <summary>
        /// Endpoint address of the service host (server configuration).
        /// </summary>
        EndpointAddress EndpointAddress { get; }

        /// <summary>
        /// The last exception caught by the service. 
        /// </summary>
        Exception LastCaughtException { get; }

        /// <summary>
        /// NLog logger for debugging / monitoring
        /// </summary>
        ILogger Logger { get; set; }
    }
}