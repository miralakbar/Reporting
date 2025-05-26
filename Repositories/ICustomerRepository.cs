using Application.Repositories.Base;
using Domain.Entities;
using Domain.Models.Customer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public  interface ICustomerRepository: IRepository<Customer>
    {
        Task<(List<Customer> Customers, int Count)> GetCustomers(CustomerRequestModel input);
        Task<Customer> GetCustomerById(Guid id);
    }
}
