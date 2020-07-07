using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;
using BaseClients.Core;
using GAAPICommon.Core;
using GAAPICommon.Core.Dtos;

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
