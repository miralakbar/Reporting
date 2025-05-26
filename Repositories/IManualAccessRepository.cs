using Application.Repositories.Base;
using Domain.Entities;
using Domain.Models.Kyc;
using Domain.Models.ManualAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
	public interface IManualAccessRepository : IRepository<ManualAccess>
	{
		Task<(List<ManualAccess> manualAccess, int Count)> GetManualAccess(GetManualAccessRequestModel input);

	}
}
