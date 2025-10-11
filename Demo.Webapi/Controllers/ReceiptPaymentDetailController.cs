using Demo.Webapi.BLayer;
using Demo.Webapi.BLayer.BaseBL;
using Demo.Webapi.Common;
using Demo.Webapi.Common.Entities;
using Demo.Webapi.Common.Entities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptPaymentDetailController : BaseController<receipt_payment_detail>
    {
        private IReceiptPaymentDetailBL _receiptPaymentDetailBL;

        public ReceiptPaymentDetailController(IBaseBL<receipt_payment_detail> baseBL, IReceiptPaymentDetailBL receiptPaymentDetailBL) : base(baseBL)
        {
            _receiptPaymentDetailBL = receiptPaymentDetailBL;
        }

        [HttpGet("GetAllByReId")]
        public IActionResult GetAllByReId([FromQuery] Guid re_id)
        {
            var result = _receiptPaymentDetailBL.GetAllRPDByReId(re_id);
            if (result != null)
            {
                return StatusCode(200, result);
            }
            else
            {
                return NotFound();
            }
        }

        //[HttpPost("BulkCreate")]
        //public IActionResult BulkCreate([FromBody] IEnumerable<ReceiptPaymentDetail> data)
        //{
        //    int result = _receiptPaymentDetailBL.BulkInsert(data);
        //    if (result != 0)
        //    {
        //        return StatusCode(201, new ServiceResult
        //        {
        //            IsSuccess = true,
        //            Message = Resource.createSuccess,
        //            Data = result,
        //        });
        //    }
        //    else
        //    {
        //        _exceptionResult.TraceId = HttpContext.TraceIdentifier;
        //        return StatusCode(400, _exceptionResult);
        //    }
        //}
    }
}