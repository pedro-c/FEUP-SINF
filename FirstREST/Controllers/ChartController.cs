using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

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
            public string[] semesters;
            public int[] AP;
        }

        public class CashBalance
        {
            public string[] months;
            public int[] cash;
        }

        public string Get(string id)
        {
            switch (id)
            {
                case "ARvsAP":
                    return getARvsAP();
                case "APbyPeriod":
                    return getAPbyPeriod();
                case "CashBalance":
                    return getCashBalance();
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

        public string getAPbyPeriod()
        {
            APbyPeriod chart = new APbyPeriod();
            chart.semesters = new string[4] { "<30 days", "<60 days", "<90 days", "<120 days" };
            chart.AP = new int[4] { 100, 80, 60, 40 };
            string response = JsonConvert.SerializeObject(chart);
            return response;
        }

        public string getCashBalance()
        {
            CashBalance chart = new CashBalance();
            chart.months = new string[7] { "January", "February", "March", "April", "May", "June", "July" };
            chart.cash = new int[7] { 31, 74, 6, 39, 20, 85, 7 };
            string response = JsonConvert.SerializeObject(chart);
            return response;
        }
    }
}
