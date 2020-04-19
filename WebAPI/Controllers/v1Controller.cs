using BusinessObjects;
using BusinessObjects.Category;
using BusinessObjects.Department;
using BusinessObjects.Location;
using BusinessObjects.Product;
using BusinessObjects.SubCategory;
using BusinessObjects.User;
using InmarTest.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Common;

namespace WebAPI.Controllers
{
    [Authorize]
    public class v1Controller : ApiController
    {
        private Logger _olog = new Logger(Convert.ToString(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType));


        [AllowAnonymous]
        [HttpPost]
        [Route("api/v1/Login")]
        public IHttpActionResult UserLogin(UserLoginModel objUserLoginModel)
        {
            try
            {
                User objUser = UserFactory.Instance.GetUserData(objUserLoginModel.UserName);
                if(objUser.Password == objUserLoginModel.Password)
                {
                    var token = RSAClass.Encrypt(Convert.ToString(objUserLoginModel.UserName));

                    objUserLoginModel.Id = objUser.Id;
                    objUserLoginModel.FirstName = objUser.FirstName;
                    objUserLoginModel.LastName = objUser.LastName;
                    objUserLoginModel.LoginStatus = "Login Success";
                    objUserLoginModel.Token = token;
                    objUserLoginModel.ValidDate = DateTime.Now.AddDays(1);
                }
                else
                {
                    objUserLoginModel.LoginStatus = "Login Failed!";
                
                }

            }
            catch (Exception ex)
            {
                _olog.LogException(ex, "UserLogin", "v1Controller");
            }

            return Ok(objUserLoginModel);
        }


        //GET api/v1/
        [HttpGet]
        [Route("api/v1/location")]
        public IHttpActionResult GetLocations()
        {
            List<Location> objLocations = new List<Location>();
            try
            {
                objLocations = LocationFactory.Instance.GetAllLocations();
            }
            catch (Exception ex)
            {
                _olog.Error("An exception occurred while processing the GetLocations API:" + ex.Message);
                _olog.LogException(ex, "api/v1/location", "v1Controller");
            }

            return Ok(objLocations);
        }


        [HttpGet]
        [Route("api/v1/location/{location_id}/department")]
        public IHttpActionResult GetDepartmentsByLocation(int location_id)
        {
            List<Department> objDepartments = new List<Department>();
            try
            {
                objDepartments = DepartmentFactory.Instance.GetAllDepartmentsByLocationId(location_id);
            }
            catch (Exception ex)
            {
                _olog.Error("An error occurred whil processing GetDepartmentsByLocation API for locationId: " + location_id + ". Exception message: " + ex.Message);
                _olog.LogException(ex, "GetDepartmentsByLocation", "v1Controller");
            }

            return Ok(objDepartments);
        }

        [HttpGet]
        [Route("api/v1/location/{location_id}/department/{department_id}/category")]
        public IHttpActionResult GetCategoryByLocationByDepartment(int location_id, int department_id)
        {
            List<Category> objCategory = new List<Category>();
            try
            {
                objCategory = CategoryFactory.Instance.GetAllCategoriesByLocationIdByDepartmentId(location_id, department_id);
            }
            catch (Exception ex)
            {
                _olog.Error("An error occurred whil processing GetCategoryByLocationByDepartment API for LocationId: " + location_id + ", DepartmentId:" + department_id + ". Exception message: " + ex.Message);
                _olog.LogException(ex, "GetCategoryByLocationByDepartment", "v1Controller");
            }

            return Ok(objCategory);
        }

        [HttpGet]
        [Route("api/v1/location/{location_id}/department/{department_id}/category/{category_id}/subcategory")]
        public IHttpActionResult GetSubCategoryByLocDeptCategory(int location_id, int department_id, int category_id)
        {
            List<SubCategory> objSubCategory = new List<SubCategory>();
            try
            {
                objSubCategory = SubCategoryFactory.Instance.GetAllSubCategoriesByLocDeptCategory(location_id, department_id, category_id);
            }
            catch (Exception ex)
            {
                _olog.Error("Error in GetSubCategoryByLocDeptCategory API with LocationId: " + location_id + ", DepartmentId:" + department_id + ", CategoryId: " + category_id + ". Exception message: " + ex.Message);
                _olog.LogException(ex, "GetSubCategoryByLocDeptCategory", "v1Controller");
            }

            return Ok(objSubCategory);
        }

        [HttpGet]
        [Route("api/v1/location/{location_id}/department/{department_id}/category/{category_id}/subcategory/{subcategory_id}")]
        public IHttpActionResult GetProductByLocDeptCatSubCat(int location_id, int department_id, int category_id, int subcategory_id)
        {
            List<ProductData> objProducts = new List<ProductData>();
            try
            {
                objProducts = ProductFactory.Instance.GetAllProductsByLocDeptCatSubCat(location_id, department_id, category_id, subcategory_id);
            }
            catch (Exception ex)
            {
                _olog.Error("Error occurred while processing GetProductByLocDeptCatSubCat: " + ex.Message);
                _olog.Info("Used values LocationId: " + location_id + ", DepartmentId:" + department_id + ", CategoryId: " + category_id + ", SubCategoryId: " + subcategory_id);
                _olog.LogException(ex, "GetProductByLocDeptCatSubCat", "v1Controller");
            }

            return Ok(objProducts);
        }

        [HttpGet]
        [Route("api/v1/products")]
        public IHttpActionResult GetAllProducts()
        {
            List<ProductData> objProducts = new List<ProductData>();
            try
            {
                objProducts = ProductFactory.Instance.GetAllProducts();
            }
            catch (Exception ex)
            {
                _olog.Error("An error occurred while processing GetAllProducts");
                _olog.LogException(ex, "GetAllProducts", "v1Controller");
            }

            return Ok(objProducts);
        }

        [HttpGet]
        [Route("api/v1/product")]
        public IHttpActionResult GetProductByProductId(int id)
        {
            Product objProduct = new Product();
            try
            {
                objProduct = ProductFactory.Instance.GetProductByProductId(id);
            }
            catch (Exception ex)
            {
                _olog.Error("An error occurred while processing GetProductByProductId");
                _olog.LogException(ex, "GetProductByProductId", "v1Controller");
            }

            return Ok(objProduct);
        }

        [HttpPost]
        [Route("api/v1/add_product")]
        public bool AddProduct(Product objProduct)
        {
            bool addStatus = false;
            try
            {
                addStatus = ProductFactory.Instance.AddNewProduct(objProduct);
            }
            catch (Exception ex)
            {
                _olog.LogException(ex, "AddProduct", "v1Controller");
            }

            return addStatus;
        }


        [HttpPut]
        [Route("api/v1/update_product")]
        public bool UpdateProduct(Product objProduct)
        {
            bool updateStatus = false;

            try
            {
                updateStatus = ProductFactory.Instance.UpdateProductDetails(objProduct);
            }
            catch (Exception ex)
            {
                _olog.LogException(ex, "UpdateProduct", "v1Controller");
            }

            return updateStatus;
        }

        [HttpDelete]
        [Route("api/v1/delete_product")]
        public bool DeleteProduct(int id)
        {
            bool deleteStatus = false;
            try
            {
                deleteStatus = ProductFactory.Instance.DeleteProductData(id);
            }
            catch (Exception ex)
            {
                _olog.LogException(ex, "DeleteProduct", "v1Controller");
            }

            return deleteStatus;
        }


        [HttpGet]
        [Route("api/v1/data_match_product")]
        public IHttpActionResult GetMatchingProducts(string location, string department, string category, string subcategory)
        {
            List<ProductData> objProductData = new List<ProductData>();
            try
            {
                _olog.Info("Getting matching products for location: " + location + ", department: " + department + ", category: " + category + ", subcategory: " + subcategory);

                objProductData = ProductFactory.Instance.GetMatchingProductData(location, department, category, subcategory);

            }
            catch (Exception ex)
            {
                _olog.LogException(ex, "GetMatchingProducts", "v1Controller");
            }
            return Ok(objProductData);
        }

    }
}