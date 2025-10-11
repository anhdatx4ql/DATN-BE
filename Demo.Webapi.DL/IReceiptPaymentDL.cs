using Demo.Webapi.Common.Entities;
using Demo.Webapi.Common.Entities.DTO;
using Demo.Webapi.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.DL
{
    public interface IReceiptPaymentDL : IBaseDL<receipt_payment>
    {
        /// <summary>
        /// Hàm lấy số chứng từ mới nhất
        /// </summary>
        /// <author>Xuân Đào 04/05/2023</author>
        /// <returns></returns>
        public string GetNewPaymentCode();

        /// <summary>
        /// Hàm xóa hết chứng từ kèm detail
        /// </summary>
        /// <param name="re_id">id chứng từ</param>
        /// <author>Xuân Đào 04/05/2023</author>
        /// <returns></returns>
        public int DeleteFullPayment(string re_id);

        /// <summary>
        /// Hàm lấy chứng từ theo keyword
        /// </summary>
        /// <param name="keyword">Từ khóa</param>
        /// <author>Xuân Đào 05/05/2023</author>
        /// <returns></returns>
        public IEnumerable<dynamic>? GetAllWithKeyword(string? keyword);

        /// <summary>
        /// Hàm xóa hàng loạt chứng từ kèm chi tiết
        /// </summary>
        /// <param name="ids">danh sách id chứng từ</param>
        /// <author>Xuân Đào 05/05/2023</author>
        /// <returns></returns>
        public int DeleteFullMultiple(string[]? ids);

        /// <summary>
        /// Hàm cập nhật hàng loạt chứng từ
        /// </summary>
        /// <param name="rp">Chứng từ mới</param>
        /// <param name="rpds">Danh sách detail</param>
        /// <author>Xuân Đào - 03/05/2023</author>
        /// <returns></returns>
        public ServiceResult UpdateFullPayment(receipt_payment rp, receipt_payment_detail[]? rpds);
    }
}
