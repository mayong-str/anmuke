using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using S7.Net;
namespace Task_Dispatch
{
    class PLC
    {
        public static Plc plc = null;
        //模板编号
        static string gen1 = ConfigurationManager.ConnectionStrings["gen1"].ConnectionString;
        static string gen2 = ConfigurationManager.ConnectionStrings["gen2"].ConnectionString;
        static string gen3 = ConfigurationManager.ConnectionStrings["gen3"].ConnectionString;
        static string con = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        static string can = ConfigurationManager.ConnectionStrings["can"].ConnectionString;
        static string blo = ConfigurationManager.ConnectionStrings["blo"].ConnectionString;
        static string query = ConfigurationManager.ConnectionStrings["query"].ConnectionString;
        static string status = ConfigurationManager.ConnectionStrings["status"].ConnectionString;
        static string bin = ConfigurationManager.ConnectionStrings["bin"].ConnectionString;
        //xmlDB文件夹路径
        static string xmlDBgenAgvSchedulingTask1 = Application.StartupPath + ConfigurationManager.ConnectionStrings["xmlDBgenAgvSchedulingTask1"].ConnectionString;
        static string xmlDBgenAgvSchedulingTask2 = Application.StartupPath + ConfigurationManager.ConnectionStrings["xmlDBgenAgvSchedulingTask2"].ConnectionString;
        static string xmlDBgenAgvSchedulingTask3 = Application.StartupPath + ConfigurationManager.ConnectionStrings["xmlDBgenAgvSchedulingTask3"].ConnectionString;
        static string xmlDBcontinueTask = Application.StartupPath + ConfigurationManager.ConnectionStrings["xmlDBcontinueTask"].ConnectionString;
        static string xmlDBcancelTask = Application.StartupPath + ConfigurationManager.ConnectionStrings["xmlDBcancelTask"].ConnectionString;
        static string xmlDBblockArea = Application.StartupPath + ConfigurationManager.ConnectionStrings["xmlDBblockArea"].ConnectionString;
        static string xmlDBqueryPodBerthAndMat = Application.StartupPath + ConfigurationManager.ConnectionStrings["xmlDBqueryPodBerthAndMat"].ConnectionString;
        static string xmlDBqueryAgvStatus = Application.StartupPath + ConfigurationManager.ConnectionStrings["xmlDBqueryAgvStatus"].ConnectionString;
        static string xmlDBbindPodAndBerth = Application.StartupPath + ConfigurationManager.ConnectionStrings["xmlDBbindPodAndBerth"].ConnectionString;
        //全局变量
        public static List<String> l1 = new List<String>();
        public static List<String> l2 = new List<String>();
        public static List<String> l3 = new List<String>();
        public static List<String> l4 = new List<String>();
        //存放PLC请求返回的json
        public static List<String> l5 = new List<String>();
        //生成任务单返回值
        public static List<int> lU = new List<int>();
        /*连接PLC的参数值
         * cpu
         * PLCip
         * rack
         * slot
         */
        private static string PLCip = ConfigurationManager.ConnectionStrings["PLCip"].ConnectionString;
        private static short rack = Convert.ToInt16(ConfigurationManager.ConnectionStrings["rack"].ConnectionString);
        private static short slot = Convert.ToInt16(ConfigurationManager.ConnectionStrings["slot"].ConnectionString);
        /*读PLC db块-位
         * db
         * startByteAdr
         * bytes
         * initialbit
         * readybit
         * commandbit
         */
        private static short db = Convert.ToInt16(ConfigurationManager.ConnectionStrings["db"].ConnectionString);
        private static short startByteAdr = Convert.ToInt16(ConfigurationManager.ConnectionStrings["startByteAdr"].ConnectionString);
        //任务类型起点
        private static short startByteAdrV1 = Convert.ToInt16(ConfigurationManager.ConnectionStrings["startByteAdrV1"].ConnectionString);
        private static short bytes = Convert.ToInt16(ConfigurationManager.ConnectionStrings["bytes"].ConnectionString);
        //PLC初始位
        private static byte initialbit = Convert.ToByte(ConfigurationManager.ConnectionStrings["initialbit"].ConnectionString);
        //PLC就绪位
        private static byte readybit = Convert.ToByte(ConfigurationManager.ConnectionStrings["readybit"].ConnectionString);
        //PLC命令位
        private static byte commandbit = Convert.ToByte(ConfigurationManager.ConnectionStrings["commandbit"].ConnectionString);
        //PC就绪
        private static byte wjuxuplc = Convert.ToByte(ConfigurationManager.ConnectionStrings["wjuxuplc"].ConnectionString);
        //PC命令
        private static byte minglingplc = Convert.ToByte(ConfigurationManager.ConnectionStrings["minglingplc"].ConnectionString);
        //PLC-下发task
        private static byte xiafatask = Convert.ToByte(ConfigurationManager.ConnectionStrings["xiafatask"].ConnectionString);
        //PC-PLC agv任务完成
        private static byte renwuoverplc = Convert.ToByte(ConfigurationManager.ConnectionStrings["renwuoverplc"].ConnectionString);
        //PLC-PC 接收任务完成
        private static byte jieshouplc = Convert.ToByte(ConfigurationManager.ConnectionStrings["jieshouplc"].ConnectionString);
        /*读PLC 模板号
        * db
        * startByteAdr
        * Count
        */
        private static byte dbV = Convert.ToByte(ConfigurationManager.ConnectionStrings["dbV"].ConnectionString);
        private static byte startByteAdrV = Convert.ToByte(ConfigurationManager.ConnectionStrings["startByteAdrV"].ConnectionString);
        private static byte countV = Convert.ToByte(ConfigurationManager.ConnectionStrings["countV"].ConnectionString);
        //连接PLC
        public static bool ConnectePLC()
        {
            try
            {
                plc = new Plc(CpuType.S71500, PLCip, rack, slot);
                plc.Open();
                if (plc.IsConnected)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        //断开PLC连接
        public static bool ClosePLC()
        {
            try
            {
                plc.Close();
                if (!plc.IsConnected)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteErrorLog("PLC.ClosePLC：" + ex.Message);
                return false;
            }
        }
        //读PLC初始位
        public static bool InitialPLC()
        {
            try
            {
                bool InitialBit = Convert.ToBoolean(plc.Read(DataType.DataBlock, db, startByteAdr, VarType.Bit, bytes, initialbit));
                if (InitialBit)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //读PLC就绪位
        public static bool ReadyPLC()
        {
            try
            {
                bool ReadyBit = Convert.ToBoolean(plc.Read(DataType.DataBlock, db, startByteAdr, VarType.Bit, bytes, readybit));
                if (ReadyBit)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteErrorLog("PLC.ReadyPLC" + ex.Message);
                Form1.richTextBox_Error_Info.Text += ex.Message + "\r\n";
                bool conn = ConnectePLC();
                if (conn)
                {
                    GlobalLog.WriteInfoLog("打开PLC连接成功！");
                    Form1.richTextBox_Error_Info.Text += "打开PLC连接成功！" + "\r\n";
                }
                else
                {
                    GlobalLog.WriteInfoLog("打开PLC连接失败！");
                    Form1.richTextBox_Error_Info.Text += "打开PLC连接失败！" + "\r\n";
                }
                return false;
            }
        }
        //读PLC命令位
        public static bool CommandPLC()
        {
            try
            {
                bool CommandBit = Convert.ToBoolean(plc.Read(DataType.DataBlock, db, startByteAdr, VarType.Bit, bytes, commandbit));
                if (CommandBit)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        //写入PLC就绪 -true
        public static void WriteJuXuPLC()
        {
            try
            {
                plc.WriteBit(DataType.DataBlock, db, startByteAdr, wjuxuplc, true);
            }
            catch (Exception)
            {

            }
        }
        //写入PLC就绪 -false
        public static void WriteJuXu1PLC()
        {
            try
            {
                plc.WriteBit(DataType.DataBlock, db, startByteAdr, wjuxuplc, false);
            }
            catch (Exception)
            {

            }
        }

        //写入 PC-PLC命令结束 -true  minglingplc
        public static void WriteminglingPLC()
        {
            try
            {
                plc.WriteBit(DataType.DataBlock, db, startByteAdr, minglingplc, true);
            }
            catch (Exception)
            {

            }
        }
        //写入 PC-PLC命令结束 -false  minglingplc
        public static void Writemingling2PLC()
        {
            try
            {
                plc.WriteBit(DataType.DataBlock, db, startByteAdr, minglingplc, false);
            }
            catch (Exception)
            {

            }
        }

        //写入PC-PLC AGV任务完成-false renwuoverplc
        public static void Writemingling1PLC()
        {
            try
            {
                plc.WriteBit(DataType.DataBlock, db, startByteAdr, renwuoverplc, false);
            }
            catch (Exception)
            {

            }
        }
        //读取PC-PLC AGV任务完成-false renwuoverplc
        public static bool minglingFPLC()
        {
            try
            {
                bool InitialBit = Convert.ToBoolean(plc.Read(DataType.DataBlock, db, startByteAdr, VarType.Bit, bytes, renwuoverplc));
                if (InitialBit == false)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        //写入PC-PLC AGV任务完成-true renwuoverplc
        public static void mingling2FPLC()
        {
            try
            {
                plc.WriteBit(DataType.DataBlock, db, startByteAdr, renwuoverplc, true);
            }
            catch (Exception)
            {

            }
        }
        //读取PLC 接收任务完成  jieshouplc
        public static bool jieshouPLC()
        {
            try
            {
                bool InitialBit = Convert.ToBoolean(plc.Read(DataType.DataBlock, db, startByteAdr, VarType.Bit, bytes, jieshouplc));
                if (InitialBit)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        //写入PC-PLC 可上传信号-true
        public static void WriteXiaFaTaskPLC()
        {
            try
            {
                plc.WriteBit(DataType.DataBlock, db, startByteAdr, xiafatask, true);
            }
            catch (Exception)
            {

            }
        }
        //写入PC-PLC 可上传信号-false
        public static void WriteXiaFaTask1PLC()
        {
            try
            {
                plc.WriteBit(DataType.DataBlock, db, startByteAdr, xiafatask, false);
            }
            catch (Exception)
            {

            }
        }

        //读PLC模板编号值
        public static string TemplatePLC()
        {
            try
            {
                string TemplateNo = Convert.ToString(plc.Read(DataType.DataBlock, dbV, startByteAdr, VarType.Int, countV));
                if (TemplateNo != "")
                {
                    return TemplateNo;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        //读PLC任务类型
        public static int TaskTypePLC()
        {
            try
            {
                int taskType = Convert.ToInt32(plc.Read(DataType.DataBlock, dbV, startByteAdrV1, VarType.Int, countV));
                if (taskType != 0)
                {
                    return taskType;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
        #region //PLC值写入集合l5(json)
        /* 从PLC-DB地址读取值 gen1
         * plccount DB地址最大值
         * plcdb DB块
         * ReadPlcFlg(true) 使PLC命令执行一次
         */
        private static byte dbR = Convert.ToByte(ConfigurationManager.ConnectionStrings["dbR"].ConnectionString);
        private static byte startByteAdrR = Convert.ToByte(ConfigurationManager.ConnectionStrings["startByteAdrR"].ConnectionString);
        private static byte countR = Convert.ToByte(ConfigurationManager.ConnectionStrings["countR"].ConnectionString);
        static bool ReadPlcFlg = true;

        /*从PLC-DB地址读取值 gen2
         */
        private static byte dbR1 = Convert.ToByte(ConfigurationManager.ConnectionStrings["dbR1"].ConnectionString);
        private static byte startByteAdrR1 = Convert.ToByte(ConfigurationManager.ConnectionStrings["startByteAdrR1"].ConnectionString);
        private static byte countR1 = Convert.ToByte(ConfigurationManager.ConnectionStrings["countR1"].ConnectionString);

        /*从PLC-DB地址读取值 继续执行任务
         */
        private static byte dbR2 = Convert.ToByte(ConfigurationManager.ConnectionStrings["dbR2"].ConnectionString);
        private static byte startByteAdrR2 = Convert.ToByte(ConfigurationManager.ConnectionStrings["startByteAdrR2"].ConnectionString);
        private static byte countR2 = Convert.ToByte(ConfigurationManager.ConnectionStrings["countR2"].ConnectionString);

        /*从PLC-DB地址读取值 取消任务
         */
        private static byte dbR3 = Convert.ToByte(ConfigurationManager.ConnectionStrings["dbR3"].ConnectionString);
        private static byte startByteAdrR3 = Convert.ToByte(ConfigurationManager.ConnectionStrings["startByteAdrR3"].ConnectionString);
        private static byte countR3 = Convert.ToByte(ConfigurationManager.ConnectionStrings["countR3"].ConnectionString);
        /*从PLC-DB地址读取值 gen3
         */
        private static byte dbR4 = Convert.ToByte(ConfigurationManager.ConnectionStrings["dbR4"].ConnectionString);
        private static byte startByteAdrR4 = Convert.ToByte(ConfigurationManager.ConnectionStrings["startByteAdrR4"].ConnectionString);
        private static byte countR4 = Convert.ToByte(ConfigurationManager.ConnectionStrings["countR4"].ConnectionString);
        /*从PLC-DB地址读取值 区域封锁(blo)
        */
        private static byte dbR5 = Convert.ToByte(ConfigurationManager.ConnectionStrings["dbR5"].ConnectionString);
        private static byte startByteAdrR5 = Convert.ToByte(ConfigurationManager.ConnectionStrings["startByteAdrR5"].ConnectionString);
        private static byte countR5 = Convert.ToByte(ConfigurationManager.ConnectionStrings["countR5"].ConnectionString);
        private static string AreaCode = ConfigurationManager.ConnectionStrings["AreaCode"].ConnectionString;//区域编码-3
        /*货架与储位的关系绑定、解绑(bin)
        */
        private static byte dbR6 = Convert.ToByte(ConfigurationManager.ConnectionStrings["dbR6"].ConnectionString);
        private static byte startByteAdrR6 = Convert.ToByte(ConfigurationManager.ConnectionStrings["startByteAdrR6"].ConnectionString);
        private static byte countR6 = Convert.ToByte(ConfigurationManager.ConnectionStrings["countR6"].ConnectionString);
        /*查询货架\储位与物料批次绑定关系(query)
        */
        private static byte dbR7 = Convert.ToByte(ConfigurationManager.ConnectionStrings["dbR7"].ConnectionString);
        private static byte startByteAdrR7 = Convert.ToByte(ConfigurationManager.ConnectionStrings["startByteAdrR7"].ConnectionString);
        private static byte countR7 = Convert.ToByte(ConfigurationManager.ConnectionStrings["countR7"].ConnectionString);
        /// <summary>
        /// PLC位(就绪、命令)
        /// </summary>
        public static void ReadPlc()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(100);
                    //PLC就绪
                    bool ready = ReadyPLC();
                    if (ready)
                    {
                        //PLC命令
                        bool Command = CommandPLC();
                        if (Command)
                        {
                            if (ReadPlcFlg)
                            {
                                //模板编号
                                string templNo = TemplatePLC();
                                if (templNo == gen1)
                                {
                                    Value();
                                    Addl5();
                                    WriteminglingPLC();
                                    ReadPlcFlg = false;
                                }
                                else if (templNo == gen2)
                                {
                                    Value1();
                                    Addl5();
                                    WriteminglingPLC();
                                    ReadPlcFlg = false;
                                }
                                else if (templNo == gen3)
                                {
                                    Value4();
                                    Addl5();
                                    WriteminglingPLC();
                                    ReadPlcFlg = false;
                                }
                                else if (templNo == con)
                                {
                                    Value2();
                                    Addl5();
                                    WriteminglingPLC();
                                    ReadPlcFlg = false;
                                }
                                else if (templNo == can)
                                {
                                    Value3();
                                    Addl5();
                                    WriteminglingPLC();
                                    ReadPlcFlg = false;
                                }
                                else if (templNo == blo)
                                {
                                    Value5();
                                    Addl5();
                                    WriteminglingPLC();
                                    ReadPlcFlg = false;
                                }
                                else if (templNo == query)
                                {
                                    Value6();
                                    Addl5();
                                    WriteminglingPLC();
                                    ReadPlcFlg = false;
                                }
                                else if (templNo == status)
                                {
                                    Value7();
                                    Addl5();
                                    WriteminglingPLC();
                                    ReadPlcFlg = false;
                                }
                                else if (templNo == bin)
                                {
                                    Value8();
                                    Addl5();
                                    WriteminglingPLC();
                                    ReadPlcFlg = false;
                                }
                            }
                            //PC-PLC 可上传信号-false
                            WriteXiaFaTask1PLC();
                        }
                        else
                        {
                            //PC-PLC 可上传信号-true
                            WriteXiaFaTaskPLC();
                            try
                            {
                                plc.WriteBit(DataType.DataBlock, db, startByteAdr, minglingplc, false);
                                ReadPlcFlg = true;
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        /// <summary>
        /// PLC值 无缓存位 模板gen1
        /// </summary>
        private static void Value()
        {
            try
            {
                l3.Clear();
                int j = 0;
                for (int i = startByteAdrR; i < countR; i = j * 2)
                {
                    j += 1;
                    string str = Convert.ToString(plc.Read(DataType.DataBlock, dbR, i, VarType.Int, 1));
                    if (j > 1) //PLC数据第一个为模板编号
                    {
                        if (j == 4)
                        {
                            l3.Add("0" + str);
                        }
                        else if (j == 5)
                        {

                        }
                        else if (j == 6)
                        {

                        }
                        else if (j == 8)
                        {
                            l3.Add("0" + str);
                        }
                        else
                        {
                            l3.Add(str);
                        }
                    }
                }
                foreach (string item in l3)
                {
                    GlobalLog.WriteInfoLog("输出读取PLC的值集合l3-PLC-Value：" + item);
                }
            }
            catch (Exception)
            {
            }
        }
        //PLC值 有缓存位 模板gen2
        private static void Value1()
        {
            try
            {
                l3.Clear();
                int j = 0;
                for (int i = startByteAdrR1; i < countR1; i = j * 2)
                {
                    j += 1;
                    string str = Convert.ToString(plc.Read(DataType.DataBlock, dbR1, i, VarType.Int, 1));
                    int taskType = PLC.TaskTypePLC();
                    if (taskType == 3) //启-中-终
                    {
                        if (j > 1) //PLC数据第一个为模板编号
                        {
                            if (j == 4)
                            {
                                l3.Add("0" + str);
                            }
                            else if (j == 6)
                            {
                                l3.Add("0" + str);
                            }
                            else if (j == 8)
                            {
                                l3.Add("0" + str);
                            }
                            else
                            {
                                l3.Add(str);
                            }
                        }
                    }
                    else if (taskType == 1) //中-启-终
                    {
                        if (j > 1) //PLC数据第一个为模板编号
                        {
                            if (j == 4)
                            {
                                l3.Add("0" + str);
                            }
                            else if (j == 6)
                            {
                                l3.Add("0" + str);
                            }
                            else if (j == 8)
                            {
                                l3.Add("0" + str);
                            }
                            else
                            {
                                l3.Add(str);
                            }
                        }
                    }
                }
                if (PLC.TaskTypePLC() == 1)
                {
                    //切换位置(启-中到中-启)
                    string start = l3[1];
                    l3.RemoveAt(1);
                    l3.Insert(1, l3[2]);
                    l3.RemoveAt(3);
                    l3.Insert(3, start);
                }
                foreach (string item in l3)
                {
                    GlobalLog.WriteInfoLog("输出读取PLC的值集合l3-PLC-Value1：" + item);
                }
            }
            catch (Exception)
            {

            }
        }
        // 继续执行任务
        private static void Value2()
        {
            try
            {
                l3.Clear();
                int j = 0;
                for (int i = startByteAdrR2; i < countR2; i = j * 2)
                {
                    j += 1;
                    string str = Convert.ToString(plc.Read(DataType.DataBlock, dbR2, i, VarType.Int, 1));
                    if (j == 2)
                    {
                        l3.Add("0" + str);
                    }
                    else
                    {
                        l3.Add(str);
                    }

                }
                foreach (string item in l3)
                {
                    GlobalLog.WriteInfoLog("输出读取PLC的值集合l3-PLC-Value2：" + item);
                }
            }
            catch (Exception)
            {

            }
        }
        //取消任务
        private static void Value3()
        {
            try
            {
                l3.Clear();
                int j = 0;
                for (int i = startByteAdrR3; i < countR3; i = j * 2)
                {
                    j += 1;
                    string str = Convert.ToString(plc.Read(DataType.DataBlock, dbR3, i, VarType.Int, 1));
                    l3.Add(str);
                }
                foreach (string item in l3)
                {
                    GlobalLog.WriteInfoLog("输出读取PLC的值集合l3-PLC-Value3：" + item);
                }
            }
            catch (Exception)
            {

            }
        }
        //PLC值 有安全位、等待位 模板gen3
        public static void Value4()
        {
            try
            {
                l3.Clear();
                int j = 0;
                for (int i = startByteAdrR4; i < countR4; i = j * 2)
                {
                    j += 1;
                    string str = Convert.ToString(plc.Read(DataType.DataBlock, dbR4, i, VarType.Int, 1));
                    int taskType = PLC.TaskTypePLC();
                    if (taskType == 4) //启-中-终
                    {
                        if (j > 1) //PLC数据第一个为模板编号
                        {
                            if (j == 4)
                            {
                                l3.Add("0" + str);
                            }
                            else if (j == 6)
                            {
                                l3.Add("0" + str);
                            }
                            else if (j == 8)
                            {
                                l3.Add("0" + str);
                            }
                            else if (j == 11)
                            {
                                l3.Add("0" + str);
                            }
                            else
                            {
                                l3.Add(str);
                            }
                        }
                    }
                    else if (taskType == 5) //中-启-终
                    {
                        if (j > 1) //PLC数据第一个为模板编号
                        {
                            if (j == 4)
                            {
                                l3.Add("0" + str);
                            }
                            else if (j == 6)
                            {
                                l3.Add("0" + str);
                            }
                            else if (j == 8)
                            {
                                l3.Add("0" + str);
                            }
                            else if (j == 11)
                            {
                                l3.Add("0" + str);
                            }
                            else
                            {
                                l3.Add(str);
                            }
                        }
                    }
                    else if (taskType == 6)
                    {
                        if (j > 1) //PLC数据第一个为模板编号
                        {
                            if (j == 4)
                            {
                                l3.Add("0" + str);
                            }
                            else if (j == 6)
                            {
                                l3.Add("0" + str);
                            }
                            else if (j == 8)
                            {
                                l3.Add("0" + str);
                            }
                            else if (j == 11)
                            {
                                l3.Add("0" + str);
                            }
                            else
                            {
                                l3.Add(str);
                            }
                        }
                    }
                    else if (taskType == 7)
                    {
                        if (j > 1) //PLC数据第一个为模板编号
                        {
                            if (j == 4)
                            {
                                l3.Add("0" + str);
                            }
                            else if (j == 6)
                            {
                                l3.Add("0" + str);
                            }
                            else if (j == 8)
                            {
                                l3.Add("0" + str);
                            }
                            else if (j == 11)
                            {
                                l3.Add("0" + str);
                            }
                            else
                            {
                                l3.Add(str);
                            }
                        }
                    }
                }
                if (PLC.TaskTypePLC() == 4)
                {
                    string start = l3[1]; //启-中到中-启
                    l3.RemoveAt(1);
                    l3.Insert(1, l3[2]);
                    l3.RemoveAt(3);
                    l3.Insert(3, start);

                    string start1 = l3[5]; //终-安到安-终
                    l3.RemoveAt(5);
                    l3.Insert(5, l3[7]);
                    l3.RemoveAt(8);
                    l3.Insert(8, start1);

                    string start2 = l3[7];
                    l3.RemoveAt(7);
                    l3.Insert(9, start2);
                }
                else if (PLC.TaskTypePLC() == 5)
                {
                    string start3 = l3[7];
                    l3.RemoveAt(7);
                    l3.Insert(9, start3);
                }
                else if (PLC.TaskTypePLC() == 6)
                {
                    string start = l3[1]; //启-中到中-启
                    l3.RemoveAt(1);
                    l3.Insert(1, l3[2]);
                    l3.RemoveAt(3);
                    l3.Insert(3, start);

                    string start1 = l3[5]; //终-安到安-终
                    l3.RemoveAt(5);
                    l3.Insert(5, l3[7]);
                    l3.RemoveAt(8);
                    l3.Insert(8, start1);

                    string start2 = l3[7];
                    l3.RemoveAt(7);
                    l3.Insert(9, start2);
                }
                else if (PLC.TaskTypePLC() == 7)
                {
                    string start4 = l3[7];
                    l3.RemoveAt(7);
                    l3.Insert(9, start4);
                }
                foreach (string item in l3)
                {
                    GlobalLog.WriteInfoLog("输出读取PLC的值集合l3-PLC-Value4：" + item);
                }
            }
            catch (Exception)
            {

            }
        }
        //区域清空/释放
        private static void Value5()
        {
            try
            {
                l3.Clear();
                int j = 0;
                for (int i = startByteAdrR5; i < countR5; i = j * 2)
                {
                    j += 1;
                    string str = Convert.ToString(plc.Read(DataType.DataBlock, dbR5, i, VarType.Int, 1));
                    l3.Add(str);
                }
                l3.Insert(4, AreaCode);
                foreach (string item in l3)
                {
                    GlobalLog.WriteInfoLog("输出读取PLC的值集合l3-PLC-Value5：" + item);
                }
            }
            catch (Exception)
            {

            }
        }
        //查询货架\储位与物料批次绑定关系
        private static void Value6()
        {
            try
            {
                l3.Clear();
                int j = 0;
                for (int i = startByteAdrR6; i < countR6; i = j * 2)
                {
                    j += 1;
                    string str = Convert.ToString(plc.Read(DataType.DataBlock, dbR6, i, VarType.Int, 1));
                    l3.Add(str);
                }
                foreach (string item in l3)
                {
                    GlobalLog.WriteInfoLog("输出读取PLC的值集合l3-PLC-Value6：" + item);
                }
            }
            catch (Exception)
            {

            }
        }
        //查询 AGV 状态
        public static void Value7()
        {
            try
            {
                l3.Clear();
                l3.Add("GDTD");
                foreach (string item in l3)
                {
                    GlobalLog.WriteInfoLog("输出读取PLC的值集合l3-PLC-Value7：" + item);
                }
            }
            catch (Exception)
            {

            }
        }
        //货架与库位解绑、绑定
        private static void Value8()
        {
            l3.Clear();
            int j = 0;
            for (int i = startByteAdrR7; i < countR7; i = j * 2)
            {
                j += 1;
                string str = Convert.ToString(plc.Read(DataType.DataBlock, dbR7, i, VarType.Int, 1));
                l3.Add(str);
            }
            foreach (string item in l3)
            {
                GlobalLog.WriteInfoLog("输出读取PLC的值集合l3-PLC-Value8：" + item);
            }
        }
        /// <summary>
        /// json写入集合
        /// </summary>
        public static void Addl5()
        {
            try
            {
                string TemplateNo = PLC.TemplatePLC();
                if (TemplateNo != "")
                {
                    if (TemplateNo == gen1)
                    {
                        string parameter = "A";
                        //读取xml结构、配置的PLC地址(DB)
                        bool xmlFlag = Xml.ReadGenAgvSchedulingTask(xmlDBgenAgvSchedulingTask1, l1, l2);
                        if (xmlFlag)
                        {
                            //l3的值写入l4
                            bool wtiteFlag = WritePlcValue.WriteValue(l1, l2, l3, l4);
                            if (wtiteFlag)
                            {
                                string writeValue = Xml.ReadgenAgvSchedulingTaskXML(xmlDBgenAgvSchedulingTask1, l4);
                                bool jsonFlag = Xml2Json.Conver(writeValue, parameter, l5);
                                if (jsonFlag)
                                {
                                    GlobalLog.WriteInfoLog("PLC.Addl5-genAgvSchedulingTask：写入JSON成功");
                                }
                            }
                        }
                    }
                    else if (TemplateNo == gen2)
                    {
                        string parameter = "A";
                        //读取xml结构、配置的PLC地址(DB)
                        bool xmlFlag = Xml.ReadGen1AgvSchedulingTask(xmlDBgenAgvSchedulingTask2, l1, l2);
                        if (xmlFlag)
                        {
                            //l3的值写入l4
                            bool wtiteFlag = WritePlcValue.WriteValue(l1, l2, l3, l4);

                            if (wtiteFlag)
                            {
                                string writeValue = Xml.Readgen1AgvSchedulingTaskXML(xmlDBgenAgvSchedulingTask2, l4);
                                bool jsonFlag = Xml2Json.Conver(writeValue, parameter, l5);
                                if (jsonFlag)
                                {
                                    GlobalLog.WriteInfoLog("PLC.Addl5-gen1AgvSchedulingTask：写入JSON成功");
                                }
                            }
                        }
                    }
                    else if (TemplateNo == gen3)
                    {
                        string parameter = "A";
                        //读取xml结构、配置的PLC地址(DB)
                        bool xmlFlag = Xml.ReadGen2AgvSchedulingTask(xmlDBgenAgvSchedulingTask3, l1, l2);
                        if (xmlFlag)
                        {
                            //l3的值写入l4
                            bool wtiteFlag = WritePlcValue.WriteValue(l1, l2, l3, l4);

                            if (wtiteFlag)
                            {
                                string writeValue = Xml.Readgen2AgvSchedulingTaskXML(xmlDBgenAgvSchedulingTask3, l4);
                                bool jsonFlag = Xml2Json.Conver(writeValue, parameter, l5);
                                if (jsonFlag)
                                {
                                    GlobalLog.WriteInfoLog("PLC.Addl5-gen2AgvSchedulingTask：写入JSON成功");
                                }
                            }
                        }
                    }
                    else if (TemplateNo == con)
                    {
                        string parameter = "B";
                        bool xmlFlag = Xml.ReadContinueTask(xmlDBcontinueTask, l1, l2);
                        if (xmlFlag)
                        {
                            bool wtiteFlag = WritePlcValue.WriteValue(l1, l2, l3, l4);
                            if (wtiteFlag)
                            {
                                string writeValue = Xml.ReadContinueTaskXML(xmlDBcontinueTask, l4);
                                bool jsonFlag = Xml2Json.Conver(writeValue, parameter, l5);
                                if (jsonFlag)
                                {
                                    GlobalLog.WriteInfoLog("PLC.Addl5-ContinueTask：写入JSON成功");
                                }
                            }
                        }
                    }
                    else if (TemplateNo == can)
                    {
                        string parameter = "C";
                        bool xmlFlag = Xml.ReadCancelTask(xmlDBcancelTask, l1, l2);
                        if (xmlFlag)
                        {
                            bool wtiteFlag = WritePlcValue.WriteValue(l1, l2, l3, l4);
                            if (wtiteFlag)
                            {
                                string writeValue = Xml.ReadCancelTaskXML(xmlDBcancelTask, l4);
                                bool jsonFlag = Xml2Json.Conver(writeValue, parameter, l5);
                                if (jsonFlag)
                                {
                                    GlobalLog.WriteInfoLog("PLC.Addl5-CancelTask：写入JSON成功");
                                }
                            }
                        }
                    }
                    else if (TemplateNo == blo)
                    {
                        string parameter = "D";
                        bool xmlFlag = Xml.ReadBlockArea(xmlDBblockArea, l1, l2);
                        if (xmlFlag)
                        {
                            bool wtiteFlag = WritePlcValue.WriteValue(l1, l2, l3, l4);
                            if (wtiteFlag)
                            {
                                string writeValue = Xml.ReadBlockAreaXML(xmlDBblockArea, l4);
                                bool jsonFlag = Xml2Json.Conver(writeValue, parameter, l5);
                                if (jsonFlag)
                                {
                                    GlobalLog.WriteInfoLog("PLC.Addl5-blockArea：写入JSON成功");
                                }
                            }
                        }
                    }
                    else if (TemplateNo == query)
                    {
                        string parameter = "E";
                        bool xmlFlag = Xml.ReadQueryPodBerthAndMat(xmlDBqueryPodBerthAndMat, l1, l2);
                        if (xmlFlag)
                        {
                            bool wtiteFlag = WritePlcValue.WriteValue(l1, l2, l3, l4);
                            if (wtiteFlag)
                            {
                                string writeValue = Xml.ReadQueryPodBerthAndMatXML(xmlDBqueryPodBerthAndMat, l4);
                                bool jsonFlag = Xml2Json.Conver(writeValue, parameter, l5);
                                if (jsonFlag)
                                {
                                    GlobalLog.WriteInfoLog("PLC.Addl5-queryPodBerthAndMat：写入JSON成功");
                                }
                            }
                        }
                    }
                    else if (TemplateNo == status)
                    {
                        string parameter = "F";
                        bool xmlFlag = Xml.ReadQueryPodBerthAndMat(xmlDBqueryAgvStatus, l1, l2);
                        if (xmlFlag)
                        {
                            bool wtiteFlag = WritePlcValue.WriteValue(l1, l2, l3, l4);
                            if (wtiteFlag)
                            {
                                string writeValue = Xml.ReadQueryPodBerthAndMatXML(xmlDBqueryAgvStatus, l4);
                                bool jsonFlag = Xml2Json.Conver(writeValue, parameter, l5);
                                if (jsonFlag)
                                {
                                    GlobalLog.WriteInfoLog("PLC.Addl5-queryAgvStatus：写入JSON成功");
                                }
                            }
                        }
                    }
                    else if (TemplateNo == bin)
                    {
                        string parameter = "G";
                        bool xmlFlag = Xml.ReadQueryPodBerthAndMat(xmlDBbindPodAndBerth, l1, l2);
                        if (xmlFlag)
                        {
                            bool wtiteFlag = WritePlcValue.WriteValue(l1, l2, l3, l4);
                            if (wtiteFlag)
                            {
                                string writeValue = Xml.ReadQueryPodBerthAndMatXML(xmlDBbindPodAndBerth, l4);
                                bool jsonFlag = Xml2Json.Conver(writeValue, parameter, l5);
                                if (jsonFlag)
                                {
                                    GlobalLog.WriteInfoLog("PLC.Addl5-bindPodAndBerth：写入JSON成功");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion
        #region //写入PLC
        //写入PLC
        public static short dbW = Convert.ToInt16(ConfigurationManager.ConnectionStrings["dbW"].ConnectionString);
        public static short startByteAdrW = Convert.ToInt16(ConfigurationManager.ConnectionStrings["startByteAdrW"].ConnectionString);
        public static short maxW = Convert.ToInt16(ConfigurationManager.ConnectionStrings["maxW"].ConnectionString);
        //库位和货架关系
        public static short dbR8 = Convert.ToInt16(ConfigurationManager.ConnectionStrings["dbR8"].ConnectionString);
        public static short startByteAdrR8 = Convert.ToInt16(ConfigurationManager.ConnectionStrings["startByteAdrR8"].ConnectionString);
        public static short countR8 = Convert.ToInt16(ConfigurationManager.ConnectionStrings["countR8"].ConnectionString);
        public static bool WritePlcFlg = true;
        public static void Write()
        {
            try
            {
                for (int i = 0; i < Form1.lR.Count; i++)
                {
                    if (WritePlcFlg)
                    {
                        if (Form1.lR[i].Substring(0, 1) == "E")
                        {
                            lU.Clear();
                            lU = tools.JsonOperate.Json_Decode(Form1.lR[i]);
                            int k = 0;
                            for (int j = startByteAdrR8; j < countR8; j = k * 2)
                            {
                                //写入PLC(int类型)-RCS发起时为(string格式)
                                plc.Write(DataType.DataBlock, dbR8, j, Convert.ToInt16(lU[k]));
                                k += 1;
                            }
                            GlobalLog.WriteInfoLog("写入PLC中的状态--" + lU[1] + "：" + lU[2]);
                            Form1.lR.Remove(Form1.lR[i]);
                            WritePlcFlg = false;
                        }
                        else
                        {
                            lU.Clear();
                            lU = tools.JsonOperate.Json_Decode(Form1.lR[i]);
                            int k = 0;
                            for (int j = startByteAdrW; j < maxW; j = k * 2)
                            {
                                //写入PLC(int类型)
                                plc.Write(DataType.DataBlock, dbW, j, Convert.ToInt16(lU[k]));
                                k += 1;
                            }
                            GlobalLog.WriteInfoLog("写入PLC中的状态--" + lU[1] + "：" + lU[2]);
                            Form1.lR.Remove(Form1.lR[i]);
                            WritePlcFlg = false;
                        } 
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        public static short dbW1 = Convert.ToInt16(ConfigurationManager.ConnectionStrings["dbW1"].ConnectionString);
        public static short startByteAdrW1 = Convert.ToInt16(ConfigurationManager.ConnectionStrings["startByteAdrW1"].ConnectionString);
        public static short maxW1 = Convert.ToInt16(ConfigurationManager.ConnectionStrings["maxW1"].ConnectionString);
        public static bool WriteStatus(int status)
        {
            try
            {
                int k = 0;
                for (int j = startByteAdrW1; j < maxW1; j = k * 2)
                {
                    //写入PLC(int类型)
                    plc.Write(DataType.DataBlock, dbW1, j, Convert.ToInt16( status ));
                    k += 1;
                }
                GlobalLog.WriteInfoLog("写入PLC中的AGV状态--" + status);
                return true;
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("PLC.WriteStatus：" + ex.Message);
                return false;
            }
        }
        #endregion
        #region //间隔时间，读取PLC数据写入数据库
        static List<string> ls = new List<string>();
        //位地址
        private static byte dbRDB1 = Convert.ToByte(ConfigurationManager.ConnectionStrings["dbRDB1"].ConnectionString);
        private static byte startByteAdrRDB1 = Convert.ToByte(ConfigurationManager.ConnectionStrings["startByteAdrRDB1"].ConnectionString);
        private static byte bytesDB1 = Convert.ToByte(ConfigurationManager.ConnectionStrings["bytesDB1"].ConnectionString);
        private static byte writedbPLC = Convert.ToByte(ConfigurationManager.ConnectionStrings["writedbPLC"].ConnectionString);
        private static byte zhuangxiangjiuxuPLC = Convert.ToByte(ConfigurationManager.ConnectionStrings["zhuangxiangjiuxuPLC"].ConnectionString);
        private static byte uploaddbPC = Convert.ToByte(ConfigurationManager.ConnectionStrings["uploaddbPC"].ConnectionString);
        private static byte writedbPLC102 = Convert.ToByte(ConfigurationManager.ConnectionStrings["writedbPLC102"].ConnectionString);
        private static byte uploaddbPC102 = Convert.ToByte(ConfigurationManager.ConnectionStrings["uploaddbPC102"].ConnectionString);
        //数据地址
        //1
        private static byte dbRDB = Convert.ToByte(ConfigurationManager.ConnectionStrings["dbRDB"].ConnectionString);
        private static byte startByteAdrRDB = Convert.ToByte(ConfigurationManager.ConnectionStrings["startByteAdrRDB"].ConnectionString);
        private static byte countRDB = Convert.ToByte(ConfigurationManager.ConnectionStrings["countRDB"].ConnectionString);
        private static byte numberDB = Convert.ToByte(ConfigurationManager.ConnectionStrings["numberDB"].ConnectionString);
        //2
        private static byte dbRDB102 = Convert.ToByte(ConfigurationManager.ConnectionStrings["dbRDB102"].ConnectionString);
        private static byte startByteAdrRDB102 = Convert.ToByte(ConfigurationManager.ConnectionStrings["startByteAdrRDB102"].ConnectionString);
        private static byte countRDB102 = Convert.ToByte(ConfigurationManager.ConnectionStrings["countRDB102"].ConnectionString);
        private static byte numberDB102 = Convert.ToByte(ConfigurationManager.ConnectionStrings["numberDB102"].ConnectionString);
        public static bool uploadDBFlg = true;
        public static bool uploadDBFlg102 = true;
        public static Plc plc2 = null;
        private static string PLCip2 = ConfigurationManager.ConnectionStrings["PLCip2"].ConnectionString;
        private static short rack2 = Convert.ToInt16(ConfigurationManager.ConnectionStrings["rack2"].ConnectionString);
        private static short slot2 = Convert.ToInt16(ConfigurationManager.ConnectionStrings["slot2"].ConnectionString);
        public static bool ConnectePLC2()
        {
            try
            {
                plc2 = new Plc(CpuType.S71500, PLCip2, rack2, slot2);
                plc2.Open();
                if (plc2.IsConnected)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void zhuangxiangjiuxu1PLC()
        {
            try
            {
                plc2.WriteBit(DataType.DataBlock, dbRDB1, startByteAdrRDB1, zhuangxiangjiuxuPLC, false);
            }
            catch (Exception)
            {

            }
        }
        public static void WriteDataBase()
        {
            while (true)
            {
                try
                {
                    WriteDB101();
                    WriteDB102();
                }
                catch (Exception)
                {

                }
            }
        }
        //DB-101写入数据库
        public static void WriteDB101()
        {
            Thread.Sleep(100);
            //PC-PLC装箱就绪
            plc2.WriteBit(DataType.DataBlock, dbRDB1, startByteAdrRDB1, zhuangxiangjiuxuPLC, true);
            bool InitialBit = Convert.ToBoolean(plc2.Read(DataType.DataBlock, dbRDB1, startByteAdrRDB1, VarType.Bit, bytesDB1, writedbPLC));
            if (InitialBit)
            {
                if (uploadDBFlg)
                {
                    ls.Clear();
                    int j = 0;
                    for (int i = startByteAdrRDB; i < countRDB * numberDB; i = j * 256)
                    {
                        j += 1;
                        string str = Convert.ToString(plc2.Read(DataType.DataBlock, dbRDB, i, VarType.S7String, 32));
                        ls.Add(str);
                    }
                    tools.DbHelp.sql_Insert(ls);
                    plc2.WriteBit(DataType.DataBlock, dbRDB1, startByteAdrRDB1, uploaddbPC, true);
                    uploadDBFlg = false;
                }
            }
            else
            {
                uploadDBFlg = true;
                plc2.WriteBit(DataType.DataBlock, dbRDB1, startByteAdrRDB1, uploaddbPC, false);
            }
        }
        //DB-102写入数据库
        public static void WriteDB102()
        {
            Thread.Sleep(100);
            //PC-PLC装箱就绪
            plc2.WriteBit(DataType.DataBlock, dbRDB1, startByteAdrRDB1, zhuangxiangjiuxuPLC, true);
            bool InitialBit = Convert.ToBoolean(plc2.Read(DataType.DataBlock, dbRDB1, startByteAdrRDB102, VarType.Bit, bytesDB1, writedbPLC102));
            if (InitialBit)
            {
                if (uploadDBFlg102)
                {
                    ls.Clear();
                    int j = 0;
                    for (int i = startByteAdrRDB102; i < countRDB102 * numberDB102; i = j * 256)
                    {
                        j += 1;
                        string str = Convert.ToString(plc2.Read(DataType.DataBlock, dbRDB102, i, VarType.S7String, 32));
                        ls.Add(str);
                    }
                    tools.DbHelp.sql_Insert(ls);
                    plc2.WriteBit(DataType.DataBlock, dbRDB1, startByteAdrRDB102, uploaddbPC102, true);
                    uploadDBFlg102 = false;
                }
            }
            else
            {
                uploadDBFlg102 = true;
                plc2.WriteBit(DataType.DataBlock, dbRDB1, startByteAdrRDB102, uploaddbPC102, false);
            }
        }
        #endregion

        #region //通道2
        #region //PC-PLC状态信号,PLC-DB中位
        //PLC-DB块
        private static byte db2 = Convert.ToByte(ConfigurationManager.ConnectionStrings["db2"].ConnectionString);
        //起始地址
        private static byte startByteAdr2 = Convert.ToByte(ConfigurationManager.ConnectionStrings["startByteAdr2"].ConnectionString);
        //字节
        private static byte bytes2 = Convert.ToByte(ConfigurationManager.ConnectionStrings["bytes2"].ConnectionString);
        //PLC-PC就绪
        private static byte readybit2 = Convert.ToByte(ConfigurationManager.ConnectionStrings["readybit2"].ConnectionString);
        //PLC-PC命令发起
        private static byte commandbit2 = Convert.ToByte(ConfigurationManager.ConnectionStrings["commandbit2"].ConnectionString);
        //PC-PLC就绪
        private static byte wjuxuplc2 = Convert.ToByte(ConfigurationManager.ConnectionStrings["wjuxuplc2"].ConnectionString);
        //PC-PLC命令结束
        private static byte minglingplc2 = Convert.ToByte(ConfigurationManager.ConnectionStrings["minglingplc2"].ConnectionString);
        //PC-PLC任务完成
        private static byte renwuoverplc2 = Convert.ToByte(ConfigurationManager.ConnectionStrings["renwuoverplc2"].ConnectionString);
        //PLC-PC任务接收完成
        private static byte jieshouplc2 = Convert.ToByte(ConfigurationManager.ConnectionStrings["jieshouplc2"].ConnectionString);
        //PC-PLC任务可上传信号
        private static byte xiafatask2 = Convert.ToByte(ConfigurationManager.ConnectionStrings["xiafatask2"].ConnectionString);

        //读取PLC-PC就绪位
        public static bool ReadReadyPLCBit2()
        {
            try
            {
                bool ReadReadyPLCBit2 = Convert.ToBoolean(plc.Read(DataType.DataBlock, db2, startByteAdr2, VarType.Bit, bytes2, readybit2));
                if (ReadReadyPLCBit2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("通道2-PLC.ReadReadyPLCBit2：" + ex.Message);
                return false;
            }
        }
        //读取PLC-PC命令发起位
        public static bool ReadCommandPLCBit2()
        {
            try
            {
                bool ReadCommandPLCBit2 = Convert.ToBoolean(plc.Read(DataType.DataBlock, db2, startByteAdr2, VarType.Bit, bytes2, commandbit2));
                if (ReadCommandPLCBit2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("通道2-PLC.ReadCommandPLCBit2：" + ex.Message);
                return false;
            }
        }
        //写入PC-PLC就绪位-true
        public static void WriteJuXuToPLCBit2()
        {
            try
            {
                plc.WriteBit(DataType.DataBlock, db2, startByteAdr2, wjuxuplc2, true);
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("通道2-PLC.WriteJuXuToPLCBit2：" + ex.Message);
            }
        }
        //写入PC-PLC就绪位-flase
        public static void WriteJuXu1ToPLCBit2()
        {
            try
            {
                plc.WriteBit(DataType.DataBlock, db2, startByteAdr2, wjuxuplc2, false);
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("通道2-PLC.WriteJuXu1ToPLCBit2：" + ex.Message);
            }
        }
        //写入PC-PLC命令结束-true
        public static void WriteMingLingToPLCBit2()
        {
            try
            {
                plc.WriteBit(DataType.DataBlock, db2, startByteAdr2, minglingplc2, true);
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("通道2-PLC.WriteMingLingToPLCBit2：" + ex.Message);
            }
        }
        //写入PC-PLC命令结束-false
        public static void WriteMingLing1ToPLCBit2()
        {
            try
            {
                plc.WriteBit(DataType.DataBlock, db2, startByteAdr2, minglingplc2, false);
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("通道2-PLC.WriteMingLing1ToPLCBit2：" + ex.Message);
            }
        }
        //写入PC-PLC 任务完成-true
        public static void WritemRenWuOverToPLCBit2()
        {
            try
            {
                plc.WriteBit(DataType.DataBlock, db2, startByteAdr2, renwuoverplc2, true);
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("通道-PLC.WritemRenWuOverToPLCBit2：" + ex.Message);
            }
        }
        //写入PC-PLC 任务完成-false
        public static void WritemRenWuOver1ToPLCBit2()
        {
            try
            {
                plc.WriteBit(DataType.DataBlock, db2, startByteAdr2, renwuoverplc2, false);
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("通道-PLC.WritemRenWuOver1ToPLCBit2：" + ex.Message);
            }
        }
        //读取PC-PLC 任务完成
        public static bool ReadRenWuOverToPLCBit2()
        {
            try
            {
                bool ReadRenWuOverToPLCBit2 = Convert.ToBoolean(plc.Read(DataType.DataBlock, db2, startByteAdr2, VarType.Bit, bytes2, renwuoverplc2));
                if (ReadRenWuOverToPLCBit2 == false)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("通道2-PLC.ReadRenWuOverToPLCBit2：" + ex.Message);
                return false;
            }
        }
        //读取PLC-PC 任务接收完成
        public static bool ReadJieShouTaskToPLCBit2()
        {
            try
            {
                bool ReadJieShouTaskToPLCBit2 = Convert.ToBoolean(plc.Read(DataType.DataBlock, db2, startByteAdr2, VarType.Bit, bytes2, jieshouplc2));
                if (ReadJieShouTaskToPLCBit2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("通道-PLC.ReadJieShouTaskToPLCBit2：" + ex.Message);
                return false;
            }
        }
        //写入PC-PLC 任务可上传信号-true
        public static void WriteUploadTaskToPLCBit2()
        {
            try
            {
                plc.WriteBit(DataType.DataBlock, db2, startByteAdr2, xiafatask2, true);
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("通道2-PLC.WriteUploadTaskToPLCBit2：" + ex.Message);
            }
        }
        //写入PC-PLC 任务可上传信号-false
        public static void WriteUploadTask1ToPLCBit2()
        {
            try
            {
                plc.WriteBit(DataType.DataBlock, db2, startByteAdr2, xiafatask2, false);
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("通道2-PLC.WriteUploadTask1ToPLCBit2：" + ex.Message);
            }
        }
        static bool Read2PlcFlg = true;
        public static void Read2Plc()
        {
            try
            {
                while (true)
                {
                    //PLC就绪
                    bool ready = ReadReadyPLCBit2();
                    if (ready)
                    {
                        try
                        {
                            //PLC命令
                            bool Command = ReadCommandPLCBit2();
                            if (Command)
                            {
                                if (Read2PlcFlg)
                                {
                                    //模板编号
                                    string templNo = TemplatePLC();
                                    if (templNo == gen1)
                                    {
                                        Value();
                                        Addl5();
                                        WriteMingLingToPLCBit2();
                                        Read2PlcFlg = false;
                                    }
                                    else if (templNo == gen2)
                                    {
                                        Value1();
                                        Addl5();
                                        WriteMingLingToPLCBit2();
                                        Read2PlcFlg = false;
                                    }
                                    else if (templNo == gen3)
                                    {
                                        Value4();
                                        Addl5();
                                        WriteMingLingToPLCBit2();
                                        Read2PlcFlg = false;
                                    }
                                    else if (templNo == con)
                                    {
                                        Value2();
                                        Addl5();
                                        WriteMingLingToPLCBit2();
                                        Read2PlcFlg = false;
                                    }
                                    else if (templNo == can)
                                    {
                                        Value3();
                                        Addl5();
                                        WriteMingLingToPLCBit2();
                                        Read2PlcFlg = false;
                                    }
                                    else if (templNo == blo)
                                    {
                                        Value5();
                                        Addl5();
                                        WriteMingLingToPLCBit2();
                                        Read2PlcFlg = false;
                                    }
                                }
                                //PC-PLC 可上传信号-false
                                WriteUploadTask1ToPLCBit2();
                            }
                            else
                            {
                                //PC-PLC 可上传信号-true
                                WriteUploadTaskToPLCBit2();
                                plc.WriteBit(DataType.DataBlock, db2, startByteAdr2, minglingplc2, false);
                                Read2PlcFlg = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobalLog.WriteInfoLog("PLC-Read2Plc：" + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("PLC.Read2Plc：" + ex.Message);
            }
        }
        #endregion
        #endregion

    }
}