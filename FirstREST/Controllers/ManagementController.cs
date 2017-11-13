using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstREST.Controllers
{
    public class ManagementController : Controller
    {

        public class ManagementModel
        {
            public List<EmployeeModel> employees = new List<EmployeeModel>();
        }

        public class EmployeeModel
        {
            public string id;
            public string abbrvName;
            public string phone;
            public string email;
        }

        // GET: /Management/
        public ActionResult Index()
        {
            DataSet employeesTable = new DataSet();
            ManagementModel ManagementModel = new ManagementModel();

            string connectionString = FirstREST.SqlConnection.GetConnectionString();

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("Select * From dbo.Employee", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {

                        adapter.Fill(employeesTable, "Employees");

                        foreach (DataRow row in employeesTable.Tables["Employees"].Rows)
                        {
                            EmployeeModel temp_employee = new EmployeeModel();
                            temp_employee.id = row.Field<string>("id");
                            temp_employee.abbrvName = row.Field<string>("abbrv_name");
                            temp_employee.phone = row.Field<string>("phone");
                            temp_employee.email = row.Field<string>("email");
                            ManagementModel.employees.Add(temp_employee);
                        }

                        return View(ManagementModel);
                    }
                }
            }
        }
    }
}
