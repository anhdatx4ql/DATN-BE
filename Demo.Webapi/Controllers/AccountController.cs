using Demo.Webapi.BLayer.BaseBL;
using Demo.Webapi.Common;
using Demo.Webapi.Common.Entities;
using Demo.Webapi.Common.Enums;
using Demo.Webapi.DL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using Demo.Webapi.BLayer;
using Demo.Webapi.Common.Entities.DTO;

namespace Demo.Webapi.Controllers
{
    public class AccountController : BaseController<Account>
    {
        #region Field

        private IAccountBL _accountBL;

        #endregion

        public AccountController(IBaseBL<Account> baseBL, IAccountBL accountBL) : base(baseBL)
        {
            _accountBL = accountBL;
        }
        [HttpPost("ExcelExport")]
        public IActionResult ExcelExport([FromQuery] string? widthList, [FromBody] List<Account> accounts)
        {
            var stream = new MemoryStream();

            string[] width = widthList.Split(",");

            string excelTitle = Resource.accountExcelTitle;
            string excelFile = Resource.accountExcelFile;
            string excelSheet = Resource.accountExcelSheet;

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add(excelSheet);
                workSheet.Row(1).Style.Font.Name = "Arial";
                workSheet.Row(1).Style.Font.Size = 16;
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Row(1).Height = 20;
                workSheet.Cells["A1:F1"].Merge = true;
                workSheet.Cells["A1:F1"].Value = excelTitle;
                workSheet.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet.Cells["A2:F2"].Merge = true;
                workSheet.Row(2).Height = 20;
                workSheet.Row(3).Style.Font.Name = "Times New Roman";
                workSheet.Row(3).Style.Font.Bold = true;

                List<string> titleList = ExcelConfig.getAccountColName();
                List<string> dataList = ExcelConfig.getAccountModelName();
                List<string> propertyName = ExcelConfig.AccountPropertyValue();
                List<string> statusName = ExcelConfig.AccountStatusValue();
                // Title generator
                int k = 1;
                foreach (var cell in titleList)
                {
                    workSheet.Cells[3, k].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[3, k].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 212, 212, 212));
                    workSheet.Cells[3, k].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    workSheet.Cells[3, k].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[3, k].Value = cell;
                    workSheet.Column(k).Width = (Int32.Parse(width[k - 1])) / 8;
                    workSheet.Cells[3, k].Style.Font.Name = "Times New Roman";
                    k++;
                }
                // Data generator
                for (int i = 0; i < accounts.Count(); i++)
                {
                    int j = 1;
                    foreach (var cell in dataList)
                    {
                        workSheet.Cells[i+4, j].Style.Font.Name = "Times New Roman";
                        if (j == 1)
                        {
                            workSheet.Cells[i + 4, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            workSheet.Cells[i + 4, j].Style.Indent = Int32.Parse(accounts[i].GetType().GetProperty("DataLevel").GetValue(accounts[i]).ToString());
                        }
                        if ((i + 1) < accounts.Count() && Int32.Parse(accounts[i].GetType().GetProperty("DataLevel").GetValue(accounts[i]).ToString()) < Int32.Parse(accounts[i + 1].GetType().GetProperty("DataLevel").GetValue(accounts[i + 1]).ToString()))
                        {
                            workSheet.Cells[i + 4, j].Style.Font.Bold = true;
                        }
                        else
                        {
                            workSheet.Cells[i + 4, j].Value = accounts[i].GetType().GetProperty(cell).GetValue(accounts[i]);
                        }
                        workSheet.Cells[i + 4, j].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        j++;
                    }
                }
                package.Save();
            }

            stream.Position = 0;
            string fileNameExport = $"{excelFile}-{DateTime.Now.ToString()}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileNameExport);
        }

        [HttpPut("UpdateMultiple")]
        public IActionResult UpdateMultipleStatus([FromQuery] string ids, [FromQuery] int newStatus)
        {
            int result = _accountBL.UpdateMultipleStatus(ids, newStatus);
            if (result == 0)
            {
                return StatusCode(400, new ServiceResult
                {
                    IsSuccess = false,
                    Data = result,
                    Message = Resource.updateFail,
                });
            } else
            {
                return StatusCode(200, new ServiceResult
                {
                    IsSuccess = true,
                    Data = result,
                    Message = Resource.updateSuccess,
                });
            }
        }

        [HttpPut("UpdateAccountLevel")]
        public IActionResult UpdateAccountLevel([FromBody]List<Account> accounts)
        {
            int result = _accountBL.UpdateAccountLevel(accounts);
            if (result == 0)
            {
                return StatusCode(404, NotFound());
            }
            else
            {
                return StatusCode(200, new ServiceResult
                {
                    IsSuccess = true,
                    Data = result,
                    Message = Resource.updateSuccess,
                });
            }
        }

        [HttpGet("Login")]
        public IActionResult Login(string username, string password)
        {
            var result = _accountBL.Login(username, password);
            return StatusCode(200, result);
        }
    }
}
