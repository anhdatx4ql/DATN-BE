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
    public interface IAccountDL : IBaseDL<Account>
    {
        /// <summary>
        /// Hàm cập nhật trạng thái tài khoản
        /// </summary>
        /// <param name="ids">Danh sách id tài khoản</param>
        /// <param name="newStatus">Trạng thái mới</param>
        /// <author>Xuân Đào 10/05/2023</author>
        /// <returns></returns>
        public int UpdateMultipleStatus(string ids, int newStatus);

        /// <summary>
        /// Hàm cập nhật cấp độ tài khoản
        /// </summary>
        /// <param name="accounts">Danh sách tài khoản</param>
        /// <author>Xuân Đào 10/05/2023</author>
        /// <returns></returns>
        public int UpdateAccountLevel(List<Account> accounts);

        public object Login(string username, string password);
    }
}
