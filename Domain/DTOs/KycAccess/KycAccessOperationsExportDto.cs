

using System;

namespace Domain.DTOs.Kyc
{
    public class KycAccessOperationsExportDto
    {
        public long Id { get; set; }
        public string ServiceType { get; set; }
        public int PartnerId { get; set; }
        public long BiometricProfileId { get; set; }
        public string Pin { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? AddedDate { get; set; }
        public bool? IsSuccessfulOperation { get; set; }
        public double? LivenessScore { get; set; }
        public double? SimilarityScore { get; set; }
        public string ErrorCode { get; set; }
        public string UId { get; set; }
        public string LivenessStatus { get; set; }
        public string IpAddress { get; set; }
        public string DeviceInfo { get; set; }
        public string IdempotencyKey { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthAddress { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime? ExpDate { get; set; }
        public string DocumentType { get; set; }
        public string IssuingCountry { get; set; }
        public long? AccessOperationId { get; set; }
        public string Nationality { get; set; }
        public string Organization { get; set; }
        public DateTime? EventDate { get; set; }
        public bool? ManualAccessUsed { get; set; }
        public string FaceDetectionReason { get; set; }
        public string ResponseErrorCode { get; set; }
    }
}
