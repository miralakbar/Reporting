using Application.Repositories;
using Domain.Entities;
using Domain.Models.BiometricProfile;
using Infrastructure.Database;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BiometricProfileHistoryRepository : Repository<BiometricProfileHistory>, IBiometricProfileHistoryRepository
    {
        public BiometricProfileHistoryRepository(KycDBContext dbContext) : base(dbContext) { }

        public async Task<List<BiometricProfileHistory>> GetBiometricProfileHistory(GetBiometricProfileHistoryRequestModel input)
        {
            var query = _dbSet.Include(x => x.BiometricProfile).Include(x => x.DocumentType).AsNoTracking();

            if (input.BiometricProfileId.HasValue)
            {
                query = query.Where(x => x.BiometricProfileId == input.BiometricProfileId);
            };

            if (input.BiometricProfileHistoryId.HasValue)
            {
                query = query.Where(x => x.Id == input.BiometricProfileHistoryId);
            }

            query=query.Where(x=>x.IsActive==true);

            var biometricProfileHistories = await query.ToListAsync();

            return biometricProfileHistories;
        }
    }
}
