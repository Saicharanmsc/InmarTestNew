using BusinessObjects.Util;
using InmarTest.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Category
{
    public class CategoryFactory
    {
        private Logger _log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        private static volatile CategoryFactory _instance = new CategoryFactory();

        public static CategoryFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        public List<Category> GetAllCategoriesByLocationIdByDepartmentId(int LocationId, int DepartmentId)
        {
            List<Category> objResult = new List<Category>();
            try
            {
                var db = new Database();
                var objDataTable = new DataTable();

                SqlParameter[] _params = new SqlParameter[]
                {
                    new SqlParameter("@Location_Id", Utils.CheckNull(LocationId, SqlDbType.Int)),
                    new SqlParameter("@Department_Id", Utils.CheckNull(DepartmentId, SqlDbType.Int))
                };

                db.RunProc(CategoryLiterals.uspGetAllCategoriesByLocationDepartment, _params, ref objDataTable);

                if (objDataTable != null)
                {
                    objResult = new List<Category>();
                    foreach (DataRow item in objDataTable.Rows)
                    {
                        objResult.Add(new Category
                        {
                            CategoryId = Convert.ToInt32(Utils.CheckNull(item["CategoryId"], SqlDbType.Int)),
                            CategoryName = Convert.ToString(Utils.CheckNull(item["CategoryName"], SqlDbType.VarChar)),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("An error occurred while processing GetAllCategoriesByLocationIdByDepartmentId in CategoryFactory: " + ex.Message);
                _log.Info("Error while using LocationId:" + LocationId + ", DepartmentId: " + DepartmentId);
                _log.LogException(ex, "GetAllCategoriesByLocationIdByDepartmentId", "CategoryFactory");
            }

            return objResult;
        }
    }
}
