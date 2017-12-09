using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;

namespace FirstREST.Controllers
{
    public class ChartController : ApiController
    {

        public class ARvsAP
        {
            public string[] months;
            public int[] AR;
            public int[] AP;
        }

        public class APbyPeriod
        {
            public string[] months;
            public Int64[] sales;
        }

        public class LiquidAssets
        {
            public string[] months;
            public Int64[] cash;
        }

        public string[] months = new string[12] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        public string Get(string id)
        {
            switch (id)
            {
                case "ARvsAP":
                    return getARvsAP();
                case "SalesPerMonth":
                    return getSalesPerMonth();
                case "LiquidAssets":
                    return getLiquidAssets();
                default:
                    return "";
            }
        }

        public string getARvsAP()
        {
            ARvsAP chart = new ARvsAP();
            chart.months = new string[7] { "January", "February", "March", "April", "May", "June", "July" };
            chart.AR = new int[7] { 51, 30, 40, 28, 92, 50, 45 };
            chart.AP = new int[7] { 41, 56, 25, 48, 72, 34, 12 };
            string response = JsonConvert.SerializeObject(chart);
            return response;
        }

        public string getSalesPerMonth()
        {
            string connectionString = FirstREST.SqlConnection.GetConnectionString();
            DataSet monthSales = new DataSet();
            DataSet companyInfo = new DataSet();
            APbyPeriod chart = new APbyPeriod();
            Int64 tempValue = 0;
            int initialMonth;
            chart.sales = new Int64[12];

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                for (int i = 0; i < 12; i++)
                {
                    tempValue = 0;
                    var k = i + 1;
                    using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '71%' AND Month="+k, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(monthSales, "monthSales");

                            foreach (DataRow row in monthSales.Tables["monthSales"].Rows)
                            {
                                tempValue += row.Field<Int64>("Amount");
                            }
                        }
                    }
                    chart.sales[i] = tempValue;
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.Company", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(companyInfo, "company");
                        initialMonth = companyInfo.Tables["company"].Rows[0].Field<DateTime>("StartDate").Month;
                    }
                }
            }

            chart.months = monthsOrder(initialMonth);
            string response = JsonConvert.SerializeObject(chart);
            return response;
        }

        public string getLiquidAssets()
        {
            string connectionString = FirstREST.SqlConnection.GetConnectionString();
            DataSet liquidAssetsMonth = new DataSet();
            DataSet companyInfo = new DataSet();
            LiquidAssets chart = new LiquidAssets();
            chart.cash = new Int64[12];
            Int64 tempValue = 0;
            int initialMonth;

            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                for (int i = 0; i < 12; i++)
                {
                    tempValue = 0;
                    var k = i + 1;
                    using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.MonthlyAccountSums WHERE AccountId LIKE '1%' AND Month="+k, connection))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(liquidAssetsMonth, "monthSales");

                            foreach (DataRow row in liquidAssetsMonth.Tables["monthSales"].Rows)
                            {
                                tempValue += row.Field<Int64>("Amount");
                            }
                        }
                    }
                    chart.cash[i] = tempValue;
                }

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.Company", connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(companyInfo, "company");
                        initialMonth = companyInfo.Tables["company"].Rows[0].Field<DateTime>("StartDate").Month;
                    }
                }
            }

            chart.months = monthsOrder(initialMonth);
            string response = JsonConvert.SerializeObject(chart);
            return response;
        }

        public string[] monthsOrder(int initMonth)
        {
            string[] monthsArray = new string[12];
            initMonth -= 1;

            for (int i = 0; i < 12; i++)
            {
                var k = initMonth % 12;
                monthsArray[i] = months[k];
                initMonth++;
            }

            return monthsArray;
        }
    }
}