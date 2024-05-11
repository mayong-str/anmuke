using HZHHUIControlLibrary;
using Newtonsoft.Json;
using S7.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Task_Dispatch
{
    public partial class Form1 : Form
    {
        private SquareMenu squareMenu;
        bool PlcConn = false;
        //进程
        static Process myProcess = new System.Diagnostics.Process();
        static StreamReader reader = null;

        //集合
        static List<String> lO = new List<String>(); //生成任务单
        static List<String> lW = new List<String>(); //继续执行任务
        static List<String> lT = new List<String>(); //取消任务
        static List<String> lY = new List<String>(); //区域清空/释放
        static List<String> lZ = new List<String>(); //区域清空/释放
        public static List<String> lR = new List<String>(); //RCS返回值
        //是否在线
        public static bool PLConline = false;
        public static bool RCSonline = false;
        public Form1()
        {
            InitializeComponent();
            SquareMenuOne();
            KillProcess();
            richTextBox_Error_Info.Text = "";
            OpenThread.PLCThread();//ping PLCip
            PlcConn = PLC.ConnectePLC();
            PLC.ConnectePLC2();
            //写入PLC-状态位
            PLC.WriteJuXuPLC();
            PLC.Writemingling2PLC();
            PLC.Writemingling1PLC();
            //通道2
            //PLC.WriteJuXuToPLCBit2();
            //PLC.WriteMingLing1ToPLCBit2();
            //PLC.WritemRenWuOver1ToPLCBit2();

            OpenThread.ReadPlc(); //读PLC值
            OpenThread.Traversel5();//POST
            OpenThread.WritePlc(); //写PLC值
            OpenThread.WriteDataBase();
            OpenThread.WritePost();
            reader = Opens.OpenSpringboot(myProcess, springboot);
            OpenThread.Springboot();
            GlobalLog.WriteInfoLog("输出springboot地址：" + springboot);
        }
        //取Springboot服务返回值
        public static void ReadSpringboot()
        {
            while (true)
            {
                try
                {
                    if (reader == null)
                    {

                    }
                    else
                    {
                        //从流中读取一行字符(RCS返回值)
                        string str = reader.ReadLine();
                        if (str != null)
                        {
                            if (str.StartsWith("$"))
                            {
                                //Console.WriteLine("输出结果：" + str);
                                GlobalLog.WriteInfoLog("RCS执行调度任务时返回json值"+str);
                                /*lR集合存放RCS返回的json格式解析后参数
                                 * 参数：taskCode、method
                                 * 
                                 */
                                //lR = tools.JsonOperate.Json_Decode(str);
                                lR.Add(str);
                            }
                            else if (str.StartsWith("#"))
                            {
                                //Console.WriteLine("输出结果：" + str);
                                GlobalLog.WriteInfoLog("RCS执行区域封锁时返回json值" + str);
                                lR.Add(str);
                            }
                            else
                            {
                                GlobalLog.WriteInfoLog("RCS执行其他任务时返回json值" + str);
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        //发送查询AGV状态-post
        public static void WritePost()
        {
            while (true)
            {
                PostAgvStatus();
                Thread.Sleep(5000);
            }
        }
        public static void PostAgvStatus()
        {
            try
            {
                string json = "{\r\n" +
                "   \"reqCode\": \"468513\",\r\n" +
                "	\"reqTime\": \"\",\r\n" +
                "	\"clientCode\": \"\",\r\n" +
                "	\"tokenCode\": \"\",\r\n" +
                "	\"mapShortName\": \"AA\"\r\n" +
                "}\r\n" +
                "";
                string data = PostStatus(json);
                if (data != "")
                {
                    int code = tools.JsonOperate.Json_Decode1(data);
                    if (code == 0)
                    {
                        int status = tools.JsonOperate.Json_Decode_Status(data);
                        GlobalLog.WriteInfoLog("--AGV状态："+ status);
                        bool ready = PLC.ReadyPLC();
                        if (ready)
                        {
                            bool flg = PLC.WriteStatus(status);
                            if (flg)
                            {
                                GlobalLog.WriteInfoLog("--写入AGV状态成功!");
                            }
                            else
                            {
                                GlobalLog.WriteInfoLog("--写入AGV状态失败!");
                            }
                        }
                    }
                    else
                    {
                        richTextBox_Error_Info.Text += data + "\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Form1.PostAgvStatus--" + ex.Message);
            }
        }

        private void SquareMenuOne()
        {
            Dictionary<int, Control> items = new Dictionary<int, Control>();
            panel_PLC.Parent = this;
            panel_PLC.Dock = DockStyle.Fill;
            panel_PLC.Text = "PLC";
            items.Add(1, panel_PLC);
            panel_RCS.Parent = this;
            panel_RCS.Dock = DockStyle.Fill;
            panel_RCS.Text = "RCS";
            items.Add(3, panel_RCS);
            squareMenu = new SquareMenu(ref items, 200, 25);
            squareMenu.Name = "squareMenutest";
            squareMenu.Location = new Point(0, 0);
            squareMenu.Dock = DockStyle.Left;
            squareMenu.SelectMenuColor = Color.White;
            squareMenu.MenuColor = Color.FromArgb(148, 185, 219);
            squareMenu.BackColor = Color.FromArgb(77, 139, 185);
            squareMenu.SelectFontColor = Color.FromArgb(46, 117, 182);
            squareMenu.FontColor = Color.White;
            squareMenu.ConerRadius = 20;
            squareMenu.Padding = new Padding(5, 2, 2, 2);
            squareMenu.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134))); ;
            squareMenu.Width = 200;
            //squareMenu.IsHorizontal = true;
            this.Controls.Add(squareMenu);
            squareMenu.SelectItemIndex = 1;
            //items.Remove(2);
            squareMenu.ReSetShow();
        }
        public static List<String> lN = new List<String>(); //显示读取PLC值

        public static bool FormlN = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            richTextBox_PLC_Request_Parameter.Text = "";
            for (int i = 0; i < PLC.l3.Count; i++)
            {
                richTextBox_PLC_Request_Parameter.Text += PLC.l3[i] + "\r\n";
            }
        }
        #region //2022-06-06 测试
        /*
         * 2022-06-06 测试
         */
        //xmlDB文件夹路径
        static string xmlDBgen1AgvSchedulingTask = Application.StartupPath + ConfigurationManager.ConnectionStrings["xmlDBgen1AgvSchedulingTask"].ConnectionString;
        static string xmlDBcontinueTask = Application.StartupPath + ConfigurationManager.ConnectionStrings["xmlDBcontinueTask"].ConnectionString;
        static string xmlDBcancelTask = Application.StartupPath + ConfigurationManager.ConnectionStrings["xmlDBcancelTask"].ConnectionString;
        static string xmlDBagvCallback = Application.StartupPath + ConfigurationManager.ConnectionStrings["xmlDBagvCallback"].ConnectionString;
        static string xmlDBblockArea = Application.StartupPath + ConfigurationManager.ConnectionStrings["xmlDBblockArea"].ConnectionString;

        //RCS-IP
        //private static string RCSip = ConfigurationManager.ConnectionStrings["RCSip"].ConnectionString;
        //Springboot.jar地址
        static readonly string springboot = Application.StartupPath + ConfigurationManager.ConnectionStrings["springboot"].ConnectionString;

        //生成任务单
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //XmlDocument doc = new XmlDocument();
                //doc.Load(xmlDBgenAgvSchedulingTask);
                //string xmlStr = doc.OuterXml;

                //string json = Xml2Json.XmlToJson(xmlStr);
                //lO.Add(json);
                //Console.WriteLine(json);

                //richTextBox_RCS_Response_Parameter.Text = json;

                ////Post(json, "genAgvSchedulingTask");
                ///
                //测试使用(2022-07-01)
                string gen11 = Application.StartupPath + ConfigurationManager.ConnectionStrings["gen11"].ConnectionString;
                string gen12 = Application.StartupPath + ConfigurationManager.ConnectionStrings["gen12"].ConnectionString;
                string gen13 = Application.StartupPath + ConfigurationManager.ConnectionStrings["gen13"].ConnectionString;
                if (comboBox1.Text == "1")
                {
                    richTextBox_RCS_Response_Parameter.Text = "";
                    bool xmlFlag = Xml.ReadGenAgvSchedulingTask(gen11, PLC.l1, PLC.l2);
                    PLC.l3.Clear();
                    if (xmlFlag)
                    {
                        PLC.l3.Add(textBox2.Text);
                        PLC.l3.Add(textBox3.Text);
                        PLC.l3.Add(textBox4.Text);
                        PLC.l3.Add(textBox5.Text);
                        PLC.l3.Add(textBox6.Text);
                        PLC.l3.Add(textBox101.Text);
                        //l3的值写入l4
                        bool wtiteFlag = WritePlcValue.WriteValue(PLC.l1, PLC.l2, PLC.l3, PLC.l4);
                        if (wtiteFlag)
                        {
                            string writeValue = Xml.ReadgenAgvSchedulingTaskXML(gen11, PLC.l4);
                            string json = Xml2Json.xmlToJson(writeValue);
                            richTextBox_RCS_Response_Parameter.Text = json;
                            if (writeValue != "")
                            {
                                bool jsonFlag = Xml2Json.Conver(writeValue, "A", lO);
                            }
                        }
                    }
                }
                if (comboBox1.Text == "2")
                {
                    richTextBox_RCS_Response_Parameter.Text = "";
                    bool xmlFlag = Xml.ReadGen1AgvSchedulingTask(gen12, PLC.l1, PLC.l2);
                    PLC.l3.Clear();
                    if (xmlFlag)
                    {
                        PLC.l3.Add(textBox2.Text);
                        PLC.l3.Add(textBox3.Text);
                        PLC.l3.Add(textBox4.Text);
                        PLC.l3.Add(textBox5.Text);
                        PLC.l3.Add(textBox6.Text);
                        PLC.l3.Add(textBox7.Text);
                        PLC.l3.Add(textBox8.Text);
                        PLC.l3.Add(textBox101.Text);
                        //l3的值写入l4
                        bool wtiteFlag = WritePlcValue.WriteValue(PLC.l1, PLC.l2, PLC.l3, PLC.l4);
                        if (wtiteFlag)
                        {
                            string writeValue = Xml.Readgen1AgvSchedulingTaskXML(gen12, PLC.l4);
                            string json = Xml2Json.xmlToJson(writeValue);
                            richTextBox_RCS_Response_Parameter.Text = json;
                            if (writeValue != "")
                            {
                                bool jsonFlag = Xml2Json.Conver(writeValue, "A", lO);
                            }
                        }
                    }
                }
                if (comboBox1.Text == "3")
                {
                    richTextBox_RCS_Response_Parameter.Text = "";
                    bool xmlFlag = Xml.ReadGen2AgvSchedulingTask(gen13, PLC.l1, PLC.l2);
                    PLC.l3.Clear();
                    if (xmlFlag)
                    {
                        PLC.l3.Add(textBox2.Text);
                        PLC.l3.Add(textBox3.Text);
                        PLC.l3.Add(textBox4.Text);
                        PLC.l3.Add(textBox5.Text);
                        PLC.l3.Add(textBox6.Text);
                        PLC.l3.Add(textBox7.Text);
                        PLC.l3.Add(textBox8.Text);
                        PLC.l3.Add(textBox9.Text);
                        PLC.l3.Add(textBox10.Text);
                        PLC.l3.Add(textBox101.Text);
                        //l3的值写入l4
                        bool wtiteFlag = WritePlcValue.WriteValue(PLC.l1, PLC.l2, PLC.l3, PLC.l4);

                        if (wtiteFlag)
                        {
                            string writeValue = Xml.Readgen2AgvSchedulingTaskXML(gen13, PLC.l4);
                            string json = Xml2Json.xmlToJson(writeValue);
                            richTextBox_RCS_Response_Parameter.Text = json;
                            if (writeValue != "")
                            {
                                bool jsonFlag = Xml2Json.Conver(writeValue, "A", lO);
                            }
                        }
                    }
                }
                MessageBox.Show("生成任务单成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog(ex.Message);
            }
        }
        //继续执行任务
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //XmlDocument doc = new XmlDocument();
                //doc.Load(xmlDBcontinueTask);
                //string xmlStr = doc.OuterXml;

                //string json = Xml2Json.XmlToJson(xmlStr);
                //lW.Add(json);
                //Console.WriteLine(json);
                //richTextBox_RCS_Response_Parameter.Text = json;

                ////Post(json, "continueTask");
                richTextBox_RCS_Response_Parameter.Text = "";
                bool xmlFlag = Xml.ReadContinueTask(xmlDBcontinueTask, PLC.l1, PLC.l2);
                PLC.l3.Clear();
                if (xmlFlag)
                {
                    PLC.l3.Add(textBox11.Text);
                    PLC.l3.Add(textBox12.Text);

                    //l3的值写入l4
                    bool wtiteFlag = WritePlcValue.WriteValue(PLC.l1, PLC.l2, PLC.l3, PLC.l4);
                    if (wtiteFlag)
                    {
                        string writeValue = Xml.ReadContinueTaskXML(xmlDBcontinueTask, PLC.l4);
                        string json = Xml2Json.xmlToJson(writeValue);
                        richTextBox_RCS_Response_Parameter.Text = json;
                        if (writeValue != "")
                        {
                            bool jsonFlag = Xml2Json.Conver(writeValue, "B", lW);
                        }
                    }
                }
                MessageBox.Show("继续执行任务成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog(ex.Message);
            }
        }
        //取消任务
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox_RCS_Response_Parameter.Text = "";
                bool xmlFlag = Xml.ReadCancelTask(xmlDBcancelTask, PLC.l1, PLC.l2);
                PLC.l3.Clear();
                if (xmlFlag)
                {
                    PLC.l3.Add(textBox21.Text);

                    //l3的值写入l4
                    bool wtiteFlag = WritePlcValue.WriteValue(PLC.l1, PLC.l2, PLC.l3, PLC.l4);
                    if (wtiteFlag)
                    {
                        string writeValue = Xml.ReadCancelTaskXML(xmlDBcancelTask, PLC.l4);
                        string json = Xml2Json.xmlToJson(writeValue);
                        richTextBox_RCS_Response_Parameter.Text = json;
                        if (writeValue != "")
                        {
                            bool jsonFlag = Xml2Json.Conver(writeValue, "C", lT);
                        }
                    }
                }
                MessageBox.Show("取消任务成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog(ex.Message);
            }
        }
        //区域清空/释放
        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox_RCS_Response_Parameter.Text = "";
            bool xmlFlag = Xml.ReadBlockArea(xmlDBblockArea, PLC.l1, PLC.l2);
            PLC.l3.Clear();
            if (xmlFlag)
            {
                PLC.l3.Add(textBox31.Text);
                PLC.l3.Add(textBox32.Text);
                PLC.l3.Add(textBox35.Text);
                PLC.l3.Add(textBox33.Text);
                PLC.l3.Add(textBox34.Text);

                //l3的值写入l4
                bool wtiteFlag = WritePlcValue.WriteValue(PLC.l1, PLC.l2, PLC.l3, PLC.l4);

                if (wtiteFlag)
                {
                    string writeValue = Xml.ReadBlockAreaXML(xmlDBblockArea, PLC.l4);
                    string json = Xml2Json.xmlToJson(writeValue);
                    richTextBox_RCS_Response_Parameter.Text = json;
                    if (writeValue != "")
                    {
                        bool jsonFlag = Xml2Json.Conver(writeValue, "D", lY);
                    }
                }
            }
            MessageBox.Show("区域清空/释放成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //发送
        public static string Post(string data, string type)
        {
            richTextBox_RCS_Response2_Parameter.Text = "";
            try
            {
                tools.Rest client = new tools.Rest();
                client.EndPoint = @"" + RCSip + "";  //端点路径
                client.Method = tools.EnumHttpVerb.POST;
                client.PostData = data.Substring(1); //参数

                var resultPost = "";
                resultPost = client.HttpRequest("rcms/services/rest/hikRpcService/" + type + "");
                //JSon序列化 用到第三方Newtonsoft.Json.dll               
                Console.WriteLine("POST方式获取结果：" + Convert.ToString(JsonConvert.DeserializeObject(resultPost)));
                richTextBox_RCS_Response2_Parameter.Text = Convert.ToString(JsonConvert.DeserializeObject(resultPost));
                GlobalLog.WriteInfoLog("POST方式获取结果：" + Convert.ToString(JsonConvert.DeserializeObject(resultPost)));
                return Convert.ToString(JsonConvert.DeserializeObject(resultPost));
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog(ex.Message);
                return "";
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            //多任务-生成任务单
            Task1();
            //多任务-继续执行任务
            Task2();
            //多任务-取消任务
            Task3();
            //区域清空/释放
            Task4();
            //设置货架与储位绑定
            Task5();
        }
        public static void Task1()
        {
            try
            {
                if (lO.Count > 0)
                {
                    for (int i = 0; i < lO.Count; i++)
                    {
                        Post(lO[i], "genAgvSchedulingTask");
                        lO.Remove(lO[i]);
                        Console.WriteLine("Task1:" + lO.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog(ex.Message);
            }
        }
        public static void Task2()
        {
            try
            {
                if (lW.Count > 0)
                {

                    for (int i = 0; i < lW.Count; i++)
                    {
                        Post(lW[i], "continueTask");
                        lW.Remove(lW[i]);
                        Console.WriteLine("Task2:" + lW.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog(ex.Message);
            }
        }
        public static void Task3()
        {
            try
            {
                if (lT.Count > 0)
                {

                    for (int i = 0; i < lT.Count; i++)
                    {
                        Post(lT[i], "cancelTask");
                        lT.Remove(lT[i]);
                        Console.WriteLine("Task3:" + lT.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog(ex.Message);
            }
        }
        public static void Task4()
        {
            try
            {
                if (lY.Count > 0)
                {
                    for (int i = 0; i < lY.Count; i++)
                    {
                        Post(lY[i], "blockArea");
                        lY.Remove(lY[i]);
                        Console.WriteLine("Task4:" + lY.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog(ex.Message);
            }
        }
        private static void Task5()
        {
            try
            {
                if (lZ.Count > 0)
                {
                    for (int i = 0; i < lZ.Count; i++)
                    {
                        Post(lZ[i], "bindPodAndMat");
                        lZ.Remove(lZ[i]);
                        Console.WriteLine("Task5:" + lZ.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog(ex.Message);
            }
        }
        private void button_Timer2_Click(object sender, EventArgs e)
        {
            timer2.Enabled = true;
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            //RespValue();
            if (PLConline)
            {
                label_PLC_State.BackColor = Color.Green;
            }
            else
            {
                label_PLC_State.BackColor = Color.Silver;

            }
        }
        public static void RespValue()
        {
            try
            {
                if (lR.Count > 0)
                {

                    for (int i = 0; i < lR.Count; i++)
                    {
                        Console.WriteLine("RespValue:" + lR[i]);
                        lR.Remove(lR[i]);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            //InitializeProcess();
        }
        /// <summary>
        /// 初始化PLC
        /// </summary>
        private void InitializeProcess()
        {
            try
            {
                //连接PLC
                PlcConn = PLC.ConnectePLC();
                if (PlcConn)
                {
                    //label_PLC_State.BackColor = Color.Green;
                    bool InitialBit = PLC.InitialPLC();
                    if (InitialBit)
                    {
                        GlobalLog.WriteWarningLog("程序启动：" + DateTime.Now.ToString());
                    }
                    else
                    {
                        MessageBox.Show("PLC未设置初始位！");
                        Application.Exit();
                    }
                }
                else
                {
                    MessageBox.Show("PLC未连接！");
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteWarningLog(ex.Message);
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
        }
        #region //关闭服务
        private static string path = Application.StartupPath + ConfigurationManager.ConnectionStrings["reqCode"].ConnectionString;
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }
        public static void FormClose()
        {
            try
            {
                if (OpenThread.readPlc != null)
                {
                    tools.IniFile ini = new tools.IniFile(path);
                    ini.IniWriteValue("reqCode", "value", WritePlcValue.reqCode.ToString());
                    OpenThread.readPlc.Abort(); //通道1
                    OpenThread.post.Abort();
                    OpenThread.db.Abort();
                    PLC.WriteJuXu1PLC();//程序结束 写入PLC就绪-false
                    PLC.Writemingling2PLC(); //PC-PLC命令结束
                    PLC.Writemingling1PLC(); //PC-PLC任务完成
                    PLC.WriteXiaFaTask1PLC(); //PC-PLC 可上传信号
                    PLC.zhuangxiangjiuxu1PLC();
                    //OpenThread.read2Plc.Abort(); //通道2
                    //PLC.WriteJuXu1ToPLCBit2();
                    //PLC.WriteMingLing1ToPLCBit2();
                    //PLC.WritemRenWuOver1ToPLCBit2();
                    //PLC.WriteUploadTask1ToPLCBit2();
                    reader.Close();
                    myProcess.Kill();
                    myProcess.WaitForExit();
                    OpenThread.read.Abort();
                    Application.Exit();
                }
            }
            catch (Exception)
            {

            }

        }
        #endregion
        #region 遍历集合
        private static string RCSip = ConfigurationManager.ConnectionStrings["RCSip"].ConnectionString;
        private static string RCSipStatus = ConfigurationManager.ConnectionStrings["RCSipStatus"].ConnectionString;
        /// <summary>
        /// 遍历集合l5(json)
        /// </summary>
        public static void Traversel5()
        {
            while (true)
            {
                try
                {
                    if (PLC.l5.Count > 0)
                    {
                        for (int i = 0; i < PLC.l5.Count; i++)
                        {
                            if (PLC.l5[i].Substring(0, 1) == "D")
                            {
                                int bin = tools.JsonOperate.Json_Decode_Bin(PLC.l5[i]);
                                string data = Post(PLC.l5[i]);
                                Console.WriteLine("RCS返回参数" + data);
                                PLC.l5.Remove(PLC.l5[i]); //删除
                                if (data != "")
                                {
                                    if (bin == 0)
                                    {
                                        Form1.lR.Add(data);
                                    }
                                }
                            }
                            else if (PLC.l5[i].Substring(0, 1) == "E")
                            {
                                string data = Post(PLC.l5[i]);
                                Console.WriteLine("RCS返回参数" + data);
                                PLC.l5.Remove(PLC.l5[i]); //删除
                                if (data != "")
                                {
                                    Form1.lR.Add("E" + data);
                                }
                            }
                            else if (PLC.l5[i].Substring(0, 1) == "F")
                            {
                                string data = PostStatus(PLC.l5[i]);
                                Console.WriteLine("RCS返回参数" + data);
                                PLC.l5.Remove(PLC.l5[i]); //删除
                                if (data != "")
                                {
                                    Form1.lR.Add("F" + data);
                                }
                            }
                            else if (PLC.l5[i].Substring(0, 1) == "G")
                            {
                                int bin = tools.JsonOperate.Json_Decode_Bin(PLC.l5[i]);
                                string data = Post(PLC.l5[i]);
                                Console.WriteLine("RCS返回参数" + data);
                                PLC.l5.Remove(PLC.l5[i]); //删除
                                if (data != "")
                                {
                                    if (bin == 1)//绑定
                                    {
                                        Form1.lR.Add("G1" + data);
                                    }
                                    else if (bin == 0) //解绑
                                    {
                                        Form1.lR.Add("G0" + data);
                                    }
                                }
                            }
                            else
                            {
                                string data = Post(PLC.l5[i]);
                                Console.WriteLine("RCS返回参数" + data);
                                PLC.l5.Remove(PLC.l5[i]); //删除
                                if (data != "")
                                {
                                    //post返回code:0 成功 1~N 失败
                                    //int code = tools.JsonOperate.Json_Decode1(data);
                                    //if (code == 0)
                                    //{
                                    //    PLC.WriteminglingPLC();
                                    //}
                                    //Form1.lR.Add(data);
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        //Post-RCS
        private static string Post(string postData)
        {
            try
            {
                tools.Rest client = new tools.Rest();
                client.EndPoint = @"" + RCSip + "";  //端点路径
                client.Method = tools.EnumHttpVerb.POST;
                client.PostData = postData.Substring(1); //参数
                string para = postData.Substring(0, 1); //接口类型
                string resultPost = "";
                if (para == "A")
                {
                    resultPost = client.HttpRequest("rcms/services/rest/hikRpcService/genAgvSchedulingTask");
                    //lR.Add(resultPost);  //添加到lR集合
                    string json = Convert.ToString(JsonConvert.DeserializeObject(resultPost));
                    GlobalLog.WriteInfoLog("RCS返回Json" + json);
                    int code = tools.JsonOperate.Json_Decode1(json);
                    if (code != 0)
                    {
                        richTextBox_Error_Info.Text += json + "\r\n";
                    }
                    return json;
                }
                else if (para == "B")
                {
                    resultPost = client.HttpRequest("rcms/services/rest/hikRpcService/continueTask");
                    //lR.Add(resultPost);  //添加到lR集合
                    string json = Convert.ToString(JsonConvert.DeserializeObject(resultPost));
                    GlobalLog.WriteInfoLog("RCS返回Json" + json);
                    int code = tools.JsonOperate.Json_Decode1(json);
                    if (code != 0)
                    {
                        richTextBox_Error_Info.Text += json + "\r\n";
                    }
                    return json;
                }
                else if (para == "C")
                {
                    resultPost = client.HttpRequest("rcms/services/rest/hikRpcService/cancelTask");
                    //lR.Add(resultPost);  //添加到lR集合
                    string json = Convert.ToString(JsonConvert.DeserializeObject(resultPost));
                    GlobalLog.WriteInfoLog("RCS返回Json" + json);
                    int code = tools.JsonOperate.Json_Decode1(json);
                    if (code != 0)
                    {
                        richTextBox_Error_Info.Text += json + "\r\n";
                    }
                    return json;
                }
                else if (para == "D")
                {
                    resultPost = client.HttpRequest("rcms/services/rest/hikRpcService/blockArea");
                    //lR.Add(resultPost);  //添加到lR集合
                    string json = Convert.ToString(JsonConvert.DeserializeObject(resultPost));
                    GlobalLog.WriteInfoLog("RCS返回Json" + json);
                    int code = tools.JsonOperate.Json_Decode1(json);
                    if (code != 0)
                    {
                        richTextBox_Error_Info.Text += json + "\r\n";
                    }
                    return json;
                }
                else if (para == "E")
                {
                    resultPost = client.HttpRequest("rcms/services/rest/hikRpcService/queryPodBerthAndMat");
                    //lR.Add(resultPost);  //添加到lR集合
                    string json = Convert.ToString(JsonConvert.DeserializeObject(resultPost));
                    GlobalLog.WriteInfoLog("RCS返回Json" + json);
                    int code = tools.JsonOperate.Json_Decode1(json);
                    if (code != 0)
                    {
                        richTextBox_Error_Info.Text += json + "\r\n";
                    }
                    return json;
                }
                else if (para == "G")
                {
                    resultPost = client.HttpRequest("rcms/services/rest/hikRpcService/bindPodAndBerth");
                    //lR.Add(resultPost);  //添加到lR集合
                    string json = Convert.ToString(JsonConvert.DeserializeObject(resultPost));
                    GlobalLog.WriteInfoLog("RCS返回Json" + json);
                    int code = tools.JsonOperate.Json_Decode1(json);
                    if (code != 0)
                    {
                        richTextBox_Error_Info.Text += json + "\r\n";
                    }
                    return json;
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
        private static string PostStatus(string postData)
        {
            try
            {
                tools.Rest client = new tools.Rest();
                client.EndPoint = @"" + RCSipStatus + "";  //端点路径
                client.Method = tools.EnumHttpVerb.POST;
                client.PostData = postData; //参数

                string resultPost = "";

                resultPost = client.HttpRequest("rcms-dps/rest/queryAgvStatus");
                //lR.Add(resultPost);  //添加到lR集合
                string json = Convert.ToString(JsonConvert.DeserializeObject(resultPost));
                //GlobalLog.WriteInfoLog("RCS返回Json-AGV状态" + json);
                int code = tools.JsonOperate.Json_Decode1(json);
                if (code != 0)
                {
                    richTextBox_Error_Info.Text += json + "\r\n";
                }
                return json;
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Form1.PostStatus：" + ex.Message);
                return "";
            }
        }
        #endregion
        #region lR集合(json)写入PLC
        public static void WritePlc()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(100);
                    bool jieshou = PLC.jieshouPLC();
                    //读PLC命令
                    if (jieshou == false)
                    {
                        if (lR.Count > 0)
                        {
                            bool minglingF = PLC.minglingFPLC();
                            if (minglingF == false)
                            {
                                if (PLC.WritePlcFlg)
                                {
                                    PLC.Write();
                                    PLC.mingling2FPLC();
                                    PLC.WritePlcFlg = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        //读PLC命令 false
                        PLC.Writemingling1PLC();
                        PLC.WritePlcFlg = true;
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        #endregion
        private void button_ClearPLC_Click(object sender, EventArgs e)
        {
            try
            {
                if (PLC.l5.Count > 0)
                {
                    PLC.l5.Clear();
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Form1.button_ClearPLC_Click：" + ex.Message);
            }
        }
        private void button_ClearRCS_Click(object sender, EventArgs e)
        {
            try
            {
                if (lR.Count > 0)
                {
                    lR.Clear();
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Form1.button_ClearRCS_Click：" + ex.Message);
            }
        }
        public static List<int> lM = new List<int>();
        //手动写入PLC
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                lM.Clear();
                lM.Add(Convert.ToInt32(textBox41.Text));
                lM.Add(Convert.ToInt32(textBox42.Text));
                lM.Add(Convert.ToInt32(textBox43.Text));

                int k = 0;
                for (int j = 0; j < 5; j = k * 2)
                {
                    //写入PLC(int类型)
                    PLC.plc.Write(DataType.DataBlock, 104, j, Convert.ToInt16(lM[k]));
                    k += 1;
                }
                GlobalLog.WriteInfoLog("写入PLC中的状态--" + lM[1] + "：" + lM[2]);
                MessageBox.Show("写入PLC-DB成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("button6_Click：" + ex.Message);
            }

        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox_RCS_Response_Parameter.Text = "";
                string bindPodAndMat = Application.StartupPath + ConfigurationManager.ConnectionStrings["bindPodAndMat"].ConnectionString;
                bool xmlFlag = Xml.ReadCancelTask(bindPodAndMat, PLC.l1, PLC.l2);
                PLC.l3.Clear();
                if (xmlFlag)
                {
                    PLC.l3.Add(textBox52.Text);
                    PLC.l3.Add(textBox51.Text);
                    PLC.l3.Add(textBox53.Text);
                    PLC.l3.Add(textBox54.Text);
                    PLC.l3.Add(textBox55.Text);

                    //l3的值写入l4
                    bool wtiteFlag = WritePlcValue.WriteValue(PLC.l1, PLC.l2, PLC.l3, PLC.l4);
                    if (wtiteFlag)
                    {
                        string writeValue = Xml.ReadCancelTaskXML(bindPodAndMat, PLC.l4);
                        string json = Xml2Json.xmlToJson(writeValue);
                        richTextBox_RCS_Response_Parameter.Text = json;
                        if (writeValue != "")
                        {
                            bool jsonFlag = Xml2Json.Conver(writeValue, "E", lZ);
                        }
                    }
                }
                MessageBox.Show("设置货架与储位绑定成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog(ex.Message);
            }
        }
        static string java = ConfigurationManager.ConnectionStrings["java"].ConnectionString;
        private void KillProcess()
        {
            try
            {
                Process[] p = Process.GetProcesses();
                foreach (var item in p)
                {
                    if (item.ProcessName == java)
                    {
                        item.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalLog.WriteInfoLog("Form1.KillProcess:" + ex.Message);
            }
        }

       
    }
}
