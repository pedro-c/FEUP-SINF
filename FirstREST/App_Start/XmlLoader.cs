using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;


namespace FirstREST
{
    public class XmlLoader
    {
        public static XmlDocument saft = new XmlDocument();
        public static string connectionString = FirstREST.SqlConnection.GetConnectionString();
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
                foreach (XmlNode account in accounts)
                {
                    
                    var query = "INSERT INTO dbo.Account(id,accountDescription,openingDebitBalance,openingCreditBalance,closingDebitBalance,closingCreditBalance) VALUES(@id,@accountDescription,@openingDebitBalance,@openingCreditBalance,@closingDebitBalance,@closingCreditBalance)";
                    using(var command = new SqlCommand(query, connection)){
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
