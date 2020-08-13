using System;
using System.Net;

namespace BaseClients.Core
{
    public static class EndpointSettings_ExtensionMethods
    {
        public static Uri ToHttpBase(this EndpointSettings endpointSettings)
           => new Uri("http://" + new IPEndPoint(endpointSettings.IPAddress, endpointSettings.HttpPort).ToString());

        public static Uri ToTcpBase(this EndpointSettings endpointSettings)
           => new Uri("net.tcp://" + new IPEndPoint(endpointSettings.IPAddress, endpointSettings.TcpPort).ToString());
    }
}