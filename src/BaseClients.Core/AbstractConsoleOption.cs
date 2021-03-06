﻿using BaseClients.Architecture;
using GAAPICommon.Architecture;
using GAAPICommon.Core;
using System;

namespace BaseClients.Core
{
    /// <summary>
    /// Abstract class for console applications calling a client method that returns a value.
    /// </summary>
    /// <typeparam name="T">Client instance</typeparam>
    /// <typeparam name="U">IServiceCallResult<typeparamref name="U"/>value</typeparam>
    public abstract class AbstractGenericConsoleOption<T, U> where T : IClient
    {
        public IServiceCallResult<U> ExecuteOption(T client)
        {
            IServiceCallResult<U> result;

            try
            {
                result = HandleExecution(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ServiceCallResultFactory<U>.FromClientException(ex);
            }

            if (result.ServiceCode != 0)
                Console.WriteLine(result);

            return result;
        }

        protected abstract IServiceCallResult<U> HandleExecution(T client);
    }

    /// <summary>
    /// Abstract class for console applications calling a client method that returns void
    /// </summary>
    /// <typeparam name="T">Client instance</typeparam>
    public abstract class AbstractConsoleOption<T> where T : IClient
    {
        public IServiceCallResult ExecuteOption(T client)
        {
            IServiceCallResult result;

            try
            {
                result = HandleExecution(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ServiceCallResultFactory.FromClientException(ex);
            }

            if (result.ServiceCode != 0)
                Console.WriteLine(result);

            return result;
        }

        protected abstract IServiceCallResult HandleExecution(T client);
    }
}