using GAAPICommon.Core;
using GAAPICommon.Core.Dtos;
using NUnit.Framework;
using System;

namespace BaseClients.Test
{
    [TestFixture]
    [Category("ServiceOperationResult")]
    public class TServiceOperationResult
    {
        [Test]
        public void FromClientException()
        {
            ArgumentNullException nullEx = new ArgumentNullException("oak nuggins");

            ServiceCallResultDto result = ServiceCallResultFactory.FromClientException(nullEx);
        }
    }
}