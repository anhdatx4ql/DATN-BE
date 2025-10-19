using Dapper;
using Demo.Webapi.Common;
using Demo.Webapi.Common.Entites;
using Demo.Webapi.Common.Entites.DTO;
using Demo.Webapi.Common.Entities;
using Demo.Webapi.Common.Enums;
using Demo.Webapi.DL.BaseDL;
using Microsoft.AspNetCore.Http;
using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;

namespace Demo.Webapi.DL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {
        /// <summary>
        /// Hàm lấy mã nhân viên mới
        /// </summary>
        /// <returns>EmployeeCode</returns>
        /// pvdat (26/03/2023)
        public string GetNewEmployeeCode()
        {
            using (var mysqlConnection = GetOpenConnection())
            {
                string sql = "SELECT get_latest_employee_code();";

                // Cast IDbConnection sang NpgsqlConnection
                using var command = new Npgsql.NpgsqlCommand(sql, (Npgsql.NpgsqlConnection)mysqlConnection);

                var result = command.ExecuteScalar();
                var newEmployeeCode = GetNextCode(result?.ToString());
                return newEmployeeCode;
            }
        }

        /// <summary>
        /// Tăng mã với định dạng linh hoạt, chỉ tăng phần số cuối cùng
        /// Ví dụ: NV-1-123-4 -> NV-1-123-5, ABC-42 -> ABC-43
        /// </summary>
        private string GetNextCode(string currentCode)
        {
            if (string.IsNullOrWhiteSpace(currentCode))
                throw new ArgumentException("currentCode is null or empty");

            // Tìm cụm số cuối cùng trong mã
            int endPos = currentCode.Length;
            int startPos = endPos - 1;

            // Tìm vị trí bắt đầu của cụm số cuối cùng
            while (startPos >= 0 && char.IsDigit(currentCode[startPos]))
                startPos--;

            startPos++; // Điều chỉnh vị trí bắt đầu của cụm số

            // Trích xuất phần prefix và phần số
            string prefix = currentCode.Substring(0, startPos);
            string numberPart = currentCode.Substring(startPos);

            // Chuyển đổi và tăng giá trị số
            if (!int.TryParse(numberPart, out int number))
                throw new FormatException($"Không thể chuyển '{numberPart}' thành số"); number++; 
            
            // Giữ nguyên số chữ số ban đầu (padding)
            string nextNumberPart = number.ToString().PadLeft(numberPart.Length, '0'); 
            // Ghép lại
            return$"{prefix}{nextNumberPart}";
        }

        /// <summary>
        /// Hàm xóa hàng loạt bản ghi
        /// </summary>
        /// <param name="idList"> danh sách id các bản ghi cần xóa </param>
        /// <returns>Số bản ghi xóa thành công</returns>
        public int DeleteMultipleRecords(string idList)
        {
            using (var mysqlConnection = GetOpenConnection())
            {
                using(var transaction  = mysqlConnection.BeginTransaction())
                {
                    try
                    {
                        string[] list = idList.Split(',');
                        string procName = "Proc_MultipleDeleteEmployee";
                        var mySqlConnection = GetOpenConnection();
                        string param = "";
                        for(int i = 0; i < list.Length; i++)
                        {
                            if (i==0) param += $"'{list[i]}'";
                            else param += $", '{list[i]}'";
                        }
                        var parameters = new DynamicParameters();
                        parameters.Add("listOfRecordId", param);
                        int result = Execute(mySqlConnection, procName, parameters, commandType: CommandType.StoredProcedure);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return -1;
                    }
                }
            }
        }
    }
}
