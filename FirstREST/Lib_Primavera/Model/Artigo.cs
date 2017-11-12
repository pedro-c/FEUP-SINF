using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Artigo
    {
        public string CodArtigo
        {
            get;
            set;
        }

        public string DescArtigo
        {
            get;
            set;
        }

        public double PVP
        {
            get;
            set;
        }

        public double STKAtual
        {
            get;
            set;
        }

        public double STKReposicao
        {
            get;
            set;
        }

    }
}