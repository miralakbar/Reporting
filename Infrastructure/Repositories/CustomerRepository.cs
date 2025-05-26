using Application.Repositories;
using Domain.Entities;
using Domain.Models.Customer;
using Infrastructure.Database;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(KycDBContext dbContext) : base(dbContext) { }

        public async Task<Customer> GetCustomerById(Guid id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<(List<Customer>, int)> GetCustomers(CustomerRequestModel input)
        {
            var query = _dbSet.AsNoTracking();

            if (input.PartnerId != null)
            {
                query = query.Where(x => x.PartnerId == input.PartnerId);
            }
            if (!string.IsNullOrEmpty(input.Name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(input.Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(input.Surname))
            {
                query = query.Where(x => x.Surname.ToLower().Contains(input.Surname.ToLower()));
            }
            if (!string.IsNullOrEmpty(input.Patronymic))
            {
                query = query.Where(x => x.Patronymic.ToLower().Contains(input.Patronymic.ToLower()));
            }
            if (!string.IsNullOrEmpty(input.Pin))
            {
                query = query.Where(x => x.Pin.ToLower().Contains(input.Pin.ToLower()));
            }
            if (input.DateFrom.HasValue)
            {
                query = query.Where(x => x.AddedDate >= input.DateFrom);
            }
            if (input.DateTo.HasValue)
            {
                query = query.Where(x => x.AddedDate <= input.DateTo);
            }
            query = query.OrderByDescending(x => x.AddedDate).Include(x => x.Partner);


            var operation = await GetAllWithPaginationAsync(input.PageNumber, input.PageSize, query, x => x.Partner);
            var count = await CountAsync(query);

            return (operation, count);
        }
    }
}
