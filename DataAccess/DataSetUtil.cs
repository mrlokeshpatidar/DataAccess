using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace DataAccess
{
    public class DataSetUtil
    {
        public static Boolean DataSetNullOrEmpty(DataSet ds)
        {
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static long GetID(DataSet ds)
        {
            return long.Parse(ds.Tables[0].Rows[0][0].ToString());
        }

        public static ulong GetCount(DataSet ds)
        {
            return ulong.Parse(ds.Tables[0].Rows[0][0].ToString());
        }

        public static string GetStringData(DataSet ds)
        {
            if (IsNullOrEmpty(ds))
                return "";

            return ds.Tables[0].Rows[0][0].ToString();
        }

        public static bool IsNullOrEmpty(DataSet ds)
        {
            if (ds == null)
                return true;

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Count > 0)
                return false;

            return true;
        }

        public static object RowValue(DataSet ds, string col, int row)
        {
            if (IsNullOrEmpty(ds))
                return null;

            return ds.Tables[0].Rows[row][col];
        }

        public static object RowValue(DataSet ds, int col, int row)
        {
            if (IsNullOrEmpty(ds))
                return null;

            return ds.Tables[0].Rows[row][col];
        }

        public static string RowStringValue(DataSet ds, string col, int row)
        {
            if (IsNullOrEmpty(ds))
                return null;

            object obj = ds.Tables[0].Rows[row][col];
            return (obj == null) ? "" : obj.ToString().Trim();
        }
        public static string RowStringValue(DataSet ds, int col, int row)
        {
            if (IsNullOrEmpty(ds))
                return null;

            object obj = ds.Tables[0].Rows[row][col];
            return (obj == null) ? "" : obj.ToString().Trim();
        }

        public static Boolean RowBoolValue(DataSet ds, string col, int row)
        {
            if (IsNullOrEmpty(ds))
                return false;

            object obj = ds.Tables[0].Rows[row][col];
            return (obj == null) ? false : Convert.ToBoolean(obj.ToString().Trim());
        }
        public static Boolean RowBoolValue(DataSet ds, int col, int row)
        {
            if (IsNullOrEmpty(ds))
                return false;

            object obj = ds.Tables[0].Rows[row][col];
            return (obj == null) ? false : Convert.ToBoolean(obj.ToString().Trim());
        }

        public static string RowStringValue(DataSet ds, string col, int row, string defaultValue)
        {
            if (IsNullOrEmpty(ds))
                return null;

            object obj = ds.Tables[0].Rows[row][col];
            return (obj == null) ? defaultValue : obj.ToString();
        }
        public static string RowStringValue(DataSet ds, int col, int row, string defaultValue)
        {
            if (IsNullOrEmpty(ds))
                return null;

            object obj = ds.Tables[0].Rows[row][col];
            return (obj == null) ? defaultValue : obj.ToString();
        }

        public static string RowDateTimeValue(DataSet ds, string col, int row)
        {
            if (IsNullOrEmpty(ds))
                return "";

            DateTime dtRet = DateTime.Now;
            object obj = ds.Tables[0].Rows[row][col];

            if (obj == null)
                return "";

            if (!DateTime.TryParse(obj.ToString(), out dtRet))
                return "";

            return dtRet.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string RowDateTimeValue(DataSet ds, int col, int row)
        {
            if (IsNullOrEmpty(ds))
                return "";

            DateTime dtRet = DateTime.Now;
            object obj = ds.Tables[0].Rows[row][col];

            if (obj == null)
                return "";

            if (!DateTime.TryParse(obj.ToString(), out dtRet))
                return "";

            return dtRet.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string RowDateTimeValue(DataSet ds, string col, int row, string formatString)
        {
            if (IsNullOrEmpty(ds))
                return "";

            DateTime dtRet = DateTime.Now;
            object obj = ds.Tables[0].Rows[row][col];

            if (obj == null)
                return "";

            if (!DateTime.TryParse(obj.ToString(), out dtRet))
                return "";

            return dtRet.ToString(formatString);
        }
        public static string RowDateTimeValue(DataSet ds, int col, int row, string formatString)
        {
            if (IsNullOrEmpty(ds))
                return "";

            DateTime dtRet = DateTime.Now;
            object obj = ds.Tables[0].Rows[row][col];

            if (obj == null)
                return "";

            if (!DateTime.TryParse(obj.ToString(), out dtRet))
                return "";

            return dtRet.ToString(formatString);
        }

        public static int RowIntValue(DataSet ds, string col, int row)
        {
            if (IsNullOrEmpty(ds))
                return -1;

            object obj = ds.Tables[0].Rows[row][col];
            if (obj == null)
                return -1;

            int iRet = 0;
            if (!int.TryParse(obj.ToString(), out iRet))
                return -1;

            return iRet;
        }
        public static int RowIntValue(DataSet ds, int col, int row)
        {
            if (IsNullOrEmpty(ds))
                return -1;

            object obj = ds.Tables[0].Rows[row][col];
            if (obj == null)
                return -1;

            int iRet = 0;
            if (!int.TryParse(obj.ToString(), out iRet))
                return -1;

            return iRet;
        }

        public static long RowLongValue(DataSet ds, string col, int row)
        {
            if (IsNullOrEmpty(ds))
                return 0;

            object obj = ds.Tables[0].Rows[row][col];
            if (obj == null)
                return 0;

            long lRet = 0;
            if (!long.TryParse(obj.ToString(), out lRet))
                return 0;

            return lRet;
        }
        public static long RowLongValue(DataSet ds, int col, int row)
        {
            if (IsNullOrEmpty(ds))
                return 0;

            object obj = ds.Tables[0].Rows[row][col];
            if (obj == null)
                return 0;

            long lRet = 0;
            if (!long.TryParse(obj.ToString(), out lRet))
                return 0;

            return lRet;
        }

        public static double RowDoubleValue(DataSet ds, string col, int row)
        {
            if (IsNullOrEmpty(ds))
                return -1;

            object obj = ds.Tables[0].Rows[row][col];
            if (obj == null)
                return 0;

            double fRet = 0;
            if (!double.TryParse(obj.ToString(), out fRet))
                return -1;

            return fRet;
        }
        public static double RowDoubleValue(DataSet ds, int col, int row)
        {
            if (IsNullOrEmpty(ds))
                return -1;

            object obj = ds.Tables[0].Rows[row][col];
            if (obj == null)
                return 0;

            double fRet = 0;
            if (!double.TryParse(obj.ToString(), out fRet))
                return -1;

            return fRet;
        }

        public static float RowFloatValue(DataSet ds, string col, int row)
        {
            if (IsNullOrEmpty(ds))
                return -1;

            object obj = ds.Tables[0].Rows[row][col];
            if (obj == null)
                return 0;

            float fRet = 0;
            if (!float.TryParse(obj.ToString(), out fRet))
                return -1;

            return fRet;
        }
        public static float RowFloatValue(DataSet ds, int col, int row)
        {
            if (IsNullOrEmpty(ds))
                return -1;

            object obj = ds.Tables[0].Rows[row][col];
            if (obj == null)
                return 0;

            float fRet = 0;
            if (!float.TryParse(obj.ToString(), out fRet))
                return -1;

            return fRet;
        }

        public static byte[] RowByteArrayValue(DataSet ds, string col, int row)
        {
            if (IsNullOrEmpty(ds))
                return null;

            object obj = ds.Tables[0].Rows[row][col];

            //BinaryFormatter bf = new BinaryFormatter();
            //MemoryStream ms = new MemoryStream();
            //bf.Serialize(ms, obj);

            return (obj == null) ? null : (byte[])obj;// ms.ToArray();
        }

        public static byte[] RowByteArrayValue(DataSet ds, int col, int row)
        {
            if (IsNullOrEmpty(ds))
                return null;

            object obj = ds.Tables[0].Rows[row][col];

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return (obj == null) ? null : ms.ToArray();
        }

        public static int RowCount(DataSet ds)
        {
            if (IsNullOrEmpty(ds))
                return 0;

            return ds.Tables[0].Rows.Count;
        }

    }
}
