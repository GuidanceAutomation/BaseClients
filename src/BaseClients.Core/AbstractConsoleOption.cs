using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClients.Core
{
	public abstract class AbstractConsoleOption<T> where T:IClient
	{
		public ServiceOperationResult ExecuteOption(T client)
		{
			ServiceOperationResult result;

			try
			{
				result = HandleExecution(client);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				result = ServiceOperationResult.FromClientException(ex);
			}

			if (!result.IsSuccessfull) Console.WriteLine(result);

			return result;
		}

		protected abstract ServiceOperationResult HandleExecution(T client);
	}
}
