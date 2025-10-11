using Demo.Webapi.BLayer.BaseBL;
using Demo.Webapi.Common.Entites;
using Demo.Webapi.Common.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.BLayer
{
    public interface IEmployeeBL : IBaseBL<Employee>
    {
        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns></returns>
        public string GetNewEmployeeCode();

        public int DeleteMultipleRecords(string idList);
    }
}
