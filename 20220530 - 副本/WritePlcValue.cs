using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task_Dispatch
{
    public class WritePlcValue
    {
        private static string path = Application.StartupPath + ConfigurationManager.ConnectionStrings["reqCode"].ConnectionString;
        static tools.IniFile ini = new tools.IniFile(path);
        public static int reqCode = Convert.ToInt32( ini.IniReadValue("reqCode", "value") );
        /// <summary>
        /// PLC值写入集合
        /// </summary>
        /// <param name="list1">xml结构</param>
        /// <param name="list2">plc地址索引</param>
        /// <param name="list3">plc值</param>
        /// <param name="list4">索引+值</param>
        /// <returns></returns>
        public static bool WriteValue(List<String> list1, List<String> list2, List<String> list3, List<String> list4)
        {
            try
            {
                if (list3.Count > 0)
                {
                    int k = 0;
                    list4.Clear(); //移除list4所有元素
                    for (int i = 0; i < list1.Count; i++)
                    {
                        list4.Add(i.ToString());
                    }
                    //按下标值将list3的值写入list4中
                    int o; //集合下标值
                    for (int t = 0; t < list1.Count; t++) //(t最大值应与list1.Count一致)|20
                    {
                        if (list2[t].ToString() == "")
                        {
                            o = -1; //list2中值为空时 转成‘0’
                        }
                        else
                        {
                            o = Convert.ToInt32(list2[t]); 
                        }

                        if (o != -1)
                        {
                            list4.Insert(o, list3[k]); //在指定位置插入元素
                            k++;
                        }
                        else
                        {
                            list4.Insert(t, "");
                        }
                    }
                    //生成reqCode
                    list4.RemoveAt(0);
                    reqCode += 1;
                    list4.Insert(0, reqCode.ToString());

                    ini.IniWriteValue("reqCode", "value", reqCode.ToString());
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("WritePlcValue.WriteValue：" + ex.Message);
                return false;
            }
        }
    }
}
