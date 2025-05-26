using System;

namespace Domain.DTOs.BiometricProfile
{
    public class BiometricProfileHistoryDto
    {
        public int Id { get; set; }
        public int? BiometricProfileId { get; set; }
        public bool? IsBlocked { get; set; }
        public bool? IsActive { get; set; }
        public string IamasImage { get; set; }
        public DateTime? AddedDate { get; set; }
        public string Pin { get; set; }
        public string DocumentNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthAddress { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Citizenship { get; set; }
        public string IssuingCountry { get; set; }
        public DateTime? EventDate { get; set; }
        public DateTime? ExpDate { get; set; }
        public string Organization { get; set; }
        
        public DocumentTypeDto DocumentType { get; set; }
    }
}
