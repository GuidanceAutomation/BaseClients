using System;
using System.ComponentModel;

namespace BaseClients
{
    public interface ICallbackClient : IClient
    {
        event Action<DateTime> Connected;

        event Action<DateTime> Disconnected;

        bool IsConnected { get; }

        Guid Key { get; }
    }
}