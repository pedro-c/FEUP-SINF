using Interop.GcpBE900;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstREST.Controllers
{
    public class SaftController : Controller
    {
        //
        // GET: /Saft/

        public ActionResult Index()
        {

            if (Lib_Primavera.PriEngine.InitializeCompany("DEMOSINF","user","Feup2017."))
            {
                var saft = new GcpBESaftExportacao();
                Lib_Primavera.PriEngine.Engine.Comercial.SaftExportacoes.GeraFicheiro(saft);

                return View(saft);
            }
            return View();
        }

    }
}
