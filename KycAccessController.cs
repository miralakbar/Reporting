using Application.CQRS.Core;
using Application.CQRS.Kyc.Queries;
using Application.CQRS.KycAccess.Queries;
using Application.Extension;
using Domain.DTOs.Base;
using Domain.DTOs.Kyc;
using Domain.DTOs.KycAccess;
using Domain.Models.Kyc;
using Domain.Models.KycAccess;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.V1
{
    [Route("api/v{version:apiVersion}/kyc-access")]
    [ApiController]
    [ApiVersion("1.0")]
    public class KycAccessController : BaseController
    {
        [HttpPost]
        [Route("operations")]
        public async Task<ActionResult<ApiResult<PagedResult<KycAccessOperationsDto>>>> KycOperations(KycAccessOperationRequestModel input)
        {
            var query = new GetKycAccessOperationQuery(input);
            return await Mediator.HandleRequest(query, HttpContext);
        }

        [HttpGet]
        [Route("operations/{id}")]
        public async Task<ActionResult<ApiResult<KycAccessOperationDto>>> KycOperation(long id)
        {
            var query = new GetKycAccessOperationByIdQuery(id);
            return await Mediator.HandleRequest(query, HttpContext);
        }

        [HttpPost]
        [Route("count")]
        public async Task<ActionResult<ApiResult<CountDto>>> Count(KycAccessCountRequestModel model)
        {
            var query = new KycAccessCountQuery(model);
            return await Mediator.HandleRequest(query, HttpContext);
        }

        [HttpPost]
        [Route("export")]
        public async Task<ActionResult<ApiResult<string>>> KycAccessOperationsExport(KycAccessOperationExportRequestModel input)
        {
            var query = new GetKycAccessOperationsExportQuery(input);
            return await Mediator.HandleRequest(query, HttpContext);
        }
    }
}
