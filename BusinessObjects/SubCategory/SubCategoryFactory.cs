using BusinessObjects.Util;
using InmarTest.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.SubCategory
{
    public class SubCategoryFactory
    {
        private Logger _log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        private static volatile SubCategoryFactory _instance = new SubCategoryFactory();

        public static SubCategoryFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        public List<SubCategory> GetAllSubCategoriesByLocDeptCategory(int LocationId, int DepartmentId, int CategoryId)
        {
            List<SubCategory> objResult = new List<SubCategory>();
            try
            {
                var db = new Database();
                var objDataTable = new DataTable();

                SqlParameter[] _params = new SqlParameter[]
                {
                    new SqlParameter("@Location_Id", Utils.CheckNull(LocationId, SqlDbType.Int)),
                    new SqlParameter("@Department_Id", Utils.CheckNull(DepartmentId, SqlDbType.Int)),
                    new SqlParameter("@Category_Id", Utils.CheckNull(CategoryId, SqlDbType.Int))
                };

                db.RunProc(SubCategoryLiterals.uspGetAllSubCategoriesByLocDeptCategory, _params, ref objDataTable);

                if (objDataTable != null)
                {
                    objResult = new List<SubCategory>();
                    foreach (DataRow item in objDataTable.Rows)
                    {
                        objResult.Add(new SubCategory
                        {
                            SubCategoryId = Convert.ToInt32(Utils.CheckNull(item["SubCategoryId"], SqlDbType.Int)),
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
    }
}
