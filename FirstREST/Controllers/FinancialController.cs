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
            public companyIndicators CompanyIndicators = new companyIndicators();
            public SaftFileDateModel SaftInfo = new SaftFileDateModel();
        }
        
        public class companyIndicators
        {
            public Int64 EBIT;
            public Int64 EBITDA;
            public Int64 quickRatio;
            public Int64 currentRatio;

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

        public class SaftFileDateModel
        {
            public DateTime startDate;
            public DateTime endDate;
        }

        public ActionResult Index(int period1 =  13, int period2 = 13)
        {
            DataSet invoiceTable = new DataSet();
            DataSet customerTable = new DataSet();
            DataSet FinancialTable = new DataSet();
            DataSet companyTable = new DataSet();
            FinancialModel FinancialDashboardModel = new FinancialModel();
           

            string connectionString = FirstREST.SqlConnection.GetConnectionString();

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("Select * From dbo.Company", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {

                        adapter.Fill(companyTable, "company");
                        SaftFileDateModel temp = new SaftFileDateModel();
                        temp.startDate = companyTable.Tables["company"].Rows[0].Field<DateTime>("StartDate");
                        temp.endDate = companyTable.Tables["company"].Rows[0].Field<DateTime>("EndDate");
                        FinancialDashboardModel.SaftInfo = temp;

                    }
                }
            }

            string monthQuery = "";

            if (period1 > 12)
            {
                period1 = FinancialDashboardModel.SaftInfo.startDate.Month;
                period2 = FinancialDashboardModel.SaftInfo.endDate.Month;
            }

            if (period1 == period2)
            {
                monthQuery = " Month=" + period1;
            }else if(period1 < period2){
                monthQuery = " (Month >=" + period1 + "AND Month <=" + period2 + ")";
            }
            else if (period1 > period2 && period2 < FinancialDashboardModel.SaftInfo.startDate.Month)
            {
                monthQuery = " (Month >=" + period1 + "AND Month <=12) OR (Month >=1 AND Month <=" + period2 + ")";
            }
            else
            {
                monthQuery = " Month LIKE '%%'"; 
            }

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("Select Year, Month, AccountID, accountDescription, Amount, IsCredit From dbo.MonthlyAccountSums INNER JOIN dbo.Account on MonthlyAccountSums.AccountID=Account.id WHERE " + monthQuery, connection))
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
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '71%' AND" + monthQuery, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "sales");

                        tempModel.sales = 0;
                        foreach (DataRow row in FinancialTable.Tables["sales"].Rows)
                        {
                            tempModel.sales += row.Field<Int64>("Amount");
                        }
                    }
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '72%' AND" + monthQuery, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "services");

                        tempModel.services = 0;
                        foreach (DataRow row in FinancialTable.Tables["services"].Rows)
                        {
                            tempModel.services += row.Field<Int64>("Amount");
                        }
                    }
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '61%' AND" + monthQuery, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "salesExpenses");

                        tempModel.salesExpenses = 0;
                        foreach (DataRow row in FinancialTable.Tables["salesExpenses"].Rows)
                        {
                            tempModel.salesExpenses += row.Field<Int64>("Amount");
                        }
                    }
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '62%' AND" + monthQuery, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "administrativeExpenses");

                        tempModel.administrativeExpenses = 0;
                        foreach (DataRow row in FinancialTable.Tables["administrativeExpenses"].Rows)
                        {
                            tempModel.administrativeExpenses += row.Field<Int64>("Amount");
                        }
                    }
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '63%' OR AccountId LIKE '64%' OR AccountId LIKE '65%' OR AccountId LIKE '66%' OR AccountId LIKE '67%' or AccountId LIKE '68%' or AccountId LIKE '69%'  AND" + monthQuery, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "otherExpenses");

                        tempModel.otherExpenses = 0;
                        foreach (DataRow row in FinancialTable.Tables["otherExpenses"].Rows)
                        {
                            tempModel.otherExpenses += row.Field<Int64>("Amount");
                        }
                    }
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '73%' OR AccountId LIKE '74%' OR AccountId LIKE '75%' OR AccountId LIKE '76%' OR AccountId LIKE '77%' or AccountId LIKE '78%' or AccountId LIKE '79%' AND" + monthQuery, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "otherIncomes");

                        tempModel.otherIncomes = 0;
                        foreach (DataRow row in FinancialTable.Tables["otherIncomes"].Rows)
                        {
                            tempModel.otherIncomes += row.Field<Int64>("Amount");
                        }
                    }
                }

                tempModel.totalSalesAndRevenue = tempModel.sales + tempModel.services + tempModel.otherIncomes;
                tempModel.totalOperatingCosts = tempModel.salesExpenses + tempModel.administrativeExpenses + tempModel.otherExpenses;
                tempModel.netIncome = tempModel.totalSalesAndRevenue - tempModel.totalOperatingCosts;
                FinancialDashboardModel.ProfitsAndLosses.Add(tempModel);


                Int64 revenue = 0;
                Int64 operatingExpensesWithoutAmortizations = 0;
                Int64 amortizationAndDepreciation = 0;
                Int64 currentLiabilities = 0;
                Int64 inventories = 0;
                Int64 accountsReceivable = 0;
                Int64 liquidAssets = 0;


                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '22%' OR AccountId LIKE '23%' OR AccountId LIKE '24%' OR AccountId LIKE '25%' OR AccountId LIKE '26%' OR AccountId LIKE '27%' OR AccountId LIKE '28%' OR AccountId LIKE '29%' OR AccountId LIKE '6%' AND" + monthQuery, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "currentLiabilities");

                        foreach (DataRow row in FinancialTable.Tables["currentLiabilities"].Rows)
                        {
                            currentLiabilities += row.Field<Int64>("Amount");
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '3%' AND" + monthQuery, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "inventories");

                        foreach (DataRow row in FinancialTable.Tables["inventories"].Rows)
                        {
                            inventories += row.Field<Int64>("Amount");
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '21%' AND" + monthQuery, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "accountsReceivable");

                        foreach (DataRow row in FinancialTable.Tables["accountsReceivable"].Rows)
                        {
                            accountsReceivable += row.Field<Int64>("Amount");
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '1%' AND" + monthQuery, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "liquidAssets");

                        foreach (DataRow row in FinancialTable.Tables["liquidAssets"].Rows)
                        {
                            liquidAssets += row.Field<Int64>("Amount");
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '7%' AND" + monthQuery, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "revenue");

                        foreach (DataRow row in FinancialTable.Tables["revenue"].Rows)
                        {
                            revenue += row.Field<Int64>("Amount");
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '61%' OR AccountId LIKE '62%' OR AccountId LIKE '63%' AND" + monthQuery, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "operatingExpensesWithoutAmortizations");

                        foreach (DataRow row in FinancialTable.Tables["operatingExpensesWithoutAmortizations"].Rows)
                        {
                            operatingExpensesWithoutAmortizations += row.Field<Int64>("Amount");
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '64%' OR AccountId LIKE '65%' OR AccountId LIKE '66%' OR AccountId LIKE '67%' AND" + monthQuery, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(FinancialTable, "amortizationAndDepreciation");

                        foreach (DataRow row in FinancialTable.Tables["amortizationAndDepreciation"].Rows)
                        {
                            amortizationAndDepreciation += row.Field<Int64>("Amount");
                        }
                    }
                }

                FinancialDashboardModel.CompanyIndicators.EBIT = revenue - operatingExpensesWithoutAmortizations + amortizationAndDepreciation;
                FinancialDashboardModel.CompanyIndicators.EBITDA = revenue - operatingExpensesWithoutAmortizations;
                FinancialDashboardModel.CompanyIndicators.quickRatio = (liquidAssets + accountsReceivable + inventories + revenue) / (accountsReceivable);
                FinancialDashboardModel.CompanyIndicators.currentRatio = (liquidAssets + accountsReceivable + revenue) / (accountsReceivable);
            }

            return View(FinancialDashboardModel);
        }
    }
}
