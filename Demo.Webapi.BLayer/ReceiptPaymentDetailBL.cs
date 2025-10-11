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
    public class ReceiptPaymentDetailBL : BaseBL<receipt_payment_detail>, IReceiptPaymentDetailBL
    {
        private IReceiptPaymentDetailDL _receiptPaymentDetailDL;

        public ReceiptPaymentDetailBL(IReceiptPaymentDetailDL receiptPaymentDetailDL) : base(receiptPaymentDetailDL)
        {
            this._receiptPaymentDetailDL = receiptPaymentDetailDL;
        }

        /// <summary>
        /// Hàm lấy danh sách chi tiết chứng từ theo id chứng từ
        /// </summary>
        /// <param name="re_id">id chứng từ</param>
        /// <author>Xuân Đào 01/05/2023</author>
        /// <returns></returns>
        public ServiceResult GetAllRPDByReId(Guid re_id)
        {
            return _receiptPaymentDetailDL.GetAllRPDByReId(re_id);
        }

        /// <summary>
        /// Hàm thêm hàng loạt chi tiết chứng từ
        /// </summary>
        /// <param name="ReceiptPaymentDetails">Danh sách chi tiết chứng từ</param>
        /// <author>Xuân Đào 01/05/2023</author>
        /// <returns></returns>
        public int BulkInsert(IEnumerable<receipt_payment_detail> ReceiptPaymentDetails)
        {
            return _receiptPaymentDetailDL.BulkInsert(ReceiptPaymentDetails);
        }

        /// <summary>
        /// Hàm cập nhật hàng loạt chi tiết chứng từ
        /// </summary>
        /// <param name="records">Danh sách chi tiết chứng từ</param>
        /// <author>Xuân Đào 01/05/2023</author>
        /// <returns></returns>
        public ServiceResult UpdateMultiple(Guid re_id, IEnumerable<receipt_payment_detail> records)
        {
            return _receiptPaymentDetailDL.UpdateMultiple(re_id, records);
        }
    }
}
