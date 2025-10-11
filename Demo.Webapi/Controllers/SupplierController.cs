using Demo.Webapi.BLayer.BaseBL;
using Demo.Webapi.Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;

namespace Demo.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : BaseController<Supplier>
    {
        public SupplierController(IBaseBL<Supplier> baseBL) : base(baseBL)
        {
        }

        [HttpGet("Excel")]
        public IActionResult Excel([FromQuery] string title)
        {
            var stream = new MemoryStream();

            string excelSheet = "Sheet 1";
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add(excelSheet);
                workSheet.Row(1).Style.Font.Name = "Arial";
                workSheet.Row(1).Style.Font.Size = 16;
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Row(1).Height = 20;
                workSheet.Cells["A1:J1"].Merge = true;
                workSheet.Cells["A1:J1"].Value = title;
                workSheet.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet.Cells["A2:J2"].Merge = true;
                workSheet.Row(2).Height = 20;
                workSheet.Row(3).Style.Font.Name = "Times New Roman";
                workSheet.Row(3).Style.Font.Bold = true;

                int k = 1;
                var properties = typeof(Supplier).GetProperties();
                foreach (var prop in properties)
                {
                    workSheet.Cells[3, k].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[3, k].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 212, 212, 212));
                    workSheet.Cells[3, k].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    workSheet.Cells[3, k].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    workSheet.Cells[3, k].Value = prop.Name;
                    workSheet.Column(k).Width = 30;
                    workSheet.Cells[3, k].Style.Font.Name = "Times New Roman";
                    k++;
                }

                int i = 4;
                var datas = _baseBL.GetRecords();
                foreach (var data in datas)
                {
                    int j = 1;
                    foreach (var prop in properties)
                    {
                        workSheet.Cells[i, j].Style.Font.Name = "Times New Roman";
                        workSheet.Cells[i, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        workSheet.Cells[i, j].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        switch (j)
                        {
                            case 1:
                                {
                                    workSheet.Cells[i, j].Value = data.supplier_id;
                                    break;
                                }
                            case 2:
                                {
                                    workSheet.Cells[i, j].Value = data.supplier_code;
                                    break;
                                }
                            case 3:
                                {
                                    workSheet.Cells[i, j].Value = data.supplier_name;
                                    break;
                                }
                            case 4:
                                {
                                    workSheet.Cells[i, j].Value = data.tax_code;
                                    break;
                                }
                            case 5:
                                {
                                    workSheet.Cells[i, j].Value = data.address;
                                    break;
                                }
                            case 6:
                                {
                                    workSheet.Cells[i, j].Value = data.phone_number;
                                    break;
                                }
                            case 7:
                                {
                                    workSheet.Cells[i, j].Value = data.created_date;
                                    break;
                                }
                            case 8:
                                {
                                    workSheet.Cells[i, j].Value = data.created_by;
                                    break;
                                }
                            case 9:
                                {
                                    workSheet.Cells[i, j].Value = data.modified_date;
                                    break;
                                }
                            case 10:
                                {
                                    workSheet.Cells[i, j].Value = data.modified_by;
                                    break;
                                }
                            default:
                                {
                                    workSheet.Cells[i, j].Value = "";
                                    break;
                                }
                        }
                        j++;
                    }
                    i++;
                }
                package.Save();
            }
            stream.Position = 0;
            string fileNameExport = $"Export-{DateTime.Now.ToString()}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileNameExport);
        }
    }
}