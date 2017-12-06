using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstREST.Controllers
{
  
    public class HomeController : Controller
    {

         public class CompanyModel
         {
             public DateTime startDate { get; set; }
             public DateTime endDate { get; set; }
             public string companyName { get; set; }
             public string fiscalYear { get; set; }
             public string city { get; set; }
             public string country { get; set; }
             public string street { get; set; }
         }

        public ActionResult Index()
        {
            DataTable company = new DataTable();
            CompanyModel CompanyInformation = new CompanyModel();

            string connectionString = FirstREST.SqlConnection.GetConnectionString();

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("Select * From dbo.Company", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(company);
                        CompanyInformation.startDate = company.Rows[0].Field<DateTime>("StartDate");
                        CompanyInformation.endDate = company.Rows[0].Field<DateTime>("EndDate");
                        CompanyInformation.fiscalYear = company.Rows[0].Field<String>("FiscalYear");
                        CompanyInformation.companyName = company.Rows[0].Field<String>("CompanyName");
                        CompanyInformation.city = company.Rows[0].Field<String>("City");
                        CompanyInformation.country = company.Rows[0].Field<String>("Country");
                        CompanyInformation.street = company.Rows[0].Field<String>("StreetName");

                        return View(CompanyInformation);
                    }
                }
            }
        }
    }
}


