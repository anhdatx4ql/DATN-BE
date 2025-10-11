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
    public class AccountBL : BaseBL<Account>, IAccountBL
    {
        IAccountDL _accountDL;

        public AccountBL(IBaseDL<Account> baseDL, IAccountDL accountDL) : base(baseDL)
        {
            _accountDL = accountDL;
        }

        /// <summary>
        /// Cập nhật trạng thái cho danh sách tài khoản
        /// </summary>
        /// <param name="ids">danh sách id</param>
        /// <param name="newStatus">trạng thái mới</param>
        /// <author>Xuân Đào 27/04/2023</author>
        /// <returns></returns>
        public int UpdateMultipleStatus(string ids, int newStatus)
        {
            return _accountDL.UpdateMultipleStatus(ids, newStatus);
        }

        /// <summary>
        /// Cập nhật level tài khoản
        /// </summary>
        /// <param name="accounts">danh sách tài khoản</param>
        /// <author>Xuân Đào 27/04/2023</author>
        /// <returns></returns>
        public int UpdateAccountLevel(List<Account> accounts)
        {
            return _accountDL.UpdateAccountLevel(accounts);
        }

        public object Login(string username, string password)
        {
            return _accountDL.Login(username, password);
        }
    }
}
