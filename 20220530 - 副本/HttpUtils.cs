using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Dispatch
{
    class HttpUtils
    {
        /// <summary>
        /// 整理Json格式
        /// </summary>
        /// <param name="str">json字符串</param>
        /// <returns></returns>
        public static string ConvertJsonString(string str)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                TextReader reader = new StringReader(str);
                JsonTextReader jsonreader = new JsonTextReader(reader);
                object ob = serializer.Deserialize(jsonreader);
                if (ob != null)
                {
                    StringWriter writer = new StringWriter();
                    JsonTextWriter jsonWriter = new JsonTextWriter(writer)
                    {
                        Formatting = Formatting.Indented,
                        Indentation = 4,  //缩进
                        IndentChar = ' '  //空格
                    };
                    serializer.Serialize(jsonWriter, ob);
                    return writer.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("HttpUtils.ConvertJsonString：" + ex.Message);
                return "";
            }
        }
    }
}
