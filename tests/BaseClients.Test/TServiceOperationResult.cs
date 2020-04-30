using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;
using BaseClients.Core;

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

			ServiceOperationResult result = ServiceOperationResult.FromClientException(nullEx);

			Assert.IsTrue(result.IsClientError);
			Assert.IsFalse(result.IsSuccessfull);
		}
	}
}
