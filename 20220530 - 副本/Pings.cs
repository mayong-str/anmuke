using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task_Dispatch
{
    class Pings
    {
        private static string PLCip = ConfigurationManager.ConnectionStrings["PLCip"].ConnectionString;
        private static string RCSip = ConfigurationManager.ConnectionStrings["RCSip"].ConnectionString;
        private static string RCS = ConfigurationManager.ConnectionStrings["RCS"].ConnectionString;
        //private static Ping ping = new Ping();
       
        //ping PLC-IP
        public static void PingPLC()
        {
            while (true)
            {
                try
                {
                    using (Ping ping = new Ping())
                    {
                        PingReply pingReplyPLC = ping.Send(PLCip);
                        if (pingReplyPLC.Status == IPStatus.Success)
                        {
                            Form1.PLConline = true;
                        }
                        else
                        {
                            Form1.PLConline = false;
                        }
                        Thread.Sleep(2000);
                    }                   
                }
                catch (Exception)
                {

                }              
            }
        }
        //Ping RCS-IP
        //public static void PingRCS()
        //{
        //    while (true)
        //    {
        //        try
        //        {
        //            PingReply pingReplyPLC = ping.Send(RCS);
        //            if (pingReplyPLC.Status == IPStatus.Success)
        //            {
        //                Form1.RCSonline = true;
        //            }
        //            else
        //            {
        //                Form1.RCSonline = false;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            GlobalLog.WriteInfoLog("Pings-PingRCS：" + ex.Message);
        //            //Form1.FormClose();
        //        }
                
        //    }
        //}
    }
}
