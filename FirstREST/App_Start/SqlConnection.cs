using System.Web;
using System.Web.Mvc;


namespace FirstREST
{
    public class SqlConnection
    {
        public static System.Data.SqlClient.SqlConnection connection;
        public static void load_db()
        {

            string connectionString = GetConnectionString();
            
            try
            {
                connection = new System.Data.SqlClient.SqlConnection(connectionString);

                using (connection)
                {
                    connection.Open();
                }
                System.Diagnostics.Debug.WriteLine("Connected");

            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Failed to connect");
                System.Diagnostics.Debug.WriteLine(e);
            }

        }

        static public string GetConnectionString()
        {
            return "Server=USER-PC\\PRIMAVERA;Database=staging;User Id=sa; Password=Feup2014";
        }
    }
}
