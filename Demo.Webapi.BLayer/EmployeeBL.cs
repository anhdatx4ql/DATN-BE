using Demo.Webapi.BLayer.BaseBL;
using Demo.Webapi.Common;
using Demo.Webapi.Common.Entites;
using Demo.Webapi.Common.Entites.DTO;
using Demo.Webapi.Common.Entities;
using Demo.Webapi.Common.Entities.DTO;
using Demo.Webapi.Common.Enums;
using Demo.Webapi.DL;
using Demo.Webapi.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.BLayer
{
    public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
    {
        private IEmployeeDL _employeeDL;

        #region Constructor

        public EmployeeBL(IEmployeeDL employeeDL) : base(employeeDL)
        {
            this._employeeDL = employeeDL;
        }

        public int DeleteMultipleRecords(string idList)
        {
            return _employeeDL.DeleteMultipleRecords(idList);
        }

        #endregion

        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns></returns>
        public string GetNewEmployeeCode()
        {
            return _employeeDL.GetNewEmployeeCode();
        }

        public override List<ErrorResult> ValidateCustomize(Employee record)
        {
            List<ErrorResult> errList = new List<ErrorResult>();
            string code = record.EmployeeCode;
            if (!CodeIsValid(code))
            {
                errList.Add(new ErrorResult(ErrorCode.Duplicate, Resource.devMsg_DuplicateEmployee, Resource.userMsg_DuplicateEmployee, ""));
            }
            return errList;
        }
    }
}
