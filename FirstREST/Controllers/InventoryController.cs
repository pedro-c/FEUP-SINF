using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstREST.Controllers
{
    public class InventoryController : Controller
    {
        //
        // GET: /Inventory/

        public class InventoryModel
        {
            public List<SupplierModel> suppliers = new List<SupplierModel>();
        }

        public class SupplierModel
        {
            public int id;
            public string name;
            public string phoneNumber;
            public string email;
        }

        public ActionResult Index()
        {
            DataSet supplierTable = new DataSet();
            InventoryModel InventoryDashboardModel = new InventoryModel();

            string connectionString = FirstREST.SqlConnection.GetConnectionString();

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("Select * From dbo.Supplier", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {

                        adapter.Fill(supplierTable, "Supplier");

                        foreach (DataRow row in supplierTable.Tables["Supplier"].Rows)
                        {
                            SupplierModel temp_supplier = new SupplierModel();
                            temp_supplier.id = row.Field<int>("id");
                            temp_supplier.name = row.Field<string>("name");
                            temp_supplier.phoneNumber = row.Field<string>("phoneNumber");
                            temp_supplier.email = row.Field<string>("email");
                            //temp_invoice.grossTotal = Convert.ToString(row.Field<float>("grossTotal"));
                            InventoryDashboardModel.suppliers.Add(temp_supplier);
                        }

                        return View(InventoryDashboardModel);
                    }
                }
            }
        }
    }
}
