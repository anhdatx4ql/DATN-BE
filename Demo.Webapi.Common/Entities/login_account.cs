using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.Common.Entities
{
    public class login_account
    {
        /// <summary>
        /// ID Chứng từ
        /// </summary>
        public Guid AccountID { get; set; }

        /// <summary>
        /// Số chứng từ
        /// </summary>
        public Guid EmployeeID { get; set; }

        /// <summary>
        /// Diễn giải
        /// </summary>
        [MaxLength(255)]
        public string Username { get; set; }

        /// <summary>
        /// Lý do chi
        /// </summary>
        [MaxLength(255)]
        public string Password { get; set; }

        /// <summary>
        /// Loại chứng từ
        /// </summary>
        public int? role { get; set; }


        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? createddate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? createdby { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime? modifieddate { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public string? modifiedby { get; set; }

    }
}
