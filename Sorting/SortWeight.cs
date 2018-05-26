using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using CrystalDecisions.CrystalReports.Engine;
using Seagull.BarTender.Print;
using Seagull.BarTender.PrintServer;
using Seagull.BarTender.SystemDatabase;
using Seagull.BarTender.Librarian;
using System.Configuration;

//using NoTest = Sorting.net.f3322.scyueve1;

using Test = Sorting.cn.com.sngoo.app7;
namespace Sorting
{
    public partial class SortWeight : Form
    {
        Main ad;
        bool selfmotion=false;  //true称重分拣 false 按个分拣
        int i = 0;
        Test.WebService1 SQL = new Test.WebService1();
        //Sorting.net.f3322.scyueve1.GoodsView info = new Sorting.net.f3322.scyueve1.GoodsView();  //生成商品信息
        Test.GoodsView info = new Test.GoodsView();

        User user = new User();//用户信息
        public Action<Test.GoodsView> SetGoodsInfo { get; set; }  //委托导入主窗口数据

        ChooseGood ChooseView;
        int count=0;
        //public bool youxiao = true; //此窗口是否有效

        Test.Goods GoodsInfo = new Test.Goods();  
        public SortWeight(User a,bool sf,Main flag)
        {
            InitializeComponent();
            ad= flag;
            selfmotion = sf;   //称重判断标志赋值
            
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            user = a;
            //ChooseView = new ChooseGood(selfmotion);
            //ChooseView.AfterChangeTextDel = new Action<Test.Goods>(this.AfterChildChange);
            if (selfmotion == true)
            {
                ChooseView = new ChooseGood(selfmotion);
                ChooseView.AfterChangeTextDel = new Action<Test.Goods>(this.AfterChildChange);
                button4.Hide();
                label1.Text = "称重分拣";
                label8.Show();
                label9.Hide();
                SortNum.Hide();
                //Thread th = new Thread(new ThreadStart(ThreadMethod)); //也可简写为new Thread(ThreadMethod);                
                //th.Start(); //启动线程 
                serialPort1.Close();
                serialPort1 = new SerialPort("COM3", 2400, Parity.None, 8, StopBits.One);

                ////关键 为 serialPort1绑定事件句柄
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                try
                {
                    serialPort1.Open();
                }
                catch(Exception err)
                {
                    MessageBox.Show(err.Message);
                    
                }



            }
            else     //按个分拣
            {
                ChooseView = new ChooseGood(selfmotion);
                ChooseView.AfterChangeTextDel = new Action<Test.Goods>(this.AfterChildChange);
                button4.Show();
                label3.Text = "数量";
                label5.Text = "个";
                label1.Text = "按个分拣";
                label8.Hide();
                label9.Show();
                SortNum.Show();

            
            }
        }
        private delegate void myDelegate(string s);
        private void button3_Click(object sender, EventArgs e)
        {     
              ChooseView.ShowDialog();
        }

        private void SortWeight_Load(object sender, EventArgs e)
        {
            
        }

        public void AfterChildChange(Test.Goods text)
        {
            GoodsInfo = text;
            GoodName.Text = GoodsInfo.DrugName;      //赋值选中的货物
            
        }

        private void button2_Click(object sender, EventArgs e)
        {   //获取重量
            Weight.Text = (WeightNumber()*2).ToString();
            if (Weight.Text != "0")
            {
                DateTime time = DateTime.Now;
                string strtime = DateTime.Now.ToString("yyyyMMddHHmmssff");
                FJ_id.Text = "FJ" + strtime;
                //FJ_id.Text = strtime;

                //赋值
                info.FJ_id = FJ_id.Text;
                info.FJ_time = time;
                info.DrugCode = GoodsInfo.DrugCode;
                info.DrugName = GoodsInfo.DrugName;
                info.DrugPrice = GoodsInfo.DrugPrice;
                info.packed = 0;
                info.UserName = user.UserName;
                info.UserTel = user.UserTel;
                info.DrugWeight = Convert.ToDecimal(Weight.Text);
                info.TotalPrice = info.DrugWeight * info.DrugPrice;

                DrugPrice.Text = info.DrugPrice.ToString("0.00");
                TotalPrice.Text = info.TotalPrice.ToString("0.00");                  

                MessageBox.Show("称重成功，可以继续称重下一份商品");
                //写入数据库
                SQL.WriteIntoFJ(info);
                //打印
                BartenderprintShow("个");

                //SetGoodsInfo += new Action<Test.GoodsView>();

                SetGoodsInfo(info);  //执行委托到父级窗口    
            }
            else
            {
                MessageBox.Show("称重失败，请重新称重");
            }
            

        }


        public decimal WeightNumber()
        {   

            if(SerialPort.GetPortNames().Length==0)
            {
                MessageBox.Show("未能连接电子秤");
                return 0;
            }
             serialPort1 = new SerialPort(SerialPort.GetPortNames()[0], 2400, Parity.None, 8, StopBits.One);
            decimal FinallNumber = 0;
             serialPort1.Open();

            if (serialPort1.IsOpen == true)
            {
                //byte[] readBuffer = new byte[serialPort1.ReadBufferSize];
                byte[] readBuffer = new byte[6];
                //int int32 = BitConverter.ToInt32(readBuffer[], 0);
                //string hexStr="";

                //  for (int i = 0; i < 6; i++)
                //{
                //         string a =Convert.ToString(readBuffer[i], 16);
                //         hexStr = hexStr + a;

                //  }
                Thread.Sleep(500);
                int cout = 0;
                bool flag = true;
                while (true)
                {   
                    serialPort1.Read(readBuffer, 0, readBuffer.Length);
                    if (readBuffer[0] == 255&&readBuffer[2]!=0&&readBuffer[1]!=255&&readBuffer[1]!=0&&readBuffer[5]==0 )
                    {
                        break;
                    }
                    cout++;
                    if (cout >100&&readBuffer[0]==255)
                    {
                        MessageBox.Show("没放东西");
                        flag = false;
                        break;
                    }
                }
                //string readstr = Encoding.UTF8.GetString(readBuffer);
                string readstr = "";
                string[] binary=new string[6];  //      二进制字符串数组
                int[] cache = new int[6];       //      二进制整形数组
                //string NumMin = "";
                //string NumMin=
                string[] Final = new string[6];    //  8位二进制字符窜数组


                if (flag)
                {

                    for (int i = 0; i < 6; i++)
                    {
                        //readstr = readstr + readBuffer[i].ToString("X");
                        //readstr += readBuffer[i].ToString() + "\n";
                        readstr += readBuffer[i].ToString() + "\n";
                        binary[i] = Convert.ToString(readBuffer[i], 2);
                        cache[i] = Convert.ToInt32(binary[i]);
                        Final[i] = string.Format("{0:00000000}", cache[i]);
                        
                       
  
                    }

                    String Minshiwei=Final[2].Substring(0,4);
                    String Mingewei = Final[2].Substring(4, 4);

                    int Minshi=Convert.ToInt32(Minshiwei,2);
                    int Minge = Convert.ToInt32(Mingewei,2);
                    



                    String Midshiwei = Final[3].Substring(0, 4);
                    String Midgewei = Final[3].Substring(4, 4);

                    int Midshi = Convert.ToInt32(Midshiwei, 2);
                    int Midge = Convert.ToInt32(Midgewei, 2);

                    String Maxshiwei = Final[4].Substring(0, 4);
                    String Maxgewei = Final[4].Substring(4, 4);

                    int Maxshi = Convert.ToInt32(Maxshiwei, 2);
                    int Maxge = Convert.ToInt32(Maxgewei, 2);

                     FinallNumber = Convert.ToDecimal(Maxshi * 100 + Maxge * 10 + Midshi * 1 + Midge * 0.1 + Minshi * 0.01 + Minge * 0.001);
                    


                   
                       

                    



                        
                }
                
            }

            serialPort1.Close();

            return FinallNumber;




        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        public void printShow()
        {



            FJprintMoble newmoban = new FJprintMoble();

            //物品姓名
            TextObject GoodsName = (TextObject)newmoban.ReportDefinition.ReportObjects["GoodsName"];
            GoodsName.Text = info.DrugName;

            //物品重量
            TextObject GoodsWeight = (TextObject)newmoban.ReportDefinition.ReportObjects["GoodsWeight"];
            GoodsWeight.Text = info.DrugWeight.ToString();

            //分拣序号条形码
            TextObject FJ_idM = (TextObject)newmoban.ReportDefinition.ReportObjects["FJ_idM"];
            FJ_idM.Text = info.FJ_id;


            //分拣序号

            TextObject FJ_id = (TextObject)newmoban.ReportDefinition.ReportObjects["FJ_id"];
            FJ_id.Text = info.FJ_id;
           


           

          



            System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();
           
            int rawKind = 1;
            for (int i = 0; i <= doc.PrinterSettings.PaperSizes.Count - 1; i++)
            {
                if (doc.PrinterSettings.PaperSizes[i].PaperName == "xiannvguo")
                {
                    rawKind = doc.PrinterSettings.PaperSizes[i].RawKind;
                }
            }


            newmoban.PrintOptions.PrinterName = doc.PrinterSettings.PrinterName;

            newmoban.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)rawKind;    // 设置打印纸张样式
            newmoban.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.DefaultPaperOrientation;//默认纸张方向
            newmoban.PrintToPrinter(1, false, 1, 1);

            return;


        }



        public void BartenderprintShow(string Units)
        {
            Engine btEngine = new Engine();
            bool isAlive = btEngine.IsAlive;

            string path = ConfigurationManager.AppSettings["BartenderPath"];
            btEngine.Start();
            LabelFormatDocument btFormat = btEngine.Documents.Open(path);//这里是Bartender软件生成的模板文件，你需要先把模板文件做好。
            //btFormat.PrintSetup.PrinterName = "Foxit Reader PDF Printer";
            //btFormat.PrintSetup.IdenticalCopiesOfLabel = 1; //打印份数
            //btFormat.SubStrings["s0"].Value = value;
            //Messages messages;
            //int waitout = 10000; // 10秒 超时
            //Result nResult = btFormat.Print("标签打印软件", waitout, out messages);
            //string messageString = "\n\nMessages:";
            //foreach (Seagull.BarTender.Print.Message message in messages)
            //{
            //    messageString += "\n\n" + message.Text;
            //}
            System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();

            int rawKind = 1;
            for (int i = 0; i <= doc.PrinterSettings.PaperSizes.Count - 1; i++)
            {
                if (doc.PrinterSettings.PaperSizes[i].PaperName == "称重标签")
                {
                    rawKind = doc.PrinterSettings.PaperSizes[i].RawKind;
                }
            }


            btFormat.PrintSetup.PrinterName = doc.PrinterSettings.PrinterName;

            if (Units == "斤")
            {
                btFormat.SubStrings["UnitsName"].Value = "重量：";
            
            }
            if (Units == "个")
            {
                btFormat.SubStrings["UnitsName"].Value = "数量：";
            
            }


            btFormat.SubStrings["GoodsName"].Value = info.DrugName;//为Bartender里的数据源（文本框、条码等等）传值
            btFormat.SubStrings["GoodsWeight"].Value = info.DrugWeight.ToString("0.00") + Units;
            btFormat.SubStrings["FJ_id"].Value = info.FJ_id;
            btFormat.SubStrings["TotalPrice"].Value = "￥"+info.TotalPrice.ToString("0.00");
            //btFormat.SubStrings["k3"].Value = batch;
            //btFormat.SubStrings["s0"].Value = value;
            //Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
            //btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;


            btFormat.Print();
            btFormat.Close(SaveOptions.DoNotSaveChanges);//不保存对打开模板的修改
            btEngine.Stop();


        }

        public void ThreadMethod()
        {
             //Action<Test.GoodsView> SetGoodsInfo;
            while (true)
            {
                if (Weight.Text != "")
                {
                    info = new Test.GoodsView();
                    //MessageBox.Show("触发事件");
                    DateTime time = DateTime.Now;
                    string strtime = DateTime.Now.ToString("yyyyMMddHHmmssff");
                    FJ_id.Text = "FJ" + strtime;
                    //FJ_id.Text = strtime;

                    //赋值
                    info.FJ_id = FJ_id.Text;
                    info.FJ_time = time;
                    info.DrugCode = GoodsInfo.DrugCode;
                    info.DrugName = GoodsInfo.DrugName;
                    info.DrugPrice = GoodsInfo.DrugPrice;
                    info.packed = 0;
                    info.UserName = user.UserName;
                    info.UserTel = user.UserTel;
                    info.DrugWeight = Convert.ToDecimal(Weight.Text);
                    info.TotalPrice = info.DrugWeight * info.DrugPrice;

                    DrugPrice.Text = info.DrugPrice.ToString("0.00");
                    TotalPrice.Text = info.TotalPrice.ToString("0.00");

                    //MessageBox.Show("称重成功，可以继续称重下一份商品");
                    //写入数据库
                    SQL.WriteIntoFJ(info);
                    ////打印
                    //BartenderprintShow();



                    SetGoodsInfo(info);  //执行委托到父级窗口   
                    //Invoke(SetGoodsInfo, info);
                    Weight.Text = "";
                    
                }
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
            //关键 代理
            myDelegate md = new myDelegate(SetText);
            
            
            
                
                if (serialPort1.IsOpen == true)
                {
                    if (serialPort1.BytesToRead == 6)
                    {

                        string readstr = "";
                        string[] binary = new string[6];  //      二进制字符串数组
                        int[] cache = new int[6];       //      二进制整形数组
                        byte[] readBuffer = new byte[6];
                        string[] Final = new string[6];    //  8位二进制字符窜数组
                        serialPort1.Read(readBuffer, 0, readBuffer.Length);
                        for (int i = 0; i < 6; i++)
                        {
                            //readstr = readstr + readBuffer[i].ToString("X");
                            //readstr += readBuffer[i].ToString() + "\n";
                            readstr += readBuffer[i].ToString() + "\n";
                            binary[i] = Convert.ToString(readBuffer[i], 2);
                            cache[i] = Convert.ToInt32(binary[i]);
                            Final[i] = string.Format("{0:00000000}", cache[i]);



                        }

                        String Minshiwei = Final[2].Substring(0, 4);
                        String Mingewei = Final[2].Substring(4, 4);

                        int Minshi = Convert.ToInt32(Minshiwei, 2);
                        int Minge = Convert.ToInt32(Mingewei, 2);




                        String Midshiwei = Final[3].Substring(0, 4);
                        String Midgewei = Final[3].Substring(4, 4);

                        int Midshi = Convert.ToInt32(Midshiwei, 2);
                        int Midge = Convert.ToInt32(Midgewei, 2);

                        String Maxshiwei = Final[4].Substring(0, 4);
                        String Maxgewei = Final[4].Substring(4, 4);

                        int Maxshi = Convert.ToInt32(Maxshiwei, 2);
                        int Maxge = Convert.ToInt32(Maxgewei, 2);

                        double FinallNumber = Maxshi * 100 + Maxge * 10 + Midshi * 1 + Midge * 0.1 + Minshi * 0.01 + Minge * 0.001;

                        Invoke(md, FinallNumber.ToString() + "\n");
                        //serialPort1.Close();
                    }
                    else
                    {
                        serialPort1.DiscardOutBuffer();
                    }
                }
             
            
           

            
        }

        private void SetText(string s)
        {
            count++;
            Weight.Text = (Convert.ToDecimal(s) * 2).ToString();
            tiaoban.Text = count.ToString();
            

        }

        private void Weight_TextChanged(object sender, EventArgs e)
        {   
            //if (Weight.Text != "")
            //{
            //    //MessageBox.Show("sdas");
            //    DateTime time = DateTime.Now;
            //    string strtime = DateTime.Now.ToString("yyyyMMddHHmmssff");
            //    FJ_id.Text = "FJ" + strtime;
            //    //FJ_id.Text = strtime;

            //    //赋值
            //    info.FJ_id = FJ_id.Text;
            //    info.FJ_time = time;
            //    info.DrugCode = GoodsInfo.DrugCode;
            //    info.DrugName = GoodsInfo.DrugName;
            //    info.DrugPrice = GoodsInfo.DrugPrice;
            //    info.packed = 0;
            //    info.UserName = user.UserName;
            //    info.UserTel = user.UserTel;
            //    info.DrugWeight = Convert.ToDecimal(Weight.Text);
            //    info.TotalPrice = info.DrugWeight * info.DrugPrice;

            //    DrugPrice.Text = info.DrugPrice.ToString("0.00");
            //    TotalPrice.Text = info.TotalPrice.ToString("0.00");

            //    //MessageBox.Show("称重成功，可以继续称重下一份商品");
            //    ////写入数据库
            //    //SQL.WriteIntoFJ(info);
            //    ////打印
            //    //BartenderprintShow();


                
            //    //SetGoodsInfo(info);  //执行委托到父级窗口    
            //    //Invoke(SetGoodsInfo, info);
            //    ad.dataGridView1.Rows.Add(info.FJ_id, info.DrugPrice, info.TotalPrice, info.DrugCode, info.DrugName, info.DrugWeight, info.UserName, info.UserTel, info.FJ_time, "重新称重", "打印", "删除");
            //    Weight.Text = "";
            //}
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("sdas");
            DateTime time = DateTime.Now;
            string strtime = DateTime.Now.ToString("yyyyMMddHHmmssff");
            FJ_id.Text = "FJ" + strtime;
            //FJ_id.Text = strtime;

            //赋值
            info.FJ_id = FJ_id.Text;
            info.FJ_time = time;
            info.DrugCode = GoodsInfo.DrugCode;
            info.DrugName = GoodsInfo.DrugName;
            info.DrugPrice = GoodsInfo.DrugPrice;
            info.packed = 0;
            info.UserName = user.UserName;
            info.UserTel = user.UserTel;
            info.DrugWeight = 0;
            info.TotalPrice = info.DrugWeight * info.DrugPrice;

            DrugPrice.Text = info.DrugPrice.ToString("0.00");
            TotalPrice.Text = info.TotalPrice.ToString("0.00");

            MessageBox.Show("称重成功，可以继续称重下一份商品");
            ////写入数据库
            //SQL.WriteIntoFJ(info);
            ////打印
            //BartenderprintShow();
            ad.dataGridView1.Rows.Add(info.FJ_id, info.DrugPrice, info.TotalPrice, info.DrugCode, info.DrugName, info.DrugWeight, info.UserName, info.UserTel, info.FJ_time, "重新称重", "打印", "删除");

            
            //SetGoodsInfo(info);  //执行委托到父级窗口    
        }

        private void tiaoban_TextChanged(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now;
            string strtime = DateTime.Now.ToString("yyyyMMddHHmmssff");
            FJ_id.Text = "FJ" + strtime;
            //FJ_id.Text = strtime;

            //赋值
            info.FJ_id = FJ_id.Text;
            info.FJ_time = time;
            info.DrugCode = GoodsInfo.DrugCode;
            info.DrugName = GoodsInfo.DrugName;
            info.DrugPrice = GoodsInfo.DrugPrice;
            info.Units = GoodsInfo.Untis;
            info.packed = 0;
            info.UserName = user.UserName;
            info.UserTel = user.UserTel;
            info.DrugWeight = Convert.ToDecimal(Weight.Text);
            info.TotalPrice = info.DrugWeight * info.DrugPrice;
            info.PuzzlePrice = GoodsInfo.PuzzlePrice;
            info.PuzzleTotlePrice = info.DrugWeight * info.PuzzlePrice;

            DrugPrice.Text = info.DrugPrice.ToString("0.00");
            TotalPrice.Text = info.TotalPrice.ToString("0.00");

            //MessageBox.Show("称重成功，可以继续称重下一份商品");
            //写入数据库
            SQL.WriteIntoFJ(info);
            //打印
            BartenderprintShow("斤");



            //SetGoodsInfo(info);  //执行委托到父级窗口    
            //Invoke(SetGoodsInfo, info);
            ad.dataGridView1.Rows.Add(info.FJ_id, info.DrugPrice.ToString("0.00"), info.TotalPrice.ToString("0.00"), info.DrugCode, info.DrugName, info.DrugWeight,info.Units ,info.UserName, info.UserTel, info.FJ_time,info.PuzzlePrice.ToString("0.00"),info.PuzzleTotlePrice.ToString("0.00"), "重新称重", "打印", "删除");
            //MessageBox.Show("fenjian");
            jishi times = new jishi(info.FJ_id);
            times.ShowDialog();
           
        }


        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort1.Close();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            serialPort1.Close();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (Weight.Text.ToString() == "")
            {
                MessageBox.Show("请输入个数");
                return;
            }

            

            //DateTime time = DateTime.Now;
            //string strtime = DateTime.Now.ToString("yyyyMMddHHmmssff");
            //FJ_id.Text = "FJ" + strtime;
            //FJ_id.Text = strtime;

            //赋值
            //info.FJ_id = FJ_id.Text;
            //info.FJ_time = time;
            info.DrugCode = GoodsInfo.DrugCode;
            info.DrugName = GoodsInfo.DrugName;
            info.DrugPrice = GoodsInfo.DrugPrice;
            info.Units = GoodsInfo.Untis;
            info.packed = 0;
            info.UserName = user.UserName;
            info.UserTel = user.UserTel;
            info.DrugWeight = Convert.ToDecimal(Weight.Text);
            info.TotalPrice = Convert.ToDecimal(Weight.Text) * info.DrugPrice;
            info.PuzzlePrice = GoodsInfo.PuzzlePrice;
            info.PuzzleTotlePrice = Convert.ToDecimal(Weight.Text) * info.PuzzlePrice;

            DrugPrice.Text = info.DrugPrice.ToString("0.00");
            TotalPrice.Text = info.TotalPrice.ToString("0.00");

            int count = Convert.ToInt32(SortNum.Text);//生成分拣记录的数量
            //MessageBox.Show("称重成功，可以继续称重下一份商品");


            for (int i = 0; i < count; i++)
            {

                DateTime time = DateTime.Now;
                string strtime = DateTime.Now.ToString("yyyyMMddHHmmssff");
                FJ_id.Text = "FJ" + strtime;
                info.FJ_id = FJ_id.Text;
                info.FJ_time = time;
                //写入数据库
                SQL.WriteIntoFJ(info);
                //打印
                BartenderprintShow("个");



                //SetGoodsInfo(info);  //执行委托到父级窗口    
                //Invoke(SetGoodsInfo, info);
                ad.dataGridView1.Rows.Add(info.FJ_id, info.DrugPrice.ToString("0.00"), info.TotalPrice.ToString("0.00"), info.DrugCode, info.DrugName, info.DrugWeight, info.Units, info.UserName, info.UserTel, info.FJ_time, info.PuzzlePrice, info.PuzzleTotlePrice, "重新称重", "打印", "删除");
            }
            //MessageBox.Show("fenjian");
            //jishi times = new jishi(info.FJ_id);
            //times.ShowDialog();
        }
    }
}
