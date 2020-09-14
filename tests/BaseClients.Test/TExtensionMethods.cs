using BaseClients.Architecture;
using BaseClients.Core;
using NUnit.Framework;
using System;
using System.Configuration;

namespace BaseClients.Test
{
    [TestFixture]
    public class TExtensionMethods
    {
        [TestCase("", false)]
        [TestCase("http://localhost:8080/foo.svc", true)]
        [TestCase("net.tcp://localhost:8080/foo.svc", false)]
        [TestCase("net.pipe://localhost/guidanceAutomation/foo.svc", false)]
        [TestCase("httphttp", false)]
        public void IsHttp(Uri uri, bool expected)
        {
            Assert.AreEqual(expected, uri.IsHttp());

            if (expected)
                Assert.AreEqual(Scheme.Http, uri.ToScheme());
        }

        [TestCase("", false)]
        [TestCase("http://localhost:8080/foo.svc", false)]
        [TestCase("net.tcp://localhost:8080/foo.svc", true)]
        [TestCase("net.pipe://localhost/guidanceAutomation/foo.svc", false)]
        [TestCase("nettcp", false)]
        public void IsNetTcp(Uri uri, bool expected)
        {
            Assert.AreEqual(expected, uri.IsNetTcp());

            if (expected)
                Assert.AreEqual(Scheme.NetTcp, uri.ToScheme());
        }

        [TestCase("", false)]
        [TestCase("http://localhost:8080/foo.svc", false)]
        [TestCase("net.tcp://localhost:8080/foo.svc", false)]
        [TestCase("net.pipe://localhost/guidanceAutomation/foo.svc", true)]
        [TestCase("netpipe", false)]
        public void IsPipe(Uri uri, bool expected)
        {
            Assert.AreEqual(expected, uri.IsNetPipe());

            if (expected)
                Assert.AreEqual(Scheme.NetPipe, uri.ToScheme());
        }
    }
}
