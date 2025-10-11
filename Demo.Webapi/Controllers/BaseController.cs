using Dapper;
using Demo.Webapi.BLayer.BaseBL;
using Demo.Webapi.Common;
using Demo.Webapi.Common.Entites.DTO;
using Demo.Webapi.Common.Entities;
using Demo.Webapi.Common.Entities.DTO;
using Demo.Webapi.Common.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.Style;
using System.Drawing;

namespace Demo.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase
    {
        #region Field

        protected IBaseBL<T> _baseBL;
        public ErrorResult _exceptionResult = new ErrorResult(ErrorCode.Exception, Resource.devMsg_Exception, Resource.userMsg_Exception, "More Infor: ");

        #endregion

        #region Contructor

        public BaseController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }

        #endregion

        /// <summary>
        /// Lấy toàn bộ bản ghi
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllRecord()
        {
            try 
            {
                var result = _baseBL.GetRecords();
                if (result == null)
                {
                    return NotFound();
                } 
                else
                {
                    return StatusCode(200, _baseBL.GetRecords());
                       
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                _exceptionResult.TraceId = HttpContext.TraceIdentifier;
                return StatusCode(500, _exceptionResult);
            }
        }

        /// <summary>
        /// Tìm bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetRecordById([FromRoute]Guid id) 
        {
            try
            {
                var result = _baseBL.GetRecordById(id);
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(200, result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _exceptionResult.TraceId = HttpContext.TraceIdentifier;
                return StatusCode(500, _exceptionResult);
            }
        }

        /// <summary>
        /// Lọc bản ghi theo điều kiện
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="departmentId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Filter")]

        public IActionResult FilterRecord(
            [FromQuery] string? keyWord,
            [FromQuery] int? pageSize,
            [FromQuery] int? pageNumber)
        {
            try
            {
                var result = _baseBL.FilterRecord(keyWord, pageSize, pageNumber);
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(200, result);
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _exceptionResult.TraceId= HttpContext.TraceIdentifier;
                return StatusCode(500, _exceptionResult);
            }
        }

        /// <summary>
        /// Tạo mới một bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateRecord([FromBody] T record) 
        {
            try
            {
                var result = _baseBL.CreateRecord(record);
                return StatusCode(201, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _exceptionResult.DevMsg = ex.Message;
                _exceptionResult.TraceId = HttpContext.TraceIdentifier;
                return StatusCode(500, _exceptionResult);
            }
        }

        [HttpPost("BulkCreate")]
        public IActionResult BulkCreate([FromBody] IEnumerable<T> records)
        {
            try
            {
                var result = _baseBL.BulkCreate(records);
                return StatusCode(201, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _exceptionResult.DevMsg = ex.Message;
                _exceptionResult.TraceId = HttpContext.TraceIdentifier;
                return StatusCode(500, _exceptionResult);
            }
        }

        /// <summary>
        /// Cập nhật thông tin bản ghi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateRecord([FromQuery]Guid id, [FromBody] T record)
        {
            var result = _baseBL.UpdateRecord(id, record);
            if (result.IsSuccess)
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(400, result);
            }
        }

        /// <summary>
        /// Xóa một bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteRecord([FromRoute] Guid id)
        {
            try
            {
                int rowEffected = _baseBL.DeleteRecord(id);
                if (rowEffected == 0)
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(200, new ServiceResult()
                    {
                        IsSuccess = true,
                        Message = Resource.deleteSuccess,
                        Data = null,
                    });
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _exceptionResult.TraceId = HttpContext.TraceIdentifier;
                return StatusCode(500, _exceptionResult);
            }
        }

        /// <summary>
        /// Hàm xóa hàng loạt
        /// </summary>
        /// <param name="recordIds">Danh sách id</param>
        /// <author>Xuân Đào - 02/05/2023</author>
        /// <returns></returns>
        [HttpDelete]
        [Route("MultipleDelete")]
        public IActionResult MultipleDelete([FromQuery]string recordIds)
        {
            try
            {
                int rowEffected = _baseBL.MultipleDeleteRecord(recordIds);
                if (rowEffected >= -1)
                {
                    return StatusCode(200, new ServiceResult()
                    {
                        IsSuccess = true,
                        Message = Resource.deleteSuccess,
                        Data = rowEffected,
                    }
                    );
                } else
                {
                    return StatusCode(200, new ServiceResult()
                    {
                        IsSuccess = false,
                        Message = Resource.deleteFail,
                        Data = rowEffected,
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _exceptionResult.TraceId = HttpContext.TraceIdentifier;
                return StatusCode(500, _exceptionResult);
            }
        }
    }
}
