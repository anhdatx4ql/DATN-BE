using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.Common.Entities
{
    public class Asset
    {
        public Guid AssetID { get; set; }
        public string? AssetCode { get; set; }
        public string? AssetName { get; set; }
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public int? Status { get; set; }
        public int? Price { get; set; }
        public int? Quantity { get; set; }
        public DateTime? BoughtAt { get; set; }
        public DateTime? WarrantyTo { get; set; }
        public DateTime? created_date { get; set; }
        public string? created_by { get; set; }

        public DateTime? modified_date { get; set; }

        public string? modified_by { get; set; }

        public Guid? EmployeeID { get; set; }

    }
}
