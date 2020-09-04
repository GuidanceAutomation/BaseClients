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