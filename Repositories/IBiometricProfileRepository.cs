using Application.Repositories.Base;
using Domain.Entities;
using Domain.Models.BiometricProfile;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repositories
{
	public interface IBiometricProfileRepository : IRepository<BiometricProfile>
	{
		Task<(List<BiometricProfile> BiometricProfiles, int Count)> GetBiometricProfile(GetBiometricProfileRequestModel input);

		Task<bool> UpdateBiometricProfileStatus(UpdateBiometricProfileStatusRequestModel input);
	}
}
