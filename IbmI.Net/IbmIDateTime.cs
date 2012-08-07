using System;
using System.Text;
using System.Diagnostics;

namespace IbmI.Net
{
    public static class IbmIDateTime
    {
        /// <summary>
        /// Converts to a DateTime.
        /// </summary>
        /// <param name="numericDate">The numeric 8 digit date.</param>
        /// <returns></returns>
        static public DateTime ConvertToDateTime(int numericDate)
        {
            try
            {
                if (numericDate != 0)
                {
                    string strNumericDate = numericDate.ToString();
                    return new DateTime(Convert.ToInt32(strNumericDate.Substring(0, 4)),
                                        Convert.ToInt32(strNumericDate.Substring(4, 2)),
                                        Convert.ToInt32(strNumericDate.Substring(6, 2)));
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Converts to a DateTime.
        /// </summary>
        /// <param name="numericDate">The numeric 8 digit date.</param>
        /// <returns></returns>
        static public DateTime ConvertToDateTime(decimal numericDate)
        {
            try
            {
                if (numericDate != 0)
                {
                    string strNumericDate = numericDate.ToString();
                    return new DateTime(Convert.ToInt32(strNumericDate.Substring(0, 4)),
                                        Convert.ToInt32(strNumericDate.Substring(4, 2)),
                                        Convert.ToInt32(strNumericDate.Substring(6, 2)));
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Converts to a DateTime.
        /// </summary>
        /// <param name="numericDate">The numeric 8 digit date.</param>
        /// <param name="numericTime">The numeric time.</param>
        /// <param name="timeLength">Length of the time field (4 or 6).</param>
        /// <returns></returns>
        static public DateTime ConvertToDateTime(int numericDate, int numericTime, int timeLength)
        {
            string strNumericDate = numericDate.ToString();
            string strNumericTime = String.Empty;

            // Catch zero dates being passed
            if (numericDate == 0 && numericTime == 0)
            {
                return DateTime.MinValue;
            }

            if (timeLength == 4)
            {
                strNumericTime = String.Format("{0:0000}", numericTime);
                //strNumericTime = numericTime.ToString("D4");
                return new DateTime(Convert.ToInt32(strNumericDate.Substring(0, 4)),
                                    Convert.ToInt32(strNumericDate.Substring(4, 2)),
                                    Convert.ToInt32(strNumericDate.Substring(6, 2)),
                                    Convert.ToInt32(strNumericTime.Substring(0, 2)),
                                    Convert.ToInt32(strNumericTime.Substring(2, 2)),
                                    0);
            }
            else if (timeLength == 6)
            {
                strNumericTime = String.Format("{0:000000}", numericTime);
                //strNumericTime = numericTime.ToString("D6");
                return new DateTime(Convert.ToInt32(strNumericDate.Substring(0, 4)),
                                    Convert.ToInt32(strNumericDate.Substring(4, 2)),
                                    Convert.ToInt32(strNumericDate.Substring(6, 2)),
                                    Convert.ToInt32(strNumericTime.Substring(0, 2)),
                                    Convert.ToInt32(strNumericTime.Substring(2, 2)),
                                    Convert.ToInt32(strNumericTime.Substring(4, 2)));
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Converts to a DateTime.
        /// </summary>
        /// <param name="numericDate">The numeric 8 digit date.</param>
        /// <param name="numericTime">The numeric time.</param>
        /// <param name="timeLength">Length of the time field (4 or 6).</param>
        /// <returns></returns>
        static public DateTime ConvertToDateTime(decimal numericDate, decimal numericTime, int timeLength)
        {
            string strNumericDate = numericDate.ToString();
            string strNumericTime = String.Empty;

            // Catch zero dates being passed
            if (numericDate == 0 && numericTime == 0)
            {
                return DateTime.MinValue;
            }

            if (timeLength == 4)
            {
                strNumericTime = String.Format("{0:0000}", numericTime);
                //strNumericTime = numericTime.ToString("D4");
                try
                {
                    return new DateTime(Convert.ToInt32(strNumericDate.Substring(0, 4)),
                                        Convert.ToInt32(strNumericDate.Substring(4, 2)),
                                        Convert.ToInt32(strNumericDate.Substring(6, 2)),
                                        Convert.ToInt32(strNumericTime.Substring(0, 2)),
                                        Convert.ToInt32(strNumericTime.Substring(2, 2)),
                                        0);
                }
                catch (ArgumentOutOfRangeException)
                {
                    return new DateTime(Convert.ToInt32(strNumericDate.Substring(0, 4)),
                                        Convert.ToInt32(strNumericDate.Substring(4, 2)),
                                        Convert.ToInt32(strNumericDate.Substring(6, 2)),
                                        0, 0, 0);
                }
            }
            else if (timeLength == 6)
            {
                strNumericTime = String.Format("{0:000000}", numericTime);
                //strNumericTime = numericTime.ToString("D6");
                return new DateTime(Convert.ToInt32(strNumericDate.Substring(0, 4)),
                                    Convert.ToInt32(strNumericDate.Substring(4, 2)),
                                    Convert.ToInt32(strNumericDate.Substring(6, 2)),
                                    Convert.ToInt32(strNumericTime.Substring(0, 2)),
                                    Convert.ToInt32(strNumericTime.Substring(2, 2)),
                                    Convert.ToInt32(strNumericTime.Substring(4, 2)));
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Converts to a numeric date.
        /// </summary>
        /// <param name="dateTime">The DateTime.</param>
        /// <returns></returns>
        static public int ConvertToNumericDate(DateTime dateTime)
        {
            if (dateTime.Date == new DateTime(1, 1, 1))
            {
                return 0;
            }
            return dateTime.Year * 10000 + dateTime.Month * 100 + dateTime.Day;
        }

        /// <summary>
        /// Converts to numeric time.
        /// </summary>
        /// <param name="dateTime">The DateTime.</param>
        /// <param name="timeLength">Length of the time field (4 or 6).</param>
        /// <returns></returns>
        static public int ConvertToNumericTime(DateTime dateTime, int timeLength)
        {
            if (timeLength == 6)
            {
                return dateTime.Hour * 10000 + dateTime.Minute * 100 + dateTime.Second;
            }
            else if (timeLength == 4)
            {
                return dateTime.Hour * 100 + dateTime.Minute;
            }
            else
            {
                return 0;
            }
        }
    }
}
