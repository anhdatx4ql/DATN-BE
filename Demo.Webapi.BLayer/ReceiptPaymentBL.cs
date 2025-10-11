using Demo.Webapi.BLayer.BaseBL;
using Demo.Webapi.Common.Entities;
using Demo.Webapi.Common.Entities.DTO;
using Demo.Webapi.DL;
using Demo.Webapi.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.BLayer
{
    public class ReceiptPaymentBL : BaseBL<receipt_payment>, IReceiptPaymentBL
    {
        private IReceiptPaymentDL _receiptPaymentDL;

        public ReceiptPaymentBL(IReceiptPaymentDL receiptPaymentDL) : base(receiptPaymentDL)
        {
            this._receiptPaymentDL = receiptPaymentDL;
        }

        /// <summary>
        /// Hàm lấy số chứng từ mới
        /// </summary>
        /// <author>Xuân Đào 29/04/2023</author>
        /// <returns></returns>
        public string GetNewPaymentCode()
        {
            return _receiptPaymentDL.GetNewPaymentCode();
        }

        /// <summary>
        /// Hàm xóa chứng từ theo số chứng từ
        /// </summary>
        /// <param name="re_id">Số chứng từ</param>
        /// <author>Xuân Đào 29/04/2023</author>
        /// <returns></returns>
        public int DeleteFullPayment(string re_id)
        {
            return _receiptPaymentDL.DeleteFullPayment(re_id);
        }

        /// <summary>
        /// Hàm lấy toàn bộ chứng từ theo keyword
        /// </summary>
        /// <param name="keyword">Từ khóa</param>
        /// <author>Xuân Đào 29/04/2023</author>
        /// <returns></returns>
        public IEnumerable<dynamic>? GetAllWithKeyword(string? keyword)
        {
            return _receiptPaymentDL.GetAllWithKeyword(keyword);
        }

        /// <summary>
        /// Hàm xóa hàng loạt chứng từ
        /// </summary>
        /// <param name="ids">Danh sách chứng từ</param>
        /// <author>Xuân Đào 29/04/2023</author>
        /// <returns></returns>
        public int DeleteFullMultiple(string[]? ids)
        {
            return _receiptPaymentDL.DeleteFullMultiple(ids);
        }

        /// <summary>
        /// Hàm cập nhật chứng từ
        /// </summary>
        /// <param name="rp">Chứng từ</param>
        /// <param name="rpds">Chi tiết chừng từ</param>
        /// <author>Xuân Đào 29/04/2023</author>
        /// <returns></returns>
        public ServiceResult UpdateFullPayment(receipt_payment rp, receipt_payment_detail[]? rpds)
        {
            return _receiptPaymentDL.UpdateFullPayment(rp, rpds);
        }
    }
}
