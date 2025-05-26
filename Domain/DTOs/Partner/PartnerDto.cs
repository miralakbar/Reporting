using System;

namespace Domain.DTOs.Partner
{
    public class PartnerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
