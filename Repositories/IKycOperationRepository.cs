using Application.Repositories.Base;
using Domain.DTOs.Kyc;
using Domain.Entities;
using Domain.Models.Kyc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IKycOperationRepository : IRepository<KycOperation>
    {
        Task<(List<KycOperation> Operatios, int Count)> GetOperations(KycOperationRequestModel input);
        Task<KycOperation> GetOperationById(long kycOpearationId);
        Task<OperationCount> OperationCountAsync(KycCountRequestModel model);
        Task<List<KycOperation>> GetOperationsForExport(KycOperationExportRequestModel input);


    }
}
