using System;
using System.Net;

namespace BaseClients
{
	/// <summary>
	/// A simple struct for representing an ip address and associated ports used for http, tcp and udp communication.
	/// </summary>
	public struct EndpointSettings
	{
		public const UInt16 DEFAULTHTTPPORT = 41916;

		public const UInt16 DEFAULTTCPPORT = 41917;

		public const UInt16 DEFAULTUDPPORT = 41918;

		private readonly UInt16? httpPort;

		private readonly IPAddress ipAddress;

		private readonly UInt16? tcpPort;

		private readonly UInt16? udpPort;

		public EndpointSettings(IPAddress ipAddress = default(IPAddress), UInt16 httpPort = DEFAULTHTTPPORT, UInt16 tcpPort = DEFAULTTCPPORT, UInt16 udpPort = DEFAULTUDPPORT)
		{
			if (ipAddress == null) throw new ArgumentNullException("ipAddress");

			this.ipAddress = ipAddress;
			this.httpPort = httpPort;
			this.tcpPort = tcpPort;
			this.udpPort = udpPort;
		}

		public UInt16 HttpPort => httpPort == null ? EndpointSettings.DEFAULTHTTPPORT : (UInt16)httpPort;

		public IPAddress IPAddress => ipAddress == null ? IPAddress.Loopback : ipAddress;

		public UInt16 TcpPort => tcpPort == null ? EndpointSettings.DEFAULTTCPPORT : (UInt16)tcpPort;

		public UInt16 UdpPort => udpPort == null ? EndpointSettings.DEFAULTUDPPORT : (UInt16)udpPort;

		public override string ToString() => ToSummaryString();

		public string ToSummaryString()
			=> string.Format("IPAddress:{0} Ports: Http:{1} Tcp:{2}, Udp:{3}", IPAddress, HttpPort, TcpPort, UdpPort);
	}
}