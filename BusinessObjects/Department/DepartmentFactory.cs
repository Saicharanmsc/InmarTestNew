using BusinessObjects.Util;
using InmarTest.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Department
{
    public class DepartmentFactory
    {
        private Logger _log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        private static volatile DepartmentFactory _instance = new DepartmentFactory();

        public static DepartmentFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        public List<Department> GetAllDepartmentsByLocationId(int LocationId)
        {
            List<Department> objResult = new List<Department>();
            try
            {
                var db = new Database();
                var objDataTable = new DataTable();

                SqlParameter[] _params = new SqlParameter[]
                {
                    new SqlParameter("@Location_Id", Utils.CheckNull(LocationId, SqlDbType.Int))
                };
                
                db.RunProc(DepartmentLiterals.uspGetAllDepartmentsByLocationId, _params, ref objDataTable);

                if(objDataTable != null)
                {
                    objResult = new List<Department>();
                    foreach(DataRow item in objDataTable.Rows)
                    {
                        objResult.Add(new Department
                        {
                            DepartmentId = Convert.ToInt32(Utils.CheckNull(item["DepartmentId"], SqlDbType.Int)),
                            DepartmentName = Convert.ToString(Utils.CheckNull(item["DepartmentName"], SqlDbType.VarChar)),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("An error occurred while processing GetAllDepartmentsByLocationId in DepartmentFactory: " + ex.Message);
                _log.LogException(ex, "GetAllDepartmentsByLocationId", "DepartmentFactory");
            }

            return objResult;
        }
    }
}
