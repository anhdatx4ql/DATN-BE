using Dapper;
using Demo.Webapi.Common;
using Demo.Webapi.Common.Entities;
using Demo.Webapi.Common.Entities.DTO;
using Demo.Webapi.DL.BaseDL;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.DL
{
    public class ReceiptPaymentDetailDL : BaseDL<receipt_payment_detail>, IReceiptPaymentDetailDL
    {
        /// <summary>
        /// Hàm cập nhật hàng loạt chi tiết chứng từ
        /// </summary>
        /// <param name="re_id">id chứng từ</param>
        /// <param name="records">danh sách chi tiết chứng từ</param>
        /// <author>Xuân Đào 12/05/2023</author>
        /// <returns></returns>
        public ServiceResult UpdateMultiple(Guid re_id, IEnumerable<receipt_payment_detail> records)
        {
            // Xóa dữ liệu cũ
            string queryString = $"Delete from receipt_payment_detail where rp_id = '{re_id}'";
            var sqlConnection = GetOpenConnection();
            int rowEffected = sqlConnection.Execute(queryString, commandType: System.Data.CommandType.Text);
            // Thêm dữ liệu mới
            var result = BulkCreate(records);
            sqlConnection.Close();
            return result;
        }

        /// <summary>
        /// Hàm lấy danh sách chi tiết theo id chứng từ
        /// </summary>
        /// <param name="re_id"></param>
        /// <author>Xuân Đào 12/05/2023</author>
        /// <returns></returns>
        public ServiceResult GetAllRPDByReId(Guid re_id)
        {
            string queryString = $"Select * from receipt_payment_detail rpd left join receipt_payment rp on rpd.rp_id = rp.re_id left join " +
                $"supplier s on rpd.object_id = cast(s.supplier_id as text) where rpd.rp_id = '{re_id}';";
            string getSummary = $"select sum(rpd.amount) as \"Total amount\" from receipt_payment_detail rpd where rpd.rp_id = '{re_id}';";
            string getTotalRecord = $"select count(*) as \"Total record\" from receipt_payment_detail rpd where rpd.rp_id = '{re_id}';";
            var sqlConnection = GetOpenConnection();
            var result = sqlConnection.QueryMultiple(queryString + getSummary + getTotalRecord, commandType: System.Data.CommandType.Text);
            var data = result.Read();
            var summary = result.Read();
            var totalRecord = result.Read();
            sqlConnection.Close();
            return new ServiceResult
            {
                IsSuccess = true,
                Message = "",
                Data = new
                {
                    data,
                    summary,
                    totalRecord
                }
            };
        }

        /// <summary>
        /// Hàm thêm hàng loạt chi tiết chứng từ
        /// </summary>
        /// <param name="ReceiptPaymentDetails">Danh sách chi tiết chứng từ</param>
        /// <author>Xuân Đào 12/05/2023</author>
        /// <returns></returns>
        public int BulkInsert(IEnumerable<receipt_payment_detail> ReceiptPaymentDetails)
        {
            using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=password;Database=postgres"))
            {
                connection.Open();

                var param = ReceiptPaymentDetails.Select(p => new
                {
                    rpd_id = p.rpd_id,
                    rp_id = p.rp_id,
                    rpd_description = p.rpd_description,
                    debit_account = p.debit_account,
                    credit_account = p.credit_account,
                    amount = p.amount,
                    object_id = p.object_id,
                    created_date = p.created_date,
                    created_by = p.created_by,
                    modified_date = p.modified_date,
                    modified_by = p.modified_by
                });
                connection.Execute("INSERT INTO receipt_payment_detail (rpd_id, rp_id, rpd_description, debit_account, credit_account, amount, object_id, created_date, created_by," +
                    "modified_date, modified_by) VALUES (@rpd_id, @rp_id, @rpd_description, @debit_account, @credit_account, @amount, @object_id, @created_date, @created_by," +
                    "@modified_date, @modified_by)", param);
                return ReceiptPaymentDetails.Count();
            }
        }
    }
}
