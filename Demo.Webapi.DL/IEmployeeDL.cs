using Demo.Webapi.Common.Entites;
using Demo.Webapi.Common.Entities;
using Demo.Webapi.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.DL
{
    public interface IEmployeeDL : IBaseDL<Employee>
    {
        /// <summary>
        /// Hàm lấy mã nhân viên mới
        /// </summary>
        /// <returns>EmployeeCode</returns>
        /// Xuân Đào (26/03/2023)
        public string GetNewEmployeeCode();

        /// <summary>
        /// Hàm xóa hàng loạt
        /// </summary>
        /// <returns>Số bản ghi bị xóa</returns>
        /// Xuân Đào (26/03/2023)
        public int DeleteMultipleRecords(string idList);
    }
}
