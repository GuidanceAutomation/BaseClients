using System;

namespace BaseClients.Architecture
{
    /// <summary>
    /// Callback clients implement a heartbeat mechanism to resister for callbacks (events) fired by the server. 
    /// </summary>
    public interface ICallbackClient : IClient
    {
        /// <summary>
        /// Fired whenever a callback client successfully registers with the server.
        /// </summary>
        event Action<DateTime> Connected;

        /// <summary>
        /// Fired whenever a callback client fails to register or maintain its registration with the server. 
        /// </summary>
        event Action<DateTime> Disconnected;

        /// <summary>
        /// True if the client has made a successful registration attempt. 
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Instance Id of the client. Used to uniquely identify clients to the server. 
        /// </summary>
        Guid Key { get; }

        /// <summary>
        /// The interval between registration events.
        /// </summary>
        TimeSpan Heartbeat { get; }
    }
}