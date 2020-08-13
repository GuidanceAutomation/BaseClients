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
			Assert.AreEqual(41916, settings.HttpPort);
			Assert.AreEqual(41917, settings.TcpPort);
			Assert.AreEqual(41918, settings.UdpPort);
		}

		/// <summary>
		/// Quick sanity check
		/// </summary>
		[Test]
		public void Set()
		{
			IPAddress ipAddress = IPAddress.Parse("192.168.1.1");

            ushort httpPort = 1;
            ushort tcpPort = 2;
            ushort udpPort = 3;

			EndpointSettings settings = new EndpointSettings(ipAddress, httpPort, tcpPort, udpPort);

			Assert.AreEqual(ipAddress, settings.IPAddress);
			Assert.AreEqual(httpPort, settings.HttpPort);
			Assert.AreEqual(tcpPort, settings.TcpPort);
			Assert.AreEqual(udpPort, settings.UdpPort);
		}
	}
}