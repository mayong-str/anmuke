using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Task_Dispatch
{
    class Xml2Json
    {
        public static bool Conver(string xmlStr, string A,List<String> l5)
        {
            try
            {
                //XML文件结构转JSON
                string json = A + xmlToJson(xmlStr);
                if (json.Substring(1) != "")
                {
                    Console.WriteLine("请求参数：" + json.Substring(1));
                    l5.Add(json);

                    //2022-07-01测试
                    //l5.Add(json.Substring(1));
                    GlobalLog.WriteInfoLog("请求参数Json："+ json.Substring(1));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Xml2Json.Conver：" + ex.Message);
                return false;
            }
        }

        public static string xmlToJson(string xmlStr)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<RequestParameter>" + xmlStr + "</RequestParameter>");
                string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
                return HttpUtils.ConvertJsonString(json.Substring(20, json.Length - 21));
            }
            catch (Exception)
            {
                return "";
            }
        }
        //2022-06-06 测试
        public static string XmlToJson(string xmlstr)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlstr);
                string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
                return HttpUtils.ConvertJsonString(json.Substring(20, json.Length - 21));
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog(ex.Message);
                return "";
            }
        }
    }
}
