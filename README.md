<h1># DataAccess</h1>
Standard data access layer for MS SQL Server Query execution in C#

<hr/>
<h2>##Get Table All Data </h2> 
  DataSet ds = SQLServerDBConn.RunSelectQuery("select * from TABLENAME ");

<hr/>
<h2>##Get Data By ID   </h2>  
  DataSet ds = SQLServerDBConn.RunSelectQuery("select * from TABLENAME where id={0}", new List<string>() { "@id" }, new List<object>() { id });
     
<hr/>
<h2>##Create New Record  </h2>          
  SQLServerDBConn.RunInsertQuery("insert into TABLENAME(Name,Address) values({0}, {1})", new List<string>() { "@Name", "@Address" }, new List<object>() { Model.Name, Model.Address });
     
<hr/>  
<h2>##Update Record   </h2> 
  SQLServerDBConn.RunUpdateQuery("Update TABLENAME set Name={0}, Address={1} where id={2} ", new List<string>() { "@Name", "@Address", "@id" }, new List<object>() { Model.Name, Model.Address, id });
     
<hr/>  
<h2>##Delete Record   </h2> 
  SQLServerDBConn.RunDeleteQuery("Detete From TABLENAME where id={0}", new List<string>() { "@id" }, new List<object>() { id });
    
