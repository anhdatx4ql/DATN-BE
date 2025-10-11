using Demo.Webapi.Common.Entites;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using Demo.Webapi.Common;
using Demo.Webapi.Common.Entites.DTO;
using System.Net.Http;
using Demo.Webapi.Common.Enums;
using Microsoft.AspNetCore.Http;
using Demo.Webapi.Common.Entities;
using Demo.Webapi.DL.BaseDL;

namespace Demo.Webapi.DL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {
        /// <summary>
        /// Hàm lấy mã nhân viên mới
        /// </summary>
        /// <returns>EmployeeCode</returns>
        /// Xuân Đào (26/03/2023)
        public string GetNewEmployeeCode()
        {
            var mysqlConnection = GetOpenConnection();
            string procName = "Proc_GetLastestEmployeeCode";
            string result = QueryFirstOrDefault(mysqlConnection, procName, commandType: CommandType.StoredProcedure).EmployeeCode;
            int code = Int32.Parse(result.Substring(3, result.Length - 3));
            return $"{result.Substring(0,3)}{code+1}";
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
