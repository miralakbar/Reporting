using Application.Repositories.Base;
using Domain.Entities;
using Domain.Models.Partner;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IPartnerRepository : IRepository<Partner>
	{
		Task<(List<Partner> partners,int count)> GetPartners(PartnerRequestModel input);
        Task<bool> AnyAsync(Expression<Func<Partner, bool>> predicate);
    }
}
