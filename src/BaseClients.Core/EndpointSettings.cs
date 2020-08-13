using System.Net;

namespace BaseClients.Core
{
    /// <summary>
    /// A simple struct for representing an ip address and associated ports used for http, tcp and udp communication.
    /// </summary>
    public class EndpointSettings
	{
		public EndpointSettings(IPAddress ipAddress = default(IPAddress), ushort httpPort = 41916, ushort tcpPort = 41917, ushort udpPort = 41918)
		{
			IPAddress = ipAddress ?? IPAddress.Loopback;
			HttpPort = httpPort;
			TcpPort = tcpPort;
			UdpPort = udpPort;
		}

		public ushort HttpPort { get; private set; } = 41916;

		public IPAddress IPAddress { get; private set; } = IPAddress.Loopback;

		public ushort TcpPort { get; private set; } = 41917;

		public ushort UdpPort { get; private set; } = 41918;

		public override string ToString() => ToSummaryString();

		public string ToSummaryString()
			=> ($"IPAddress:{IPAddress} Ports: Http:{HttpPort} Tcp:{TcpPort}, Udp:{UdpPort}");
	}
}