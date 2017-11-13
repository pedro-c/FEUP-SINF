using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FirstREST.Controllers
{
    public class ChartController : ApiController
    {
        public int[] Get(string id)
        {
            return new int[7] { 51, 30, 40, 28, 92, 50, 45 };
        }
    }
}
