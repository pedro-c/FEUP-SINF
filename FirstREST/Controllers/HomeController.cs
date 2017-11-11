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
                public string CompanyName {get; set;}
                public string CompanyEmail {get; set;}
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
                        CompanyInformation.CompanyName = company.Rows[0].Field<String>("companyName");
                        CompanyInformation.CompanyEmail = company.Rows[0].Field<String>("Email");
                        
                        return View(CompanyInformation);
                    }
                }
            }
        }
    }
}
