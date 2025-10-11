using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.Common.Entities
{
    public class receipt_payment
    {
        /// <summary>
        /// ID Chứng từ
        /// </summary>
        public Guid re_id { get; set; }

        /// <summary>
        /// Số chứng từ
        /// </summary>
        [MaxLength(20)]
        public string re_ref_no { get; set; }

        /// <summary>
        /// Ngày hạch toán
        /// </summary>
        public DateTime re_date { get; set; }

        /// <summary>
        /// Diễn giải
        /// </summary>
        [MaxLength(255)]
        public string? re_description { get; set; }

        /// <summary>
        /// ID đối tượng
        /// </summary>
        public Guid? account_id { get; set; }

        /// <summary>
        /// Lý do chi
        /// </summary>
        [MaxLength(255)]
        public string? re_reason { get; set; }

        /// <summary>
        /// Loại chứng từ
        /// </summary>
        public int? ca_type { get; set; }

        /// <summary>
        /// Ngày phiếu chi
        /// </summary>
        public DateTime ca_date { get; set; }

        /// <summary>
        /// ID Nhân viên
        /// </summary>
        public string? employee_id { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? created_date { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? created_by { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime? modified_date { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public string? modified_by { get; set; }

        /// <summary>
        /// Tên đối tượng
        /// </summary>
        public string? re_name { get; set; }

        /// <summary>
        /// Người nhận
        /// </summary>
        public string? re_receive { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? re_address { get; set; }

        /// <summary>
        /// Kèm theo
        /// </summary>
        public int? re_attach { get; set; }


    }
}
