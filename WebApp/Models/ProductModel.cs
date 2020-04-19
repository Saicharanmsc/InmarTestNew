using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Mvc;

namespace WebApp.Models
{
    //[FluentValidation.Attributes.Validator(typeof(ProductValidator))]
    public class ProductModel
    {
        public int ProductId { get; set; }
        public int SKU { get; set; }
        public string Name { get; set; }

        public int Location { get; set; }
        public int Department { get; set; }
        public int Category { get; set; }
        public int SubCategory { get; set; }

        [Display(Name = "Location")]
        public string LocationName { get; set; }

        [Display(Name = "Department")]
        public string DepartmentName { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "SubCategory")]
        public string SubCategoryName { get; set; }
        public List<ProductModel> ProductList { get; set; }

        public IEnumerable<SelectListItem> LocationList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> SubCategoryList { get; set; }

    }

}