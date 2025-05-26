using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Kyc
{
	public class CountDto
	{
        public OperationCount Count { get; set; }
		public int Customer { get; set; }
    }

	public class OperationCount
	{
		public long All { get; set; }
		public long Success { get; set; }
		public long Fail { get; set; }
	}
}
