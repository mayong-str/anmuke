using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task_Dispatch
{
    class OpenThread
    {
        //PingPLC
        public static void PLCThread()
        {
            ThreadStart plc = new ThreadStart(Pings.PingPLC);
            Thread ip = new Thread(plc);
            ip.IsBackground = true;
            ip.Start();
        }
        //PingRCS
        public static Thread ip;
        public static void RCSThread()
        {
            //ThreadStart rcs = new ThreadStart(Pings.PingRCS);
            //ip = new Thread(rcs);
            //ip.IsBackground = true;
            //ip.Start();
        }
        /* ReadPlc
         * PLC值写入集合l5(json)
         */
        public static Thread readPlc;
        public static void ReadPlc()
        {
            ThreadStart add = new ThreadStart(PLC.ReadPlc);
            readPlc = new Thread(add);
            readPlc.IsBackground = true;
            readPlc.Start();
        }
        //通道2
        public static Thread read2Plc;
        public static void Read2Plc()
        {
            ThreadStart add = new ThreadStart(PLC.Read2Plc);
            read2Plc = new Thread(add);
            read2Plc.IsBackground = true;
            read2Plc.Start();
        }
        /*Post-遍历集合l5
         *集合不为空 
         */
        public static Thread l5;
        public static void Traversel5()
        {
            ThreadStart traverse = new ThreadStart(Form1.Traversel5);
            l5 = new Thread(traverse);
            l5.IsBackground = true;
            l5.Start();
        }
        //springboot
        public static Thread read;
        public static void Springboot()
        {
            ThreadStart Springboot = new ThreadStart(Form1.ReadSpringboot);
            read = new Thread(Springboot);
            read.IsBackground = true;
            read.Start();
        }
        /* WritePlc
         * 集合lR(json)写入PLC
         */
        public static Thread lR;
        public static void WritePlc()
        {
            ThreadStart Write = new ThreadStart(Form1.WritePlc);
            lR = new Thread(Write);
            lR.IsBackground = true;
            lR.Start();
        }
        /*WriteDataBase
         * 间隔时间，读取PLC数据写入数据库
         */
        public static Thread db;
        public static void WriteDataBase()
        {
            ThreadStart writedb = new ThreadStart(PLC.WriteDataBase);
            db = new Thread(writedb);
            db.IsBackground = true;
            db.Start();
        }
        //agv运行/不运行
        public static Thread post;
        public static void WritePost()
        {
            ThreadStart writepost = new ThreadStart(Form1.WritePost);
            post = new Thread(writepost);
            post.IsBackground = true;
            post.Start();
        }
    }
}
