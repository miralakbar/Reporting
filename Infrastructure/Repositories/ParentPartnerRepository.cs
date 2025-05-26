using Application.Repositories;
using Domain.Entities;
using Domain.Models.Partner;
using Infrastructure.Database;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ParentPartnerRepository : Repository<ParentPartner>, IParentPartnerRepository
    {
        public ParentPartnerRepository(KycDBContext dbContext) : base(dbContext) { }


        public async Task<(List<ParentPartner>, int)> GetParentPartner(ParentPartnerRequestModel input)
        {
            var query = _dbSet.AsNoTracking();

            if (input.PartnerId != null)
            {
                query = query.Where(pp => pp.Partners.Any(p => p.Id == input.PartnerId));
            }
            if (!string.IsNullOrEmpty(input.GlobalSearch))
            {
                int idSearchValue;
                bool isNumericSearch = int.TryParse(input.GlobalSearch, out idSearchValue);

                query = query.Where(x =>
                    isNumericSearch
                        ? x.Partners.Any(p => p.Id.ToString().StartsWith(idSearchValue.ToString()))
                        : x.Name.ToLower().Contains(input.GlobalSearch.ToLower())
                );

            }
            if (!string.IsNullOrEmpty(input.Name))
            {
                query = query.Where(x => x.Name.Contains(input.Name.ToLower()));
            }
            if (input.DateFrom.HasValue)
            {
                query = query.Where(x => x.CreatedDate >= input.DateFrom);
            }
            if (input.DateTo.HasValue)
            {
                query = query.Where(x => x.CreatedDate <= input.DateTo);
            }

            query = query.Where(x => x.Status == true).Include(x => x.Partners).OrderByDescending(x => x.CreatedDate);

            var parentpartner = await GetAllWithPaginationAsync(input.PageNumber, input.PageSize, query);
            var count = await CountAsync(query);

            return (parentpartner, count);
        }
        public async Task<bool> AnyAsync(Expression<Func<ParentPartner, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<ParentPartner> GetParentPartnerById(int parentPartnerId)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == parentPartnerId && e.Status == true);
        }

        public async Task<bool> UpdateParentPartner(UpdateParentPartnerRequestModel input)
        {
            var parentPartner = await GetByIdAsync(input.ParentPartnerId);
            if (parentPartner == null)
            {
                return false;
            }

            parentPartner.Name = input.Name;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteParentPartner(int parentPartnerId)
        {
            var parentPartner = await GetByIdAsync(parentPartnerId);
            if (parentPartner == null)
            {
                return false;
            }

            parentPartner.Status = false;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
