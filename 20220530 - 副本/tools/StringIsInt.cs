using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Dispatch.tools
{
    class StringIsInt
    {
        public static int isInt(string str)
        {
            try
            {
                int taskCode = Convert.ToInt32(str);
                return taskCode;
            }
            catch (Exception)
            {
                GlobalLog.WriteInfoLog("输入字符串的格式不正确-taskCode：" + str);
                return -1;
            }
        }
    }
}
