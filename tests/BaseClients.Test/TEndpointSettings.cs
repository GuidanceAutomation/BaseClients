using NUnit.Framework;
using System;
using System.Net;
using BaseClients.Core;

namespace BaseClients.Test
{
	[TestFixture]
	[Category("EndpointSettings")]
	public class TEndpointSettings
	{
		[Test]
		public void Defaults()
		{
			EndpointSettings settings = new EndpointSettings();

			Assert.AreEqual(IPAddress.Loopback, settings.IPAddress);
			Assert.AreEqual(EndpointSettings.DEFAULTHTTPPORT, settings.HttpPort);
			Assert.AreEqual(EndpointSettings.DEFAULTTCPPORT, settings.TcpPort);
			Assert.AreEqual(EndpointSettings.DEFAULTUDPPORT, settings.UdpPort);
		}

		/// <summary>
		/// Quick sanity check
		/// </summary>
		[Test]
		public void Set()
		{
			IPAddress ipAddress = IPAddress.Parse("192.168.1.1");

			UInt16 httpPort = 1;
			UInt16 tcpPort = 2;
			UInt16 udpPort = 3;

			EndpointSettings settings = new EndpointSettings(ipAddress, httpPort, tcpPort, udpPort);

			Assert.AreEqual(ipAddress, settings.IPAddress);
			Assert.AreEqual(httpPort, settings.HttpPort);
			Assert.AreEqual(tcpPort, settings.TcpPort);
			Assert.AreEqual(udpPort, settings.UdpPort);
		}
	}
}