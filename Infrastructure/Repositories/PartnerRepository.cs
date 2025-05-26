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
	public class PartnerRepository : Repository<Partner>, IPartnerRepository
	{
		public PartnerRepository(KycDBContext dbContext) : base(dbContext) { }

		public async Task<(List<Partner>, int)> GetPartners(PartnerRequestModel input)
		{
			var query = _dbSet.AsNoTracking();
			
			if (input.PartnerId != null)
			{
				query = query.Where(x => x.Id == input.PartnerId);
			}
			if (input.ParentId!=null)
			{
				query = query.Where(x => x.ParentId == input.ParentId);
			}
			if (!string.IsNullOrEmpty(input.GlobalSearch))
			{
				int idSearchValue;
				bool isNumericSearch = int.TryParse(input.GlobalSearch, out idSearchValue);

				query = query.Where(x =>
					isNumericSearch
						? x.Id == idSearchValue
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

			query = query.OrderByDescending(x => x.CreatedDate);

			var partners= await GetAllWithPaginationAsync(input.PageNumber, input.PageSize, query);
			var count = await CountAsync(query);

			return (partners, count);
		}

        public async Task<bool> AnyAsync(Expression<Func<Partner, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
