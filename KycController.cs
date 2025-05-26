using Application.CQRS.Core;
using Application.CQRS.Kyc.Queries;
using Application.Extension;
using Domain.DTOs.Base;
using Domain.DTOs.Kyc;
using Domain.Models.Kyc;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace API.Controllers.V1
{
    [Route("api/v{version:apiVersion}/kyc")]
    [ApiController]
    [ApiVersion("1.0")]
    public class KycController : BaseController
    {
        [HttpPost]
        [Route("operations")]
        public async Task<ActionResult<ApiResult<PagedResult<KycOperationsDto>>>> KycOperations(KycOperationRequestModel input)
        {
            var query = new GetKycOperationsQuery(input);
            return await Mediator.HandleRequest(query, HttpContext);
        }

        [HttpGet]
        [Route("operations/{id}")]
        public async Task<ActionResult<ApiResult<KycOperationDto>>> KycOperation(long id)
        {
            var query = new GetKycOperationByIdQuery(id);
            return await Mediator.HandleRequest(query, HttpContext);
        }

        [HttpPost]
        [Route("count")]
        public async Task<ActionResult<ApiResult<CountDto>>> Count(KycCountRequestModel input)
        {
            var query = new KycCountQuery(input);
            return await Mediator.HandleRequest(query, HttpContext);
        }

        [HttpPost]
        [Route("export")]
        public async Task<ActionResult<ApiResult<string>>> KycOperationsExport(KycOperationExportRequestModel input)
        {
            var query = new GetKycOperationsExportQuery(input);
            return await Mediator.HandleRequest(query, HttpContext);
        }
    }
}
