using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Webapi.Common.Entities
{
    public class Category
    {
        public Guid CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryCode { get; set;}
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public string? ModifiedBy { get; set; }
    }
}
