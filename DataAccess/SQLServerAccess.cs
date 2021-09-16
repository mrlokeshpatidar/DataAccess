using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataAccess
{
    public class SQLServerAccess
    {

        protected const int DBCONN_TIMEOUT = 30;
        private string m_strDBServer = "example\\SQLSERVER2014";
        private string m_strDBName = "BasicWebApp";
        private string m_strDBID = "sa";
        private string m_strDBPwd = "*******";
        private string m_strDBPort = "1433";
        private bool m_bIsConnected = false;

        protected SqlConnection m_dbConnection = null;

        protected SqlTransaction m_trans = null;

        protected static SQLServerAccess s_instance = null;

        public static SQLServerAccess getInstance()
        {
            if (s_instance == null)
            {
                s_instance = new SQLServerAccess();
                s_instance.Connect();
            }
            return s_instance;
        }
        public string DBServer
        {
            get
            {
                return m_strDBServer;
            }
            set
            {
                m_strDBServer = value;
            }
        }

        public string DBName
        {
            get
            {
                return m_strDBName;
            }
            set
            {
                m_strDBName = value;
            }
        }

        public string DBID
        {
            get
            {
                return m_strDBID;
            }
            set
            {
                m_strDBID = value;
            }
        }

        public string DBPwd
        {
            get
            {
                return m_strDBPwd;
            }
            set
            {
                m_strDBPwd = value;
            }
        }

        public string DBPort
        {
            get
            {
                return m_strDBPort;
            }
            set
            {
                m_strDBPort = value;
            }
        }

        public bool IsConnected
        {
            get
            {
                return m_bIsConnected;
            }
            set
            {
                m_bIsConnected = value;
            }
        }

        public virtual void Connect()
        {
            if (IsConnected)
            {
                throw new Exception("Service already is connected in database.");
            }
            else
            {
                string strTemp;

                strTemp = "Connection Timeout=" + DBCONN_TIMEOUT +
                    ";Password=" + this.DBPwd +
                    ";Persist Security Info=True;User ID=" + this.DBID +
                    ";Initial Catalog=" + this.DBName +
                    ";Data Source=" + this.DBServer;


                try
                {
                    if (m_dbConnection != null)
                    {
                        m_dbConnection.Close();
                        m_dbConnection.Dispose();
                    }
                    m_dbConnection = new SqlConnection(strTemp);
                    m_dbConnection.Open();
                }
                catch (Exception ex)
                {
                    string strErrMsg = string.Format("Service couldn't connect in database server.");
                    m_dbConnection = null;
                    throw new Exception(strErrMsg);
                }
                this.IsConnected = true;
            }
        }

        public virtual void Disconnect()
        {
            if (this.IsConnected)
            {
                m_dbConnection.Close();
                m_dbConnection.Dispose();
                m_dbConnection = null;
                this.IsConnected = false;
            }
        }



        public virtual string GetDataBaseBakup(string filePath)
        {
            string ret = "";

            Server srv = new Server(DBServer);

            ///////// really you would get these from config or elsewhere:
            //srv.ConnectionContext.Login = "sa";
            //srv.ConnectionContext.Password = "Password@123";
            //srv.ConnectionContext.ServerInstance = "NILESH\\SQLSERVER2014";
            string dbName = DBName;

            Database db = new Database();
            db = srv.Databases[dbName];

            StringBuilder sb = new StringBuilder();

            foreach (Table tbl in db.Tables)
            {
                //// https://stackoverflow.com/questions/11658143/how-to-set-smo-scriptingoptions-to-guarantee-exact-copy-of-table
                ScriptingOptions options = new ScriptingOptions();
                //options.ClusteredIndexes = true;
                //options.Default = true;
                //options.DriAll = true;
                //options.Indexes = true;
                //options.IncludeHeaders = true;

                //   options.Indexes = true;

                options.ScriptData = true;



                // StringCollection coll = tbl.Script(options);
                IEnumerable<string> coll = tbl.EnumScript(options);
                foreach (string str in coll)
                {
                    sb.Append(str);
                    sb.Append(Environment.NewLine);
                }
            }
            System.IO.StreamWriter fs = System.IO.File.CreateText(filePath);
            fs.Write(sb.ToString());
            fs.Close();

            ret = filePath;

            return ret;
        }

        public virtual string GetDataBaseBakup(string filePath, string TableName)
        {
            string ret = "";

            Server srv = new Server(DBServer);

            string dbName = DBName;

            Database db = new Database();
            db = srv.Databases[dbName];

            StringBuilder sb = new StringBuilder();

            foreach (Table tbl in db.Tables)
            {
                if (tbl.Name.ToLower() == TableName.Trim().ToLower())
                {
                    ScriptingOptions options = new ScriptingOptions();

                    options.ScriptData = true;

                    IEnumerable<string> coll = tbl.EnumScript(options);
                    foreach (string str in coll)
                    {
                        sb.Append(str);
                        sb.Append(Environment.NewLine);
                    }
                }
            }
            System.IO.StreamWriter fs = System.IO.File.CreateText(filePath);
            fs.Write(sb.ToString());
            fs.Close();

            ret = filePath;

            return ret;
        }




        public virtual void RunCreateTable(string strQuery)
        {
            if (!this.IsConnected)
            {
                throw new Exception("Service couldn't connect in database.");
            }
            try
            {
                SqlCommand cmd = new SqlCommand(strQuery, m_dbConnection);
                ///  m_dbConnection.Open();
                cmd.ExecuteNonQuery();
                ///  m_dbConnection.Close();
            }
            catch (SqlException ex)
            {
                string strMsg = string.Format("Query error:{0} {1}", strQuery, ex.Message);
                throw new Exception(strMsg);
            }
            catch (Exception ex)
            {
                string strMsg = string.Format("Error:{0}", ex.Message);
                throw new Exception(strMsg);
            }
        }

        public virtual DataSet RunSelectQuery(string strParamQuery, List<string> strParamNames, List<object> paramValues)
        {
            if (strParamNames == null && paramValues == null)
                return RunSelectQuery(strParamQuery);

            if (!this.IsConnected)
            {
                throw new Exception("Service couldn't connect in database.");
            }

            if (strParamNames.Count != paramValues.Count)
            {
                throw new Exception("Parameter names and values are not mapped in SelectQuery.");
            }

            string strQuery = "";
            try
            {
                DataSet dataset = new DataSet();
                lock (this)
                {
                    strQuery = string.Format(strParamQuery, strParamNames.ToArray());

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    SqlCommand sqlCommand = new SqlCommand(strQuery, m_dbConnection);
                    sqlCommand.Transaction = m_trans;

                    for (int i = 0; i < strParamNames.Count; i++)
                    {
                        sqlCommand.Parameters.AddWithValue(strParamNames[i], paramValues[i]);
                    }
                    adapter.SelectCommand = sqlCommand;
                    adapter.Fill(dataset);
                }
                return dataset;
            }
            catch (SqlException e)
            {
                string strMsg = string.Format("Query error:{0} {1}", strQuery, e.Message);
                throw new Exception(strMsg);
            }
            catch (Exception e)
            {
                string strMsg = string.Format("Error:{0}", e.Message);
                throw new Exception(strMsg);
            }
        }

        public virtual DataSet RunSelectQuery(string strQuery)
        {
            if (!this.IsConnected)
            {
                throw new Exception("Service couldn't connect in database.");
            }
            try
            {
                DataSet dataset = new DataSet();
                lock (this)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();

                    adapter.SelectCommand = new SqlCommand(strQuery, m_dbConnection);
                    adapter.SelectCommand.Transaction = m_trans;
                    adapter.Fill(dataset);
                }
                return dataset;
            }
            catch (SqlException ex)
            {
                string strMsg = string.Format("Query error:{0} {1}", strQuery, ex.Message);
                throw new Exception(strMsg);
            }
            catch (Exception ex)
            {
                string strMsg = string.Format("Error:{0}", ex.Message);
                throw new Exception(strMsg);
            }
        }


        public virtual DataTable RunSelectQueryWithdatatable(string strQuery)
        {
            if (!this.IsConnected)
            {
                throw new Exception("Service couldn't connect in database.");
            }
            try
            {
                DataTable dataset = new DataTable();
                lock (this)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();

                    adapter.SelectCommand = new SqlCommand(strQuery, m_dbConnection);
                    adapter.SelectCommand.Transaction = m_trans;
                    adapter.Fill(dataset);
                }
                return dataset;
            }
            catch (SqlException ex)
            {
                string strMsg = string.Format("Query error:{0} {1}", strQuery, ex.Message);
                throw new Exception(strMsg);
            }
            catch (Exception ex)
            {
                string strMsg = string.Format("Error:{0}", ex.Message);
                throw new Exception(strMsg);
            }
        }

        public virtual void RunUpdateQuery(string strParamQuery, List<string> strParamNames, List<object> paramValues)
        {
            if (!this.IsConnected)
            {
                throw new Exception("Service couldn't connect in database.");
            }
            if (strParamNames.Count != paramValues.Count)
            {
                throw new Exception("it doesn't match in UpdateQuery.");
            }
            string strQuery = "";
            try
            {
                lock (this)
                {
                    strQuery = string.Format(strParamQuery, strParamNames.ToArray());

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    SqlCommand sqlCommand = new SqlCommand(strQuery, m_dbConnection);

                    for (int i = 0; i < strParamNames.Count; i++)
                    {
                        sqlCommand.Parameters.AddWithValue(strParamNames[i], paramValues[i]);
                    }
                    sqlCommand.Transaction = m_trans;
                    adapter.UpdateCommand = sqlCommand;
                    adapter.UpdateCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("Query error:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("Error:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual void RunUpdateQueryForBinary(string strParamQuery, string strParamNames, byte[] paramValues)
        {
            if (!this.IsConnected)
            {
                throw new Exception("Service couldn't connect in database.");
            }
            string strQuery = "";
            try
            {
                lock (this)
                {
                    strQuery = string.Format(strParamQuery, strParamNames);
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    SqlCommand sqlCommand = new SqlCommand(strQuery, m_dbConnection);

                    sqlCommand.Parameters.Add(strParamNames, SqlDbType.Binary, paramValues.Length).Value = paramValues;
                    sqlCommand.Transaction = m_trans;
                    adapter.UpdateCommand = sqlCommand;
                    adapter.UpdateCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("Query error:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("Error:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual void RunUpdateQuery(string strQuery)
        {
            if (!this.IsConnected)
            {
                throw new Exception("Service couldn't connect in database.");
            }
            try
            {
                lock (this)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.UpdateCommand = new SqlCommand(strQuery, m_dbConnection);
                    adapter.UpdateCommand.Transaction = m_trans;
                    adapter.UpdateCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("Query error:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("Error:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual void RunDeleteQuery(string strParamQuery, List<string> strParamNames, List<object> paramValues)
        {
            if (!this.IsConnected)
            {
                throw new Exception("Service couldn't connect in database.");
            }
            if (strParamNames.Count != paramValues.Count)
            {
                throw new Exception("Parameter names and values are not maptched in DeleteQuery.");
            }
            string strQuery = "";
            try
            {
                lock (this)
                {
                    strQuery = string.Format(strParamQuery, strParamNames.ToArray());
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    SqlCommand sqlCommand = new SqlCommand(strQuery, m_dbConnection);
                    for (int i = 0; i < strParamNames.Count; i++)
                    {
                        sqlCommand.Parameters.AddWithValue(strParamNames[i], paramValues[i]);
                    }
                    sqlCommand.Transaction = m_trans;
                    adapter.DeleteCommand = sqlCommand;
                    adapter.DeleteCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("Query error:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("Error:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual void RunDeleteQuery(string strQuery)
        {
            if (!this.IsConnected)
            {
                throw new Exception("Service couldn't connect in database.");
            }
            try
            {
                lock (this)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.DeleteCommand = new SqlCommand(strQuery, m_dbConnection);
                    adapter.DeleteCommand.Transaction = m_trans;
                    adapter.DeleteCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("Query error:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("Error:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual void RunNonQuery(string strParamQuery, List<string> strParamNames, List<object> paramValues)
        {
            if (!this.IsConnected)
            {
                throw new Exception("Service couldn't connect in database.");
            }

            if (strParamNames.Count != paramValues.Count)
            {
                throw new Exception("Parameter names and values are not mapped in NonQuery.");
            }

            string strQuery = "";
            try
            {
                lock (this)
                {
                    strQuery = string.Format(strParamQuery, strParamNames.ToArray());
                    SqlCommand sqlCommand = new SqlCommand(strQuery, m_dbConnection);
                    for (int i = 0; i < strParamNames.Count; i++)
                    {
                        sqlCommand.Parameters.AddWithValue(strParamNames[i], paramValues[i]);
                    }
                    sqlCommand.Transaction = m_trans;
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("Query error:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("Error:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual void RunNonQuery(string strQuery)
        {
            if (!this.IsConnected)
            {
                throw new Exception("Service couldn't connect in database.");
            }
            try
            {
                lock (this)
                {
                    SqlCommand sqlCommand = new SqlCommand(strQuery, m_dbConnection);
                    sqlCommand.Transaction = m_trans;
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("Query error:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("Error:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual void RunNonQueryNoLimitTime(string strQuery)
        {
            if (!this.IsConnected)
            {
                throw new Exception("Service couldn't connect in database.");
            }
            try
            {
                lock (this)
                {
                    SqlCommand sqlCommand = new SqlCommand(strQuery, m_dbConnection);
                    sqlCommand.Transaction = m_trans;
                    sqlCommand.CommandTimeout = 0;
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("Query error:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("Error:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }


        public virtual long RunInsertQuery(string strParamQuery, List<string> strParamNames, List<object> paramValues, bool bReturnID)
        {
            string strQuery = "";
            try
            {
                if (bReturnID == true)
                {
                    strParamQuery = strParamQuery.Trim();
                    if (strParamQuery[strParamQuery.Length - 1] != ';')
                        strQuery += ";";
                    strParamQuery = string.Format("{0} SELECT @@IDENTITY", strParamQuery);

                    DataSet dsRet = this.RunSelectQuery(strParamQuery, strParamNames, paramValues);
                    return DataSetUtil.GetID(dsRet);
                }
                else
                {
                    if (!this.IsConnected)
                    {
                        throw new Exception("Service couldn't connect in database.");
                    }

                    if (strParamNames.Count != paramValues.Count)
                    {
                        string strErrMsg = "Parameter names and values are not match in InsertQuery.";
                        throw new Exception(strErrMsg);
                    }

                    long nRows = 0;
                    lock (this)
                    {
                        strQuery = string.Format(strParamQuery, strParamNames.ToArray());
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        SqlCommand sqlCommand = new SqlCommand(strQuery, m_dbConnection);
                        for (int i = 0; i < strParamNames.Count; i++)
                        {
                            sqlCommand.Parameters.AddWithValue(strParamNames[i], paramValues[i]);
                        }
                        sqlCommand.Transaction = m_trans;
                        adapter.InsertCommand = sqlCommand;
                        nRows = adapter.InsertCommand.ExecuteNonQuery();
                    }
                    return nRows;
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("Query error:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("Error:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual long RunInsertQuery(string strParamQuery, List<string> strParamNames, List<object> paramValues)
        {
            return RunInsertQuery(strParamQuery, strParamNames, paramValues, false);
        }

        public virtual long RunInsertQuery(string strQuery, bool bReturnID)
        {
            try
            {
                if (bReturnID == true)
                {
                    strQuery = strQuery.Trim();
                    if (strQuery[strQuery.Length - 1] != ';')
                        strQuery += ";";
                    strQuery = string.Format("{0} SELECT @@IDENTITY", strQuery);
                    DataSet dsRet = this.RunSelectQuery(strQuery);
                    return DataSetUtil.GetID(dsRet);
                }
                else
                {
                    if (!this.IsConnected)
                    {
                        throw new Exception("Service couldn't connect in database.");
                    }

                    long nRows = 0;
                    lock (this)
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.InsertCommand = new SqlCommand(strQuery, m_dbConnection);
                        adapter.InsertCommand.Transaction = m_trans;
                        nRows = adapter.InsertCommand.ExecuteNonQuery();
                    }
                    return nRows;
                }
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("Query error:{0} {1}", strQuery, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("Error:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual long RunInsertQuery(string strQuery)
        {
            return RunInsertQuery(strQuery, false);
        }

        public virtual void BeginTrans()
        {
            if (!this.IsConnected)
            {
                throw new Exception("Service couldn't connect in database.");
            }

            if (m_trans != null)
                EndTrans();

            try
            {
                m_trans = m_dbConnection.BeginTransaction();
            }
            catch (System.Exception ex)
            {
                string strErrMsg = string.Format("Error:{0}", ex.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual void BeginTrans(IsolationLevel iso)
        {
            if (!this.IsConnected)
            {
                throw new Exception("Service couldn't connect in database.");
            }

            if (m_trans != null)
                EndTrans();

            try
            {
                m_trans = m_dbConnection.BeginTransaction(iso);
            }
            catch (System.Exception ex)
            {
                string strErrMsg = string.Format("Error:{0}", ex.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual void EndTrans()
        {
            if (m_trans == null)
                return;

            try
            {
                m_trans.Commit();
                m_trans = null;
            }
            catch (System.Exception ex)
            {
                string strErrMsg = string.Format("Error:{0}", ex.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual void RollbackTrans()
        {
            if (m_trans == null)
                return;

            try
            {
                m_trans.Rollback();
                m_trans = null;
            }
            catch (System.Exception ex)
            {
                string strErrMsg = string.Format("Error:{0}", ex.Message);
                throw new Exception(strErrMsg);
            }
        }

        public virtual SqlParameter MakeParam(string strParamName, ParameterDirection pdDirection, object objParamValue)
        {
            SqlParameter retParam = new SqlParameter(strParamName, objParamValue);
            retParam.Direction = pdDirection;

            return retParam;
        }

        public virtual SqlParameter MakeParam(string strParamName, SqlDbType paramType, ParameterDirection pdDirection, int iLen, object objParamValue)
        {
            SqlParameter retParam = new SqlParameter(strParamName, objParamValue);
            retParam.SqlDbType = paramType;
            retParam.Size = iLen;
            retParam.Direction = pdDirection;

            return retParam;
        }


        public DataSet RunSelectQuery(string p1, string[] p2, string[] p3, object[] p4)
        {
            throw new NotImplementedException();
        }


        public virtual DataSet RunStoreProcedure(string strSPName, List<string> strParamNames, List<object> paramValues)
        {
            if (!this.IsConnected)
            {
                throw new Exception("Service couldn't connect in database.");
            }
            try
            {
                DataSet dataset = new DataSet();
                lock (this)
                {
                    SqlCommand selectCommand = new SqlCommand(strSPName, m_dbConnection);
                    selectCommand.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < strParamNames.Count; i++)
                    {
                        if (paramValues[i] != null && Type.GetTypeCode(paramValues[i].GetType()) == TypeCode.String)
                            paramValues[i] = getUnicodeString(paramValues[i]);

                        selectCommand.Parameters.AddWithValue(strParamNames[i], paramValues[i]);
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                    adapter.Fill(dataset);
                }
                return dataset;
            }
            catch (SqlException e)
            {
                string strErrMsg = string.Format("Query error:{0} {1}", strSPName, e.Message);
                throw new Exception(strErrMsg);
            }
            catch (Exception e)
            {
                string strErrMsg = string.Format("Error:{0}", e.Message);
                throw new Exception(strErrMsg);
            }
        }

        private string getUnicodeString(object objSrc)
        {
            if (objSrc == null)
                return "";

            if (string.IsNullOrEmpty(objSrc.ToString()))
                return "";

            return new string(Encoding.Unicode.GetChars(Encoding.Convert(Encoding.UTF8, Encoding.Unicode, Encoding.UTF8.GetBytes(objSrc.ToString()))));
        }


    }
}
