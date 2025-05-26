namespace Domain.DTOs
{
    public class GetPartnersDto
    {
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerDescription { get; set; }
        public byte PartnerStatus { get; set; }
		public string MasterKey { get; set; }
        public string PartnerStatusName { get; set; }
		public string IdempotencyKey { get; set; }
		public bool AuthRequired { get; set; }
	}
}
