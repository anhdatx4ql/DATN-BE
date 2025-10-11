using Dapper;
using Demo.Webapi.Common;
using Demo.Webapi.Common.Entites.DTO;
using Demo.Webapi.Common.Entities;
using Demo.Webapi.Common.Entities.DTO;
using Demo.Webapi.Common.Enums;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Npgsql;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Demo.Webapi.DL.BaseDL
{
    public class BaseDL<T> : IBaseDL<T>
    {
        /// <summary>
        /// Hàm thêm mới bản ghi
        /// </summary>
        /// <param name="record">Bản ghi</param>
        /// <author>Xuân Đào - 04/05/2023</author>
        /// <returns></returns>
        public ServiceResult CreateRecord(T record)
        {
            try
            {
                string queryString = $"insert into {typeof(T).Name}";
                string colName = "(";
                string colValue = "(";
                string newId = Guid.NewGuid().ToString();
                var properties = typeof(T).GetProperties();
                int i = 0;
                foreach (var property in properties)
                {
                    var value = property.GetValue(record);
                    if (property.PropertyType.Name == "Nullable`1" && property.PropertyType.FullName.Contains("DateTime"))
                    {
                        if (property.Name == "created_date")
                        {
                            value = DateTime.Now;
                        }
                        if (value != null)
                        {
                            DateTime dateTime = (DateTime)value;
                            string day = dateTime.Day >= 10 ? dateTime.Day.ToString() : "0"+ dateTime.Day.ToString();
                            string month = dateTime.Month >= 10 ? dateTime.Month.ToString() : "0"+ dateTime.Month.ToString();
                            string year = dateTime.Year.ToString();
                            value = $"{year}/{month}/{day}";
                        }
                    }
                    if (i != properties.Length - 1)
                    {
                        colName += property.Name + ',';
                        if (i == 0)
                        {
                            colValue += $"'{newId}',";
                        }
                        else
                        {
                            if (value == null)
                            {
                                colValue += "null" + ',';
                            } 
                            else
                            {
                                if (property.PropertyType.Name.CompareTo("Int32") != 0)
                                    colValue += $"'{value}'" + ',';
                                else 
                                    colValue += $"{value}" + ',';
                            }
                        }
                    } 
                    else
                    {
                        colName += property.Name + ')';
                        if (value == null)
                        {
                            colValue += "null" + ')';
                        }
                        else
                        {
                            if (property.PropertyType.Name.CompareTo("Int32") != 0)
                                colValue += $"'{value}'" + ')';
                            else
                                colValue += $"{value})";
                        }
                    }
                    i++;
                }

                queryString += colName + " values " + colValue + ';';
                var sqlConnection = GetOpenConnection();
                int result = sqlConnection.Execute(queryString, commandType: CommandType.Text);
                sqlConnection.Close();
                return new ServiceResult
                {
                    IsSuccess = true,
                    Message = Resource.createSuccess,
                    Data = newId,
                };
            
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ServiceResult
                {
                    IsSuccess = false,
                    Message = Resource.createFail,
                    Data = ex.ToString(),
                };
            }
        }

        /// <summary>
        /// Hàm thêm mới hàng loạt bản ghi
        /// </summary>
        /// <param name="RecordList">Danh sách bản ghi</param>
        /// <author>Xuân Đào - 04/05/2023</author>
        /// <returns></returns>
        public ServiceResult BulkCreate(IEnumerable<T> RecordList)
        {
            try
            {
                string queryString = $"insert into {typeof(T).Name}";
                string colName = "(";
                string colValue = "";
                var properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    if (i != properties.Length - 1)
                    {
                        colName += properties[i].Name + ',';
                    }
                    else
                    {
                        colName += properties[i].Name + ')';
                    }
                }
                for (int i = 0; i < RecordList.Count(); i++)
                {
                    string colVal = "";
                    for (int j = 0; j < properties.Length; j++)
                    {
                        if (j == 0)
                        {
                            string id = Guid.NewGuid().ToString();
                            colVal += "('" + id + "',";
                        }
                        else if (j != properties.Length - 1)
                        {
                            if (properties[j].GetValue(RecordList.ElementAt(i)) == null)
                                colVal += "null,";
                            else
                            {
                                if (properties[j].PropertyType.Name.CompareTo("Int32") != 0)
                                    colVal += $"'{properties[j].GetValue(RecordList.ElementAt(i)).ToString()}'" + ',';
                                else
                                    colVal += properties[j].GetValue(RecordList.ElementAt(i)).ToString() + ',';
                            }
                        }
                        else
                        {
                            if (properties[j].GetValue(RecordList.ElementAt(i)) == null)
                                colVal += "null" + ')';
                            else
                            {
                                if (properties[j].PropertyType.Name.CompareTo("Int32") != 0) 
                                    colVal += $"'{properties[j].GetValue(RecordList.ElementAt(i)).ToString()}'" + ')';
                                else
                                    colVal += properties[j].GetValue(RecordList.ElementAt(i)).ToString() + ')';
                            }
                        }
                    }
                    if (i != 0)
                    {
                        colValue += ',' + colVal;
                    }
                    else
                    {
                        colValue += colVal;
                    }
                }
                queryString += colName + " values " + colValue + ';';
                var sqlConnection = GetOpenConnection();
                int result = sqlConnection.Execute(queryString, commandType: CommandType.Text);
                sqlConnection.Close();
                return new ServiceResult
                {
                    IsSuccess = true,
                    Message = Resource.createSuccess,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ServiceResult
                {
                    IsSuccess = false,
                    Message = Resource.createFail,
                    Data = ex.ToString(),
                };
            }
        }

        /// <summary>
        /// Xóa một bản ghi theo id
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Xuân Đào (28/03/2023)
        public int DeleteRecord(Guid recordId)
        {
            try
            {
                // Chuẩn bị query string
                string queryString = $"Delete from {typeof(T).Name} where {typeof(T).Name}ID = '{recordId}'";
                // Kết nối tới db
                var mySqlConection = GetOpenConnection();
                // Thực hiện query
                int rowEffected = Execute(mySqlConection, queryString, commandType: System.Data.CommandType.Text);

                mySqlConection.Close();
                return rowEffected;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }

        /// <summary>
        /// Hàm xóa hàng loạt bản ghi
        /// </summary>
        /// <param name="recordIds">Danh sách id bản ghi</param>
        /// <author>Xuân Đào 11/05/2023</author>
        /// <returns></returns>
        public int MultipleDeleteRecord(string recordIds)
        {
            using (var mysqlConnection = GetOpenConnection())
            {
                using (var transaction = mysqlConnection.BeginTransaction())
                {
                    try
                    {
                        string[] list = recordIds.Split(',');
                        string funcName = $"func_delete_{typeof(T).Name}";
                        var mySqlConnection = GetOpenConnection();
                        string param = "";
                        for (int i = 0; i < list.Length; i++)
                        {
                            if (i == 0) param += $"'{list[i]}'";
                            else param += $", '{list[i]}'";
                        }
                        string queryString = $"Delete from {typeof(T).Name.ToLower()} where {typeof(T).Name.ToLower()}id = ANY(array[{param}])";
                        int result = Execute(mySqlConnection, queryString, commandType: CommandType.Text);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return -2;
                    }
                }
            }
        }

        /// <summary>
        /// Truy vấn trả về số bản ghi bị ảnh hưởng
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns> Số bản ghi bị ảnh hưởng </returns>
        /// Xuân Đào (28/03/2023)
        public int Execute(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return cnn.Execute(sql, param, transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// Filter Record
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        /// Xuân Đào (28/03/2023)
        public ServiceResult FilterRecord(string? keyWord, int? pageSize, int? pageNumber)
        {
            try
            {
                int? offset = (pageSize * pageNumber) - pageSize;
                keyWord ??= "";
                string selectOption = "*";
                string joinOption = "";
                string? optionalQuery = null;
                string orderOption = "created_date desc";
                string whereOption = "";
                string getTotalRecord = $"select count(*) as \"Total record\" from {typeof(T).Name};";
                if (keyWord != null && keyWord.Length > 0)
                {
                    var properties = typeof(T).GetProperties();
                    int i = 0;
                    foreach (var property in properties)
                    {
                        if (i == 0)
                            whereOption += $"where cast({typeof(T).Name}.{property.Name} as text) ilike '%{keyWord}%'";
                        else
                            whereOption += $" or cast({typeof(T).Name}.{property.Name} as text) ilike '%{keyWord}%'";
                        i++;
                    }
                    getTotalRecord = $"select count(*) as \"Total record\" from {typeof(T).Name} {whereOption};";
                }
                if (typeof(T).Name == "receipt_payment")
                {
                    selectOption = "*, (select sum(rpd.amount) as \"total_amount\" from receipt_payment_detail rpd where rpd.rp_id = re_id)";
                    joinOption = "left join supplier on account_id = supplier_id left join employee on cast(employee.employeeid as text) = receipt_payment.employee_id";
                    orderOption = "re_ref_no desc";
                    if (keyWord != null && keyWord.Length > 0)
                    {
                        whereOption = $"where cast (ca_date as text) ilike '%{keyWord}%' or cast (re_date as text) ilike '%{keyWord}%' or cast (re_ref_no as text) ilike '%{keyWord}%'  or cast (re_description as text) ilike '%{keyWord}%' " +
                            $" or cast (supplier.supplier_name as text) ilike '%{keyWord}%' or cast (supplier.supplier_code as text) ilike '%{keyWord}%' or cast (ca_type as text) ilike '%{keyWord}%' or cast (re_reason as text) ilike '%{keyWord}%'";
                    }
                    optionalQuery = "select sum(rpd.amount) from receipt_payment_detail rpd left join receipt_payment on rpd.rp_id = receipt_payment.re_id left join supplier on account_id = supplier_id " + whereOption + ";";
                    getTotalRecord = $"select count(*) as \"Total record\" from receipt_payment left join supplier on account_id = supplier_id {whereOption};";
                }
                if (typeof(T).Name == "Supplier")
                {
                    orderOption = "supplier_code desc";
                }
                string queryString = $"select {selectOption} from {typeof(T).Name.ToLower()} {joinOption} {whereOption} order by {orderOption} limit {pageSize} offset {offset};";
                var sqlConection = GetOpenConnection();
                // Thực hiện truy vấn
                string excuteQuery = queryString + getTotalRecord;
                if (optionalQuery != null) excuteQuery += optionalQuery;
                var resultSets = QueryMultiple(sqlConection, excuteQuery, commandType:CommandType.Text);
                // Kiểm tra kết quả trả về
                var data = resultSets.Read();
                var totalRecord = resultSets.Read();
                var optionResult = optionalQuery != null ? resultSets.Read() : null;
                sqlConection.Close();
                return new ServiceResult
                {
                    IsSuccess = true,
                    Message = "",
                    Data = new
                    {
                        data,
                        totalRecord,
                        optionResult,
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ServiceResult
                {
                    IsSuccess = false,
                    Data = new ErrorResult
                    {
                        ErrorCode = ErrorCode.Exception,
                        DevMsg = Resource.devMsg_Exception,
                        UserMsg = Resource.userMsg_Exception,
                        MoreInfo = "More info: "
                    },
                };
            }
        }

        /// <summary>
        /// Khởi tạo kết nối tới db
        /// </summary>
        /// <returns></returns>
        /// Xuân Đào (28/03/2023)
        public IDbConnection GetOpenConnection()
        {
            var mySqlConnection = new NpgsqlConnection("Host=ep-jolly-unit-97576361.ap-southeast-1.aws.neon.tech;Username=xuandao9876;Password=Qq8pD0UdnfiS;Database=neondb;");
            mySqlConnection.Open();
            return mySqlConnection;
        }


        /// <summary>
        /// Tìm kiếm bản ghi theo id
        /// </summary>
        /// <param name="id">id bản ghi cần tìm</param>
        /// <returns>Generic</returns>
        /// Xuân Đào (28/03/2023)
        public ServiceResult GetRecordById(Guid id)
        {

            // Chuẩn bị tên stored
            string idField = $"{typeof(T).Name}_id";
            string joinOption = "";
            string selectOption = "*";
            if (typeof(T).Name == "receipt_payment")
            {
                idField = "re_id";
                joinOption = "left join supplier on account_id = supplier_id left join employee on cast(employee.employeeid as text) = receipt_payment.employee_id";
                selectOption = "*, supplier.address";
            }
            string queryString = $"Select {selectOption} from {typeof(T).Name} {joinOption} where {idField} = '{id}'";
            // Kết nối tới db
            var mySqlConnection = GetOpenConnection();
            // Thực hiện gọi vào db chạy proc
            var record = Query(mySqlConnection, queryString, commandType: System.Data.CommandType.Text);
            // Xử lý kết quả trả về
            mySqlConnection.Close();
            return new ServiceResult
            {
                IsSuccess = true,
                Message = "",
                Data = record,
            };
        }

        /// <summary>
        /// Lấy toàn bộ record
        /// </summary>
        /// <returns>IEnumerable</returns>
        /// Xuân Đào (28/03/2023)
        public IEnumerable<dynamic> GetRecords()
        {
            try
            {
                // Chuẩn bị tên stored
                //string storedName = $"Proc_GetAll{typeof(T).Name}";
                string orderBy = "created_date";
                string orderSort = "desc";
                if (typeof(T).Name == "Account") { 
                    orderBy = "datalevel asc, accountnumber::text ";
                    orderSort = "asc";
                } 
                string queryString = $"select *, (select count(*) from {typeof(T).Name.ToLower()}) as \"Total record\" from {typeof(T).Name} order by {orderBy} {orderSort}";
                // Kết nối tới db
                var mySqlConnection = GetOpenConnection();
                // Thực hiện gọi vào db chạy proc
                var resultSets = Query(mySqlConnection, queryString, commandType: System.Data.CommandType.Text);
                // Xử lý kết quả trả về
                mySqlConnection.Close();
                if (resultSets == null)
                {
                    return null;
                }
                else
                {
                    return resultSets.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Thực hiện truy vấn
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns> IEnumerable </returns>
        /// Xuân Đào (28/03/2023)
        public IEnumerable<dynamic> Query(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            return cnn.Query(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        /// <summary>
        /// Truy vấn bản ghi đầu tiên
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns>Generic</returns>
        /// Xuân Đào (28/03/2023)
        public T QueryFirstOrDefault(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return cnn.QueryFirstOrDefault<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public GridReader QueryMultiple(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return cnn.QueryMultiple(sql, param, transaction, commandTimeout, commandType);
        }
        /// <summary>
        /// Cập nhật thông tin bản ghi
        /// </summary>
        /// <param name="record">Thông tin bản ghi</param>
        /// <returns>Service result</returns>
        /// Xuân Đào (28/03/2023)
        public ServiceResult UpdateRecord(Guid id, T record)
        {
            var properties = typeof(T).GetProperties();
            string excuteString = $"update {typeof(T).Name} set ";
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
                            if (propName == "ModifiedDate" || propName == "modified_date")
                            {
                                excuteString += "," + $"{propName}" + "=" + $"'{DateTime.Now}'";
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
            excuteString += $" where {properties[0].Name} = '{id}';";
            var mysqlConnection = GetOpenConnection();
            var result = Execute(mysqlConnection, excuteString, commandType: CommandType.Text);
            mysqlConnection.Close();
            if (result == 0)
            {
                return new ServiceResult { IsSuccess = false};
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

        /// <summary>
        /// Hàm lấy toàn bộ dữ liệu theo keyword phục vụ xuất excel
        /// </summary>
        /// <param name="keyword">Từ khóa</param>
        /// <author>Xuân Đào 01/05/2023</author>
        /// <returns></returns>
        public IEnumerable<dynamic> getAllByKeyword(string keyword)
        {
            // Chuẩn bị tên stored
            string storedName = $"Proc_Filter{typeof(T).Name}";

            int? offset = 0;
            var param = new DynamicParameters();
            param.Add("keyWord", keyword);
            param.Add("pageSize", 25);
            param.Add("offset", offset);
            // Kết nối tới db
            var mySqlConnection = GetOpenConnection();
            // Thực hiện query
            var result = Query(mySqlConnection, storedName, commandType: System.Data.CommandType.StoredProcedure);
            mySqlConnection.Close();
            return result;

        }
    }
}
