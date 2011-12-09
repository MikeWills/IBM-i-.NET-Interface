using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IBM.Data.DB2.iSeries;

namespace IbmI.Net
{
    public static class IbmISql
    {
        #region Get data
        /// <summary>
        /// Gets the data from the IBM i.
        /// </summary>
        /// <param name="connString">The connection string</param>
        /// <param name="sqlStatement">The SQL statement</param>
        /// <param name="parameters">The parameters (iDB2Parameter)</param>
        /// <returns>DataTable</returns>
        public static DataTable GetData(string connString, string sqlStatement, Action<iDB2ParameterCollection> parameters)
        {
            DataTable dt = new DataTable();

            using (iDB2Connection conn = new iDB2Connection(connString))
            {
                using (iDB2Command cmd = new iDB2Command(sqlStatement, conn))
                {
                    conn.Open();
                    if (parameters != null) { parameters(cmd.Parameters); }
                    using (iDB2DataAdapter da = new iDB2DataAdapter(cmd)) { da.Fill(dt); }
                    conn.Close();
                }
            }

            return dt;
        }

        /// <summary>
        /// Gets the data from the IBM i.
        /// </summary>
        /// <param name="connString">The connection string</param>
        /// <param name="sqlStatement">The SQL statement</param>
        /// <returns>DataTable</returns>
        public static DataTable GetData(string connString, string sqlStatement)
        {
            return GetData(connString, sqlStatement, null);
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// Executes a statement on the IBM i that doesn't return data (INSERT, UPDATE, DELETE)
        /// </summary>
        /// <param name="connString">The connection string</param>
        /// <param name="sqlStatement">The SQL statement</param>
        /// <param name="parameters">The parameters (iDB2Parameter)</param>
        public static void ExecuteNonQuery(string connString, string sqlStatement, Action<iDB2ParameterCollection> parameters)
        {
            using (iDB2Connection conn = new iDB2Connection(connString))
            {
                using (iDB2Command cmd = new iDB2Command(sqlStatement, conn))
                {
                    conn.Open();
                    if (parameters != null) { parameters(cmd.Parameters); }
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Executes a statement on the IBM i that doesn't return data (INSERT, UPDATE, DELETE)
        /// </summary>
        /// <param name="connString">The connection string</param>
        /// <param name="sqlStatement">The SQL statement</param>
        public static void ExecuteNonQuery(string connString, string sqlStatement)
        {
            ExecuteNonQuery(connString, sqlStatement, null);
        }
        #endregion

        #region ExecuteStoredProcedure
        /// <summary>
        /// Executes a stored procedure on the IBM i that doesn't return data.
        /// </summary>
        /// <param name="connString">The connection string</param>
        /// <param name="sqlStatement">The SQL statement</param>
        /// <param name="parameters">The parameters (iDB2Parameter)</param>
        public static void ExecuteStoredProcedure(string connString, string sqlStatement, Action<iDB2ParameterCollection> parameters)
        {
            using (iDB2Connection conn = new iDB2Connection(connString))
            {
                using (iDB2Command cmd = new iDB2Command(sqlStatement, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    if (parameters != null) { parameters(cmd.Parameters); }
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Executes a stored procedure on the IBM i that doesn't return data.
        /// </summary>
        /// <param name="connString">The connection string</param>
        /// <param name="sqlStatement">The SQL statement</param>
        public static void ExecuteStoredProcedure(string connString, string sqlStatement)
        {
            ExecuteStoredProcedure(connString, sqlStatement, null);
        }
        #endregion
    }
}