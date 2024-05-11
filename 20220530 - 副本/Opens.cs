using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Dispatch
{
    class Opens
    {
        //打开Springboot服务
        public static StreamReader OpenSpringboot(Process myProcess, string springboot)
        {
            StreamReader reader = null;
            try
            {
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.FileName = "java";
                myProcess.StartInfo.Arguments = "-jar " + springboot + "";
                myProcess.StartInfo.RedirectStandardOutput = true;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();

                reader = myProcess.StandardOutput;
                return reader;
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Opens.OpenSpringboot：" + ex.Message);
                return null;
            }
        }
    }
}
