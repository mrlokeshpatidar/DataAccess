using ExcelDataReader;
using System;
using System.Data;
using System.IO;


namespace CommonFunctions
{
    public class FileImport
    {

        public static DataTable ReadExcel(string file_path)
        {
            DataTable dt = new DataTable();
            int column_count = 0;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(file_path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        while (reader.Read()) //Each ROW
                        {
                            DataRow dr = dt.NewRow();
                            column_count = reader.FieldCount;
                            for (int column = 0; column < column_count; column++)
                            {

                                if (dt.Columns.Count == 0 || dt.Columns.Count < column_count)
                                {
                                    string col_name = "";
                                    if (reader.GetValue(column) != null && !string.IsNullOrEmpty(reader.GetValue(column).ToString().Trim()) && !dt.Columns.Contains(reader.GetValue(column).ToString().Trim()))
                                    {
                                        col_name = reader.GetValue(column).ToString().Trim().ToLower();
                                    }
                                    else
                                    {
                                        col_name = "col_" + column;
                                    }
                                    dt.Columns.Add(col_name);

                                    Console.Write(reader.GetValue(column) + "   ");
                                }
                                else
                                {
                                    //Console.WriteLine(reader.GetString(column));//Will blow up if the value is decimal etc. 
                                    // Console.WriteLine(reader.GetValue(column));//Get Value returns object
                                    dr[column] = reader.GetValue(column);

                                    Console.Write(reader.GetValue(column) + "   ");
                                }
                            }
                            dt.Rows.Add(dr);
                            Console.WriteLine();
                        }
                    } while (reader.NextResult()); //Move to NEXT SHEET
                }
                return dt;
            }
        }



    }
}
