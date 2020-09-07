using System.Net;

namespace BaseClients.Core
{
    /// <summary>
    /// Tightly couuples an IP Address associated ports used for http, tcp and udp communication.
    /// </summary>
    public class EndpointSettings
    {
        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="ipAddress">Server IP Address.</param>
        /// <param name="httpPort"></param>
        /// <param name="tcpPort"></param>
        /// <param name="udpPort"></param>
        public EndpointSettings(IPAddress ipAddress = default(IPAddress), ushort httpPort = 41916, ushort tcpPort = 41917, ushort udpPort = 41918)
        {
            IPAddress = ipAddress ?? IPAddress.Loopback;
            HttpPort = httpPort;
            TcpPort = tcpPort;
            UdpPort = udpPort;
        }

        /// <summary>
        /// Server HTTP port (default 41916).
        /// </summary>
        public ushort HttpPort { get; private set; } = 41916;

        /// <summary>
        /// Server IP Address (default loop-back).
        /// </summary>
        public IPAddress IPAddress { get; private set; } = IPAddress.Loopback;

        /// <summary>
        /// Server TCP port (default 41917).
        /// </summary>
        public ushort TcpPort { get; private set; } = 41917;

        /// <summary>
        /// Server UDP port (default 41918).
        /// </summary>
        public ushort UdpPort { get; private set; } = 41918;

        public override string ToString() => ToSummaryString();

        public string ToSummaryString()
            => ($"IPAddress:{IPAddress} Ports: HTTP:{HttpPort} TCP:{TcpPort}, UDP:{UdpPort}");
    }
}