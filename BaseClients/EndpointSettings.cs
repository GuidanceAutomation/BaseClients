using System.Net;

namespace BaseClients
{
    public struct EndpointSettings
    {
        public const int DEFAULTHTTPPORT = 41916;

        public const int DEFAULTTCPPORT = 41917;

        public const int DEFAULTUDPPORT = 41918;

        private readonly int httpPort;

        private readonly IPAddress ipAddress;

        private readonly int tcpPort;

        private readonly int udpPort;

        public EndpointSettings(IPAddress ipAddress = default(IPAddress), int httpPort = DEFAULTHTTPPORT, int tcpPort = DEFAULTTCPPORT, int udpPort = DEFAULTUDPPORT)
        {
            this.ipAddress = ipAddress == null ? IPAddress.Loopback : ipAddress;
            this.httpPort = httpPort;
            this.tcpPort = tcpPort;
            this.udpPort = udpPort;
        }

        public int HttpPort { get { return httpPort; } }

        public IPAddress IPAddress { get { return ipAddress; } }

        public int TcpPort { get { return tcpPort; } }

        public int UdpPort { get { return udpPort; } }
    }
}