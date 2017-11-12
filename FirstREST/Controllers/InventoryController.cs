using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace FirstREST.Controllers
{
    public class InventoryController : Controller
    {
        public class InventoryModel{
            public List<ProductModel> CompanyProducts = new List<ProductModel>();
        }

        public class ProductModel{
            public string code;
            public string description;
            public float alertStock;
            public float currentStock;
            public double pvp;
        }
        //
        // GET: /Inventory/

        public ActionResult Index()
        {
            DataSet productTable = new DataSet();
            InventoryModel inventoryModel = new InventoryModel();
            
            string connectionString = FirstREST.SqlConnection.GetConnectionString();

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString)){
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.Artigo", connection)){
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command)){
                        adapter.Fill(productTable, "Products");

                        foreach (DataRow row in productTable.Tables["Products"].Rows){
                            ProductModel tempModel = new ProductModel();
                            tempModel.code = row.Field<string>("CodArtigo");
                            tempModel.description = row.Field<string>("DescArtigo");
                            tempModel.alertStock = row.Field<float>("STKReposicao");
                            tempModel.currentStock = row.Field<float>("STKAtual");
                            tempModel.pvp = row.Field<double>("PVP");
                            
                            inventoryModel.CompanyProducts.Add(tempModel);
                        }

                        return View(inventoryModel);
                    }
                }
            }
        }
    }
}
