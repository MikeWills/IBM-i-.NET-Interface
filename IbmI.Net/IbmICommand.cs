using System;
using System.Diagnostics;
using System.Text;
using IBM.Data.DB2.iSeries;

namespace IbmI.Net
{
    public static class IbmICommand
    {
        /// <summary>
        /// Runs a command on the IBM i.
        /// </summary>
        /// <param name="cmdText">The command text.</param>
        /// <param name="cn">The connection object.</param>
        /// <returns></returns>
        public static bool RunCmd(string cmdText, iDB2Connection cn)
        {
            bool rc = true;

            // Construct a string which contains the call to QCMDEXC.
            string pgmParm = String.Format("CALL QSYS.QCMDEXC('{0}', {1})", cmdText.Replace("'", "''").Trim(), cmdText.Trim().Length.ToString("0000000000.00000"));

            using (cn)
            {
                using (iDB2Command cmd = new iDB2Command(pgmParm, cn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        rc = false;
                    }
                }
            }

            // Return success or failure
            return rc;
        }
    }
}
