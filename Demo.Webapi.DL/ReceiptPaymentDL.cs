using Dapper;
using Demo.Webapi.Common.Entities;
using Demo.Webapi.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Demo.Webapi.Common.Entities.DTO;
using Npgsql;
using Demo.Webapi.Common;
using System.Data;

namespace Demo.Webapi.DL
{
    public class ReceiptPaymentDL : BaseDL<receipt_payment>, IReceiptPaymentDL
    {
        /// <summary>
        /// Hàm lấy số chứng từ mới nhất
        /// </summary>
        /// <author>Xuân Đào 04/05/2023</author>
        /// <returns></returns>
        public string GetNewPaymentCode()
        {
            string queryString = "select rp.re_ref_no from receipt_payment rp order by rp.re_ref_no desc, length(rp.re_ref_no) asc";
            var sqlConnection = GetOpenConnection();
            var lastestCode = sqlConnection.QueryFirstOrDefault(queryString);
            string code = lastestCode.re_ref_no;
            Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
            Match result = re.Match(code);

            string alphaPart = result.Groups[1].Value;
            string numberPart = result.Groups[2].Value;
            int codeNum = Int32.Parse(numberPart);
            string newCode = $"{alphaPart}{++codeNum}";
            sqlConnection.Close();
            return newCode;
        }

        /// <summary>
        /// Hàm xóa hết chứng từ kèm detail
        /// </summary>
        /// <param name="re_id">id chứng từ</param>
        /// <author>Xuân Đào 04/05/2023</author>
        /// <returns></returns>
        public int DeleteFullPayment(string re_id)
        {
            try
            {
                string deleteMaster = $"delete from receipt_payment where re_id = '{re_id}';";
                string deleteDetail = $"delete from receipt_payment_detail where rp_id = '{re_id}';";
                var sqlConnection = GetOpenConnection();
                var result = QueryMultiple(sqlConnection, deleteMaster + deleteDetail, commandType: System.Data.CommandType.Text);
                if (result == null)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
                //using (var transaction = sqlConnection.BeginTransaction())
                //{
                //    try
                //    {
                //        var result = QueryMultiple(sqlConnection, deleteMaster + deleteDetail, commandType: System.Data.CommandType.Text);
                //        if (result == null)
                //        {
                //            transaction.Rollback();
                //            return -1;
                //        }
                //        else
                //        {
                //            transaction.Commit();
                //            return 1;
                //        }
                //    }
                //    catch (Exception e)
                //    {
                //        transaction.Rollback();
                //        Console.WriteLine(e.Message);
                //        return -1;
                //    }
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        /// <summary>
        /// Hàm lấy chứng từ theo keyword
        /// </summary>
        /// <param name="keyword">Từ khóa</param>
        /// <author>Xuân Đào 05/05/2023</author>
        /// <returns></returns>
        public IEnumerable<dynamic>? GetAllWithKeyword(string? keyword)
        {
            string queryString = "Select *, (select sum(rpd.amount) as \"amount\" from receipt_payment_detail rpd where rpd.rp_id = re_id) from receipt_payment left join supplier on receipt_payment.account_id = supplier.supplier_id";

            if (keyword != null && keyword.Length > 0)
            {
                queryString += $" where cast (ca_date as text) ilike '%{keyword}%' or cast (re_date as text) ilike '%{keyword}%' or cast (re_ref_no as text) ilike '%{keyword}%'  or cast (re_description as text) ilike '%{keyword}%' " +
                            $" or cast (supplier.supplier_name as text) ilike '%{keyword}%' or cast (supplier.supplier_code as text) ilike '%{keyword}%' or cast (ca_type as text) ilike '%{keyword}%' or cast (re_reason as text) ilike '%{keyword}%'";
            }
            queryString += "  order by re_ref_no desc;";

            try
            {
                var sqlConnection = GetOpenConnection();
                var result = sqlConnection.Query(queryString, commandType: System.Data.CommandType.Text);
                sqlConnection.Close();
                return result;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Hàm xóa hàng loạt chứng từ kèm chi tiết
        /// </summary>
        /// <param name="ids">danh sách id chứng từ</param>
        /// <author>Xuân Đào 05/05/2023</author>
        /// <returns></returns>
        public int DeleteFullMultiple(string[]? ids)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=password;Database=postgres;";
            int masterEffected = 0;
            int detailEffected = 0;
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                // Construct the SQL query with a parameter for the record IDs
                string sql = $"DELETE FROM receipt_payment WHERE cast(re_id as text) = ANY(:ids)";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    // Add the recordIds array as a parameter
                    command.Parameters.AddWithValue("ids", ids);

                    // Execute the query
                    masterEffected = command.ExecuteNonQuery();
                }
            }

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                // Construct the SQL query with a parameter for the record IDs
                string sql = $"DELETE FROM receipt_payment_detail WHERE cast(rp_id as text) = ANY(:ids)";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    // Add the recordIds array as a parameter
                    command.Parameters.AddWithValue("ids", ids);

                    // Execute the query
                    detailEffected = command.ExecuteNonQuery();
                }
            }

            return masterEffected + detailEffected;
            //foreach (var id in ids)
            //{

            //}
        }

        /// <summary>
        /// Hàm cập nhật hàng loạt chứng từ
        /// </summary>
        /// <param name="rp">Chứng từ mới</param>
        /// <param name="rpds">Danh sách detail</param>
        /// <author>Xuân Đào - 03/05/2023</author>
        /// <returns></returns>
        public ServiceResult UpdateFullPayment(receipt_payment record, receipt_payment_detail[]? rpds)
        {
            string excuteString = $"update receipt_payment set ";
            var properties = typeof(receipt_payment).GetProperties();
            int i = 0;
            foreach (var property in properties)
            {
                var value = property.GetValue(record);
                var propName = property.Name;
                if (i == 1)
                {
                    if (value == null)
                    {
                        excuteString += $"{propName}" + "=" + "null";
                    }
                    else
                    {
                        if (property.PropertyType.Name.CompareTo("Int32") != 0)
                            excuteString += $"{propName}" + "=" + $"'{value}'";
                        else
                            excuteString += propName + "=" + value;
                    }
                }
                else if (i > 1)
                {
                    if ((value == null || value.ToString().Length == 0) && propName != "Created_Date" && propName != "Created_By" && propName != "CreatedBy" && propName != "ModifiedDate" && propName != "modified_date")
                    {
                        excuteString += $",{propName}" + "=" + "null";
                    }
                    else
                    {
                        if (property.PropertyType.Name.CompareTo("Int32") != 0)
                        {
                            if (propName == "modified_date")
                            {
                                excuteString += "," + $"{propName}" + "=" + $"'{new DateTime()}'";
                            }
                            else if (propName != "Created_Date" && propName != "Created_By" && propName != "CreatedBy")
                            {
                                excuteString += "," + $"{propName}" + "=" + $"'{value}'";
                            }
                        }
                        else
                            excuteString += "," + $"{propName}" + "=" + value;
                    }
                }
                i++;
            }
            excuteString += $" where {properties[0].Name} = '{record.re_id}';";
            var mysqlConnection = GetOpenConnection();
            var result = Execute(mysqlConnection, excuteString, commandType: CommandType.Text);
            mysqlConnection.Close();
            if (result == 0)
            {
                return null;
            }
            else
            {
                return new ServiceResult()
                {
                    IsSuccess = true,
                    Message = Resource.updateSuccess,
                    Data = result,
                };
            }
        }
    }
}
