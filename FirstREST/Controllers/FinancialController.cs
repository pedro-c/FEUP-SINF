using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace FirstREST.Controllers
{
    public class FinancialModel
    {
        public List<AccountModel> CompanyAccounts = new List<AccountModel>();
        public List<JournalModel> CompanyJournals = new List<JournalModel>();
    }

    public class AccountModel
    {
        public Int64 accountId;
        public string accountDescription;
        public string accountOpeningDebitBalance;
        public string accountOpeningCreditBalance;
        public string accountClosingDebitBalance;
        public string accountClosingCreditBalance;
    }

    public class JournalModel
    {
        public string journalID;
        public string journalDescription;
        public double journalTotalCredit;
        public double journalTotalDebit;
    }

    public class FinancialController : Controller
    {
        //
        // GET: /Financial/

        public ActionResult Index()
        {
            DataSet invoiceTable = new DataSet();
            DataSet customerTable = new DataSet();
            FinancialModel FinancialDashboardModel = new FinancialModel();

            string connectionString = FirstREST.SqlConnection.GetConnectionString();

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("Select * From dbo.Account", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {

                        adapter.Fill(invoiceTable, "Account");

                        foreach (DataRow row in invoiceTable.Tables["Account"].Rows)
                        {
                            AccountModel tempAccount = new AccountModel();
                            tempAccount.accountId = row.Field<Int64>("id");
                            tempAccount.accountDescription = row.Field<string>("accountDescription");
                            tempAccount.accountOpeningDebitBalance = row.Field<string>("openingDebitBalance");
                            tempAccount.accountOpeningCreditBalance = row.Field<string>("openingCreditBalance");
                            tempAccount.accountClosingDebitBalance = row.Field<string>("closingDebitBalance");
                            tempAccount.accountClosingCreditBalance = row.Field<string>("closingCreditBalance");
                            FinancialDashboardModel.CompanyAccounts.Add(tempAccount);
                        }

                    }
                }

                using (SqlCommand command = new SqlCommand("Select * from dbo.Journal", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(customerTable, "Journal");

                        foreach (DataRow row in customerTable.Tables["Journal"].Rows)
                        {
                            JournalModel tempJournal = new JournalModel();
                            tempJournal.journalID = row.Field<string>("JournalID");
                            tempJournal.journalDescription = row.Field<string>("Description");
                            tempJournal.journalTotalCredit = row.Field<double>("TotalCredit");
                            tempJournal.journalTotalDebit = row.Field<double>("TotalDebit");
                            FinancialDashboardModel.CompanyJournals.Add(tempJournal);
                        }
                    }
                }
            }

            return View(FinancialDashboardModel);
        }
    }
}
