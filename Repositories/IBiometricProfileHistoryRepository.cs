using Application.Repositories.Base;
using Domain.Entities;
using Domain.Models.BiometricProfile;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repositories
{
	public interface IBiometricProfileHistoryRepository : IRepository<BiometricProfileHistory>
	{
		Task<List<BiometricProfileHistory>> GetBiometricProfileHistory(GetBiometricProfileHistoryRequestModel input);

	}
}
