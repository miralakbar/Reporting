using System.Collections.Generic;

namespace Domain.DTOs.BiometricProfile
{
    public class BiometricProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Pin { get; set; }
        public string Citizenship { get; set; }
        public bool? IsBlocked { get; set; }
        public List<BiometricProfileInfo> BiometricProfileInfo { get; set; }
    }

    public class BiometricProfileInfo
    {
        public int BiometricProfileHistoryId { get; set; }
        public string DocumentTypeName { get; set; }
        public string DocumentTypeDescription { get; set; }
    }
}