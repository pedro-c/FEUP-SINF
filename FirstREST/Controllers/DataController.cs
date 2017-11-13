using Interop.GcpBE900;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PriIntegration = FirstREST.Lib_Primavera.PriIntegration;
using Artigo = FirstREST.Lib_Primavera.Model.Artigo;
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

            processArtigos();
            readSaft();
            return View();
        }

        public static void readSaft()
        {
            saft.Load("C:\\SINF\\FEUP-SINF\\FirstREST\\SAFT.xml");

            proccessAccounts();
            proccessInvoices();
            proccessCustomers();
            proccessLines();
            processFinancialInformation();

        }

        public static void processFinancialInformation()
        {
            XmlNodeList salesInvoices = saft.GetElementsByTagName("SalesInvoices");

             using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
             {
                 connection.Open();

                 // Drop table
                 var dropQuery = "DROP TABLE dbo.Financial";
                 using (var command = new SqlCommand(dropQuery, connection))
                 {
                     command.ExecuteNonQuery();
                 }

                 // Create table
                 var createQuery =
                             "CREATE TABLE [dbo].[Financial](" +
	                           " [InvoicesTotalDebit] [float] NULL," +
	                           " [InvoicesTotalCredit] [float] NULL" +
                            ") ON [PRIMARY]"
                         
                 ;
                 using (var command = new SqlCommand(createQuery, connection))
                 {
                     command.ExecuteNonQuery();
                 }

                 //Populate table
                 foreach (XmlNode info in salesInvoices)
                 {

                     var query = "INSERT INTO dbo.Financial(InvoicesTotalDebit,InvoicesTotalCredit)VALUES(@InvoicesTotalDebit,@InvoicesTotalCredit)";
                     using (var command = new SqlCommand(query, connection))
                     {
                         command.Parameters.AddWithValue("@InvoicesTotalDebit", info["TotalDebit"].InnerText);
                         command.Parameters.AddWithValue("@InvoicesTotalCredit", info["TotalCredit"].InnerText);
                         command.ExecuteNonQuery();
                     }

                 }
             }

        }

        public static void proccessLines()
        {
            XmlNodeList invoices = saft.GetElementsByTagName("Invoice");

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                connection.Open();

                // Drop table
                var dropQuery = "DROP TABLE dbo.Line";
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create table
                var createQuery =
                        "CREATE TABLE [dbo].[Line](" +
                        "[LineId] [bigint] NOT NULL," +
	                    "[InvoiceNo] [nchar](30) NOT NULL," +
                        "[LineNo] [nchar](30) NOT NULL," +
	                    "[ProductCode] [nchar](30) NOT NULL," +
	                    "[Quantity] [int] NOT NULL," +
                        "[UnitPrice] [nchar](30) NOT NULL," +
                        "[CreditAmount] [nchar](30) NOT NULL," +
	                    "[TaxType] [nchar](20) NOT NULL," +
                        "[TaxPercentage] [nchar](30) NOT NULL," +
                     "CONSTRAINT [PK_Line] PRIMARY KEY CLUSTERED " +
                    "(" +
                     "   [LineId] ASC" +
                    ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" +
                    ") ON [PRIMARY]"
                ;
                using (var command = new SqlCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }


                //Populate table
                var id = 0;
                foreach (XmlNode invoice in invoices)
                {
                    XmlNodeList lines = invoice.SelectNodes("/Line");

                    foreach (XmlNode line in lines)
                    {
                        id++;
                        var query = "INSERT INTO dbo.Line(LineId,InvoiceNo,LineNo,ProductCode,Quantity,UnitPrice,CreditAmount,TaxType,TaxPercentage) VALUES(@LineId,@InvoiceNo,@LineNo,@ProductCode,@Quantity,@UnitPrice,@CreditAmount,@TaxType,@TaxPercentage)";
                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@LineId", id);
                            command.Parameters.AddWithValue("@InvoiceNo", invoice["InvoiceNo"].InnerText);
                            command.Parameters.AddWithValue("@LineNo", line["LineNumber"].InnerText);
                            command.Parameters.AddWithValue("@ProductCode", line["ProductCode"].InnerText);
                            command.Parameters.AddWithValue("@Quantity", line["Quantity"].InnerText);
                            command.Parameters.AddWithValue("@UnitPrice", line["UnitPrice"].InnerText);
                            command.Parameters.AddWithValue("@CreditAmount", line["CreditAmount"].InnerText);
                            command.Parameters.AddWithValue("@TaxType", line["Tax"]["TaxType"].InnerText);                            
                            command.Parameters.AddWithValue("@TaxPercentage", line["Tax"]["TaxPercentage"].InnerText);
                            command.ExecuteNonQuery();
                        }
                           
                    }

                }

            }

        }

        public static void proccessCustomers()
        {
            XmlNodeList customers = saft.GetElementsByTagName("Customer");

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                connection.Open();

                // Drop table
                var dropQuery = "DROP TABLE dbo.Customer";
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create table
                var createQuery =
                        "CREATE TABLE [dbo].[Customer](" +
                        "[CustomerTaxID] [nchar](40) NOT NULL," +
                        "[CustumerID] [nchar](40) NOT NULL," +
	                    "[AccountID] [nchar](40) NOT NULL," +
	                    "[CompanyName] [nchar](40) NOT NULL," +
	                    "[Country] [nchar](40) NOT NULL," +
                        "[TotalCashSpent] [nchar](40) NOT NULL," +
                     "CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED " +
                    "(" +
                     "   [CustomerTaxID] ASC" +
                    ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" +
                    ") ON [PRIMARY]"
                ;
                using (var command = new SqlCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                //Populate table
                foreach (XmlNode customer in customers)
                {

                    var query = "INSERT INTO dbo.Customer(CustomerTaxID,CustumerID,AccountID,CompanyName,Country,TotalCashSpent) VALUES(@CustomerTaxID,@CustumerID,@AccountID,@CompanyName,@Country,@TotalCashSpent)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerTaxID", customer["CustomerTaxID"].InnerText);
                        command.Parameters.AddWithValue("@CustumerID", customer["CustomerID"].InnerText);
                        command.Parameters.AddWithValue("@AccountID", customer["AccountID"].InnerText);
                        command.Parameters.AddWithValue("@CompanyName", customer["CompanyName"].InnerText);
                        command.Parameters.AddWithValue("@Country", customer["BillingAddress"]["Country"].InnerText);
                        command.Parameters.AddWithValue("@TotalCashSpent", "");
                        command.ExecuteNonQuery();
                    }

                }
                
            
            }
        }

        public static void proccessInvoices()
        {
            XmlNodeList invoices = saft.GetElementsByTagName("Invoice");

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                connection.Open();

                // Drop table
                var dropQuery = "DROP TABLE dbo.Invoice";
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create table
                var createQuery =
                        "CREATE TABLE [dbo].[Invoice](" +
	                    "[invoiceNo] [nchar](25) NOT NULL," +
	                    "[invoiceStatus] [nchar](10) NOT NULL," +
	                    "[period] [int] NOT NULL," +
	                    "[invoiceDate] [date] NOT NULL," +
	                    "[invoiceType] [nchar](15) NOT NULL," +
	                    "[customerID] [nchar](25) NOT NULL," +
                        "[grossTotal] [nchar](25) NOT NULL," +
                        "[netTotal] [nchar](25) NOT NULL," +
                        "[taxTotal] [nchar](25) NOT NULL," +
                     "CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED " +
                    "(" +
	                 "   [invoiceNo] ASC" +
                    ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" +
                    ") ON [PRIMARY]"
                ;
                using (var command = new SqlCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                //Populate table
                foreach (XmlNode invoice in invoices)
                {

                    var query = "INSERT INTO dbo.Invoice(invoiceNo,invoiceStatus,period,invoiceDate,invoiceType,customerID,grossTotal,netTotal,taxTotal) VALUES(@invoiceNo,@invoiceStatus,@period,@invoiceDate,@invoiceType,@customerID,@grossTotal,@netTotal,@taxTotal)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@invoiceNo", invoice["InvoiceNo"].InnerText);
                        command.Parameters.AddWithValue("@invoiceStatus", invoice["DocumentStatus"]["InvoiceStatus"].InnerText);
                        command.Parameters.AddWithValue("@period", invoice["Period"].InnerText);
                        command.Parameters.AddWithValue("@invoiceDate", invoice["InvoiceDate"].InnerText);
                        command.Parameters.AddWithValue("@invoiceType", invoice["InvoiceType"].InnerText);
                        command.Parameters.AddWithValue("@customerID", invoice["CustomerID"].InnerText);
                        command.Parameters.AddWithValue("@grossTotal", invoice["DocumentTotals"]["GrossTotal"].InnerText);
                        command.Parameters.AddWithValue("@netTotal", invoice["DocumentTotals"]["NetTotal"].InnerText);
                        command.Parameters.AddWithValue("@taxTotal", invoice["DocumentTotals"]["TaxPayable"].InnerText);
                        command.ExecuteNonQuery();
                    }

                }
            }
        }

        private void processArtigos(){

            List<Artigo> artigos = PriIntegration.ListaArtigos();

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                connection.Open();

                // Drop table
                var dropQuery = "IF OBJECT_ID('dbo.Artigo', 'U') IS NOT NULL DROP TABLE dbo.Artigo;";
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create table
                var createQuery =
                    "CREATE TABLE [dbo].[Artigo](" +
                        "[artigo] [nvarchar](48) NOT NULL ," +
                        "[descricao] [nvarchar](50) ," +
                        "[stk_reposicao] [float] ," +
                        "[stk_atual] [float] ," +
                        "[pvp] [float] NOT NULL ," +
                        "[needs_restock] [bit], " +
                    "CONSTRAINT [PK_Artigo] PRIMARY KEY CLUSTERED ( [artigo] ) " +
                    "WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" +
                    ") ON [PRIMARY]";

                using (var command = new SqlCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Populate table
                foreach (Artigo artigo in artigos)
                {
                    var query = "INSERT INTO dbo.Artigo(artigo, descricao, stk_reposicao, stk_atual, pvp, needs_restock)" +
                        "VALUES (@artigo, @descricao, @stk_reposicao, @stk_atual, @pvp, @needs_restock)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@artigo", artigo.CodArtigo);
                        command.Parameters.AddWithValue("@descricao", artigo.DescArtigo);
                        command.Parameters.AddWithValue("@stk_reposicao", artigo.STKReposicao);
                        command.Parameters.AddWithValue("@stk_atual", artigo.STKAtual);
                        command.Parameters.AddWithValue("@pvp", artigo.PVP);
                        command.Parameters.AddWithValue("@needs_restock", artigo.STKAtual <= artigo.STKReposicao ? 1 : 0);
                        command.ExecuteNonQuery();
                    }
                }
            }


        }


        public static void proccessAccounts()
        {
            XmlNodeList accounts = saft.GetElementsByTagName("Account");
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
