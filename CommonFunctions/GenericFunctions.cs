using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Reflection;

namespace CommonFunctions
{
    public static class GenericFunctions
    {

        /// <summary>
        ///  pass DataTable in parameter and get data in list type model
        /// </summary>
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    T item = ConvertDataRow<T>(row);
                    data.Add(item);
                }
                catch (Exception ex) { }
            }
            return data;
        }

        /// <summary>
        ///  pass DataRow in parameter and get data in model
        /// </summary>
        public static T ConvertDataRow<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                try
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        try
                        {
                            if (pro.Name == column.ColumnName)
                            {
                                var vvv = dr[column.ColumnName];
                                if (dr[column.ColumnName] is System.DBNull)
                                {
                                    // pro.SetValue(obj, string.Empty, null);
                                }
                                else
                                {
                                    pro.SetValue(obj, dr[column.ColumnName], null);
                                }
                            }
                            else
                            { continue; }
                        }
                        catch (Exception ex) { }
                    }
                }
                catch (Exception ex) { }
            }
            return obj;
        }

        /// <summary>
        /// convert Datatable to list of datarow
        /// </summary>
        public static List<DataRow> ConvertDataTableToListDataRow(DataTable dt)
        {
            List<DataRow> data = new List<DataRow>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    data.Add(item);
                }
            }
            return data;
        }


        /// <summary>
        /// convert datatable to Dynamic list
        /// </summary>
        public static List<dynamic> ConvertDataTableToDynamicList(this DataTable dt)
        {
            List<dynamic> dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            return dynamicDt;
        }

        /// <summary>
        /// pass list type model in parameter and get datatable
        /// </summary>
        public static DataTable ConvertLINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            // column names 
            PropertyInfo[] oProps = null;
            if (varlist == null) return dtReturn;
            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                        (rec, null);
                }
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }


        public static DataTable search_in_all_columns(DataTable dtobj, string searchpram)
        {
            DataTable dt = new DataTable();
            dt = dtobj.Copy();
            dt.Rows.Clear();
            if (!string.IsNullOrEmpty(searchpram) && !string.IsNullOrEmpty(searchpram.Trim()))
            {
                searchpram = searchpram.ToUpper().Trim();
                if (dtobj != null && dtobj.Rows.Count > 0)
                {
                    for (int r = 0; r < dtobj.Rows.Count; r++)
                    {
                        bool goin = false;
                        for (int c = 0; c < dtobj.Columns.Count; c++)
                        {
                            if (goin == false && (dtobj.Rows[r][c].ToString().ToUpper()).Contains(searchpram))
                            {
                                dt.Rows.Add(dtobj.Rows[r].ItemArray);
                                goin = true;
                            }
                        }
                    }
                }
            }
            int dtco = dt.Rows.Count;
            int dtobjco = dtobj.Rows.Count;
            return dt;
        }


    }
}
