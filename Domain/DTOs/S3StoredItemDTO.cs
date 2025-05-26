using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
	public class S3StoredItemDTO
	{
		public string BucketName { get; set; }
		public string ObjectName { get; set; }
		public string ObjectTempLink { get; set; }
	}
}
