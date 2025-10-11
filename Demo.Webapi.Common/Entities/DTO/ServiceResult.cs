using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.Common.Entities.DTO
{
    public class ServiceResult
    {
        #region Properties
        /// <summary>
        /// Trạng thái
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Thông báo
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Dữ liệu
        /// </summary>
        public object Data { get; set; }

        #endregion
    }
}
