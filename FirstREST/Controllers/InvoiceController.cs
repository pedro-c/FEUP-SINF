using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using FirstREST.Lib_Primavera.Model;
using System.Data.SqlClient;

namespace FirstREST.Controllers
{
    public class InvoiceController : Controller
    {
        public class InvoiceControllerModel
        {
            public CompanyModel companyInfo;
            public CustomerModel customerInfo;
            public InvoiceModel invoiceInfo;
        }

        public class CompanyModel
        {
            public string companyName;
            public string companyAddress;
        }

        public class CustomerModel
        {
            public string customerName;
            public string customerCountry;
        }

        public class InvoiceLineModel
        {
            public string productCode;
            public string productDescription;
            public int quantity;
            public string unitPrice;
            public string subtotal;

        }
        public class InvoiceModel
        {
            public string invoiceNo;
            public string invoiceStatus;
            public int period;
            public DateTime invoiceDate;
            public string invoiceType;
            public string customerID;
            public double grossTotal;
            public double netTotal;
            public double taxTotal;

            public List<InvoiceLineModel> lines = new List<InvoiceLineModel>();
        }

        // GET: /invoice/ab123
        public ActionResult Index(int id)
        {
            InvoiceControllerModel model = new InvoiceControllerModel();
            InvoiceModel invoice = new InvoiceModel();
     
            string connectionString = FirstREST.SqlConnection.GetConnectionString();

            DataSet invoiceTable = new DataSet();
            DataSet invoiceLinesTable = new DataSet();
            DataSet companyTable = new DataSet();
            DataSet customerTable = new DataSet();

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                var query = "SELECT * FROM dbo.Invoice WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        System.Diagnostics.Debug.WriteLine("ID");
                        System.Diagnostics.Debug.WriteLine(id);
                        adapter.SelectCommand.Parameters.AddWithValue("@Id", id);
                        adapter.Fill(invoiceTable, "Invoices");
                        
                        foreach(DataRow row in invoiceTable.Tables["Invoices"].Rows)
                        {
                            invoice.invoiceNo = row.Field<string>("invoiceNo");
                            invoice.invoiceStatus = row.Field<string>("invoiceStatus");
                            invoice.period = row.Field<int>("period");
                            invoice.invoiceDate = row.Field<DateTime>("invoiceDate");
                            invoice.invoiceType = row.Field<string>("invoiceType");
                            invoice.customerID = row.Field<string>("customerID");
                            invoice.grossTotal = row.Field<double>("grossTotal");
                            invoice.netTotal = row.Field<double>("netTotal");
                            invoice.taxTotal = row.Field<double>("taxTotal");
                            model.invoiceInfo = invoice;
                        }
                    }
                }

                query = "SELECT * FROM dbo.Line JOIN dbo.Artigo ON dbo.Artigo.artigo = dbo.Line.ProductCode WHERE InvoiceNo = @InvoiceNo";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@InvoiceNo", invoice.invoiceNo);
                        adapter.Fill(invoiceLinesTable, "Lines");

                        foreach(DataRow row in invoiceLinesTable.Tables["Lines"].Rows)
                        {
                            InvoiceLineModel tmp_line = new InvoiceLineModel();
                            tmp_line.productCode = row.Field<string>("ProductCode");
                            tmp_line.quantity = row.Field<int>("Quantity");
                            tmp_line.subtotal = row.Field<string>("CreditAmount");
                            tmp_line.unitPrice = row.Field<string>("UnitPrice");
                            tmp_line.productDescription = row.Field<string>("descricao");
                            invoice.lines.Add(tmp_line);
                        }
                    }
                }

                query = "SELECT * FROM dbo.Company";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(companyTable, "Company");

                        foreach(DataRow row in companyTable.Tables["Company"].Rows)
                        {
                            CompanyModel company = new CompanyModel();
                            company.companyAddress = row.Field<string>("StreetName");
                            company.companyName = row.Field<string>("CompanyName");
                            model.companyInfo = company;
                        }
                    }
                }

                query = "SELECT * FROM dbo.Customer WHERE CustomerID = @CustomerId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@CustomerId", invoice.customerID);
                        adapter.Fill(customerTable, "Customer");
                        
                        foreach(DataRow row in customerTable.Tables["Customer"].Rows)
                        {
                            CustomerModel customer = new CustomerModel();
                            customer.customerCountry = row.Field<string>("Country");
                            customer.customerName = row.Field<string>("CustomerName");
                            model.customerInfo = customer;
                        }
                    }
                }
            }

            return View(model);
        }
    }
}

