using Application.Repositories;
using Domain.Entities;
using Domain.Models.ManualAccess;
using Infrastructure.Database;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
	internal class ManualAccessRepository : Repository<ManualAccess>, IManualAccessRepository
	{
		public ManualAccessRepository(KycDBContext dbContext) : base(dbContext) { }

		public async Task<(List<ManualAccess> manualAccess, int Count)> GetManualAccess(GetManualAccessRequestModel input)
		{
			var query = _dbSet.AsNoTracking();

			if (input.PartnerId != null)
			{
				query = query.Where(x => x.PartnerId == input.PartnerId);
			}
			if (!string.IsNullOrEmpty(input.Pin))
			{
				query = query.Where(x => x.Pin.Contains(input.Pin));
			}
			
			var manualAccesses = await GetAllWithPaginationAsync(input.PageNumber, input.PageSize, query, x => x.Partner);
			var count = await CountAsync(query);

			return (manualAccesses, count);
		}
	}
}
