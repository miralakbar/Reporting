using System;
using System.Collections.Generic;

namespace Domain.DTOs.Partner
{
    public class ParentPartnerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? PartnerId { get; set; }
    }
}
