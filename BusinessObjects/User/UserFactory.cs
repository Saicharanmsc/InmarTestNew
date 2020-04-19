using BusinessObjects.Util;
using InmarTest.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.User
{
    public class UserFactory
    {
        private Logger _log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        private static volatile UserFactory _instance = new UserFactory();

        public static UserFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        public User GetUserData(string UserName)
        {
            User objUser = new User();
            try
            {
                var db = new Database();
                var objDataTable = new DataTable();

                SqlParameter[] _params = new SqlParameter[]
                {
                    new SqlParameter("@UserName", Utils.CheckNull(UserName, SqlDbType.VarChar))
                };

                db.RunProc(UserLiterals.uspGetUserData, _params, ref objDataTable);

                if (objDataTable != null)
                {
                    objUser = new User();
                    foreach (DataRow item in objDataTable.Rows)
                    {
                        objUser = new User
                        {
                            Id = Convert.ToInt32(Utils.CheckNull(item["Id"], SqlDbType.Int)),
                            UserName = Convert.ToString(Utils.CheckNull(item["UserName"], SqlDbType.VarChar)),
                            Password = Convert.ToString(Utils.CheckNull(item["Password"], SqlDbType.VarChar)),
                            FirstName = Convert.ToString(Utils.CheckNull(item["FirstName"], SqlDbType.VarChar)),
                            LastName = Convert.ToString(Utils.CheckNull(item["LastName"], SqlDbType.VarChar)),
                            CreateDate = Convert.ToDateTime(Utils.CheckNull(item["CreateDate"], SqlDbType.DateTime)),
                            ModifyDate = Convert.ToDateTime(Utils.CheckNull(item["ModifyDate"], SqlDbType.DateTime)),
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "GetUserData", "UserFactory");
            }

            return objUser;
        }

    }
}
