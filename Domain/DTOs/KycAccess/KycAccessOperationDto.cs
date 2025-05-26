using System;

namespace Domain.DTOs.KycAccess
{
    public class KycAccessOperationDto
    {
        public long Id { get; set; }
        public string Pin { get; set; }

        public string DocumentNumber { get; set; }

        public DateTime? AddedDate { get; set; }

        public bool? IsSuccessfulOperation { get; set; }
        public double? SimilarityScore { get; set; }

        public double? LivenessScore { get; set; }
        public bool? LivenessStatus { get; set; }

        public string ErrorMessage { get; set; }

        public int? ErrorCode { get; set; }

        public string LiveImage { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string BirthDate { get; set; }

        public string BirthAddress { get; set; }

        public string Address { get; set; }

        public string Gender { get; set; }

        public string ExpDate { get; set; }
    }
}
