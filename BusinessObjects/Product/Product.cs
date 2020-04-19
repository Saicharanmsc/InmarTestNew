using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Product
{
    public class Product
    {
        public int ProductId { get; set; }
        public int SKU { get; set; }
        public string Name { get; set; }
        public int Location { get; set; }
        public int Department { get; set; }
        public int Category { get; set; }
        public int SubCategory { get; set; }
    }

    public class ProductData
    {
        public int ProductId { get; set; }
        public int SKU { get; set; }
        public string Name { get; set; }
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
    }
}
