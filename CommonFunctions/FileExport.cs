using System;
using System.IO;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Text;

namespace CommonFunctions
{
    public static class FileExport
    {

        public static string ExcelExport(DataTable dtData, string FilePath)
        {
            string ret = "";

            if (dtData != null && dtData.Rows.Count > 0 && !string.IsNullOrEmpty(FilePath))
            {
                string SheetName = "sheet1";
                if (dtData.TableName != null && dtData.TableName.Trim() != "")
                {
                    SheetName = dtData.TableName;
                }


                FileInfo file = new FileInfo(FilePath);
                var memory = new MemoryStream();
                using (var fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook;
                    workbook = new XSSFWorkbook();

                    ISheet excelSheet = workbook.CreateSheet(SheetName);

                    IRow row = excelSheet.CreateRow(0);

                    for (int h = 0; h < dtData.Columns.Count; h++)
                    {
                        row.CreateCell(h).SetCellValue(dtData.Columns[h].ColumnName.ToString().Trim());
                    }

                    for (int r = 0; r < dtData.Rows.Count; r++)
                    {
                        row = excelSheet.CreateRow(r + 1);////one row alredy inserted in form of header
                        for (int c = 0; c < dtData.Columns.Count; c++)
                        {
                            row.CreateCell(c).SetCellValue(dtData.Rows[r][c].ToString());
                        }
                    }

                    workbook.Write(fs);
                }
                using (var stream = new FileStream(FilePath, FileMode.Open))
                {
                    stream.CopyToAsync(memory);
                }
                memory.Position = 0;

                ret = FilePath;
            }

            return ret;
        }

        public static string JsonExport(string dtstr, string FilePath)
        {
            //Get Current Response  
            using (StreamWriter sw = File.CreateText(FilePath))
            {

                sw.WriteLine(dtstr);

            }

            using (StreamReader sr = File.OpenText(FilePath))
            {
                string s = dtstr;
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }


            return FilePath;


        }


        public static string CSVExport(DataTable dtE, string FilePath)
        {

            DataTable dtdata = new DataTable();
            dtdata = dtE;
            var sb = new StringBuilder();


            for (int i = 0; i < dtdata.Rows.Count; i++)
            {
                string str = "";
                for (int j = 0; j < dtdata.Columns.Count; j++)
                {
                    if (str == "")
                    {
                        str = dtdata.Rows[i][j].ToString();
                    }
                    else
                    {
                        str = str + "," + dtdata.Rows[i][j].ToString();

                    }
                }
                sb.AppendFormat(str, Environment.NewLine).AppendLine();
            }


            //Get Current Response  
            using (StreamWriter sw = File.CreateText(FilePath))
            {

                sw.WriteLine(sb);

            }

            using (StreamReader sr = File.OpenText(FilePath))
            {
                string s = sb.ToString();
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }
            return FilePath;
        }


        public static string XMLExport(DataTable dtE, string FilePath)
        {


            System.IO.StringWriter writer = new System.IO.StringWriter();
            dtE.WriteXml(writer, XmlWriteMode.WriteSchema, false);
            string result = writer.ToString();

            using (StreamWriter sw = File.CreateText(FilePath))
            {

                sw.WriteLine(result);

            }

            using (StreamReader sr = File.OpenText(FilePath))
            {
                string s = result;
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }
            return FilePath;
        }


    }
}
