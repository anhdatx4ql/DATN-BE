using Demo.Webapi.Common.Entities;
using Demo.Webapi.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using static Dapper.SqlMapper;

namespace Demo.Webapi.DL.BaseDL
{
    public interface IBaseDL<T>
    {
        /// <summary>
        /// Khởi tạo kết nối tới db
        /// </summary>
        /// <returns></returns>
        /// Xuân Đào (28/03/2023)
        public IDbConnection GetOpenConnection();

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
        public IEnumerable<dynamic> Query(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);

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
        public T QueryFirstOrDefault(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

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
        public int Execute(IDbConnection cnn, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        public GridReader QueryMultiple(IDbConnection cnn, string sql, object? param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        /// <summary>
        /// Filter Record
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        /// Xuân Đào (28/03/2023)
        public ServiceResult FilterRecord(string? keyWord, int? pageSize, int? pageNumber);

        /// <summary>
        /// Tìm kiếm bản ghi theo id
        /// </summary>
        /// <param name="id">id bản ghi cần tìm</param>
        /// <returns>Generic</returns>
        /// Xuân Đào (28/03/2023)
        public ServiceResult GetRecordById(Guid id);

        /// <summary>
        /// Lấy toàn bộ record
        /// </summary>
        /// <returns>IEnumerable</returns>
        /// Xuân Đào (28/03/2023)
        public IEnumerable<dynamic> GetRecords();

        /// <summary>
        /// Xóa một bản ghi theo id
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Xuân Đào (28/03/2023)
        public int DeleteRecord(Guid recordId);

        public int MultipleDeleteRecord(string recordIds);

        /// <summary>
        /// Tạo mới bản ghi
        /// </summary>
        /// <param name="record">Thông tin bản ghi</param>
        /// <returns>Service result</returns>
        /// Xuân Đào (28/03/2023)
        public ServiceResult CreateRecord(T record);

        /// <summary>
        /// Cập nhật thông tin bản ghi
        /// </summary>
        /// <param name="record">Thông tin bản ghi</param>
        /// <returns>Service result</returns>
        /// Xuân Đào (28/03/2023)
        public ServiceResult UpdateRecord(Guid id, T record);

        public ServiceResult BulkCreate(IEnumerable<T> RecordList);

    }
}
