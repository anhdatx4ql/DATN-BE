using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.Common.Entities
{
    public class receipt_payment_detail
    {
        public Guid rpd_id { get; set; }

        public Guid rp_id { get; set; }

        public string? rpd_description { get; set; }

        public string debit_account { get; set; }

        public string credit_account { get; set; }

        public int? amount { get; set; }

        public Guid? object_id { get; set; }

        public DateTime? created_date { get; set; }

        public string? created_by { get; set; }

        public DateTime? modified_date { get; set; }

        public string? modified_by { get; set; }
    }
}
