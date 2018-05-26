using System;
using System.Collections.Generic;
using System.ComponentModel;

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
//using Sorting.net.f3322.scyueve1;
//using NoTest = Sorting.net.f3322.scyueve1;

using Test = Sorting.cn.com.sngoo.app7;
namespace Sorting
{   
    
    public partial class Main : Form
    {
        string  Startime;
        string  Endtime;
        User User = new User();
        bool flag=false;  // false 按个分拣 true 按斤分拣
        
        //SortWeight SortView;   //分拣窗口 
        Test.GoodsView View;  //货物信息
        Test.WebService1 SQL = new Test.WebService1();
        Test.GoodsView infoOne = new Test.GoodsView();
        SortWeight SortView;
        private delegate void myDelegate(string s);
        public Main(User a)
        {
            InitializeComponent();
            User = a;
            View = new Test.GoodsView();       //创建一个货品实体类
            SortView = new SortWeight(User, flag, this);
            SortView.SetGoodsInfo = new Action<Test.GoodsView>(this.fuzhi);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            flag = true;
            dataGridView1.Rows.Clear();


            SortView = new SortWeight(User, flag, this);
            
            
            SortView.GoodName.Text = "";
            SortView.Weight.Text = "";
            SortView.FJ_id.Text = "";
          
                SortView.ShowDialog();
            

        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        public void fuzhi(Test.GoodsView text)
        {
            View = text;

            dataGridView1.Rows.Add(View.FJ_id,View.DrugPrice,View.TotalPrice,View.DrugCode,View.DrugName,View.DrugWeight,View.UserName,View.UserTel,View.FJ_time,"重新称重","打印","删除");
            


        }

        private void button2_Click(object sender, EventArgs e)
        {

             dataGridView1.Rows.Clear();
             setTime time =new setTime();
             time.ShowDialog();
             Startime = time.StarTime.Value.ToString();
             Endtime = time.EndTime.Value.ToString();
             Test.GoodsView[] info =null;
             if (SQL.readFJINFO(Startime, Endtime,User.UserName) != null)
             {
                 info = SQL.readFJINFO(Startime, Endtime, User.UserName).ToArray();
             }

             if (info != null)
             {
                 for (int i = 0; i < info.Length; i++)
                 {
                     if (info[i].packed == 0)
                     {
                         dataGridView1.Rows.Add(info[i].FJ_id,info[i].DrugPrice,info[i].TotalPrice, info[i].DrugCode, info[i].DrugName, info[i].DrugWeight,info[i].Units,info[i].UserName, info[i].UserTel, info[i].FJ_time,info[i].PuzzlePrice,info[i].PuzzleTotlePrice, "重新称重", "打印", "删除");
                     }
                     else
                     {


                         dataGridView1.Rows.Add(info[i].FJ_id, info[i].DrugPrice, info[i].TotalPrice, info[i].DrugCode, info[i].DrugName, info[i].DrugWeight, info[i].Units, info[i].UserName, info[i].UserTel, info[i].FJ_time, info[i].PuzzlePrice, info[i].PuzzleTotlePrice, "已打包", "已打包", "已打包");
                     }

                 }
             }
             else
             {
                 MessageBox.Show("没有信息");

             }
            
             

          }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "reWeight" && dataGridView1.Rows[e.RowIndex].Cells["reWeight"].Value!="已打包")
            {
                //SerialPort serialPort1 = new SerialPort("COM3", 2400, Parity.None, 8, StopBits.One);

                //////关键 为 serialPort1绑定事件句柄
                //serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                //serialPort1.Open();


                //dataGridView1.Rows[e.RowIndex].Cells["DrugWeight"].Value =(WeightNumber()*2).ToString();
                ReWeight newACtion = new ReWeight(dataGridView1, e.RowIndex, e.ColumnIndex,dataGridView1.Rows[e.RowIndex].Cells["Units"].Value.ToString());
                newACtion.ShowDialog();
                Decimal weight=Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells["DrugWeight"].Value);
                Decimal price = Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells["DrugePrice"].Value);
                Decimal puzzleprice =Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells["PuzzlePrice"].Value);

                dataGridView1.Rows[e.RowIndex].Cells["TotalPrice"].Value = (weight*price).ToString("0.00");
                SQL.ALterWeight(dataGridView1.Rows[e.RowIndex].Cells["FJ_id"].Value.ToString(), Convert.ToDouble(weight), Convert.ToDouble(weight * price), Convert.ToDouble(puzzleprice*weight));
                
            }
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Print" && dataGridView1.Rows[e.RowIndex].Cells["Print"].Value != "已打包")
            {
                infoOne.DrugName = dataGridView1.Rows[e.RowIndex].Cells["DrugName"].Value.ToString();
                infoOne.DrugWeight = Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells["DrugWeight"].Value);
                infoOne.FJ_id = dataGridView1.Rows[e.RowIndex].Cells["FJ_id"].Value.ToString();
                infoOne.TotalPrice = Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells["TotalPrice"].Value);
                BartenderprintShow();


               
            }
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete" && dataGridView1.Rows[e.RowIndex].Cells["Delete"].Value!= "已打包")
            {
                if (dataGridView1.Rows[e.RowIndex].Cells["FJ_id"].Value != null)
                {
                    SQL.Delete(dataGridView1.Rows[e.RowIndex].Cells["FJ_id"].Value.ToString());
                    dataGridView1.Rows.Remove(dataGridView1.Rows[e.RowIndex]);
                }
            }
        }

        public double WeightNumber()
        {
            SerialPort serialPort1 = new SerialPort("COM3", 2400, Parity.None, 8, StopBits.One);
            double FinallNumber = 0;
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
                    if (readBuffer[0] == 255 && readBuffer[2] != 0 && readBuffer[1] != 255 && readBuffer[1] != 0 && readBuffer[5] == 0)
                    {
                        break;
                    }
                    cout++;
                    if (cout > 100 && readBuffer[0] == 255)
                    {
                        MessageBox.Show("没放东西");
                        flag = false;
                        break;
                    }
                }
                //string readstr = Encoding.UTF8.GetString(readBuffer);
                string readstr = "";
                string[] binary = new string[6];  //      二进制字符串数组
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

                    FinallNumber = Maxshi * 100 + Maxge * 10 + Midshi * 1 + Midge * 0.1 + Minshi * 0.01 + Minge * 0.001;











                }

            }

            serialPort1.Close();

            return FinallNumber;




        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


        public void printShow()
        {



            FJprintMoble newmoban = new FJprintMoble();

            //物品姓名
            TextObject GoodsName = (TextObject)newmoban.ReportDefinition.ReportObjects["GoodsName"];
            GoodsName.Text = infoOne.DrugName;

            //物品重量
            TextObject GoodsWeight = (TextObject)newmoban.ReportDefinition.ReportObjects["GoodsWeight"];
            GoodsWeight.Text = infoOne.DrugWeight.ToString()+"市斤";

            //分拣序号条形码
            TextObject FJ_idM = (TextObject)newmoban.ReportDefinition.ReportObjects["FJ_idM"];
            //FJ_idM.Font = new Font("code 128", 20);
            FJ_idM.Text = "*" + "2016080614592031" + "*";


            //分拣序号

            TextObject FJ_id = (TextObject)newmoban.ReportDefinition.ReportObjects["FJ_id"];
            FJ_id.Text = infoOne.FJ_id;









            System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();

            int rawKind = 1;
            for (int i = 0; i <= doc.PrinterSettings.PaperSizes.Count - 1; i++)
            {
                if (doc.PrinterSettings.PaperSizes[i].PaperName == "称重标签")
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


        public void BartenderprintShow()
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



            btFormat.SubStrings["GoodsName"].Value = infoOne.DrugName;//为Bartender里的数据源（文本框、条码等等）传值
            btFormat.SubStrings["GoodsWeight"].Value = infoOne.DrugWeight.ToString("0.00")+"斤";
            btFormat.SubStrings["FJ_id"].Value = infoOne.FJ_id;
            btFormat.SubStrings["TotalPrice"].Value = "￥"+infoOne.TotalPrice.ToString("0.00");
            //btFormat.SubStrings["k3"].Value = batch;
            //btFormat.SubStrings["s0"].Value = value;
            //Result nResult1 = btFormat.Print("标签打印软件", waitout, out messages);
            //btFormat.PrintSetup.Cache.FlushInterval = CacheFlushInterval.PerSession;


            btFormat.Print();
            btFormat.Close(SaveOptions.DoNotSaveChanges);//不保存对打开模板的修改
            btEngine.Stop();
        
        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (FJ_IDSearche.Text.ToString() != "")
            {
                Test.GoodsView searcheConsequence = SQL.searcheGoods(FJ_IDSearche.Text.ToString(), User.UserName);
                if (searcheConsequence != null)
                {
                    dataGridView1.Rows.Clear();
                    if (searcheConsequence.packed == 0)
                    {
                        dataGridView1.Rows.Add(searcheConsequence.FJ_id, searcheConsequence.DrugPrice, searcheConsequence.TotalPrice, searcheConsequence.DrugCode, searcheConsequence.DrugName, searcheConsequence.DrugWeight,searcheConsequence.Units, searcheConsequence.UserName, searcheConsequence.UserTel, searcheConsequence.FJ_time, searcheConsequence.PuzzlePrice,searcheConsequence.PuzzleTotlePrice,"重新称重", "打印", "删除");
                    }
                    else
                    {


                        dataGridView1.Rows.Add(searcheConsequence.FJ_id, searcheConsequence.DrugPrice, searcheConsequence.TotalPrice, searcheConsequence.DrugCode, searcheConsequence.DrugName, searcheConsequence.DrugWeight,searcheConsequence.Units, searcheConsequence.UserName, searcheConsequence.UserTel, searcheConsequence.FJ_time, searcheConsequence.PuzzlePrice,searcheConsequence.PuzzleTotlePrice,"已打包", "已打包", "已打包");
                    }

                
                }
                else
                {

                    MessageBox.Show("没有相应信息");
                }
            }
            else
            {
                MessageBox.Show("请输入分拣账号");
            }

            FJ_IDSearche.Text = "";

        }

        private void button5_Click(object sender, EventArgs e)
        {
            flag = false;
            dataGridView1.Rows.Clear();


            SortView = new SortWeight(User, flag, this);


            SortView.GoodName.Text = "";
            SortView.Weight.Text = "";
            SortView.FJ_id.Text = "";
            SortView.ShowDialog();
        }

      

    }
}
