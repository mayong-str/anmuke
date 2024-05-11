using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Dispatch.tools
{
    class JsonOperate
    {
        private static List<int> list = null;  //存放解析Json后的值
        /// <summary>
        /// 解析Json字符串
        /// </summary>
        /// <param name="json">Json</param>
        /// <returns></returns>
        public static List<int> Json_Decode(string json)
        {
            try
            {               
                if (json != "")
                {
                    list = new List<int>();
                    list.Clear();
                    /* 区分返回Json
                     * Post返回Json(无$)
                     * RCS任务执行通知返回Json(有$)
                     */
                    if (json.Substring(0, 1) == "$")
                    {
                        JObject jo = JObject.Parse(json.Substring(1));
                        int taskCode1 = Convert.ToInt32(jo["taskCode"].ToString());
                        list.Add(0); //code
                        //int taskCode = Convert.ToInt32(jo["taskCode"].ToString());
                        int taskCode = tools.StringIsInt.isInt(jo["taskCode"].ToString()); //判断taskCode是int还是string (string:-1)
                        list.Add(taskCode); //任务单号
                        string method = jo["method"].ToString();
                        if (method == "outbin")
                        {
                            list.Add(1);
                        }
                        else if (method == "arrive")
                        {
                            list.Add(2);
                        }
                        else if (method == "end")
                        {
                            list.Add(3);
                        }
                        else if (method == "cancel")
                        {
                            list.Add(4);
                        }
                        else if (method == "leave")
                        {
                            list.Add(5);
                        }
                        else
                        {
                            list.Add(0);
                        }
                        /*list.Add(method)*/
                        return list;
                    }
                    else if (json.Substring(0, 1) == "#")
                    {
                        JObject jo = JObject.Parse(json.Substring(1));
                        list.Add(0);
                        list.Add(0);
                        int data = Convert.ToInt32(jo["data"].ToString());
                        list.Add(6); //返回码
                        //foreach (int item in list)
                        //{
                        //    Console.Write("输出#的值：" + item);
                        //}
                        return list;
                    }
                    else if (json.Substring(0, 1) == "E")
                    {
                        JObject jo = JObject.Parse(json.Substring(1));
                        list.Add(0);
                        list.Add(0);
                        int data = Convert.ToInt32(jo["data"][0]["podCode"].ToString());
                        list.Add(data); //返回码
                        //foreach (int item in list)
                        //{
                        //    Console.Write("输出E的值：" + item);
                        //}
                        return list;
                    }
                    else if (json.Substring(0, 1) == "F")
                    {
                        JObject jo = JObject.Parse(json.Substring(1));
                        list.Add(0);
                        list.Add(0);
                        int data1 = Convert.ToInt32(jo["data"][0]["status"].ToString());
                        int data2 = Convert.ToInt32(jo["data"][1]["status"].ToString());
                        int data3 = Convert.ToInt32(jo["data"][2]["status"].ToString());
                        if (data1 == 4 && data2 == 4 && data3 == 4)
                        {
                            list.Add(2); 
                        }
                        else
                        {
                            list.Add(1);
                        }
                        //foreach (int item in list)
                        //{
                        //    Console.Write("输出F的值：" + item);
                        //}
                        return list;
                    }
                    else if (json.Substring(0, 1) == "G1")
                    {
                        JObject jo = JObject.Parse(json.Substring(1));
                        list.Add(0);
                        list.Add(0);
                        list.Add(8);
                        return list;
                    }
                    else if (json.Substring(0, 1) == "G0")
                    {
                        JObject jo = JObject.Parse(json.Substring(1));
                        list.Add(0);
                        list.Add(0);
                        list.Add(9);
                        return list;
                    }
                    else
                    {
                        JObject jo = JObject.Parse(json);
                        int code = Convert.ToInt32(jo["code"].ToString());
                        list.Add(code); //返回码
                        if (code==0)
                        {
                            list.Add(0);
                            list.Add(7);//区域解封成功
                        }
                        else
                        {
                            list.Add(-1);
                            list.Add(-1);
                        }
                        return list;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("tools.JsonOperate.Json_Decode：" + ex.Message +"请重启软件！");
                return null;
            }
        }
        //解析POST返回参数code
        public static int Json_Decode1(string json)
        {
            try
            {
                if (json != "")
                {
                    JObject jo = JObject.Parse(json);
                    int code = Convert.ToInt32(jo["code"].ToString());
                    return code;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("tools.JsonOperate.Json_Decode1：" + ex.Message);
                return -1;
            }
}
        //解析POST发送参数(绑定、解绑)
        public static int Json_Decode_Bin(string json)
        {
            try
            {
                if (json != "")
                {
                    JObject jo = JObject.Parse(json.Substring(1));
                    int code = Convert.ToInt32(jo["indBind"].ToString());
                    return code;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("tools.JsonOperate.Json_Decode_Bin：" + ex.Message);
                return -1;
            }
        }
        //解析AGV状态
        public static short agvposX1 = Convert.ToInt16(ConfigurationManager.ConnectionStrings["agvposX1"].ConnectionString);
        public static short agvposY1 = Convert.ToInt16(ConfigurationManager.ConnectionStrings["agvposY1"].ConnectionString);
        public static short agvposX2 = Convert.ToInt16(ConfigurationManager.ConnectionStrings["agvposX2"].ConnectionString);
        public static short agvposY2 = Convert.ToInt16(ConfigurationManager.ConnectionStrings["agvposY2"].ConnectionString);
        public static short agvposX3 = Convert.ToInt16(ConfigurationManager.ConnectionStrings["agvposX3"].ConnectionString);
        public static short agvposY3 = Convert.ToInt16(ConfigurationManager.ConnectionStrings["agvposY3"].ConnectionString);
        public static short agvposX4 = Convert.ToInt16(ConfigurationManager.ConnectionStrings["agvposX4"].ConnectionString);
        public static short agvposY4 = Convert.ToInt16(ConfigurationManager.ConnectionStrings["agvposY4"].ConnectionString);
        public static short wucha = Convert.ToInt16(ConfigurationManager.ConnectionStrings["wucha"].ConnectionString);
        static bool ASta = false;
        static bool BSta = false;
        static bool CSta = false;
        public static int Json_Decode_Status(string json)
        {
            try
            {
                if (json != "")
                {
                    JObject jo = JObject.Parse(json);
                    int posX1 = Convert.ToInt32(jo["data"][0]["posX"].ToString());
                    int posY1 = Convert.ToInt32(jo["data"][0]["posY"].ToString());
                    int posX2 = Convert.ToInt32(jo["data"][1]["posX"].ToString());
                    int posY2 = Convert.ToInt32(jo["data"][1]["posY"].ToString());
                    int posX3 = Convert.ToInt32(jo["data"][2]["posX"].ToString());
                    int posY3 = Convert.ToInt32(jo["data"][2]["posY"].ToString());
                    if (
                        ((agvposX1 - wucha <= posX1 && posX1 <= agvposX1 + wucha)|| (agvposX2 - wucha <= posX1 && posX1 <= agvposX2 + wucha)|| (agvposX3 - wucha <= posX1 && posX1 <= agvposX3 + wucha)|| (agvposX4 - wucha <= posX1 && posX1 <= agvposX4 + wucha))&&
                        ((agvposY1 - wucha <= posY1 && posY1 <= agvposY1 + wucha)|| (agvposY2 - wucha <= posY1 && posY1 <= agvposY2 + wucha)|| (agvposY3 - wucha <= posY1 && posY1 <= agvposY3 + wucha)|| (agvposY4 - wucha <= posY1 && posY1 <= agvposY4 + wucha))
                       )
                    {
                        ASta = true;
                    }
                    else
                    {
                        ASta = false;
                    }
                    if (
                        ((agvposX1 - wucha <= posX2 && posX2 <= agvposX1 + wucha) || (agvposX2 - wucha <= posX2 && posX2 <= agvposX2 + wucha) || (agvposX3 - wucha <= posX2 && posX2 <= agvposX3 + wucha) || (agvposX4 - wucha <= posX2 && posX2 <= agvposX4 + wucha)) &&
                        ((agvposY1 - wucha <= posY2 && posY2 <= agvposY1 + wucha) || (agvposY2 - wucha <= posY2 && posY2 <= agvposY2 + wucha) || (agvposY3 - wucha <= posY2 && posY2 <= agvposY3 + wucha) || (agvposY4 - wucha <= posY2 && posY2 <= agvposY4 + wucha))
                       )
                    {
                        BSta = true;
                    }
                    else
                    {
                        BSta = false;
                    }
                    if (
                        ((agvposX1 - wucha <= posX3 && posX3 <= agvposX1 + wucha) || (agvposX2 - wucha <= posX3 && posX3 <= agvposX2 + wucha) || (agvposX3 - wucha <= posX3 && posX3 <= agvposX3 + wucha) || (agvposX4 - wucha <= posX3 && posX3 <= agvposX4 + wucha)) &&
                        ((agvposY1 - wucha <= posY3 && posY3 <= agvposY1 + wucha) || (agvposY2 - wucha <= posY3 && posY3 <= agvposY2 + wucha) || (agvposY3 - wucha <= posY3 && posY3 <= agvposY3 + wucha) || (agvposY4 - wucha <= posY3 && posY3 <= agvposY4 + wucha))
                       )
                    {
                        CSta = true;
                    }
                    else
                    {
                        CSta = false;
                    }

                    if (ASta == true && BSta == true && CSta == true)
                    {
                        return 2;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("tools.JsonOperate.Json_Decode_Status：" + ex.Message);
                return 0;
            }
        }
    }
}
