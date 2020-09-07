using BaseClients.Architecture;
using GAAPICommon.Architecture;
using GAAPICommon.Core;
using GAAPICommon.Core.Dtos;
using NLog;
using System;
using System.ServiceModel;

namespace BaseClients.Core
{
    public abstract class AbstractClient<T> : IClient
    {
        protected readonly NetTcpBinding binding;

        private bool isDisposed = false;

        private Exception lastCaughtException = null;

        private ILogger logger = LogManager.CreateNullLogger();

        /// <summary>
        /// Primary constructor.
        /// </summary>
        /// <param name="netTcpUri">.net tcp uri of the server side endpoint.</param>
        /// <param name="binding">Transport and security binding settings.</param>
        public AbstractClient(Uri netTcpUri, NetTcpBinding netTcpBinding = null)
        {
            EndpointAddress = new EndpointAddress(netTcpUri);

            binding = netTcpBinding ?? new NetTcpBinding(SecurityMode.None) { PortSharingEnabled = true };
        }

        ~AbstractClient()
        {
            Dispose(false);
        }

        /// <summary>
        /// Handles an API call that returns an IServiceCallResult
        /// </summary>
        /// <param name="apiCall">Method the handles the channel call</param>
        protected IServiceCallResult HandleAPICall(Func<T, ServiceCallResultDto> apiCall)
        {
            try
            {
                using (ChannelFactory<T> channelFactory = CreateChannelFactory())
                {
                    T channel = channelFactory.CreateChannel();
                    ServiceCallResultDto result = apiCall(channel);
                    channelFactory.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                LastCaughtException = ex;
                return ServiceCallResultFactory.FromClientException(ex);
            }
        }

        /// <summary>
        /// Handles an API call that returns a value of type T
        /// </summary>
        /// <typeparam name="T">Dto type to be returned</typeparam>
        /// <param name="apiCall">Method the handles the channel call</param>
        protected IServiceCallResult<U> HandleAPICall<U>(Func<T, ServiceCallResultDto<U>> apiCall)
        {
            try
            {
                using (ChannelFactory<T> channelFactory = CreateChannelFactory())
                {
                    T channel = channelFactory.CreateChannel();
                    ServiceCallResultDto<U> result = apiCall(channel);
                    channelFactory.Close();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                LastCaughtException = ex;
                return ServiceCallResultFactory<U>.FromClientException(ex);
            }
        }

        /// <summary>
        /// Endpoint address of the server.
        /// </summary>
        public EndpointAddress EndpointAddress { get; }

        /// <summary>
        /// Last caught exception handled by the client
        /// </summary>
        public Exception LastCaughtException
        {
            get { return lastCaughtException; }

            protected set
            {
                if (lastCaughtException != value)
                {
                    lastCaughtException = value;
                    if (value is EndpointNotFoundException)                   
                        Logger.Warn("EndpointNotFoundException - is the server running?");
                    else
                        Logger.Error(value);
                }
            }
        }

        /// <summary>
        /// NLog logger for diagnostics and debugging.
        /// </summary>
        public ILogger Logger
        {
            get { return logger; }

            set
            {
                logger = value ?? LogManager.CreateNullLogger();

                logger.Info("Binding:{0} PortSharing:{1}", binding.Name, binding.PortSharingEnabled);
                logger.Info("Endpoint Address:{0}", EndpointAddress);
            }
        }

        /// <summary>
        /// Disposes of the client.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        private ChannelFactory<T> CreateChannelFactory()
            => new ChannelFactory<T>(binding, EndpointAddress);

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposed) 
                return;

            isDisposed = true;
        }
    }
}