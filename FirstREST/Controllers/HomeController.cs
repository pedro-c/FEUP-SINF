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
             public int numberOfEmployees { get; set; }
             public string companyEmail { get; set; }
             public string companyWebsite { get; set; }
             public string companyName { get; set; }
             public DateTime saftCreationDate { get; set; }
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
                        CompanyInformation.numberOfEmployees = company.Rows[0].Field<int>("numberOfEmployees");
                        CompanyInformation.companyEmail = company.Rows[0].Field<String>("email");
                        CompanyInformation.companyWebsite = company.Rows[0].Field<String>("website");
                        CompanyInformation.companyName = company.Rows[0].Field<String>("name");
                        CompanyInformation.saftCreationDate = company.Rows[0].Field<DateTime>("saftCreationDate");

                        return View(CompanyInformation);
                    }
                }
            }
        }
    }
}
