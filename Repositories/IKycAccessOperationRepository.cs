using Application.Repositories.Base;
using Domain.DTOs.Kyc;
using Domain.Entities;
using Domain.Models.Kyc;
using Domain.Models.KycAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repositories
{
	public interface IKycAccessOperationRepository : IRepository<BiometricAccessOperation>
	{
        //KycAccess expressed with the BiometricAccess in some places. The meaning is same.
        Task<(List<BiometricAccessOperation> Operations, int Count)> GetOperations(KycAccessOperationRequestModel input);
        Task<BiometricAccessOperation> GetOperationById(long kycOpearationId);
        Task<OperationCount> OperationCountAsync(KycAccessCountRequestModel input);
        Task<List<BiometricAccessOperation>> GetOperationsForExport(KycAccessOperationExportRequestModel input);
    }
}
