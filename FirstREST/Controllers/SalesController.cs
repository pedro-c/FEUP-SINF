using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace FirstREST.Controllers
{
    public class SalesController : Controller
    {
        public class SalesModel
        {
            public List<InvoiceModel> CompanyInvoices = new List<InvoiceModel>();
            public List<CustomerModel> CompanyCustomers = new List<CustomerModel>();
            public SalesInfoModel SalesInfo = new SalesInfoModel();
            public SaftFileDateModel SaftInfo = new SaftFileDateModel();
            public double averageTransactionPrice;
            public double sumTotalTaxes;
            public double sumGrossTotal;
            public double averageTotalTaxes;
        }

        public class CustomerModel
        {
            public string customerId;
            public string customerAddr;
            public string customerTaxId;
            public string customerName;
            public string customerAccountId;
        }

        public class InvoiceModel
        {
            public int id;
            public string invoiceNo;
            public string invoiceStatus;
            public int period;
            public DateTime invoiceDate;
            public string invoiceType;
            public string customerID;
            public double grossTotal;
            public double netTotal;
            public double taxTotal;
        }

        public class SalesInfoModel
        {
            public double totalInvoiceDebit;
            public double totalInvoiceCredit;
        }

        public class SaftFileDateModel
        {
            public DateTime startDate;
            public DateTime endDate;
        }


        // GET: /sales/period1/period2
        public ActionResult Index(int period1 = 1, int period2 = 12)
        {
            DataSet invoiceTable = new DataSet();
            DataSet companyTable = new DataSet();
            DataSet customerTable = new DataSet();
            SalesModel SalesDashboardModel = new SalesModel();

            string connectionString = FirstREST.SqlConnection.GetConnectionString();

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                var query = " ";
                
                if(period1 < period2)
                    query = "Select * From dbo.Invoice where period >= "+ period1 + "and period <="+ period2;
                else if(period1 == period2)
                    query = "Select * From dbo.Invoice where period = "+ period1;
                else query = "Select * From dbo.Invoice where period <= 12 and period >=" + period1 + "or period >= 1 and period <=" + period2;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {

                        adapter.Fill(invoiceTable, "Invoices");

                        foreach (DataRow row in invoiceTable.Tables["Invoices"].Rows)
                        {
                            InvoiceModel temp_invoice = new InvoiceModel();
                            temp_invoice.id = row.Field<int>("id");
                            temp_invoice.invoiceNo = row.Field<string>("invoiceNo");
                            temp_invoice.invoiceStatus = row.Field<string>("invoiceStatus");
                            temp_invoice.period = row.Field<int>("period");
                            temp_invoice.invoiceDate = row.Field<DateTime>("invoiceDate");
                            temp_invoice.invoiceType = row.Field<string>("invoiceType");
                            temp_invoice.customerID = row.Field<string>("customerID");
                            temp_invoice.grossTotal = row.Field<double>("grossTotal");
                            temp_invoice.netTotal = row.Field<double>("netTotal");
                            temp_invoice.taxTotal = row.Field<double>("taxTotal");
                            SalesDashboardModel.CompanyInvoices.Add(temp_invoice);
                        }

                    }
                }

                using (SqlCommand command = new SqlCommand("Select * from dbo.Customer", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(customerTable, "Customers");

                        foreach (DataRow row in customerTable.Tables["Customers"].Rows)
                        {
                            CustomerModel tempCostumer = new CustomerModel();
                            tempCostumer.customerAccountId = row.Field<string>("AccountID");
                            tempCostumer.customerAddr = row.Field<string>("Country");
                            tempCostumer.customerId = row.Field<string>("CustomerID");
                            tempCostumer.customerName = row.Field<string>("CustomerName");
                            tempCostumer.customerTaxId = row.Field<string>("CustomerTaxID");
                            SalesDashboardModel.CompanyCustomers.Add(tempCostumer);
                        }
                    }
                }
            }

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("Select * From dbo.Sales", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {

                        adapter.Fill(invoiceTable, "sales");
                        SalesInfoModel temp = new SalesInfoModel();
                        temp.totalInvoiceCredit = invoiceTable.Tables["sales"].Rows[0].Field<double>("InvoicesTotalCredit");
                        temp.totalInvoiceDebit = invoiceTable.Tables["sales"].Rows[0].Field<double>("InvoicesTotalDebit");
                        SalesDashboardModel.SalesInfo = temp;

                    }
                }
            }

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                var query = " ";

                if (period1 < period2)
                    query = "Select AVG(GrossTotal) as average From dbo.Invoice where period >= " + period1 + "and period <=" + period2;
                else if (period1 == period2)
                    query = "Select AVG(GrossTotal) as average From dbo.Invoice where period = " + period1;
                else query = "Select AVG(GrossTotal) as average From dbo.Invoice where period <= 12 and period >=" + period1 + "or period >= 1 and period <=" + period2;


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(invoiceTable, "AverageGT");
                        SalesDashboardModel.averageTransactionPrice = invoiceTable.Tables["AverageGT"].Rows[0].Field<double>("average");
                    }
                }
            }

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                var query = " ";

                if (period1 < period2)
                    query = "Select SUM(GrossTotal) as sum From dbo.Invoice where period >= " + period1 + "and period <=" + period2;
                else if (period1 == period2)
                    query = "Select SUM(GrossTotal) as sum From dbo.Invoice where period = " + period1;
                else query = "Select SUM(GrossTotal) as sum From dbo.Invoice where period <= 12 and period >=" + period1 + "or period >= 1 and period <=" + period2;


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(invoiceTable, "SumGT");
                        SalesDashboardModel.sumGrossTotal = invoiceTable.Tables["SumGT"].Rows[0].Field<double>("sum");
                    }
                }
            }


            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                var query = " ";

                if (period1 < period2)
                    query = "Select SUM(taxTotal) as sum From dbo.Invoice where period >= " + period1 + "and period <=" + period2;
                else if (period1 == period2)
                    query = "Select SUM(taxTotal) as sum From dbo.Invoice where period = " + period1;
                else query = "Select SUM(taxTotal) as sum From dbo.Invoice where period <= 12 and period >=" + period1 + "or period >= 1 and period <=" + period2;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(invoiceTable, "SumT");
                        SalesDashboardModel.sumTotalTaxes = invoiceTable.Tables["SumT"].Rows[0].Field<double>("sum");
                    }
                }
            }

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                var query = " ";

                if (period1 < period2)
                    query = "Select AVG(taxTotal) as average From dbo.Invoice where period >= " + period1 + "and period <=" + period2;
                else if (period1 == period2)
                    query = "Select AVG(taxTotal) as average From dbo.Invoice where period = " + period1;
                else query = "Select AVG(taxTotal) as average From dbo.Invoice where period <= 12 and period >=" + period1 + "or period >= 1 and period <=" + period2;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(invoiceTable, "AverageT");
                        SalesDashboardModel.averageTotalTaxes = invoiceTable.Tables["AverageT"].Rows[0].Field<double>("average");
                    }
                }
            }

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
                        SalesDashboardModel.SaftInfo = temp;
                        
                    }
                }
            }


            return View(SalesDashboardModel);
        }
    }

}
