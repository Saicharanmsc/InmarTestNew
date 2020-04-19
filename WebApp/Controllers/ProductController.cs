using BusinessObjects.Product;
using DataAccessEntityFramework;
using InmarTest.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebApp.Common;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private Logger _log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

       public ActionResult GetAllProducts()
        {
            ProductModel objModel = new ProductModel();
            try
            {
                APIRepository objAPI = new APIRepository();
                _log.Info("Calling API for getting the Product details");
                HttpResponseMessage responseResult = objAPI.GetResponse("api/v1/products");

                objModel.ProductList = responseResult.Content.ReadAsAsync<List<ProductModel>>().Result;

                ViewBag.Title = "All Products";
                _log.Info("Getting all product details");
            }
            catch (Exception ex)
            {
                _log.Error("An error occurred in GetAllProducts GET Method: " + ex.Message);
                _log.LogException(ex, "GetAllProducts", "Product");
            }

            return View(objModel);
        }
        
        [HttpGet]
        public ActionResult CreateProduct()
        {
            ProductModel objModel = new ProductModel();
            using (var objEF = new TestEntities())
            {
                objModel.LocationList = objEF.Locations.ToList().Select(l => new SelectListItem { Text = l.LocationName, Value = l.LocationId.ToString() });
                objModel.DepartmentList = objEF.Departments.ToList().Select(d => new SelectListItem { Text = d.DepartmentName, Value = d.DepartmentId.ToString() });
                objModel.CategoryList = objEF.Categories.ToList().Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryId.ToString() });
                objModel.SubCategoryList = objEF.SubCategories.ToList().Select(s => new SelectListItem { Text = s.SubCategoryName, Value = s.SubCategoryId.ToString() });
            }
            return View(objModel);
        }

        [HttpPost]
        public ActionResult CreateProduct(ProductModel objModel)
        {
            try
            {
              BusinessObjects.Product.Product objProduct = new BusinessObjects.Product.Product();
                objProduct.SKU = objModel.SKU;
                objProduct.Name = objModel.Name;
                objProduct.Location = objModel.Location;
                objProduct.Department = objModel.Department;
                objProduct.Category = objModel.Category;
                objProduct.SubCategory = objModel.SubCategory;

                _log.Info("Adding a new product with details of:" + Convert.ToString(JsonConvert.SerializeObject(objProduct)));
                HttpResponseMessage responseResult = APIRepository.Instance.PostResponse("api/v1/add_product", objProduct);
                responseResult.EnsureSuccessStatusCode();
                bool response = responseResult.Content.ReadAsAsync<bool>().Result;
                if (response == true)
                {
                    TempData["SuccessMsg"] = "Product added successfully";
                }
                else
                {
                    TempData["ErrorMsg"] = "Error in adding product";
                }

                return RedirectToAction("GetAllProducts");
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "CreateProduct", "ProductController");
            }

            using (var objEF = new TestEntities())
            {
                objModel.LocationList = objEF.Locations.ToList().Select(l => new SelectListItem { Text = l.LocationName, Value = l.LocationId.ToString() });
                objModel.DepartmentList = objEF.Departments.ToList().Select(d => new SelectListItem { Text = d.DepartmentName, Value = d.DepartmentId.ToString() });
                objModel.CategoryList = objEF.Categories.ToList().Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryId.ToString() });
                objModel.SubCategoryList = objEF.SubCategories.ToList().Select(s => new SelectListItem { Text = s.SubCategoryName, Value = s.SubCategoryId.ToString() });
            }

            TempData["ErrorMsg"] = "Error in adding product. Please retry with other data";
            return View(objModel);
        }


        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            ProductModel objModel = new ProductModel();
            try
            {
                _log.Info("Getting the product details for the ProductId: " + id);

                HttpResponseMessage responseResult = APIRepository.Instance.GetResponse("api/v1/product?id=" + id.ToString());
                responseResult.EnsureSuccessStatusCode();
                BusinessObjects.Product.Product objResult = responseResult.Content.ReadAsAsync<BusinessObjects.Product.Product>().Result;

                objModel.ProductId = objResult.ProductId;
                objModel.SKU = objResult.SKU;
                objModel.Name = objResult.Name;
                objModel.Location = objResult.Location;
                objModel.Department = objResult.Department;
                objModel.Category = objResult.Category;
                objModel.SubCategory = objResult.SubCategory;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "EditProduct", "ProductController");
            }

            using (var objEF = new TestEntities())
            {
                objModel.LocationList = objEF.Locations.ToList().Select(l => new SelectListItem { Text = l.LocationName, Value = l.LocationId.ToString() });
                objModel.DepartmentList = objEF.Departments.ToList().Select(d => new SelectListItem { Text = d.DepartmentName, Value = d.DepartmentId.ToString() });
                objModel.CategoryList = objEF.Categories.ToList().Select(c => new SelectListItem { Text = c.CategoryName, Value = c.CategoryId.ToString() });
                objModel.SubCategoryList = objEF.SubCategories.ToList().Select(s => new SelectListItem { Text = s.SubCategoryName, Value = s.SubCategoryId.ToString() });
            }
            return View(objModel);
        }

        [HttpPost]
        public ActionResult UpdateProduct(ProductModel objModel)
        {
            try
            {
                _log.Info("Updating the product details for the ProductId:" + objModel.ProductId);

                BusinessObjects.Product.Product objProduct = new BusinessObjects.Product.Product();

                objProduct.ProductId = objModel.ProductId;
                objProduct.SKU = objModel.SKU;
                objProduct.Name = objModel.Name;
                objProduct.Location = objModel.Location;
                objProduct.Department = objModel.Department;
                objProduct.Category = objModel.Category;
                objProduct.SubCategory = objModel.SubCategory;

                HttpResponseMessage responseResult = APIRepository.Instance.PutResponse("api/v1/update_product", objProduct);
                responseResult.EnsureSuccessStatusCode();
                bool response = responseResult.Content.ReadAsAsync<bool>().Result;
                if (response == true)
                {
                    TempData["SuccessMsg"] = "Product updated successfully";
                }
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "UpdateProduct", "ProductController");
            }

            return RedirectToAction("GetAllProducts");
        }

        public ActionResult DeleteProduct(int id)
        {
            try
            {
                HttpResponseMessage responseResult = APIRepository.Instance.DeleteResponse("api/v1/delete_product?id=" + id.ToString());
                responseResult.EnsureSuccessStatusCode();
                bool response = responseResult.Content.ReadAsAsync<bool>().Result;
                if (response == true)
                {
                    TempData["SuccessMsg"] = "Product deleted successfully";
                }
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "DeleteProduct", "ProductController");
            }

            return RedirectToAction("GetAllProducts");
        }
    }
}