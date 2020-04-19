using BusinessObjects.Util;
using InmarTest.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Location
{
    public class LocationFactory
    {
        private Logger _log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        private static volatile LocationFactory _instance = new LocationFactory();

        public static LocationFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        public List<Location> GetAllLocations()
        {
            List<Location> objResult = new List<Location>();
            try
            {
                var db = new Database();
                var objDataTable = new DataTable();
                db.RunProc(LocationLiterals.uspGetAllLocations, ref objDataTable);

                if(objDataTable != null)
                {
                    objResult = new List<Location>();
                    foreach(DataRow item in objDataTable.Rows)
                    {
                        objResult.Add(new Location { 
                        LocationId = Convert.ToInt32(Utils.CheckNull(item["LocationId"], SqlDbType.Int)),
                        LocationName = Convert.ToString(Utils.CheckNull(item["LocationName"], SqlDbType.VarChar))
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                _log.Error("An error occurred while processing GetAllLocations in LocationFactory: " + ex.Message);
                _log.LogException(ex, "GetAllLocations", "LocationFactory");
            }

            return objResult;
        }
    }
}
