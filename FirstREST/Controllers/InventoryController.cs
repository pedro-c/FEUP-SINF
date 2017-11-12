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
            public double alertStock;
            public double currentStock;
            public Boolean needsRestock;
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
                            tempModel.code = row.Field<string>("artigo");
                            tempModel.description = row.Field<string>("descricao");
                            tempModel.alertStock = row.Field<double>("stk_reposicao");
                            tempModel.currentStock = row.Field<double>("stk_atual");
                            tempModel.needsRestock = row.Field<Boolean>("needs_restock");
                            tempModel.pvp = row.Field<double>("pvp");
                            
                            inventoryModel.CompanyProducts.Add(tempModel);
                        }

                        return View(inventoryModel);
                    }
                }
            }
        }
    }
}
