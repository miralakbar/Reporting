using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Customer
{
    public class CustomerDto
    {
        public Guid Id { get; set; }

        public string Pin { get; set; }

        public string DocumentNumber { get; set; }

        public DateTime? AddedDate { get; set; }

        public string ErrorMessage { get; set; }

        public int? ErrorCode { get; set; }

        public string LivePhoto { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string Birth { get; set; }

        public string BirthAddress { get; set; }

        public string Address { get; set; }

        public string Gender { get; set; }
    }
}
