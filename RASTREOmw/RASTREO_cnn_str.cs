using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace RASTREOmw
{
    public class cnn_str
    {
        public static string CadenaDeConexion
        {
            get
            {
                string sTargetDir = AppDomain.CurrentDomain.BaseDirectory + "Bin\\";
                XmlDocument myXML = new XmlDocument();
                if (!System.IO.File.Exists(sTargetDir + "Web.config"))
                    sTargetDir = AppDomain.CurrentDomain.BaseDirectory;
                if (!System.IO.File.Exists(sTargetDir + "Web.config"))
                    return RASTREOmw.Properties.Settings.Default["DefaultCNNSTR"].ToString();
                myXML.Load(sTargetDir + "Web.config");
                XmlNodeList XL = myXML.SelectNodes("configuration/connectionStrings/add");
                if (XL.Count > 0)
                    return XL.Item(0).Attributes.GetNamedItem("connectionString").Value;
                return RASTREOmw.Properties.Settings.Default["DefaultCNNSTR"].ToString();
            }
        }

    }
}