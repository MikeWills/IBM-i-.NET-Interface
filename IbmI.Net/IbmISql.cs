using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IBM.Data.DB2.iSeries;

namespace IbmI.Net
{
    public class IbmISql : IDisposable
    {
        private const string _LOG_SOURCE = "IbmISql";
        private const string _LOG_APPLICATION = "Application";
        private string _CONN_STRING;
        private iDB2Connection conn;

        /// <summary>
        /// Initializes a new instance of the <see cref="IbmISql"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public IbmISql(string connectionString)
        {
            _CONN_STRING = connectionString;
            conn = new iDB2Connection(_CONN_STRING);
        }

        /// <summary>
        /// Opens the conn.
        /// </summary>
        public void OpenConn()
        {
            try
            {
                conn.Open();
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// Closes the conn.
        /// </summary>
        public void CloseConn()
        {
            conn.Close();
        }

        #region Get data
        /// <summary>
        /// Gets the data from the IBM i.
        /// </summary>
        /// <param name="connString">The connection string</param>
        /// <param name="sqlStatement">The SQL statement</param>
        /// <param name="parameters">The parameters (iDB2Parameter)</param>
        /// <returns>DataTable</returns>
        public DataTable GetData(string sqlStatement, Action<iDB2ParameterCollection> parameters)
        {
            DataTable dt = new DataTable();

            using (iDB2Command cmd = new iDB2Command(sqlStatement, conn))
            {
                if (parameters != null) { parameters(cmd.Parameters); }
                try
                {
                    using (iDB2DataAdapter da = new iDB2DataAdapter(cmd)) { da.Fill(dt); }
                }
                catch (iDB2SQLErrorException e)
                {
                    if (!EventLog.SourceExists(_LOG_SOURCE))
                        EventLog.CreateEventSource(_LOG_SOURCE, _LOG_APPLICATION);

                    EventLog.WriteEntry(_LOG_SOURCE, "**ERROR** on da.Fill(dt): " + e.Message, EventLogEntryType.Error);
                    throw e;
                }
                catch (Exception e)
                {
                    if (!EventLog.SourceExists(_LOG_SOURCE))
                        EventLog.CreateEventSource(_LOG_SOURCE, _LOG_APPLICATION);

                    EventLog.WriteEntry(_LOG_SOURCE, "**ERROR** on da.Fill(dt): " + e.Message, EventLogEntryType.Error);
                    throw e;
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
        public DataTable GetData(string sqlStatement)
        {
            return GetData(sqlStatement, null);
        }
        #endregion

        #region Get value
        /// <summary>
        /// Gets the data from the IBM i.
        /// </summary>
        /// <param name="connString">The connection string</param>
        /// <param name="sqlStatement">The SQL statement</param>
        /// <param name="parameters">The parameters (iDB2Parameter)</param>
        /// <returns>DataTable</returns>
        public Object GetValue(string sqlStatement, Action<iDB2ParameterCollection> parameters)
        {
            Object obj;

            using (iDB2Command cmd = new iDB2Command(sqlStatement, conn))
            {
                if (parameters != null) { parameters(cmd.Parameters); }
                try
                {
                    obj = cmd.ExecuteScalar();
                }
                catch (iDB2SQLErrorException e)
                {
                    if (!EventLog.SourceExists(_LOG_SOURCE))
                        EventLog.CreateEventSource(_LOG_SOURCE, _LOG_APPLICATION);

                    EventLog.WriteEntry(_LOG_SOURCE, "**ERROR** on da.Fill(dt): " + e.Message, EventLogEntryType.Error);
                    throw e;
                }
                catch (Exception e)
                {
                    if (!EventLog.SourceExists(_LOG_SOURCE))
                        EventLog.CreateEventSource(_LOG_SOURCE, _LOG_APPLICATION);

                    EventLog.WriteEntry(_LOG_SOURCE, "**ERROR** on da.Fill(dt): " + e.Message, EventLogEntryType.Error);
                    throw e;
                }
            }

            return obj;
        }

        /// <summary>
        /// Gets the data from the IBM i.
        /// </summary>
        /// <param name="connString">The connection string</param>
        /// <param name="sqlStatement">The SQL statement</param>
        /// <returns>DataTable</returns>
        public Object GetValue(string sqlStatement)
        {
            return GetValue(sqlStatement, null);
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// Executes a statement on the IBM i that doesn't return data (INSERT, UPDATE, DELETE)
        /// </summary>
        /// <param name="connString">The connection string</param>
        /// <param name="sqlStatement">The SQL statement</param>
        /// <param name="parameters">The parameters (iDB2Parameter)</param>
        public void ExecuteNonQuery(string sqlStatement, Action<iDB2ParameterCollection> parameters)
        {
            using (iDB2Command cmd = new iDB2Command(sqlStatement, conn))
            {
                if (parameters != null) { parameters(cmd.Parameters); }
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (iDB2SQLErrorException e)
                {
                    if (!EventLog.SourceExists(_LOG_SOURCE))
                        EventLog.CreateEventSource(_LOG_SOURCE, _LOG_APPLICATION);

                    EventLog.WriteEntry(_LOG_SOURCE, "**ERROR** on cmd.ExecuteNonQuery(): " + e.Message, EventLogEntryType.Error);
                    throw e;
                }
                catch (Exception e)
                {
                    if (!EventLog.SourceExists(_LOG_SOURCE))
                        EventLog.CreateEventSource(_LOG_SOURCE, _LOG_APPLICATION);

                    EventLog.WriteEntry(_LOG_SOURCE, "**ERROR** on da.Fill(dt): " + e.Message, EventLogEntryType.Error);
                    throw e;
                }
            }
        }

        /// <summary>
        /// Executes a statement on the IBM i that doesn't return data (INSERT, UPDATE, DELETE)
        /// </summary>
        /// <param name="connString">The connection string</param>
        /// <param name="sqlStatement">The SQL statement</param>
        public void ExecuteNonQuery(string sqlStatement)
        {
            ExecuteNonQuery(sqlStatement, null);
        }
        #endregion

        #region ExecuteStoredProcedure
        /// <summary>
        /// Executes a stored procedure on the IBM i that doesn't return data.
        /// </summary>
        /// <param name="connString">The connection string</param>
        /// <param name="sqlStatement">The SQL statement</param>
        /// <param name="parameters">The parameters (iDB2Parameter)</param>
        public void ExecuteStoredProcedure(string sqlStatement, Action<iDB2ParameterCollection> parameters)
        {
            using (iDB2Command cmd = new iDB2Command(sqlStatement, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null) { parameters(cmd.Parameters); }
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (iDB2SQLErrorException e)
                {
                    if (!EventLog.SourceExists(_LOG_SOURCE))
                        EventLog.CreateEventSource(_LOG_SOURCE, _LOG_APPLICATION);

                    EventLog.WriteEntry(_LOG_SOURCE, "**ERROR** on cmd.ExecuteNonQuery(): " + e.Message, EventLogEntryType.Error);
                    throw e;
                }
                catch (Exception e)
                {
                    if (!EventLog.SourceExists(_LOG_SOURCE))
                        EventLog.CreateEventSource(_LOG_SOURCE, _LOG_APPLICATION);

                    EventLog.WriteEntry(_LOG_SOURCE, "**ERROR** on da.Fill(dt): " + e.Message, EventLogEntryType.Error);
                    throw e;
                }
            }
        }

        /// <summary>
        /// Executes a stored procedure on the IBM i that doesn't return data.
        /// </summary>
        /// <param name="connString">The connection string</param>
        /// <param name="sqlStatement">The SQL statement</param>
        public void ExecuteStoredProcedure(string sqlStatement)
        {
            ExecuteStoredProcedure(sqlStatement, null);
        }

        /// <summary>
        ///  Executes a stored procedure that returns a result set.
        /// </summary>
        /// <param name="connString">The conn string.</param>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="parameters">The parameters.</param>
        public DataTable ExecuteStoredProcedureWithResultSet(string sqlStatement, Action<iDB2ParameterCollection> parameters)
        {
            DataTable dt = new DataTable();

            using (iDB2Command cmd = new iDB2Command(sqlStatement, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null) { parameters(cmd.Parameters); }
                try
                {
                    using (iDB2DataAdapter da = new iDB2DataAdapter(cmd)) { da.Fill(dt); }
                }
                catch (iDB2SQLErrorException e)
                {
                    if (!EventLog.SourceExists(_LOG_SOURCE))
                        EventLog.CreateEventSource(_LOG_SOURCE, _LOG_APPLICATION);

                    EventLog.WriteEntry(_LOG_SOURCE, "**ERROR** on cmd.ExecuteNonQuery(): " + e.Message, EventLogEntryType.Error);
                    throw e;
                }
                catch (Exception e)
                {
                    if (!EventLog.SourceExists(_LOG_SOURCE))
                        EventLog.CreateEventSource(_LOG_SOURCE, _LOG_APPLICATION);

                    EventLog.WriteEntry(_LOG_SOURCE, "**ERROR** on da.Fill(dt): " + e.Message, EventLogEntryType.Error);
                    throw e;
                }
            }

            return dt;
        }

        /// <summary>
        /// Executes a stored procedure that returns a result set.
        /// </summary>
        /// <param name="connString">The conn string.</param>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <returns></returns>
        public DataTable ExecuteStoredProcedureWithResultSet(string sqlStatement)
        {
            return ExecuteStoredProcedureWithResultSet(sqlStatement, null);
        }
        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            conn.Close();
            conn.Dispose();
        }
    }
}