# DataAccess
Standard data access layer for MS SQL Server Query execution in C#

##Get Table All Data  
  DataSet ds = SQLServerDBConn.RunSelectQuery("select * from TABLENAME ");
           
##Get Data By ID   
  DataSet ds = SQLServerDBConn.RunSelectQuery("select * from TABLENAME where id={0}", new List<string>() { "@id" }, new List<object>() { id });
          
##Create New Record          
  SQLServerDBConn.RunInsertQuery("insert into TABLENAME(Name,Address) values({0}, {1})", new List<string>() { "@Name", "@Address" }, new List<object>() { Model.Name, Model.Address });
             
##Update Record  
  SQLServerDBConn.RunUpdateQuery("Update TABLENAME set Name={0}, Address={1} where id={2} ", new List<string>() { "@Name", "@Address", "@id" }, new List<object>() { Model.Name, Model.Address, id });
        
##Delete Record  
  SQLServerDBConn.RunDeleteQuery("Detete From TABLENAME where id={0}", new List<string>() { "@id" }, new List<object>() { id });
    
