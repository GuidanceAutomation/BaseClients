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

        /// <summary>
        /// Converts endpoitn settings to a base TCP uri.        
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