using System;

namespace Domain.DTOs.KycAccess
{
    public class KycAccessOperationsDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Pin { get; set; }

        public string PartnerName { get; set; }

        public double? SimilarityScore { get; set; }

        public DateTime? AddedDate { get; set; }

        public bool? IsSimilaritySuccess { get; set; }

        public bool? IsLivenessSuccess { get; set; }

        public bool? IsSuccessfulOperation { get; set; }
    }
}
