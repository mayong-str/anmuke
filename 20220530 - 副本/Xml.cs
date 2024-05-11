using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Task_Dispatch
{
    class Xml
    {
        #region //读取xmlDB文件下节点名和节点值放入集合中
        public static bool ReadGenAgvSchedulingTask(string xmlDBPath, List<String> list1, List<String> list2)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlDBPath);   //xmlDB文件路径
                list1.Clear(); //移除list1所有元素
                list2.Clear(); //移除list1所有元素

                XmlNode root = doc.DocumentElement; //获取根节点
                foreach (XmlNode s in root.ChildNodes) //遍历根节点下子节点
                {
                    if (s.Attributes.Count > 0) //判断节点是否有属性
                    {
                        if (s.Attributes["ID"].Value == "1")
                        {
                            if (doc.SelectSingleNode("//positionCodePath[@ID='1']/positionCode").Name == "positionCode")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='1']/positionCode").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='1']/positionCode").InnerText.ToString());
                            }
                            if (doc.SelectSingleNode("//positionCodePath[@ID='1']/type").Name == "type")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='1']/type").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='1']/type").InnerText.ToString());
                            }
                        }
                        if (s.Attributes["ID"].Value == "2")
                        {
                            if (doc.SelectSingleNode("//positionCodePath[@ID='2']/positionCode").Name == "positionCode")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='2']/positionCode").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='2']/positionCode").InnerText.ToString());
                            }
                            if (doc.SelectSingleNode("//positionCodePath[@ID='2']/type").Name == "type")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='2']/type").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='2']/type").InnerText.ToString());
                            }
                        }
                    }
                    else
                    {
                        list1.Add(s.Name);
                        list2.Add(s.InnerText.ToString());
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Xml.ReadGenAgvSchedulingTask：" + ex.Message);
                return false;
            }
        }
        //三个点
        public static bool ReadGen1AgvSchedulingTask(string xmlDBPath, List<String> list1, List<String> list2)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlDBPath);   //xmlDB文件路径
                list1.Clear(); //移除list1所有元素
                list2.Clear(); //移除list1所有元素

                XmlNode root = doc.DocumentElement; //获取根节点
                foreach (XmlNode s in root.ChildNodes) //遍历根节点下子节点
                {
                    if (s.Attributes.Count > 0) //判断节点是否有属性
                    {
                        if (s.Attributes["ID"].Value == "1")
                        {
                            if (doc.SelectSingleNode("//positionCodePath[@ID='1']/positionCode").Name == "positionCode")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='1']/positionCode").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='1']/positionCode").InnerText.ToString());
                            }
                            if (doc.SelectSingleNode("//positionCodePath[@ID='1']/type").Name == "type")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='1']/type").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='1']/type").InnerText.ToString());
                            }
                        }
                        if (s.Attributes["ID"].Value == "2")
                        {
                            if (doc.SelectSingleNode("//positionCodePath[@ID='2']/positionCode").Name == "positionCode")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='2']/positionCode").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='2']/positionCode").InnerText.ToString());
                            }
                            if (doc.SelectSingleNode("//positionCodePath[@ID='2']/type").Name == "type")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='2']/type").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='2']/type").InnerText.ToString());
                            }
                        }
                        if (s.Attributes["ID"].Value == "3")
                        {
                            if (doc.SelectSingleNode("//positionCodePath[@ID='3']/positionCode").Name == "positionCode")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='3']/positionCode").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='3']/positionCode").InnerText.ToString());
                            }
                            if (doc.SelectSingleNode("//positionCodePath[@ID='3']/type").Name == "type")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='3']/type").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='3']/type").InnerText.ToString());
                            }
                        }
                        //if (s.Attributes["ID"].Value == "4")
                        //{
                        //    if (doc.SelectSingleNode("//positionCodePath[@ID='4']/positionCode").Name == "positionCode")
                        //    {
                        //        list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='4']/positionCode").Name);
                        //        list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='4']/positionCode").InnerText.ToString());
                        //    }
                        //    if (doc.SelectSingleNode("//positionCodePath[@ID='4']/type").Name == "type")
                        //    {
                        //        list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='4']/type").Name);
                        //        list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='4']/type").InnerText.ToString());
                        //    }
                        //}
                    }
                    else
                    {
                        list1.Add(s.Name);
                        list2.Add(s.InnerText.ToString());
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Xml.ReadGen1AgvSchedulingTask：" + ex.Message);
                return false;
            }
        }
        //四个点
        public static bool ReadGen2AgvSchedulingTask(string xmlDBPath, List<String> list1, List<String> list2)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlDBPath);   //xmlDB文件路径
                list1.Clear(); //移除list1所有元素
                list2.Clear(); //移除list1所有元素

                XmlNode root = doc.DocumentElement; //获取根节点
                foreach (XmlNode s in root.ChildNodes) //遍历根节点下子节点
                {
                    if (s.Attributes.Count > 0) //判断节点是否有属性
                    {
                        if (s.Attributes["ID"].Value == "1")
                        {
                            if (doc.SelectSingleNode("//positionCodePath[@ID='1']/positionCode").Name == "positionCode")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='1']/positionCode").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='1']/positionCode").InnerText.ToString());
                            }
                            if (doc.SelectSingleNode("//positionCodePath[@ID='1']/type").Name == "type")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='1']/type").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='1']/type").InnerText.ToString());
                            }
                        }
                        if (s.Attributes["ID"].Value == "2")
                        {
                            if (doc.SelectSingleNode("//positionCodePath[@ID='2']/positionCode").Name == "positionCode")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='2']/positionCode").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='2']/positionCode").InnerText.ToString());
                            }
                            if (doc.SelectSingleNode("//positionCodePath[@ID='2']/type").Name == "type")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='2']/type").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='2']/type").InnerText.ToString());
                            }
                        }
                        if (s.Attributes["ID"].Value == "3")
                        {
                            if (doc.SelectSingleNode("//positionCodePath[@ID='3']/positionCode").Name == "positionCode")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='3']/positionCode").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='3']/positionCode").InnerText.ToString());
                            }
                            if (doc.SelectSingleNode("//positionCodePath[@ID='3']/type").Name == "type")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='3']/type").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='3']/type").InnerText.ToString());
                            }
                        }
                        if (s.Attributes["ID"].Value == "4")
                        {
                            if (doc.SelectSingleNode("//positionCodePath[@ID='4']/positionCode").Name == "positionCode")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='4']/positionCode").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='4']/positionCode").InnerText.ToString());
                            }
                            if (doc.SelectSingleNode("//positionCodePath[@ID='4']/type").Name == "type")
                            {
                                list1.Add(doc.SelectSingleNode("//positionCodePath[@ID='4']/type").Name);
                                list2.Add(doc.SelectSingleNode("//positionCodePath[@ID='4']/type").InnerText.ToString());
                            }
                        }
                    }
                    else
                    {
                        list1.Add(s.Name);
                        list2.Add(s.InnerText.ToString());
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Xml.ReadGen1AgvSchedulingTask：" + ex.Message);
                return false;
            }
        }
        public static bool ReadContinueTask(string xmlDBPath, List<String> list1, List<String> list2)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlDBPath);   //xmlDB文件路径
                list1.Clear(); //移除list1所有元素
                list2.Clear(); //移除list1所有元素
                               
                XmlNode root = doc.DocumentElement; //获取根节点
                foreach (XmlNode node in root.ChildNodes) //遍历根节点下子节点
                {
                    if (node.Name != "nextPositionCode")
                    {
                        list1.Add(node.Name);
                        list2.Add(node.InnerText);
                    }
                    else
                    {
                        XmlNode positionCodePath = doc.SelectSingleNode("//nextPositionCode");
                        foreach (XmlNode item in positionCodePath.ChildNodes)
                        {
                            list1.Add(item.Name);
                            list2.Add(item.InnerText);
                        }
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Xml.ReadContinueTask：" + ex.Message);
                return false;
            }
        }
        public static bool ReadCancelTask(string xmlDBPath, List<String> list1, List<String> list2)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlDBPath);   //xmlDB文件路径
                list1.Clear(); //移除list1所有元素
                list2.Clear(); //移除list1所有元素
                              
                XmlNode root = doc.DocumentElement; //获取根节点
                foreach (XmlNode node in root.ChildNodes) //遍历根节点下子节点
                {
                    list1.Add(node.Name);
                    list2.Add(node.InnerText);
                }
                return true;
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Xml.ReadCancelTask：" + ex.Message);
                return false;
            }
        }

        public static bool ReadBlockArea(string xmlDBPath, List<String> list1, List<String> list2)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlDBPath);   //xmlDB文件路径
                list1.Clear(); //移除list1所有元素
                list2.Clear(); //移除list1所有元素

                XmlNode root = doc.DocumentElement; //获取根节点
                foreach (XmlNode node in root.ChildNodes) //遍历根节点下子节点
                {
                    list1.Add(node.Name);
                    list2.Add(node.InnerText);
                }
                return true;
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Xml.ReadCancelTask：" + ex.Message);
                return false;
            }
        }
        public static bool ReadQueryPodBerthAndMat(string xmlDBPath, List<String> list1, List<String> list2)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlDBPath);   //xmlDB文件路径
                list1.Clear(); //移除list1所有元素
                list2.Clear(); //移除list1所有元素

                XmlNode root = doc.DocumentElement; //获取根节点
                foreach (XmlNode node in root.ChildNodes) //遍历根节点下子节点
                {
                    list1.Add(node.Name);
                    list2.Add(node.InnerText);
                }
                return true;
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Xml.ReadQueryPodBerthAndMat：" + ex.Message);
                return false;
            }
        }
        #endregion
        #region //将l4值写入xml结构
        public static string ReadgenAgvSchedulingTaskXML(string xmlDBPath, List<string> list)
        {
            try
            {
                string str1 = "";
                string str2 = "";
                string s1 = "";
                string s2 = "";
                ArrayList ls = new ArrayList();
                ls.Clear();
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlDBPath);
                XmlNode Root = doc.FirstChild;
                //获取根节点下所有子节点.
                XmlNodeList NodeList = Root.ChildNodes;
                //遍历输出.
                Console.WriteLine("一级节点数：" + NodeList.Count);
                for (int i = 0; i < NodeList.Count; i++)
                {
                    if (NodeList[i].ChildNodes.Count > 1)
                    {
                        for (int j = 0; j < NodeList[i].ChildNodes.Count; j++)
                        {
                            if (j == 0)
                            {
                                if (i == 8)
                                {
                                    s1 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 9)
                                {
                                    s1 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                            }
                            else
                            {
                                if (i == 8)
                                {
                                    s2 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 9)
                                {
                                    s2 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1 + 1] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                            }
                        }
                        ls.Add("<positionCodePath>" + "\r\t" + s1 + "\r\t" + s2 + "\r\t" + "</positionCodePath>");
                    }
                    else
                    {
                        if (i < 8)
                        {
                            str2 = "<" + NodeList[i].Name + ">" + list[i] + "</" + NodeList[i].Name + ">";
                            ls.Add(str2);
                        }
                        if (i >= 8)
                        {
                            str2 = "<" + NodeList[i].Name + ">" + list[i + 2] + "</" + NodeList[i].Name + ">";
                            ls.Add(str2);
                        }
                    }
                }
                foreach (string item in ls)
                {
                    str1 += item + "\r\t";
                }
                return str1;
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Xml.ReadgenAgvSchedulingTaskXML：" + ex.Message);
                return "";
            }
        }
        //增加一对点位+2
        public static string Readgen1AgvSchedulingTaskXML(string xmlDBPath, List<string> list)
        {
            try
            {
                string str1 = "";
                string str2 = "";
                string s1 = "";
                string s2 = "";
                ArrayList ls = new ArrayList();
                ls.Clear();
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlDBPath);
                XmlNode Root = doc.FirstChild;
                //获取根节点下所有子节点.
                XmlNodeList NodeList = Root.ChildNodes;
                //遍历输出.
                Console.WriteLine("一级节点数：" + NodeList.Count);
                for (int i = 0; i < NodeList.Count; i++)
                {
                    //Console.WriteLine("节点名："+NodeList[i].Name);
                    if (NodeList[i].ChildNodes.Count > 1)
                    {
                        for (int j = 0; j < NodeList[i].ChildNodes.Count; j++)
                        {
                            if (j == 0)
                            {
                                if (i == 8)
                                {
                                    s1 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 9)
                                {
                                    s1 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 10)
                                {
                                    s1 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 2] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 11)
                                {
                                    s1 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1+ 2] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                            }
                            else
                            {
                                if (i == 8)
                                {
                                    s2 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 9)
                                {
                                    s2 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1 + 1] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 10)
                                {
                                    s2 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1 + 2] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 11)
                                {
                                    s2 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1 + 1 + 2] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                            }
                        }
                        ls.Add("<positionCodePath>" + "\r\t" + s1 + "\r\t" + s2 + "\r\t" + "</positionCodePath>");
                    }
                    else
                    {
                        if (i < 8)
                        {
                            str2 = "<" + NodeList[i].Name + ">" + list[i] + "</" + NodeList[i].Name + ">";
                            ls.Add(str2);
                        }
                        if (i >= 8)
                        {
                            str2 = "<" + NodeList[i].Name + ">" + list[i + 2 + 1] + "</" + NodeList[i].Name + ">";
                            ls.Add(str2);
                        }
                    }
                }
                foreach (string item in ls)
                {
                    str1 += item + "\r\t";
                }
                return str1;
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Xml.Readgen1AgvSchedulingTaskXML：" + ex.Message);
                return "";
            }
        }

        //增加二对点位+2
        public static string Readgen2AgvSchedulingTaskXML(string xmlDBPath, List<string> list)
        {
            try
            {
                string str1 = "";
                string str2 = "";
                string s1 = "";
                string s2 = "";
                ArrayList ls = new ArrayList();
                ls.Clear();
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlDBPath);
                XmlNode Root = doc.FirstChild;
                //获取根节点下所有子节点.
                XmlNodeList NodeList = Root.ChildNodes;
                //遍历输出.
                Console.WriteLine("一级节点数：" + NodeList.Count);
                for (int i = 0; i < NodeList.Count; i++)
                {
                    //Console.WriteLine("节点名："+NodeList[i].Name);
                    if (NodeList[i].ChildNodes.Count > 1)
                    {
                        for (int j = 0; j < NodeList[i].ChildNodes.Count; j++)
                        {
                            if (j == 0)
                            {
                                if (i == 8)
                                {
                                    s1 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 9)
                                {
                                    s1 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 10)
                                {
                                    s1 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 2] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 11)
                                {
                                    s1 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1 + 2] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 12)
                                {
                                    s1 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 2+2] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 13)
                                {
                                    s1 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1 + 2+1] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                            }
                            else
                            {
                                if (i == 8)
                                {
                                    s2 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 9)
                                {
                                    s2 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1 + 1] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 10)
                                {
                                    s2 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1 + 2] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 11)
                                {
                                    s2 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1 + 1 + 2] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 12)
                                {
                                    s2 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1 + 2+2] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                                if (i == 13)
                                {
                                    s2 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1 + 1 + 2+2] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                                }
                            }
                        }
                        ls.Add("<positionCodePath>" + "\r\t" + s1 + "\r\t" + s2 + "\r\t" + "</positionCodePath>");
                    }
                    else
                    {
                        if (i < 8)
                        {
                            str2 = "<" + NodeList[i].Name + ">" + list[i] + "</" + NodeList[i].Name + ">";
                            ls.Add(str2);
                        }
                        if (i >= 8)
                        {
                            str2 = "<" + NodeList[i].Name + ">" + list[i + 2 + 1+1] + "</" + NodeList[i].Name + ">";
                            ls.Add(str2);
                        }
                    }
                }
                foreach (string item in ls)
                {
                    str1 += item + "\r\t";
                }
                return str1;
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Xml.Readgen1AgvSchedulingTaskXML：" + ex.Message);
                return "";
            }
        }
        public static string ReadContinueTaskXML(string xmlDBPath, List<string> list)
        {
            try
            {
                string str1 = "";
                string str2 = "";
                string s1 = "";
                string s2 = "";
                ArrayList ls = new ArrayList();
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlDBPath);
                XmlNode Root = doc.FirstChild;
                //获取根节点下所有子节点.
                XmlNodeList NodeList = Root.ChildNodes;
                //遍历输出.
                for (int i = 0; i < NodeList.Count; i++)
                {
                    if (NodeList[i].ChildNodes.Count > 1)
                    {
                        for (int j = 0; j < NodeList[i].ChildNodes.Count; j++)
                        {
                            if (j == 0)
                            {
                                s1 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                            }
                            else
                            {

                                s2 = "<" + NodeList[i].ChildNodes[j].Name + ">" + list[i + 1] + "</" + NodeList[i].ChildNodes[j].Name + ">";
                            }
                        }
                        ls.Add("<nextPositionCode>" + "\r\t" + s1 + "\r\t" + s2 + "\r\t" + "</nextPositionCode>");
                    }
                    else
                    {
                        str2 = "<" + NodeList[i].Name + ">" + list[i] + "</" + NodeList[i].Name + ">";
                        ls.Add(str2);
                    }
                }
                foreach (string item in ls)
                {
                    str1 += item + "\r\t";
                }
                return str1;
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Xml.ReadContinueTaskXML：" + ex.Message);
                return "";
            }
        }

        public static string ReadCancelTaskXML(string xmlDBPath, List<string> list)
        {
            try
            {
                string str1 = "";
                string str2 = "";
                ArrayList ls = new ArrayList();
                ls.Clear();
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlDBPath);
                XmlNode Root = doc.FirstChild;
                //获取根节点下所有子节点.
                XmlNodeList NodeList = Root.ChildNodes;
                Console.WriteLine("NodeList：" + NodeList.Count);
                //遍历输出.
                for (int i = 0; i < NodeList.Count; i++)
                {
                    //Console.WriteLine("NodeList：" + NodeList[i].Name);
                    str2 = "<" + NodeList[i].Name + ">" + list[i] + "</" + NodeList[i].Name + ">";
                    ls.Add(str2);
                }
                foreach (string item in ls)
                {
                    str1 += item + "\r\t";
                }
                return str1;
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Xml.ReadCancelTaskXML：" + ex.Message);
                return "";
            }
        }

        public static string ReadBlockAreaXML(string xmlDBPath, List<string> list)
        {
            try
            {
                string str1 = "";
                string str2 = "";
                ArrayList ls = new ArrayList();
                ls.Clear();
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlDBPath);
                XmlNode Root = doc.FirstChild;
                //获取根节点下所有子节点.
                XmlNodeList NodeList = Root.ChildNodes;
                Console.WriteLine("NodeList：" + NodeList.Count);
                //遍历输出.
                for (int i = 0; i < NodeList.Count; i++)
                {
                    //Console.WriteLine("NodeList：" + NodeList[i].Name);
                    str2 = "<" + NodeList[i].Name + ">" + list[i] + "</" + NodeList[i].Name + ">";
                    ls.Add(str2);
                }
                foreach (string item in ls)
                {
                    str1 += item + "\r\t";
                }
                return str1;
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Xml.ReadCancelTaskXML：" + ex.Message);
                return "";
            }
        }
        public static string ReadQueryPodBerthAndMatXML(string xmlDBPath, List<string> list)
        {
            try
            {
                string str1 = "";
                string str2 = "";
                ArrayList ls = new ArrayList();
                ls.Clear();
                XmlDocument doc = new XmlDocument();
                doc.Load(xmlDBPath);
                XmlNode Root = doc.FirstChild;
                //获取根节点下所有子节点.
                XmlNodeList NodeList = Root.ChildNodes;
                Console.WriteLine("NodeList：" + NodeList.Count);
                //遍历输出.
                for (int i = 0; i < NodeList.Count; i++)
                {
                    //Console.WriteLine("NodeList：" + NodeList[i].Name);
                    str2 = "<" + NodeList[i].Name + ">" + list[i] + "</" + NodeList[i].Name + ">";
                    ls.Add(str2);
                }
                foreach (string item in ls)
                {
                    str1 += item + "\r\t";
                }
                return str1;
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Xml.ReadQueryPodBerthAndMatXML：" + ex.Message);
                return "";
            }
        }
        #endregion
    }

}
