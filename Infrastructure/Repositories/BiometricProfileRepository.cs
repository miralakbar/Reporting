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
	public class BiometricProfileRepository : Repository<BiometricProfile>, IBiometricProfileRepository
	{
		public BiometricProfileRepository(KycDBContext dbContext) : base(dbContext) { }

		public async Task<(List<BiometricProfile> BiometricProfiles, int Count)> GetBiometricProfile(GetBiometricProfileRequestModel input)
		{
			IQueryable<BiometricProfile> query = _dbSet.AsNoTracking().Include(x => x.BiometricProfileHistories).ThenInclude(x => x.DocumentType)
																	  .Include(x => x.KycOperations).ThenInclude(ko => ko.GlobalDocumentType).AsSplitQuery();

			if (input.PartnerId != null)
			{
				query = query.Where(bp => bp.KycOperations.Any(ko => ko.PartnerId == input.PartnerId));
			}

			if (input.LivenessScoreFrom.HasValue || input.LivenessScoreTo.HasValue ||
				input.SimilarityScoreFrom.HasValue || input.SimilarityScoreTo.HasValue)
			{
				query = query.Where(bp => bp.KycOperations.Any(ko =>
					(!input.LivenessScoreFrom.HasValue || ko.LivenessScore * 100 >= input.LivenessScoreFrom) &&
					(!input.LivenessScoreTo.HasValue || ko.LivenessScore * 100 <= input.LivenessScoreTo) &&
					(!input.SimilarityScoreFrom.HasValue || ko.SimilarityScore * 100 >= input.SimilarityScoreFrom) &&
					(!input.SimilarityScoreTo.HasValue || ko.SimilarityScore * 100 <= input.SimilarityScoreTo)
				));
			}
			if (!string.IsNullOrEmpty(input.GlobalSearch))
			{
				var searchText = input.GlobalSearch.Trim().ToLower(); 

				query = query.Where(bp =>
					bp.Pin.ToLower().Contains(searchText) ||
					bp.BiometricProfileHistories.Any(h =>
						h.Id == bp.BiometricProfileHistories.Min(bph => bph.Id) &&
						(
							h.Name.ToLower().Contains(searchText) ||
							h.Surname.ToLower().Contains(searchText) ||
							(h.Name.ToLower() + " " + h.Surname.ToLower()).Contains(searchText) 
						)
					)
				);
			}

			if (!string.IsNullOrEmpty(input.Pin))
			{
				query = query.Where(bp => bp.Pin.ToLower().Contains(input.Pin.ToLower()));
			}

			if (input.IsBlocked.HasValue)
			{
				query = query.Where(bp => bp.IsBlocked == input.IsBlocked);
			}

			if (!string.IsNullOrEmpty(input.Name))
			{
				query = query.Where(bp =>
					bp.BiometricProfileHistories
						.Any(h => h.Id == bp.BiometricProfileHistories.Min(bph => bph.Id) &&
								  h.Name.ToLower().Contains(input.Name.ToLower())));
			}

			if (!string.IsNullOrEmpty(input.Surname))
			{
				query = query.Where(bp =>
					bp.BiometricProfileHistories
						.Any(h => h.Id == bp.BiometricProfileHistories.Min(bph => bph.Id) &&
								  h.Surname.ToLower().Contains(input.Surname.ToLower())));
			}

			if (!string.IsNullOrEmpty(input.Citizenship))
			{
				query = query.Where(bp =>
					bp.BiometricProfileHistories
						.Any(h => h.Id == bp.BiometricProfileHistories.Min(bph => bph.Id) &&
								  h.Citizenship.ToLower().Contains(input.Citizenship.ToLower())));
			}

			query = query.OrderByDescending(bp => bp.KycOperations.Max(ko => ko.AddedDate));

			var pagedResult = await GetAllWithPaginationAsync(input.PageNumber, input.PageSize, query);
			var count = await query.CountAsync();

			if (input.PartnerId != null)
			{
				pagedResult.ForEach(bp =>
				{
					var matchingDocumentTypeIds = bp.KycOperations
						.Where(ko => ko.PartnerId == input.PartnerId && ko.GlobalDocumentTypeId != null)
						.Select(ko => ko.GlobalDocumentType.Id)
						.Distinct()
						.ToList();

					bp.BiometricProfileHistories = bp.BiometricProfileHistories
						.Where(h => h.DocumentType != null && matchingDocumentTypeIds.Contains(h.DocumentType.Id))
						.OrderBy(h => h.Id)
						.ToList();
				});
			}
			else
			{
				pagedResult.ForEach(bp =>
				{
					bp.BiometricProfileHistories = bp.BiometricProfileHistories
						.OrderBy(h => h.Id)
						.ToList();
				});
			}

			return (pagedResult, count);
		}

		public async Task<bool> UpdateBiometricProfileStatus(UpdateBiometricProfileStatusRequestModel input)
		{
			var biometricProfile = await GetByIdAsync(input.BiometricProfileId);
			if (biometricProfile == null)
			{
				return false;
			}

			biometricProfile.IsBlocked = input.IsBlocked;
			await _dbContext.SaveChangesAsync();
			return true;
		}
	}
}
