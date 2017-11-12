using System;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;


namespace FirstREST
{
    public class XmlLoader
    {
        public static void readSaft()
        {
            XDocument saft = XDocument.Load("C:\\SINF\\FEUP-SINF\\FirstREST\\SAFT.xml");

            foreach (XElement element in saft.Elements("Account"))
            {
                System.Diagnostics.Debug.WriteLine("Reading SAFT file");
                System.Diagnostics.Debug.WriteLine(element.Attribute("AccountID").Value);
            }

        }

        public void proccessHeader(XElement header)
        {

        }
    }
}
