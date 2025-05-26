using Application.Repositories.Base;
using Domain.Entities;
using Domain.Models.BiometricProfile;
using Domain.Models.Partner;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IParentPartnerRepository : IRepository<ParentPartner>
    {
        Task<(List<ParentPartner> parentPartners, int count)> GetParentPartner(ParentPartnerRequestModel input);

        Task<ParentPartner> GetParentPartnerById(int parentPartnerId);

        Task<bool> AnyAsync(Expression<Func<ParentPartner, bool>> predicate);

        Task<bool> UpdateParentPartner(UpdateParentPartnerRequestModel input);

        Task<bool> DeleteParentPartner(int parentPartnerId);

    }
}
