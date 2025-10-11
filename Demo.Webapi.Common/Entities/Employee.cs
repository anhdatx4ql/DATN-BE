using Demo.Webapi.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Demo.Webapi.Common.Entities
{
    public class Employee
    {
        /// <summary>
        /// ID nhân viên
        /// </summary>
        public Guid EmployeeID { get; set; }
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set;}

        /// <summary>
        /// Ngày sửa đổi
        /// </summary>
        public DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// Người sửa đổi
        /// </summary>
        public string? ModifiedBy { get; set;}
    }
}
