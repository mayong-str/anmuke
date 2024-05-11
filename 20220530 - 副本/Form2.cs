using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Task_Dispatch.Model;

namespace Task_Dispatch
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            string path = @"C:\Users\admin\Desktop\20220530 - 副本\bin\Debug\xmlDB\genAgvSchedulingTask - 1.xml";
            WriteXml(path);

        }

        private void WriteXml(string path)
        {
            Model.genAgvSchedulingTask g = new Model.genAgvSchedulingTask();
            g.ReqCode = "1";
            g.ReqTime = DateTime.Now.ToString();
            g.ClientCode = "1";
            g.TokenCode = "1";
            g.TaskTyp = "1";
            g.CtnrTyp = "1";
            g.CtnrCode = "1";
            g.WebCode = "1";
           
            List<positionCodePath> list = new List<positionCodePath>();
            Model.positionCodePath p1 = new Model.positionCodePath();
            p1.PositionCode = "1";
            p1.Type = "1";
            list.Add(p1);
            Model.positionCodePath p2 = new Model.positionCodePath();
            p2.PositionCode = "2";
            p2.Type = "2";
            list.Add(p2);
            g.PositionCodePath = list;
            g.PodCode = "1";
            g.PodDir = "1";
            g.PodTyp = "1";
            g.MaterialLot = "1";
            g.Priority = "1";
            g.AgvCode = "1";
            g.TaskCode = "1";
            g.Data = "1";
            string str = XmlSerialize(g);
            Debug.WriteLine(str);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(str);
            Debug.WriteLine(JsonConvert.SerializeXmlNode(xmlDocument)) ;
        }


        public static string XmlSerialize<T>(T obj)
        {
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    XmlSerializer serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(sw, obj);
                    sw.Close();
                    return sw.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("将实体对象转换成XML异常", ex);
            }
        }
    }
}
