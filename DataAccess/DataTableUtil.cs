using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace DataAccess
{
    public class DataTableUtil
    {
        public static long GetID(DataTable dt)
        {
            return long.Parse(dt.Rows[0][0].ToString());
        }

        public static ulong GetCount(DataTable dt)
        {
            return ulong.Parse(dt.Rows[0][0].ToString());
        }

        public static string GetStringData(DataTable dt)
        {
            if (IsNullOrEmpty(dt))
                return "";

            return dt.Rows[0][0].ToString();
        }

        public static bool IsNullOrEmpty(DataTable dt)
        {
            if (dt == null)
                return true;

            if (dt.Rows.Count > 0 && dt.Columns.Count > 0)
                return false;

            return true;
        }

        public static object RowValue(DataTable dt, string col, int row)
        {
            if (IsNullOrEmpty(dt))
                return null;

            return dt.Rows[row][col];
        }

        public static object RowValue(DataTable dt, int col, int row)
        {
            if (IsNullOrEmpty(dt))
                return null;

            return dt.Rows[row][col];
        }

        public static string RowStringValue(DataTable dt, string col, int row)
        {
            if (IsNullOrEmpty(dt))
                return null;

            object obj = dt.Rows[row][col];
            return (obj == null) ? "" : obj.ToString();
        }
        public static string RowStringValue(DataTable dt, int col, int row)
        {
            if (IsNullOrEmpty(dt))
                return null;

            object obj = dt.Rows[row][col];
            return (obj == null) ? "" : obj.ToString();
        }
        public static string RowStringValue(DataTable dt, string col, int row, string defaultValue)
        {
            if (IsNullOrEmpty(dt))
                return null;

            object obj = dt.Rows[row][col];
            return (obj == null) ? defaultValue : obj.ToString();
        }
        public static string RowStringValue(DataTable dt, int col, int row, string defaultValue)
        {
            if (IsNullOrEmpty(dt))
                return null;

            object obj = dt.Rows[row][col];
            return (obj == null) ? defaultValue : obj.ToString();
        }

        public static string RowDateTimeValue(DataTable dt, string col, int row)
        {
            if (IsNullOrEmpty(dt))
                return "";

            DateTime dtRet = DateTime.Now;
            object obj = dt.Rows[row][col];

            if (obj == null)
                return "";

            if (!DateTime.TryParse(obj.ToString(), out dtRet))
                return "";

            return dtRet.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string RowDateTimeValue(DataTable dt, int col, int row)
        {
            if (IsNullOrEmpty(dt))
                return "";

            DateTime dtRet = DateTime.Now;
            object obj = dt.Rows[row][col];

            if (obj == null)
                return "";

            if (!DateTime.TryParse(obj.ToString(), out dtRet))
                return "";

            return dtRet.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string RowDateTimeValue(DataTable dt, string col, int row, string formatString)
        {
            if (IsNullOrEmpty(dt))
                return "";

            DateTime dtRet = DateTime.Now;
            object obj = dt.Rows[row][col];

            if (obj == null)
                return "";

            if (!DateTime.TryParse(obj.ToString(), out dtRet))
                return "";

            return dtRet.ToString(formatString);
        }
        public static string RowDateTimeValue(DataTable dt, int col, int row, string formatString)
        {
            if (IsNullOrEmpty(dt))
                return "";

            DateTime dtRet = DateTime.Now;
            object obj = dt.Rows[row][col];

            if (obj == null)
                return "";

            if (!DateTime.TryParse(obj.ToString(), out dtRet))
                return "";

            return dtRet.ToString(formatString);
        }

        public static int RowIntValue(DataTable dt, string col, int row)
        {
            if (IsNullOrEmpty(dt))
                return -1;

            object obj = dt.Rows[row][col];
            if (obj == null)
                return -1;

            int iRet = 0;
            if (!int.TryParse(obj.ToString(), out iRet))
                return -1;

            return iRet;
        }
        public static int RowIntValue(DataTable dt, int col, int row)
        {
            if (IsNullOrEmpty(dt))
                return -1;

            object obj = dt.Rows[row][col];
            if (obj == null)
                return -1;

            int iRet = 0;
            if (!int.TryParse(obj.ToString(), out iRet))
                return -1;

            return iRet;
        }

        public static long RowLongValue(DataTable dt, string col, int row)
        {
            if (IsNullOrEmpty(dt))
                return 0;

            object obj = dt.Rows[row][col];
            if (obj == null)
                return 0;

            long lRet = 0;
            if (!long.TryParse(obj.ToString(), out lRet))
                return 0;

            return lRet;
        }
        public static long RowLongValue(DataTable dt, int col, int row)
        {
            if (IsNullOrEmpty(dt))
                return 0;

            object obj = dt.Rows[row][col];
            if (obj == null)
                return 0;

            long lRet = 0;
            if (!long.TryParse(obj.ToString(), out lRet))
                return 0;

            return lRet;
        }

        public static double RowDoubleValue(DataTable dt, string col, int row)
        {
            if (IsNullOrEmpty(dt))
                return -1;

            object obj = dt.Rows[row][col];
            if (obj == null)
                return 0;

            double fRet = 0;
            if (!double.TryParse(obj.ToString(), out fRet))
                return -1;

            return fRet;
        }
        public static double RowDoubleValue(DataTable dt, int col, int row)
        {
            if (IsNullOrEmpty(dt))
                return -1;

            object obj = dt.Rows[row][col];
            if (obj == null)
                return 0;

            double fRet = 0;
            if (!double.TryParse(obj.ToString(), out fRet))
                return -1;

            return fRet;
        }

        public static float RowFloatValue(DataTable dt, string col, int row)
        {
            if (IsNullOrEmpty(dt))
                return -1;

            object obj = dt.Rows[row][col];
            if (obj == null)
                return 0;

            float fRet = 0;
            if (!float.TryParse(obj.ToString(), out fRet))
                return -1;

            return fRet;
        }
        public static float RowFloatValue(DataTable dt, int col, int row)
        {
            if (IsNullOrEmpty(dt))
                return -1;

            object obj = dt.Rows[row][col];
            if (obj == null)
                return 0;

            float fRet = 0;
            if (!float.TryParse(obj.ToString(), out fRet))
                return -1;

            return fRet;
        }

        public static byte[] RowByteArrayValue(DataTable dt, string col, int row)
        {
            if (IsNullOrEmpty(dt))
                return null;

            object obj = dt.Rows[row][col];

            //BinaryFormatter bf = new BinaryFormatter();
            //MemoryStream ms = new MemoryStream();
            //bf.Serialize(ms, obj);

            return (obj == null) ? null : (byte[])obj;// ms.ToArray();
        }

        public static byte[] RowByteArrayValue(DataTable dt, int col, int row)
        {
            if (IsNullOrEmpty(dt))
                return null;

            object obj = dt.Rows[row][col];

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return (obj == null) ? null : ms.ToArray();
        }

        public static int RowCount(DataTable dt)
        {
            if (IsNullOrEmpty(dt))
                return 0;

            return dt.Rows.Count;
        }

    }
}
