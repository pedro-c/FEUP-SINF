using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace FirstREST.Controllers
{
    public class FinancialController : Controller
    {

        public class FinancialModel
        {
            public List<AccountModel> CompanyAccounts = new List<AccountModel>();
            public List<JournalModel> CompanyJournals = new List<JournalModel>();
            public List<ProfitsAndLossesModel> ProfitsAndLosses = new List<ProfitsAndLossesModel>();
        }

        public class AccountModel
        {
            public Int64 accountId;
            public string accountDescription;
            public double accountOpeningDebitBalance;
            public double accountOpeningCreditBalance;
            public double accountClosingDebitBalance;
            public double accountClosingCreditBalance;
        }

        public class JournalModel
        {
            public string journalID;
            public string journalDescription;
            public double journalTotalCredit;
            public double journalTotalDebit;
        }

        public class ProfitsAndLossesModel
        {
            public double sales;
            public double services;
            public double otherIncomes;
            public double salesExpenses;
            public double administrativeExpenses;
            public double otherExpenses;
            public double totalSalesAndRevenue;
            public double totalOperatingCosts;
            public double netIncome;
        }

        public ActionResult Index()
        {
            DataSet invoiceTable = new DataSet();
            DataSet customerTable = new DataSet();
            DataSet FinancialTable = new DataSet();
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
                            tempAccount.accountOpeningDebitBalance = row.Field<double>("openingDebitBalance");
                            tempAccount.accountOpeningCreditBalance = row.Field<double>("openingCreditBalance");
                            tempAccount.accountClosingDebitBalance = row.Field<double>("closingDebitBalance");
                            tempAccount.accountClosingCreditBalance = row.Field<double>("closingCreditBalance");
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

                ProfitsAndLossesModel tempModel = new ProfitsAndLossesModel();
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.Account WHERE id=71", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "sales");

                        if (FinancialTable.Tables["sales"].Rows.Count > 0)
                            tempModel.sales = ((FinancialTable.Tables["sales"].Rows[0].Field<double>("closingCreditBalance") - FinancialTable.Tables["sales"].Rows[0].Field<double>("openingCreditBalance")) - (FinancialTable.Tables["sales"].Rows[0].Field<double>("closingDebitBalance") - FinancialTable.Tables["sales"].Rows[0].Field<double>("openingDebitBalance")));

                    }
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.Account WHERE id=72", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "services");

                        if (FinancialTable.Tables["services"].Rows.Count > 0)
                            tempModel.services = ((FinancialTable.Tables["services"].Rows[0].Field<double>("closingCreditBalance") - FinancialTable.Tables["services"].Rows[0].Field<double>("openingCreditBalance")) - (FinancialTable.Tables["services"].Rows[0].Field<double>("closingDebitBalance") - FinancialTable.Tables["services"].Rows[0].Field<double>("openingDebitBalance")));
                    }
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.Account WHERE id=61", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "salesExpenses");

                        if (FinancialTable.Tables["salesExpenses"].Rows.Count > 0)
                            tempModel.salesExpenses = ((FinancialTable.Tables["salesExpenses"].Rows[0].Field<double>("closingCreditBalance") - FinancialTable.Tables["salesExpenses"].Rows[0].Field<double>("openingCreditBalance")) - (FinancialTable.Tables["salesExpenses"].Rows[0].Field<double>("closingDebitBalance") - FinancialTable.Tables["salesExpenses"].Rows[0].Field<double>("openingDebitBalance")));
                    }
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.Account WHERE id=62", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "administrativeExpenses");

                        if (FinancialTable.Tables["administrativeExpenses"].Rows.Count > 0)
                            tempModel.administrativeExpenses = ((FinancialTable.Tables["administrativeExpenses"].Rows[0].Field<double>("closingCreditBalance") - FinancialTable.Tables["administrativeExpenses"].Rows[0].Field<double>("openingCreditBalance")) - (FinancialTable.Tables["administrativeExpenses"].Rows[0].Field<double>("closingDebitBalance") - FinancialTable.Tables["administrativeExpenses"].Rows[0].Field<double>("openingDebitBalance")));
                    }
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.Account WHERE id=63 OR id=64 OR id=65 OR id=66 OR id=67 or id=68 or id=69", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "otherExpenses");

                        if(FinancialTable.Tables["otherExpenses"].Rows.Count > 0)
                            tempModel.otherExpenses = ((FinancialTable.Tables["otherExpenses"].Rows[0].Field<double>("closingCreditBalance") - FinancialTable.Tables["otherExpenses"].Rows[0].Field<double>("openingCreditBalance")) - (FinancialTable.Tables["otherExpenses"].Rows[0].Field<double>("closingDebitBalance") - FinancialTable.Tables["otherExpenses"].Rows[0].Field<double>("openingDebitBalance")));
                    }
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.Account WHERE id=73 OR id=74 OR id=75 OR id=76 OR id=77 or id=78 or id=79", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "otherIncomes");

                        if (FinancialTable.Tables["otherIncomes"].Rows.Count > 0)
                            tempModel.otherIncomes = ((FinancialTable.Tables["otherIncomes"].Rows[0].Field<double>("closingCreditBalance") - FinancialTable.Tables["otherIncomes"].Rows[0].Field<double>("openingCreditBalance")) - (FinancialTable.Tables["otherIncomes"].Rows[0].Field<double>("closingDebitBalance") - FinancialTable.Tables["otherIncomes"].Rows[0].Field<double>("openingDebitBalance")));
                    }
                }

                tempModel.totalSalesAndRevenue = tempModel.sales + tempModel.services + tempModel.otherIncomes;
                tempModel.totalOperatingCosts = tempModel.salesExpenses + tempModel.administrativeExpenses + tempModel.otherExpenses;
                tempModel.netIncome = tempModel.totalSalesAndRevenue - tempModel.totalOperatingCosts;
                FinancialDashboardModel.ProfitsAndLosses.Add(tempModel);
            }

            return View(FinancialDashboardModel);
        }
    }
}
