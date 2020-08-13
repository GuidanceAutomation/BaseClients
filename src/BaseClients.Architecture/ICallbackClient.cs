using System;

namespace BaseClients.Architecture
{
    public interface ICallbackClient : IClient
    {
        event Action<DateTime> Connected;

        event Action<DateTime> Disconnected;

        bool IsConnected { get; }

        Guid Key { get; }
    }
}