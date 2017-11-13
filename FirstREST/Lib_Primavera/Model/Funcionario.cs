using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstREST.Lib_Primavera.Model
{
    public class Funcionario
    {
        public string Morada;

        /* Exemplo para POST e GET com valores específicos
         public string Morada
        {
            get
            {
                return "MORADA: " + _morada;
            }
            set
            {
                _morada = value;
            }
        }
    
*/       
        public string CodFuncionario
        {
            get;
            set;
        }

        public string NomeAbreviado
        {
            get;
            set;
        }

        public string NumTelefone
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }


    }
}