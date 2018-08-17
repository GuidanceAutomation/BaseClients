using NUnit.Framework;
using System.Net;

namespace BaseClients.Test
{
    [TestFixture]
    [Category("EndpointSettings")]
    public class TEndpointSettings
    {
        /// <summary>
        /// Quick sanity check
        /// </summary>
        [Test]
        public void AssertSet()
        {
            IPAddress ipAddress = IPAddress.Parse("192.168.1.1");
            int httpPort = 1;
            int tcpPort = 2;
            int udpPort = 3;

            EndpointSettings settings = new EndpointSettings(ipAddress, httpPort, tcpPort, udpPort);

            Assert.AreEqual(ipAddress, settings.IPAddress);
            Assert.AreEqual(httpPort, settings.HttpPort);
            Assert.AreEqual(tcpPort, settings.TcpPort);
            Assert.AreEqual(udpPort, settings.UdpPort);
        }
    }
}