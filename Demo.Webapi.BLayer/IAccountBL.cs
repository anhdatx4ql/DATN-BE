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
    public interface IAccountBL : IBaseBL<Account>
    {
        /// <summary>
        /// Cập nhật trạng thái cho danh sách tài khoản
        /// </summary>
        /// <param name="ids">danh sách id</param>
        /// <param name="newStatus">trạng thái mới</param>
        /// <author>Xuân Đào 27/04/2023</author>
        /// <returns></returns>
        public int UpdateMultipleStatus(string ids, int newStatus);

        /// <summary>
        /// Cập nhật level tài khoản
        /// </summary>
        /// <param name="accounts">danh sách tài khoản</param>
        /// <author>Xuân Đào 27/04/2023</author>
        /// <returns></returns>
        public int UpdateAccountLevel(List<Account> accounts);

        public object Login(string username, string password);
    }
}
