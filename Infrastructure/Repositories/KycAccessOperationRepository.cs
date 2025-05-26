using Application.Repositories;
using Domain.DTOs.Kyc;
using Domain.Entities;
using Domain.Enums;
using Domain.Models.Kyc;
using Domain.Models.KycAccess;
using Infrastructure.Database;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class KycAccessOperationRepository : Repository<BiometricAccessOperation>, IKycAccessOperationRepository
    {
        public KycAccessOperationRepository(KycDBContext dbContext) : base(dbContext) { }

        public async Task<(List<BiometricAccessOperation>, int)> GetOperations(KycAccessOperationRequestModel input)
        {
            var query = _dbSet.AsNoTracking();

            if (input.PartnerId != null)
            {
                query = query.Where(x => x.PartnerId == input.PartnerId);
            }
            if (input.BiometricProfileId != null)
            {
                query = query.Where(x => x.BiometricProfileId == input.BiometricProfileId);
            }
            if (!string.IsNullOrEmpty(input.Name?.Trim()))
            {
                query = query.Where(x => x.Name.Trim().ToLower().Contains(input.Name.Trim().ToLower()));
            }
            if (!string.IsNullOrEmpty(input.Surname?.Trim()))
            {
                query = query.Where(x => x.Surname.Trim().ToLower().Contains(input.Surname.Trim().ToLower()));
            }
            if (!string.IsNullOrEmpty(input.Pin?.Trim()))
            {
                query = query.Where(x => x.Pin.Trim().ToLower().Contains(input.Pin.Trim().ToLower()));
            }
            if (input.SimilarityScoreFrom.HasValue)
            {
                query = query.Where(x => x.Similarity * 100 >= input.SimilarityScoreFrom);
            }
            if (input.SimilarityScoreTo.HasValue)
            {
                query = query.Where(x => x.Similarity * 100 <= input.SimilarityScoreTo);
            }
            if (input.DateFrom.HasValue)
            {
                query = query.Where(x => x.AddedDate >= input.DateFrom);
            }
            if (input.DateTo.HasValue)
            {
                query = query.Where(x => x.AddedDate <= input.DateTo);
            }
            if (input.IsSuccessfulOperation.HasValue)
            {
                query = query.Where(x => x.IsSuccessfulOperation == input.IsSuccessfulOperation);
            }

            query = query.OrderByDescending(x => x.AddedDate).Include(x => x.Partner);

            var operation = await GetAllWithPaginationAsync(input.PageNumber, input.PageSize, query, x => x.Partner);
            var count = await CountAsync(query);

            return (operation, count);
        }

        public async Task<BiometricAccessOperation> GetOperationById(long kycAccessOpearationId)
        {
            return await GetByIdAsync(kycAccessOpearationId);
        }

        public async Task<List<BiometricAccessOperation>> GetOperationsForExport(KycAccessOperationExportRequestModel input)
        {
            var query = _dbSet.AsNoTracking();

            if (input.BiometricProfileId != null)
            {
                query = query.Where(x => x.BiometricProfileId == input.BiometricProfileId);
            }
            if (input.PartnerId != null)
            {
                query = query.Where(x => x.PartnerId == input.PartnerId);
            }
            if (!string.IsNullOrEmpty(input.Name?.Trim()))
            {
                query = query.Where(x => x.Name.Trim().ToLower().Contains(input.Name.Trim().ToLower()));
            }
            if (!string.IsNullOrEmpty(input.Surname?.Trim()))
            {
                query = query.Where(x => x.Surname.Trim().ToLower().Contains(input.Surname.Trim().ToLower()));
            }
            if (!string.IsNullOrEmpty(input.Pin?.Trim()))
            {
                query = query.Where(x => x.Pin.Trim().ToLower().Contains(input.Pin.Trim().ToLower()));
            }
            if (input.LivenessScoreFrom.HasValue)
            {
                query = query.Where(x => x.LivenessScore * 100 >= input.LivenessScoreFrom);
            }
            if (input.LivenessScoreTo.HasValue)
            {
                query = query.Where(x => x.LivenessScore * 100 <= input.LivenessScoreTo);
            }
            if (input.SimilarityScoreFrom.HasValue)
            {
                query = query.Where(x => x.Similarity * 100 >= input.SimilarityScoreFrom);
            }
            if (input.SimilarityScoreTo.HasValue)
            {
                query = query.Where(x => x.Similarity * 100 <= input.SimilarityScoreTo);
            }
            if (input.IsSuccessfulOperation.HasValue)
            {
                query = query.Where(x => x.IsSuccessfulOperation == input.IsSuccessfulOperation);
            }

            query = query.Where(x => x.AddedDate >= input.DateFrom);
            query = query.Where(x => x.AddedDate <= input.DateTo);

            var operation = await GetAllAsync(true, query);
            return operation;
        }

        public async Task<OperationCount> OperationCountAsync(KycAccessCountRequestModel model)
        {
            var query = _dbSet.AsNoTracking();
            if (query == null) query = _dbSet.AsQueryable();

            if (model.PartnerId.HasValue)
                query = query.Where(x => x.PartnerId == model.PartnerId);

            if (model.FromDate.HasValue)
                query = query.Where(x => x.AddedDate >= model.FromDate.Value);

            if (model.ToDate.HasValue)
                query = query.Where(x => x.AddedDate <= model.ToDate.Value);

            var result = await query
                .GroupBy(x => 1)
                .Select(g => new OperationCount
                {
                    All = g.Count(),
                    Success = g.Count(x =>
                        (x.ServiceType == (int)KYCServiceType.VerifyIdentityByVoen && x.IsSuccessfulOperation == true && x.LivenessStatus == true && x.SimilarityStatus == true) ||
                        ((x.ServiceType == (int)KYCServiceType.VerifyIdentity
                        || x.ServiceType == (int)KYCServiceType.VerifyMigrationIdentity
                        || x.ServiceType == (int)KYCServiceType.VerifyIdentityWithDetails)
                        && x.IsSuccessfulOperation == true))
                })
                .FirstOrDefaultAsync();

            if (result != null)
                result.Fail = result.All - result.Success;
            else
                result = new OperationCount();

            return result;
        }
    }
}
