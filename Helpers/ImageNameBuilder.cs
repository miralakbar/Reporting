using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
	public static class ImageNameBuilder
	{
		public static string Generate(int serviceType, int partnerId, int vendorId, string pin,string type, DateTime dateTime)
		{
			string formattedDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
			string result = $"{serviceType}_{partnerId}_{vendorId}_{pin}_{type}_{DateTime.Now.Ticks}_{formattedDate}";

			return result;
		}
	}
}
