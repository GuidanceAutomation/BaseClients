using BaseClients.Architecture;
using System;
using System.Net;

namespace BaseClients.Core
{
    public static class EndpointSettings_ExtensionMethods
    {
        /// <summary>
        /// Converts endpoint settings to a base HTTP uri.
        /// </summary>
        /// <param name="endpointSettings">Endpoint settings to convert.</param>
        /// <returns>Base Uri address, e.g. http://192.168.0.1:41916</returns>
        public static Uri ToHttpBase(this EndpointSettings endpointSettings)
        {
            if (endpointSettings == null)
                throw new ArgumentNullException("endpointSettings");

           return new Uri("http://" + new IPEndPoint(endpointSettings.IPAddress, endpointSettings.HttpPort).ToString());
        }

        public static Scheme ToScheme(this Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            if (uri.IsHttp())
                return Scheme.Http;

            if (uri.IsNetTcp())
                return Scheme.NetTcp;

            if (uri.IsNetPipe())
                return Scheme.NetPipe;

            throw new ArgumentOutOfRangeException("Uri does not contain a valid scheme");
        }

        public static bool IsNetTcp(this Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            try
            {
                string scheme = uri.Scheme;

                if (scheme.Equals("net.tcp"))
                    return true;
            }
            catch
            {
            }

            return false;
        }

        public static bool IsHttp(this Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            try
            {
                string scheme = uri.Scheme;

                if (scheme.Equals("http"))
                    return true;               
            }
            catch
            {
            }

            return false;
        }

        public static bool IsNetPipe(this Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("uri");

            try
            {
                string scheme = uri.Scheme;

                if (scheme.Equals("net.pipe"))
                    return true;
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// Converts endpoint settings to a base TCP uri.        
        /// </summary>
        /// <param name="endpointSettings">Endpoint settings to convert.</param>
        /// <returns>Base Uri address, e.g. net.tcp://192.168.0.1:41917</returns>
        public static Uri ToTcpBase(this EndpointSettings endpointSettings)
        {
            if (endpointSettings == null)
                throw new ArgumentNullException("endpointSettings");

            return new Uri("net.tcp://" + new IPEndPoint(endpointSettings.IPAddress, endpointSettings.TcpPort).ToString());
        }
    }
}