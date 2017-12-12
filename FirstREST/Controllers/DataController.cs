using Interop.GcpBE900;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PriIntegration = FirstREST.Lib_Primavera.PriIntegration;
using Artigo = FirstREST.Lib_Primavera.Model.Artigo;
using Funcionario = FirstREST.Lib_Primavera.Model.Funcionario;
using System.Xml;


namespace FirstREST.Controllers
{
    public class DataController : Controller
    {

        public static XmlDocument saft = new XmlDocument();
        public static string connectionString = FirstREST.SqlConnection.GetConnectionString();

        // GET: /Saft/
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {

            
            if (file.ContentType.Contains("xml"))
            {
                string path = System.IO.Path.Combine(Server.MapPath("~/Saft_Files/"), "saft.xml");
                file.SaveAs(path);
                loadData();
                ViewBag.Message = "File uploaded successfully";
                return View();
            }
            else
            {
                ViewBag.Message = "Not a valid file!";
                return View();
            }
        }


        public ActionResult Index()
        {
            return View();
        }

        public static void loadData()
        {
            processArtigos();
            processFuncionarios();
            readSaft();
        }

        public static void readSaft()
        {
            saft.Load("C:\\SINF\\FEUP-SINF\\FirstREST\\Saft_Files\\saft.xml");
            processFiscalYear();
            processJournals();
            proccessAccounts();
            proccessInvoices();
            proccessCustomers();
            proccessLines();
            processSalesInformation();
            processFinancialInformation();
            processSupplier();
        }

        public static void processSupplier()
        {
            XmlNodeList lineHeader = saft.GetElementsByTagName("Supplier");

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {

                connection.Open();

                // Drop table
                var dropQuery = "IF OBJECT_ID('dbo.Supplier', 'U') IS NOT NULL DROP TABLE dbo.Supplier";
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create table
                var createQuery =
                    "CREATE TABLE [dbo].[Supplier](" +
                      "    [Id] [int] NOT NULL IDENTITY (1,1), " +
                      "  [supplierId] [nchar](64) NOT NULL," +
	                "    [name] [nchar](64) NOT NULL," +
	                 "   [phoneNumber] [nchar](20) NOT NULL," +
	                  "  [address] [nchar](256) NOT NULL," +
                    " CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED " +
                    "(" +
	                 "   [id] ASC" +
                    ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" +
                    ") ON [PRIMARY]"
             ;


                using (var command = new SqlCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                //Populate table
                foreach (XmlNode line in lineHeader)
                {
                    var supplierId = line["SupplierID"].InnerText;
                    var name = line["CompanyName"].InnerText;
                    var phoneNumber = line["Telephone"].InnerText;
                    var address = line["BillingAddress"]["AddressDetail"].InnerText;


                    var query = "INSERT INTO dbo.Supplier(supplierId,name,phoneNumber,address) VALUES(@supplierId,@name, @phoneNumber, @address)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@supplierId", supplierId);
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@address", address);
                        command.ExecuteNonQuery();
                    }


                }
            }
        }

        public static void processFiscalYear()
        {
        
            XmlNodeList lineHeader = saft.GetElementsByTagName("Header");

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {

                connection.Open();

                // Drop table
                var dropQuery = "IF OBJECT_ID('dbo.Company', 'U') IS NOT NULL DROP TABLE dbo.Company";
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create table
                var createQuery =
                    " CREATE TABLE [dbo].[Company]( " +
                    "     [CompanyName] [nchar](40) NOT NULL, " +
                    "     [StartDate] [date] NOT NULL, " +
                    "     [EndDate] [date] NOT NULL, " +
                    "     [FiscalYear] [nchar](60) NOT NULL, " +
                    "     [City] [nchar](40) NOT NULL, " +
                    "     [Country] [nchar](40) NOT NULL, " +
                    "     [StreetName] [nchar](60) NOT NULL, " +
                    " ) ON [PRIMARY]"
             ;


                using (var command = new SqlCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                //Populate table
                foreach (XmlNode line in lineHeader)
                {
                    var companyName = line["CompanyName"].InnerText;
                    var startDate = line["StartDate"].InnerText;
                    var endDate = line["EndDate"].InnerText;
                    var fiscalYear = line["FiscalYear"].InnerText;
                    var city = line["CompanyAddress"]["City"].InnerText;
                    var country = line["CompanyAddress"]["Country"].InnerText;
                    var streetName = line["CompanyAddress"]["StreetName"].InnerText;

  
                    var query = "INSERT INTO dbo.Company(CompanyName,StartDate,EndDate,FiscalYear, City, Country, StreetName) VALUES(@CompanyName,@StartDate, @EndDate, @FiscalYear, @City, @Country, @StreetName)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CompanyName", companyName);
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);
                        command.Parameters.AddWithValue("@FiscalYear", fiscalYear);
                        command.Parameters.AddWithValue("@City", city);
                        command.Parameters.AddWithValue("@Country", country);
                        command.Parameters.AddWithValue("@StreetName", streetName);
                        command.ExecuteNonQuery();
                    }


                }
            }
        
        }

        public static void processJournals()
        {
            XmlNodeList journals = saft.GetElementsByTagName("Journal");

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                connection.Open();

                /* Journal Table Start */
                // Drop journal table
                var dropQuery = "IF OBJECT_ID('dbo.Journal', 'U') IS NOT NULL DROP TABLE dbo.Journal";
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create journal table
                var createQuery =
                       " CREATE TABLE [dbo].[Journal]( " +
                       "     [JournalID] [nchar](20) NOT NULL, " +
                       "     [Description] [nchar](30) NOT NULL, " +
                       "     [TotalCredit] [float] NULL, " +
                       "     [TotalDebit] [float] NULL " +
                       " ) ON [PRIMARY]"
                ;

                using (var command = new SqlCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                /* Journal Table End*/


                /* Transactions table start */
                // Drop transactions table
                dropQuery = "IF OBJECT_ID('dbo.Transactions', 'U') IS NOT NULL DROP TABLE dbo.Transactions"; //( Transaction is a keyword, so the table name is Transactions)
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create transactions table
                //( Transaction is a keyword, so the table name is Transactions)
                createQuery =
                   " CREATE TABLE [dbo].[Transactions]( " +
                   "     [TransactionID] [nchar](128) NOT NULL, " +
                   "     [Period]        [int] NOT NULL," +
                   "     [TransactionDate] [int] NOT NULL, " +
                   "     [Description] [nchar](64) NULL, " +
                   "     [TransactionType] [nchar](32) NULL " +
                   ") ON [PRIMARY]"
                ;

                using (var command = new SqlCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                /* Transaction table end */

                /* Date table start */
                // Drop date table
                dropQuery = "IF OBJECT_ID('dbo.Date', 'U') IS NOT NULL DROP TABLE dbo.Date";
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create date table
                createQuery =
                       " CREATE TABLE [dbo].[Date]( " +
                       "    [Id] [int] NOT NULL IDENTITY (1,1), " +
                       "    [Year] [int] NOT NULL, " +
                       "    [Month] [int] NOT NULL, " +
                       "    CONSTRAINT [PK_Date] PRIMARY KEY CLUSTERED " +
                       "    (" +
                       "        [Id] ASC" +
                       "    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]," +
                       "    CONSTRAINT [UK_Date] UNIQUE " +
                       "    (" +
                       "        [Year], [Month]" +
                       "    ) ON [PRIMARY]" +
                       ") ON [PRIMARY]"
                ;

                using (var command = new SqlCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                /* Date table end */


                /* TransactionLine start */
                // Drop line table
                dropQuery = "IF OBJECT_ID('dbo.TransactionLine', 'U') IS NOT NULL DROP TABLE dbo.TransactionLine";
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create transactionline table
                createQuery =
                       " CREATE TABLE [dbo].[TransactionLine]( " +
                       "     [TransactionID] [nchar](128) NOT NULL, " +
                       "     [RecordID] [nchar](64) NOT NULL, " +
                       "     [AccountID] [nchar](64) NOT NULL, " +
                       "     [IsCredit] [bit] NOT NULL, " +
                       "     [Amount]   [float] NOT NULL" +
                       " ) ON [PRIMARY]"
                ;

                using (var command = new SqlCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                /* Transaction line table end*/


                //Populate table
                foreach (XmlNode journal in journals)
                {
                    double totalCredit = 0;
                    double totalDebit = 0;
                    double[] totals;   // [totalCredit, totalDebit]
                    XmlNodeList journalChildren = journal.ChildNodes;
                    foreach (XmlNode child in journalChildren)
                    {
                        if (child.Name == "Transaction")
                        {
                            totals = processTransaction(child, connection);
                            totalCredit += totals[0];
                            totalDebit += totals[1];
                        }
                    }


                    var query = "INSERT INTO dbo.Journal(JournalID,Description,TotalCredit,TotalDebit) VALUES(@JournalID,@Description,@TotalCredit,@TotalDebit)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@JournalID", journal["JournalID"].InnerText);
                        command.Parameters.AddWithValue("@Description", journal["Description"].InnerText);
                        command.Parameters.AddWithValue("@TotalCredit", totalCredit);
                        command.Parameters.AddWithValue("@TotalDebit", totalDebit);
                        command.ExecuteNonQuery();
                    }

                }

                var dropViewQuery = "IF EXISTS (SELECT 1 FROM dbo.MonthlyAccountSums) DROP VIEW dbo.MonthlyAccountSums";
                using (var command = new SqlCommand(dropViewQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                var createViewQuery = "CREATE VIEW dbo.MonthlyAccountSums (Year, Month, AccountID, Amount, IsCredit) as " +
                    "SELECT Year, Month, AccountId, sum(Amount) as Amount, IsCredit" +
                    "   FROM dbo.Date as Date LEFT JOIN (" +
                    "       dbo.Transactions as Transactions LEFT JOIN dbo.TransactionLine as TLine ON Transactions.TransactionID = TLine.TransactionID" +
                    ")" +
                    "ON Date.id = TransactionDate GROUP BY Year, Month, IsCredit, AccountID";

                using (var command = new SqlCommand(createViewQuery, connection))
                {
                   command.ExecuteNonQuery();
                }
            }
        }

        public static double[] processTransaction(XmlNode transaction, System.Data.SqlClient.SqlConnection connection)
        {
            double totalCredit = 0;
            double totalDebit = 0;

            String transactionID = transaction["TransactionID"].InnerText;
            String transactionDescription = transaction["Description"].InnerText;
            int transactionPeriod = Convert.ToInt32(transaction["Period"].InnerText, 10);
            String transactionType = transaction["TransactionType"].InnerText;

            /* Parse the date and put it in the database */
            String[] dateInfo = transaction["TransactionDate"].InnerText.Split('-');
            int year = Convert.ToInt32(dateInfo[0], 10);
            int month = Convert.ToInt32(dateInfo[1], 10);
            int dateId = insertDate(year, month, connection);


            var query = "INSERT INTO dbo.Transactions(TransactionID, Period, TransactionDate, Description, TransactionType)" +
                "VALUES (@TransactionID, @Period, @TransactionDate, @Description, @TransactionType)";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TransactionID", transactionID);
                command.Parameters.AddWithValue("@Period", transactionPeriod);
                command.Parameters.AddWithValue("@TransactionDate", dateId);
                command.Parameters.AddWithValue("@Description", transactionDescription);
                command.Parameters.AddWithValue("@TransactionType", transactionType);
                command.ExecuteNonQuery();
            }

            XmlNode linesElement = transaction["Lines"];
            XmlNodeList lines = linesElement.ChildNodes;

            foreach (XmlNode line in lines)
            {
                if (line.Name == "CreditLine")
                {
                    processLine(line, transactionID, true, connection);
                    totalCredit = totalCredit + double.Parse(line["CreditAmount"].InnerText, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    processLine(line, transactionID, false, connection);
                    totalDebit = totalDebit + double.Parse(line["DebitAmount"].InnerText, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            return new double[] { totalCredit, totalDebit };
        }

        public static void processLine(XmlNode line, String transactionID, bool isCredit, System.Data.SqlClient.SqlConnection connection)
        {
            var query = "INSERT INTO dbo.TransactionLine(TransactionID, RecordID, AccountID, IsCredit, Amount)" +
                "VALUES (@TransactionID, @RecordID, @AccountID, @IsCredit, @Amount)";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TransactionID", transactionID);
                command.Parameters.AddWithValue("@RecordID", line["RecordID"].InnerText);
                command.Parameters.AddWithValue("@AccountID", Convert.ToInt64(line["AccountID"].InnerText));
                command.Parameters.AddWithValue("@IsCredit", isCredit);
                if (isCredit)
                    command.Parameters.AddWithValue("@Amount", double.Parse(line["CreditAmount"].InnerText, System.Globalization.CultureInfo.InvariantCulture));
                else
                    command.Parameters.AddWithValue("@Amount", double.Parse(line["DebitAmount"].InnerText, System.Globalization.CultureInfo.InvariantCulture));
                command.ExecuteNonQuery();
            }
        }

        /*
         * Insert a date in the database, if it doesn't exist
         * Returns the date's Id in the database
         */
        public static Int32 insertDate(int year, int month, System.Data.SqlClient.SqlConnection connection)
        {
            Int32 insertedId;

            var query = "IF NOT EXISTS (SELECT * FROM dbo.Date WHERE Year = @Year AND Month = @Month)" +
                "INSERT INTO dbo.Date(Year, Month) OUTPUT Inserted.Id VALUES (@Year, @Month)" +
                "ELSE SELECT Id FROM dbo.Date WHERE Year = @Year AND Month = @Month";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Year", year);
                command.Parameters.AddWithValue("@Month", month);
                insertedId = (Int32)command.ExecuteScalar();
            }

            return insertedId;
        }

        public static void processFinancialInformation()
        {
            XmlNodeList GeneralLedgerEntries = saft.GetElementsByTagName("GeneralLedgerEntries");

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                connection.Open();

                // Drop table
                var dropQuery = "IF OBJECT_ID('dbo.Financial', 'U') IS NOT NULL DROP TABLE dbo.Financial";
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create table
                var createQuery =
                           " CREATE TABLE [dbo].[Financial](" +
                           "     [TotalDebit] [float] NOT NULL," +
                           "     [TotalCredit] [float] NOT NULL," +
                           "     [NumberOfTransactions] [bigint] NOT NULL" +
                           " ) ON [PRIMARY]"

                ;
                using (var command = new SqlCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }


                //Populate table
                var query = "INSERT INTO dbo.Financial(TotalDebit,TotalCredit,NumberOfTransactions)VALUES(@TotalDebit,@TotalCredit,@NumberOfTransactions)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TotalDebit", GeneralLedgerEntries[0]["TotalDebit"].InnerText);
                    command.Parameters.AddWithValue("@TotalCredit", GeneralLedgerEntries[0]["TotalCredit"].InnerText);
                    command.Parameters.AddWithValue("@NumberOfTransactions", GeneralLedgerEntries[0]["NumberOfEntries"].InnerText);
                    command.ExecuteNonQuery();
                }

            }
        }

        public static void processSalesInformation()
        {
            XmlNodeList salesInvoices = saft.GetElementsByTagName("SalesInvoices");

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                connection.Open();

                // Drop table
                var dropQuery = "IF OBJECT_ID('dbo.Sales', 'U') IS NOT NULL DROP TABLE dbo.Sales;";
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create table
                var createQuery =
                            "CREATE TABLE [dbo].[Sales](" +
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

                    var query = "INSERT INTO dbo.Sales(InvoicesTotalDebit,InvoicesTotalCredit)VALUES(@InvoicesTotalDebit,@InvoicesTotalCredit)";
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
                var dropQuery = "IF OBJECT_ID('dbo.Line', 'U') IS NOT NULL DROP TABLE dbo.Line;";
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create table
                var createQuery =
                        "CREATE TABLE [dbo].[Line](" +
                        "[LineId] [bigint] NOT NULL," +
                        "[InvoiceNo] [nchar](30) NOT NULL," +
                        "[LineNumber] [nchar](30) NOT NULL," +
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
                    XmlNodeList lines = invoice.ChildNodes;

                    foreach (XmlNode line in lines)
                    {
                        if(line.Name != "Line")
                            continue;

                        id++;
                        var query = "INSERT INTO dbo.Line(LineId,InvoiceNo,LineNumber,ProductCode,Quantity,UnitPrice,CreditAmount,TaxType,TaxPercentage) " +
                            "VALUES (@LineId,@InvoiceNo, @LineNumber,@ProductCode,@Quantity,@UnitPrice,@CreditAmount,@TaxType,@TaxPercentage)";
                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@LineId", id);
                            command.Parameters.AddWithValue("@InvoiceNo", invoice["InvoiceNo"].InnerText);
                            command.Parameters.AddWithValue("@LineNumber", line["LineNumber"].InnerText);
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
                var dropQuery = "IF OBJECT_ID('dbo.Customer', 'U') IS NOT NULL DROP TABLE dbo.Customer;";
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create table
                var createQuery =
                        "CREATE TABLE [dbo].[Customer](" +
                        "[CustomerTaxID] [nchar](40) NOT NULL," +
                        "[CustomerID] [nchar](40) NOT NULL," +
                        "[AccountID] [nchar](40) NOT NULL," +
                        "[CustomerName] [nchar](40) NOT NULL," +
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

                    var query = "INSERT INTO dbo.Customer(CustomerTaxID,CustomerID,AccountID,CustomerName,Country,TotalCashSpent) VALUES(@CustomerTaxID,@CustomerID,@AccountID,@CustomerName,@Country,@TotalCashSpent)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerTaxID", customer["CustomerTaxID"].InnerText);
                        command.Parameters.AddWithValue("@CustomerID", customer["CustomerID"].InnerText);
                        command.Parameters.AddWithValue("@AccountID", customer["AccountID"].InnerText);
                        command.Parameters.AddWithValue("@CustomerName", customer["CompanyName"].InnerText);
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
                var dropQuery = "IF OBJECT_ID('dbo.Invoice', 'U') IS NOT NULL DROP TABLE dbo.Invoice;";
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create table
                var createQuery =
                        "CREATE TABLE [dbo].[Invoice](" +
                        "[Id] [int] NOT NULL IDENTITY (1,1), " +
                        "[invoiceNo] [nchar](25) NOT NULL," +
                        "[invoiceStatus] [nchar](10) NOT NULL," +
                        "[period] [int] NOT NULL," +
                        "[invoiceDate] [date] NOT NULL," +
                        "[invoiceType] [nchar](15) NOT NULL," +
                        "[customerID] [nchar](25) NOT NULL," +
                        "[grossTotal] [float] NOT NULL," +
                        "[netTotal] [float] NOT NULL," +
                        "[taxTotal] [float] NOT NULL," +
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

        private static void processArtigos()
        {

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

        private static void processFuncionarios()
        {
            List<Funcionario> listaFuncionarios = PriIntegration.listaFuncionarios();

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                connection.Open();

                // Drop table
                var dropQuery = "IF OBJECT_ID('dbo.Employee', 'U') IS NOT NULL DROP TABLE dbo.Employee;";
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create table
                var createQuery =
                    "CREATE TABLE [dbo].[Employee](" +
                        "[id] [varchar](10) NOT NULL ," +
                        "[abbrv_name] [varchar](80) ," +
                        "[phone] [nvarchar](15) ," +
                        "[email] [nvarchar](512) ," +
                    "CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ( [id] ) " +
                    "WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" +
                    ") ON [PRIMARY]";

                using (var command = new SqlCommand(createQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Populate table
                foreach (Funcionario funcionario in listaFuncionarios)
                {
                    var query = "INSERT INTO dbo.Employee(id, abbrv_name, phone, email)" +
                        "VALUES (@id, @abbrv_name, @phone, @email)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", funcionario.CodFuncionario);
                        command.Parameters.AddWithValue("@abbrv_name", funcionario.NomeAbreviado);
                        command.Parameters.AddWithValue("@phone", funcionario.NumTelefone);
                        command.Parameters.AddWithValue("@email", funcionario.Email);
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
                var dropQuery = "IF OBJECT_ID('dbo.Account', 'U') IS NOT NULL DROP TABLE dbo.Account;";
                using (var command = new SqlCommand(dropQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create table
                var createQuery =
                    "CREATE TABLE [dbo].[Account](" +
                        "[id] [bigint] NOT NULL," +
                        "[accountDescription] [nchar](60) NOT NULL," +
                        "[openingDebitBalance] [float] NOT NULL," +
                        "[openingCreditBalance] [float] NOT NULL," +
                        "[closingDebitBalance] [float] NOT NULL," +
                        "[closingCreditBalance] [float] NOT NULL," +
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