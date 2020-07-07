using BaseClients.Architecture;
using System;
using GAAPICommon.Core.Dtos;
using GAAPICommon.Core;

namespace BaseClients.Core
{
	public abstract class AbstractConsoleOption<T> where T: IClient
	{
		public ServiceCallResultDto ExecuteOption(T client)
		{
			ServiceCallResultDto result;

			try
			{
				result = HandleExecution(client);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return ServiceCallResultFactory.FromClientException(ex);
			}

			if (result.ServiceCode != 0) Console.WriteLine(result);

			return result;
		}

		protected abstract ServiceCallResultDto HandleExecution(T client);
	}
}
