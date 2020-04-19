using BusinessObjects.Util;
using InmarTest.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Product
{
    public class ProductFactory
    {
        private Logger _log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        private static volatile ProductFactory _instance = new ProductFactory();

        public static ProductFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        public bool AddNewProduct(Product objProduct)
        {
            bool result = false;
            try
            {
                var db = new Database();
                var objDataTable = new DataTable();

                SqlParameter[] _params = new SqlParameter[]
                {
                    new SqlParameter("@SKU", Utils.CheckNull(objProduct.SKU, SqlDbType.Int)),
                    new SqlParameter("@Name", Utils.CheckNull(objProduct.Name, SqlDbType.VarChar)),
                    new SqlParameter("@Location_Id", Utils.CheckNull(objProduct.Location, SqlDbType.Int)),
                    new SqlParameter("@Department_Id", Utils.CheckNull(objProduct.Department, SqlDbType.Int)),
                    new SqlParameter("@Category_Id", Utils.CheckNull(objProduct.Category, SqlDbType.Int)),
                    new SqlParameter("@SubCategory_Id", Utils.CheckNull(objProduct.SubCategory, SqlDbType.Int))
                };

                db.RunProc(ProductLiterals.uspInsertProduct, _params);
                result = true;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "AddNewProduct", "ProductFactory");
                result = false;
            }

            return result;
        }

        public bool UpdateProductDetails(Product objProduct)
        {
            bool result = false;
            try
            {
                var db = new Database();
                var objDataTable = new DataTable();

                SqlParameter[] _params = new SqlParameter[]
                {
                    new SqlParameter("@ProductId", Utils.CheckNull(objProduct.ProductId, SqlDbType.Int)),
                    new SqlParameter("@SKU", Utils.CheckNull(objProduct.SKU, SqlDbType.Int)),
                    new SqlParameter("@Name", Utils.CheckNull(objProduct.Name, SqlDbType.VarChar)),
                    new SqlParameter("@Location_Id", Utils.CheckNull(objProduct.Location, SqlDbType.Int)),
                    new SqlParameter("@Department_Id", Utils.CheckNull(objProduct.Department, SqlDbType.Int)),
                    new SqlParameter("@Category_Id", Utils.CheckNull(objProduct.Category, SqlDbType.Int)),
                    new SqlParameter("@SubCategory_Id", Utils.CheckNull(objProduct.SubCategory, SqlDbType.Int))
                };

                db.RunProc(ProductLiterals.uspUpdateProduct, _params);
                result = true;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "UpdateProductDetails", "ProductFactory");
                result = false;
            }

            return result;
        }
        public Product GetProductByProductId(int Product_Id)
        {
            Product objProduct = new Product();
            try
            {
                var db = new Database();
                var objDataTable = new DataTable();

                SqlParameter[] _params = new SqlParameter[]
                {
                    new SqlParameter("@Product_Id", Utils.CheckNull(Product_Id, SqlDbType.Int))
                };

                db.RunProc(ProductLiterals.uspGetProductByProductId, _params, ref objDataTable);

                if (objDataTable != null)
                {
                    objProduct = new Product();
                    foreach (DataRow item in objDataTable.Rows)
                    {
                        objProduct = new Product
                        {
                            ProductId = Convert.ToInt32(Utils.CheckNull(item["ProductId"], SqlDbType.Int)),
                            SKU = Convert.ToInt32(Utils.CheckNull(item["SKU"], SqlDbType.Int)),
                            Name = Convert.ToString(Utils.CheckNull(item["Name"], SqlDbType.VarChar)),
                            Location = Convert.ToInt32(Utils.CheckNull(item["Location"], SqlDbType.Int)),
                            Department = Convert.ToInt32(Utils.CheckNull(item["Department"], SqlDbType.Int)),
                            Category = Convert.ToInt32(Utils.CheckNull(item["Category"], SqlDbType.Int)),
                            SubCategory = Convert.ToInt32(Utils.CheckNull(item["SubCategory"], SqlDbType.Int)),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "GetProductByProductId", "ProductFactory");
            }
            return objProduct;
        }


        public bool DeleteProductData(int ProductId)
        {
            bool result = false;
            try
            {
                var db = new Database();

                SqlParameter[] _params = new SqlParameter[]
                {
                    new SqlParameter("@ProductId", Utils.CheckNull(ProductId, SqlDbType.Int))
                };

                db.RunProc(ProductLiterals.uspDeleteProduct, _params);
                result = true;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "DeleteProductData", "ProductFactory");
            }

            return result;
        }

        public List<ProductData> GetAllProductsByLocDeptCatSubCat(int LocationId, int DepartmentId, int CategoryId, int SubCategoryId)
        {
            List<ProductData> objResult = new List<ProductData>();
            try
            {
                var db = new Database();
                var objDataTable = new DataTable();

                SqlParameter[] _params = new SqlParameter[]
                {
                    new SqlParameter("@Location_Id", Utils.CheckNull(LocationId, SqlDbType.Int)),
                    new SqlParameter("@Department_Id", Utils.CheckNull(DepartmentId, SqlDbType.Int)),
                    new SqlParameter("@Category_Id", Utils.CheckNull(CategoryId, SqlDbType.Int)),
                    new SqlParameter("@SubCategory_Id", Utils.CheckNull(SubCategoryId, SqlDbType.Int))
                };

                db.RunProc(ProductLiterals.uspGetAllProductsByLocDeptCatSubCat, _params, ref objDataTable);

                if (objDataTable != null)
                {
                    objResult = new List<ProductData>();
                    foreach (DataRow item in objDataTable.Rows)
                    {
                        objResult.Add(new ProductData
                        {
                            ProductId = Convert.ToInt32(Utils.CheckNull(item["ProductId"], SqlDbType.Int)),
                            SKU = Convert.ToInt32(Utils.CheckNull(item["SKU"], SqlDbType.Int)),
                            Name = Convert.ToString(Utils.CheckNull(item["Name"], SqlDbType.VarChar)),
                            LocationName = Convert.ToString(Utils.CheckNull(item["LocationName"], SqlDbType.VarChar)),
                            DepartmentName = Convert.ToString(Utils.CheckNull(item["DepartmentName"], SqlDbType.VarChar)),
                            CategoryName = Convert.ToString(Utils.CheckNull(item["CategoryName"], SqlDbType.VarChar)),
                            SubCategoryName = Convert.ToString(Utils.CheckNull(item["SubCategoryName"], SqlDbType.VarChar)),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("An error occurred while processing GetAllSubCategoriesByLocDeptCategory in SubCategoryFactory: " + ex.Message);
                _log.Info("Error while using LocationId:" + LocationId + ", DepartmentId: " + DepartmentId + ", CategoryId: " + CategoryId);
                _log.LogException(ex, "GetAllSubCategoriesByLocDeptCategory", "CategoryFactory");
            }

            return objResult;
        }


        public List<ProductData> GetAllProducts()
        {
            List<ProductData> objResult = new List<ProductData>();
            try
            {
                var db = new Database();
                var objDataTable = new DataTable(); 

                db.RunProc(ProductLiterals.uspGetAllProducts, ref objDataTable);

                if (objDataTable != null)
                {
                    objResult = new List<ProductData>();
                    foreach (DataRow item in objDataTable.Rows)
                    {
                        objResult.Add(new ProductData
                        {
                            ProductId = Convert.ToInt32(Utils.CheckNull(item["ProductId"], SqlDbType.Int)),
                            SKU = Convert.ToInt32(Utils.CheckNull(item["SKU"], SqlDbType.Int)),
                            Name = Convert.ToString(Utils.CheckNull(item["Name"], SqlDbType.VarChar)),
                            LocationName = Convert.ToString(Utils.CheckNull(item["LocationName"], SqlDbType.VarChar)),
                            DepartmentName = Convert.ToString(Utils.CheckNull(item["DepartmentName"], SqlDbType.VarChar)),
                            CategoryName = Convert.ToString(Utils.CheckNull(item["CategoryName"], SqlDbType.VarChar)),
                            SubCategoryName = Convert.ToString(Utils.CheckNull(item["SubCategoryName"], SqlDbType.VarChar)),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("An error in GetAllProducts of ProductFactory");
                _log.LogException(ex, "GetAllProducts", "ProductFactory");
            }

            return objResult;
        }


        public List<ProductData> GetMatchingProductData(string location, string department, string category, string subcategory)
        {
            List<ProductData> objResult = new List<ProductData>();
            try
            {
                _log.Info("Getting the matching product for given location: " + location + ", department: " + department + ", category: " + category + ", subcategory: " + subcategory);

                var db = new Database();
                var objDataTable = new DataTable();

                SqlParameter[] _params = new SqlParameter[]
                {
                    new SqlParameter("@Location", Utils.CheckNull(location, SqlDbType.VarChar)),
                    new SqlParameter("@Department", Utils.CheckNull(department, SqlDbType.VarChar)),
                    new SqlParameter("@Category", Utils.CheckNull(category, SqlDbType.VarChar)),
                    new SqlParameter("@SubCategory", Utils.CheckNull(subcategory, SqlDbType.VarChar))
                };

                db.RunProc(ProductLiterals.uspGetMatchingProducts, _params, ref objDataTable);

                if (objDataTable != null)
                {
                    objResult = new List<ProductData>();
                    foreach (DataRow item in objDataTable.Rows)
                    {
                        objResult.Add(new ProductData
                        {
                            ProductId = Convert.ToInt32(Utils.CheckNull(item["ProductId"], SqlDbType.Int)),
                            SKU = Convert.ToInt32(Utils.CheckNull(item["SKU"], SqlDbType.Int)),
                            Name = Convert.ToString(Utils.CheckNull(item["Name"], SqlDbType.VarChar)),
                            LocationName = Convert.ToString(Utils.CheckNull(item["LocationName"], SqlDbType.VarChar)),
                            DepartmentName = Convert.ToString(Utils.CheckNull(item["DepartmentName"], SqlDbType.VarChar)),
                            CategoryName = Convert.ToString(Utils.CheckNull(item["CategoryName"], SqlDbType.VarChar)),
                            SubCategoryName = Convert.ToString(Utils.CheckNull(item["SubCategoryName"], SqlDbType.VarChar)),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "GetMatchingProductData", "ProductFactory");
            }

            return objResult;
        }

    }
}
