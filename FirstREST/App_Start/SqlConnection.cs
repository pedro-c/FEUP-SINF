using System.Web;
using System.Web.Mvc;

public class SqlConnection
{
     string connectionString = GetConnectionString() ;  
     
    protected void load_db(){
      try{

          var connection = new System.Data.SqlClient.SqlConnection(connectionString);

          using (connection){
              connection.Open();
          }
          System.Diagnostics.Debug.Write("True");

      }
      catch (System.Exception e){
           System.Diagnostics.Debug.Write("True");
      }
	
    }

    static private string GetConnectionString(){
        return "Server=USER-PC\PRIMAVERA;Database=staging;Integrated Security= SSPI";
    }
}