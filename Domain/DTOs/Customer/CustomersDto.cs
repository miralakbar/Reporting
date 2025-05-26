using System;

namespace Domain.DTOs.Customer
{
    public class CustomersDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string Pin { get; set; }

        public string PartnerName { get; set; }

        public DateTime? AddedDate { get; set; }

    }
}
