using System;
using System.Text;

namespace BaseClients
{
	public struct ServiceOperationResult
	{
		private readonly Exception clientException;

		private readonly uint serviceCode;

		private readonly Exception serviceException;

		private readonly string serviceString;

		public ServiceOperationResult(uint serviceCode, string serviceString, Exception serviceException, Exception clientException)
		{
			this.serviceCode = serviceCode;
			this.serviceString = serviceString;
			this.serviceException = serviceException;
			this.clientException = clientException;
		}

		public string ToSummaryString()
		{
			if (IsSuccessfull) return "Success";

			StringBuilder builder = new StringBuilder();

			if (IsClientError) builder.AppendFormat("ClientError: {0}", clientException);

			if (IsServiceError) builder.AppendFormat("ServiceError: Code:{0}", serviceCode);

			return builder.ToString();
		}

		public override string ToString() => ToSummaryString();

		public Exception ClientException => clientException; 

		public bool IsClientError => ClientException != null;

		public bool IsServiceError
		{
			get
			{
				// Codes:
				// NOERROR = 0
				// CLIENTEXCEPTION = 2
				return serviceCode != 0 && serviceCode != 2;
			}
		}

		public bool IsSuccessfull => (!IsServiceError && !IsClientError); 

		public uint ServiceCode => serviceCode; 

		public Exception ServiceException => serviceException; 

		public string ServiceString => serviceString; 

		public static ServiceOperationResult FromClientException(Exception ex)
		{
			if (ex == null) throw new ArgumentNullException("ex");

			return new ServiceOperationResult
				(
					2, // Client exception
					"CLIENTEXCEPTION",
					null,
					ex
				);
		}

		public static bool operator !=(ServiceOperationResult result, bool value)
			=> !(result == value);

		public static bool operator ==(ServiceOperationResult result, bool value)
			=> result.IsSuccessfull == value;

		public override bool Equals(object obj)
		{
			if (!(obj is ServiceOperationResult)) return false;

			ServiceOperationResult other = (ServiceOperationResult)obj;

			return ServiceCode == other.ServiceCode;
		}

		public override int GetHashCode()
		{
			int hash = 17;

			unchecked
			{
				hash += hash * 23 + serviceCode.GetHashCode();
			}

			return hash;
		}
	}
}