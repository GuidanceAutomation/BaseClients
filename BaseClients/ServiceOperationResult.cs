using System;

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

        public Exception ClientException { get { return clientException; } }

        public bool IsClientError
        {
            get { return ClientException != null; }
        }

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

        public bool IsSuccessfull
        {
            get { return (!IsServiceError && !IsClientError); }
        }

        public uint ServiceCode { get { return serviceCode; } }

        public Exception ServiceException { get { return serviceException; } }

        public string ServiceString { get { return serviceString; } }

        public static ServiceOperationResult FromClientException(Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException();
            }

            return new ServiceOperationResult
                (
                    2, // Client exception
                    "CLIENTEXCEPTION",
                    null,
                    ex
                );
        }

        public static bool operator !=(ServiceOperationResult result, bool value)
        {
            return !(result == value);
        }

        public static bool operator ==(ServiceOperationResult result, bool value)
        {
            return result.IsSuccessfull == value;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ServiceOperationResult))
            {
                return false;
            }

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