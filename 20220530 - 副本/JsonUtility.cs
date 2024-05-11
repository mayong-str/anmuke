using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Dispatch;

namespace _20220530
{
    public class JsonUtility
    {
        /// <summary>
        /// json字符串格式化
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public string JsonFormat(string json)
        {
            JsonSerializer serializer = new JsonSerializer();
            TextReader reader = new StringReader(json);
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

        public void HttpPostReqest(string interFaceName, string requestData)
        {

        }
    }
}
