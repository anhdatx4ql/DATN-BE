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
    public interface IReceiptPaymentDetailDL : IBaseDL<receipt_payment_detail>
    {
        /// <summary>
        /// Hàm lấy danh sách chi tiết theo id chứng từ
        /// </summary>
        /// <param name="re_id"></param>
        /// <author>Xuân Đào 12/05/2023</author>
        /// <returns></returns>
        public ServiceResult GetAllRPDByReId(Guid re_id);

        /// <summary>
        /// Hàm thêm hàng loạt chi tiết chứng từ
        /// </summary>
        /// <param name="ReceiptPaymentDetails">Danh sách chi tiết chứng từ</param>
        /// <author>Xuân Đào 12/05/2023</author>
        /// <returns></returns>
        public int BulkInsert(IEnumerable<receipt_payment_detail> ReceiptPaymentDetails);

        /// <summary>
        /// Hàm cập nhật hàng loạt chi tiết chứng từ
        /// </summary>
        /// <param name="re_id">id chứng từ</param>
        /// <param name="records">danh sách chi tiết chứng từ</param>
        /// <author>Xuân Đào 12/05/2023</author>
        /// <returns></returns>
        public ServiceResult UpdateMultiple(Guid re_id, IEnumerable<receipt_payment_detail> records);
    }
}
