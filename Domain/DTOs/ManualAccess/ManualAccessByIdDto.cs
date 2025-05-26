using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.ManualAccess
{
	public class ManualAccessByIdDto
	{
		public int Id { get; set; }

		public string Pin { get; set; }

		public string Image { get; set; }

		public int PartnerId { get; set; }

		public DateTime? AddedDate { get; set; }

		public long? KycOperationId { get; set; }

		public long? BiometricAccessOperationId { get; set; }
	}
}
