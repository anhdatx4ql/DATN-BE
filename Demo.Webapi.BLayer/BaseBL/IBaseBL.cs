using Demo.Webapi.Common.Entities;
using Demo.Webapi.Common.Entities.DTO;
using Demo.Webapi.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.BLayer.BaseBL
{
    public interface IBaseBL<T>
    {
        /// <summary>
        /// Lọc bản ghi theo điều kiện
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        /// Xuân Đào (25/03/2023)
        public ServiceResult FilterRecord(string? keyWord, int? pageSize, int? pageNumber);

        /// <summary>
        /// Tìm kiếm bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Xuân Đào (25/03/2023)
        public ServiceResult GetRecordById(Guid id);

        /// <summary>
        /// Lấy toàn bộ bản ghi
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetRecords();

        /// <summary>
        /// Xóa 1 bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteRecord(Guid id);

        public int MultipleDeleteRecord(string recordIds);

        /// <summary>
        /// Tạo mới một bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public ServiceResult CreateRecord(T record);

        /// <summary>
        /// Cập nhật thông tin một bản ghi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        public ServiceResult UpdateRecord(Guid id, T record);

        public ServiceResult BulkCreate(IEnumerable<T> records);

    }
}
