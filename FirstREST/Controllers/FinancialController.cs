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
        
        public class companyIndicators
        {
            public int EBIT;
            public int EBITDA;
            public int quickRatio;
            public int currentRatio;
        }

        public class AccountModel
        {
            public Int64 accountId;
            public string accountDescription;
            public Int64 amount;
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
            public Int64 sales;
            public Int64 services;
            public Int64 otherIncomes;
            public Int64 salesExpenses;
            public Int64 administrativeExpenses;
            public Int64 otherExpenses;
            public Int64 totalSalesAndRevenue;
            public Int64 totalOperatingCosts;
            public Int64 netIncome;
        }

        public ActionResult Index(int period1 = 1, int period2 = 12)
        {
            DataSet invoiceTable = new DataSet();
            DataSet customerTable = new DataSet();
            DataSet FinancialTable = new DataSet();
            FinancialModel FinancialDashboardModel = new FinancialModel();

            string connectionString = FirstREST.SqlConnection.GetConnectionString();

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("Select Year, Month, AccountID, accountDescription, Amount, IsCredit From dbo.MonthlyAccountSums INNER JOIN dbo.Account on MonthlyAccountSums.AccountID=Account.id", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {

                        adapter.Fill(invoiceTable, "Account");

                        foreach (DataRow row in invoiceTable.Tables["Account"].Rows)
                        {
                            AccountModel tempAccount = new AccountModel();
                            tempAccount.accountId = row.Field<Int64>("AccountId");
                            tempAccount.accountDescription = row.Field<string>("accountDescription");
                            tempAccount.amount = row.Field<Int64>("Amount");
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
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '71%'", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "sales");

                        if (FinancialTable.Tables["sales"].Rows.Count > 0)
                            tempModel.sales = FinancialTable.Tables["sales"].Rows[0].Field<Int64>("Amount");

                    }
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '72%'", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "services");

                        if (FinancialTable.Tables["services"].Rows.Count > 0)
                            tempModel.services = FinancialTable.Tables["services"].Rows[0].Field<Int64>("Amount");
                    }
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '61%'", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "salesExpenses");

                        if (FinancialTable.Tables["salesExpenses"].Rows.Count > 0)
                            tempModel.salesExpenses = FinancialTable.Tables["salesExpenses"].Rows[0].Field<Int64>("Amount");
                    }
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '62%'", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "administrativeExpenses");

                        if (FinancialTable.Tables["administrativeExpenses"].Rows.Count > 0)
                            tempModel.administrativeExpenses = FinancialTable.Tables["administrativeExpenses"].Rows[0].Field<Int64>("Amount");
                    }
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '63%' OR AccountId LIKE '64%' OR AccountId LIKE '65%' OR AccountId LIKE '66%' OR AccountId LIKE '67%' or AccountId LIKE '68%' or AccountId LIKE '69%'", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "otherExpenses");

                        if(FinancialTable.Tables["otherExpenses"].Rows.Count > 0)
                            tempModel.otherExpenses = FinancialTable.Tables["otherExpenses"].Rows[0].Field<Int64>("Amount");
                    }
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '73%' OR AccountId LIKE '74%' OR AccountId LIKE '75%' OR AccountId LIKE '76%' OR AccountId LIKE '77%' or AccountId LIKE '78%' or AccountId LIKE '79%'", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "otherIncomes");

                        if (FinancialTable.Tables["otherIncomes"].Rows.Count > 0)
                            tempModel.otherIncomes = FinancialTable.Tables["otherIncomes"].Rows[0].Field<Int64>("Amount");
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
