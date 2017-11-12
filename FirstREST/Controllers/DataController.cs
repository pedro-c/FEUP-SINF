using Interop.GcpBE900;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace FirstREST.Controllers
{
    public class DataController : Controller
    {

        public static XmlDocument saft = new XmlDocument();
        public static string connectionString = FirstREST.SqlConnection.GetConnectionString();        
        
        // GET: /Saft/
        public ActionResult Index()
        {

            readSaft();
            return View();
        }

        public static void readSaft()
        {
            saft.Load("C:\\SINF\\FEUP-SINF\\FirstREST\\SAFT.xml");

            proccessAccounts(saft.GetElementsByTagName("Account"));
        }

        public static void proccessAccounts(XmlNodeList accounts)
        {
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                connection.Open();

                // Drop table
                var dropQuery = "DROP TABLE dbo.Account";
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create table
                var createQuery =
                    "CREATE TABLE [dbo].[Account](" +
	                    "[id] [bigint] NOT NULL," +
	                    "[accountDescription] [nchar](60) NOT NULL," +
	                    "[openingDebitBalance] [nchar](30) NOT NULL," +
	                    "[openingCreditBalance] [nchar](30) NOT NULL," +
	                    "[closingDebitBalance] [nchar](30) NOT NULL," +
	                    "[closingCreditBalance] [nchar](30) NOT NULL," +
                     "CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED " +
                    "(" +
	                "    [id] ASC" +
                    ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" +
                    ") ON [PRIMARY]"          
                ;
                using (var command = new SqlCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                //Populate table
                foreach (XmlNode account in accounts)
                {

                    var query = "INSERT INTO dbo.Account(id,accountDescription,openingDebitBalance,openingCreditBalance,closingDebitBalance,closingCreditBalance) VALUES(@id,@accountDescription,@openingDebitBalance,@openingCreditBalance,@closingDebitBalance,@closingCreditBalance)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", account["AccountID"].InnerText);
                        command.Parameters.AddWithValue("@accountDescription", account["AccountDescription"].InnerText);
                        command.Parameters.AddWithValue("@openingDebitBalance", account["OpeningDebitBalance"].InnerText);
                        command.Parameters.AddWithValue("@openingCreditBalance", account["OpeningCreditBalance"].InnerText);
                        command.Parameters.AddWithValue("@closingDebitBalance", account["ClosingDebitBalance"].InnerText);
                        command.Parameters.AddWithValue("@closingCreditBalance", account["ClosingCreditBalance"].InnerText);
                        command.ExecuteNonQuery();
                    }

                }
            }
        }

    }
}
