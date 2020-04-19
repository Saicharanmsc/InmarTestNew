using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace InmarTest.DataAccess
{
    public class Database
    {
        protected string _ConnectionString;
        protected int _CommandTimeout;

        private Logger _log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        //
        public Database(string ConnectionString)
        {
            _ConnectionString = ConnectionString;
            _CommandTimeout = 60;
        }
        public Database()
        {
            _ConnectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString.ToString();
            _CommandTimeout = 60;
        }

        public void RunProc(string procName)
        {
            try
            {
                RunProc(procName, null);
            }
            catch (SqlException oex)
            {
                _log.Error("SQL Exception in RunProc :" + oex.Message);
                _log.Info("Stack Trace for exception :" + oex.StackTrace);
                throw oex;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "RunProc", "DAL");
                throw ex;
            }
        }

        public void RunProc(string procName, ref DataTable dataTable)
        {
            try
            {
                if (dataTable == null)
                    dataTable = new DataTable("tempName" + DateTime.Now.Ticks);

                RunProc(procName, ref dataTable, dataTable.TableName);
            }
            catch (SqlException oex)
            {
                _log.Error("SQL Exception in RunProc :" + oex.Message);
                _log.Info("Stack Trace for exception :" + oex.StackTrace);
                throw oex;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "RunProc", "DAL");
                throw ex;
            }
        }

        public void RunProc(string procName, SqlParameter[] prams, ref DataTable dataTable)
        {
            try
            {
                if (dataTable == null)
                    dataTable = new DataTable("tempName" + DateTime.Now.Ticks);

                RunProc(procName, prams, ref dataTable, dataTable.TableName);
            }
            catch (SqlException oex)
            {
                _log.Error("SQL Exception in RunProc :" + oex.Message);
                _log.Info("Stack Trace for exception :" + oex.StackTrace);
                throw oex;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "RunProc", "DAL");
                throw ex;
            }
        }

        public void RunProc(string procName, SqlParameter[] prams)
        {
            try
            {
                _RunProc(procName, prams);
            }
            catch (SqlException oex)
            {
                _log.Error("SQL Exception in RunProc :" + oex.Message);
                _log.Info("Stack Trace for exception :" + oex.StackTrace);
                throw oex;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "RunProc", "DAL");
                throw ex;
            }
        }

        public void RunProc(string procName, SqlParameter[] prams, ref DataSet dataSet)
        {
            try
            {
                _RunProc(procName, prams, ref dataSet, null);
            }
            catch (SqlException sqlex)
            {
                _log.Error("SQL Exception in RunProc :" + sqlex.Message);
                _log.Info("Stack Trace for exception :" + sqlex.StackTrace);
                throw sqlex;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "RunProc", "DAL");
                throw ex;
            }
        }
        public void RunProc(string procName, ref DataSet dataSet, string tableName)
        {
            try
            {
                _RunProc(procName, null, ref dataSet, tableName);
            }
            catch (SqlException sqlex)
            {
                _log.Error("SQL Exception in RunProc :" + sqlex.Message);
                _log.Info("Stack Trace for exception :" + sqlex.StackTrace);
                throw sqlex;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "RunProc", "DAL");
                throw ex;
            }
        }

        public String RunProc(string procName, SqlParameter[] prams, Boolean returnScalerValue)
        {
            try
            {
                return _RunProc(procName, prams, returnScalerValue);
            }
            catch (SqlException sqlex)
            {
                _log.Error("SQL Exception in RunProc :" + sqlex.Message);
                _log.Info("Stack Trace for exception :" + sqlex.StackTrace);
                throw sqlex;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "RunProc", "DAL");
                throw ex;
            }
            return "";
        }

        public void RunProc(string procName, ref DataSet dataSet, int startRecord, int maxRecords, string tableName)
        {
            try
            {
                _RunProc(procName, ref dataSet, startRecord, maxRecords, tableName);
            }
            catch (SqlException sqlex)
            {
                _log.Error("SQL Exception in RunProc :" + sqlex.Message);
                _log.Info("Stack Trace for exception :" + sqlex.StackTrace);
                throw sqlex;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "RunProc", "DAL");
                throw ex;
            }
        }

        public void RunProc(string procName, ref DataTable dataTable, string tableName)
        {
            try
            {
                _RunProc(procName, null, ref dataTable, tableName);
            }
            catch (SqlException oex)
            {
                if (_CommandTimeout < 120) _CommandTimeout = 120; //increase timeout if less than 120

                _log.Error("SQL Exception in RunProc, Retrying :" + oex.Message);
                _log.Error("Retrying Again-->");
                _log.Info("Stack Trace for exception :" + oex.StackTrace);
                throw oex;

                // Try again in case we have a transient network issue
                _RunProc(procName, null, ref dataTable, tableName);
            }
        }

        public void RunProc(string procName, SqlParameter[] prams, ref DataSet dataSet, string tableName)
        {
            try
            {
                _RunProc(procName, prams, ref dataSet, tableName);
            }
            catch (SqlException sqlex)
            {
                _log.Error("SQL Exception in RunProc :" + sqlex.Message);
                _log.Info("Stack Trace for exception :" + sqlex.StackTrace);
                throw sqlex;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "RunProc", "DAL");
                throw ex;
            }
        }

        public void RunProc(string procName, SqlParameter[] prams, ref DataTable dataTable, string tableName)
        {
            try
            {
                _RunProc(procName, prams, ref dataTable, tableName);
            }
            catch (SqlException sqlex)
            {
                _log.Error("SQL Exception in RunProc :" + sqlex.Message);
                _log.Info("Stack Trace for exception :" + sqlex.StackTrace);
                throw sqlex;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "RunProc", "DAL");
                throw ex;
            }
        }

        public void Close()
        {
        }

        public void Dispose()
        {
        }


        #region Private Database Helper Functions

        private void _RunProc(string procName, SqlParameter[] prams)
        {
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(_ConnectionString))
                {
                    SqlConn.Open();

                    SqlCommand SqlCmd = new SqlCommand();
                    SqlCmd.Connection = SqlConn;
                    SqlCmd.CommandText = procName;
                    SqlCmd.CommandTimeout = _CommandTimeout;
                    SqlCmd.CommandType = GetCommandType(procName);

                    if (prams != null)
                    {
                        for (int i = 0; i < prams.Length; i++)
                        {
                            SqlCmd.Parameters.Add(prams[i]);
                        }
                    }

                    SqlCmd.ExecuteNonQuery();

                    SqlConn.Close();
                }
            }
            catch (SqlException sqlex)
            {
                _log.Error("SQL Exception in RunProc :" + sqlex.Message);
                _log.Info("Stack Trace for exception :" + sqlex.StackTrace);
                throw sqlex;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "_RunProc", "DAL");
                throw ex;
            }
        }

        private string _RunProc(string procName, SqlParameter[] prams, Boolean returnScalarValue)
        {
            string strScalarValue = "";
            try
            {
                using (SqlConnection SqlConn = new SqlConnection(_ConnectionString))
                {
                    SqlConn.Open();

                    SqlCommand SqlCmd = new SqlCommand();
                    SqlCmd.Connection = SqlConn;
                    SqlCmd.CommandText = procName;
                    SqlCmd.CommandTimeout = _CommandTimeout;
                    SqlCmd.CommandType = GetCommandType(procName);

                    if (prams != null)
                    {
                        for (int i = 0; i < prams.Length; i++)
                        {
                            SqlCmd.Parameters.Add(prams[i]);
                        }
                    }

                    strScalarValue = Convert.ToString(SqlCmd.ExecuteScalar());

                    SqlConn.Close();
                }
            }
            catch (SqlException sqlex)
            {
                _log.Error("SQL Exception in RunProc :" + sqlex.Message);
                _log.Info("Stack Trace for exception :" + sqlex.StackTrace);
                throw sqlex;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "_RunProc", "DAL");
                throw ex;
            }
            return strScalarValue;
        }
        private void _RunProc(string procName, ref DataSet dataSet, int startRecord, int maxRecords, string tableName)
        {
            try
            {
                if (dataSet == null)
                    dataSet = new DataSet(tableName);

                using (SqlConnection SqlConn = new SqlConnection(_ConnectionString))
                {
                    SqlConn.Open();

                    SqlCommand SqlCmd = new SqlCommand();
                    SqlCmd.Connection = SqlConn;
                    SqlCmd.CommandText = procName;
                    SqlCmd.CommandTimeout = _CommandTimeout;
                    SqlCmd.CommandType = GetCommandType(procName);
                    SqlDataAdapter SqlAdapter = new SqlDataAdapter(SqlCmd);
                    SqlAdapter.Fill(dataSet, startRecord, maxRecords, tableName);
                    SqlConn.Close();
                }
            }
            catch (SqlException sqlex)
            {
                _log.Error("SQL Exception in RunProc :" + sqlex.Message);
                _log.Info("Stack Trace for exception :" + sqlex.StackTrace);
                throw sqlex;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "_RunProc", "DAL");
                throw ex;
            }
        }

        private void _RunProc(string procName, SqlParameter[] prams, ref DataSet dataSet, string tableName)
        {
            try
            {
                if (dataSet == null)
                    dataSet = new DataSet(tableName);

                using (SqlConnection SqlConn = new SqlConnection(_ConnectionString))
                {
                    SqlConn.Open();

                    SqlCommand SqlCmd = new SqlCommand();
                    SqlCmd.Connection = SqlConn;
                    SqlCmd.CommandText = procName;
                    SqlCmd.CommandTimeout = _CommandTimeout;
                    SqlCmd.CommandType = GetCommandType(procName);

                    if (prams != null)
                    {
                        for (int i = 0; i < prams.Length; i++)
                        {
                            SqlCmd.Parameters.Add(prams[i]);
                        }
                    }

                    SqlDataAdapter SqlAdapter = new SqlDataAdapter(SqlCmd);
                    SqlAdapter.Fill(dataSet);

                    SqlConn.Close();
                }
            }
            catch (SqlException sqlex)
            {
                _log.Error("SQL Exception in RunProc :" + sqlex.Message);
                _log.Info("Stack Trace for exception :" + sqlex.StackTrace);
                throw sqlex;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "_RunProc", "DAL");
                throw ex;
            }
        }

        private void _RunProc(string procName, SqlParameter[] prams, ref DataTable dataTable, string tableName)
        {
            try
            {
                if (dataTable == null)
                    dataTable = new DataTable(tableName);

                using (SqlConnection SqlConn = new SqlConnection(_ConnectionString))
                {
                    SqlConn.Open();

                    SqlCommand SqlCmd = new SqlCommand();
                    SqlCmd.Connection = SqlConn;
                    SqlCmd.CommandText = procName;
                    SqlCmd.CommandTimeout = _CommandTimeout;
                    SqlCmd.CommandType = GetCommandType(procName);

                    if (prams != null)
                    {
                        for (int i = 0; i < prams.Length; i++)
                        {
                            SqlCmd.Parameters.Add(prams[i]);
                        }
                    }

                    SqlDataAdapter SqlAdapter = new SqlDataAdapter(SqlCmd);
                    try
                    {
                        SqlAdapter.Fill(dataTable);
                    }
                    catch (SqlException oex)
                    {
                        string x = oex.Message;
                        throw oex;
                    }

                    SqlConn.Close();
                }
            }
            catch (SqlException sqlex)
            {
                _log.Error("SQL Exception in RunProc :" + sqlex.Message);
                _log.Info("Stack Trace for exception :" + sqlex.StackTrace);
                throw sqlex;
            }
            catch (Exception ex)
            {
                _log.LogException(ex, "_RunProc", "DAL");
                throw ex;
            }
        }

        #endregion

        #region Private Methods

        private string SqlParmsToString(SqlParameter[] prams)
        {
            if (prams == null)
                return "SqlParms:None/empty";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            string pformat = "\nParameter: {0}\tValue: {1}";

            for (int i = 0; i < prams.Length; ++i)
            {
                try
                {
                    sb.Append(String.Format(pformat, prams[i].ParameterName, prams[i],
                        (string)(prams[i].Value)
                        ));
                }
                catch (Exception ex)
                {
                    _log.LogException(ex, "SqlParmsToString", "DAL");
                    throw ex;
                }
                sb.Append("\n\n");
            }
            return sb.ToString();
        }

        System.Data.CommandType GetCommandType(string procName)
        {

            if ((procName.ToUpper().StartsWith("SELECT")) || (procName.ToUpper().StartsWith("WITH")) || (procName.ToUpper().StartsWith("INSERT")) || (procName.ToUpper().StartsWith("UPDATE")) || (procName.ToUpper().StartsWith("DELETE")))
                return CommandType.Text;
            else
                return CommandType.StoredProcedure;
        }

        public static int Bool2Int(bool b)
        {
            if (b)
                return 1;
            else
                return 0;
        }

        #endregion
    }
}