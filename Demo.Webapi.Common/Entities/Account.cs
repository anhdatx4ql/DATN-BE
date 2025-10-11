using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.Common.Entities
{
    public class Account
    {
        // ID Tài khoản
        [Required]
        public Guid AccountId { get; set; }

        public Guid EmployeeID { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public int? Role { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
