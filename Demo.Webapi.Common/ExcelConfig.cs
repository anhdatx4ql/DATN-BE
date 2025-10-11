using Demo.Webapi.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.Common
{
    public static class ExcelConfig
    {
        /// <summary>
        /// Lấy danh sách tên tài khoản
        /// </summary>
        /// <returns></returns>
        public static List<string> getAccountColName()
        {
            List<string> list = new List<string>();
            list.Add("Số tài khoản");
            list.Add("Tên tài khoản");
            list.Add("Tính chất");
            list.Add("Tên tiếng anh");
            list.Add("Diễn giải");
            list.Add("Trạng thái");
            return list;
        }

        /// <summary>
        /// Lấy danh sách các trường tài khoản tương ứng
        /// </summary>
        /// <returns></returns>
        public static List<string> getAccountModelName()
        {
            List<string> list = new List<string>();
            list.Add("AccountNumber");
            list.Add("AccountName");
            list.Add("Property");
            list.Add("EnglishName");
            list.Add("Description");
            list.Add("Status");
            return list;
        }

        /// <summary>
        /// Lấy danh sách tính chất
        /// </summary>
        /// <returns></returns>
        public static List<String> AccountPropertyValue()
        {
            List<string> list = new List<string>();
            list.Add("Dư nợ");
            list.Add("Dư có");
            list.Add("Lưỡng tính");
            list.Add("Không có số dư");
            return list;
        }

        /// <summary>
        /// Lấy danh sách trạng thái
        /// </summary>
        /// <returns></returns>
        public static List<String> AccountStatusValue()
        {
            List<string> list = new List<string>();
            list.Add("Đang sử dụng");
            list.Add("Ngưng sử dụng");
            return list;
        }

        /// <summary>
        /// Lấy danh sách chứng từ
        /// </summary>
        /// <returns></returns>
        public static List<String> GetPaymentColName()
        {
            List<String> list = new List<string>();
            list.Add("STT");
            list.Add("Ngày hạch toán");
            list.Add("Ngày chứng từ");
            list.Add("Số chứng từ");
            list.Add("Diễn giải");
            list.Add("Số tiền");
            list.Add("Mã đối tượng");
            list.Add("Đối tượng");
            list.Add("Lý do thu/chi");
            list.Add("Loại chứng từ");
            return list;
        }

        /// <summary>
        /// Lấy tên cột chứng từ
        /// </summary>
        /// <returns></returns>
        public static List<String> GetPaymentModelName()
        {
            List<String> list = new List<string>();
            list.Add("num");
            list.Add("re_date");
            list.Add("ca_date");
            list.Add("re_ref_no");
            list.Add("re_description");
            list.Add("amount");
            list.Add("supplier_code");
            list.Add("supplier_name");
            list.Add("re_reason");
            list.Add("ca_type");
            return list;
        }
    }
}
