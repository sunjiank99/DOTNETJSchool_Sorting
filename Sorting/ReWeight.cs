using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace Sorting
{
    public partial class ReWeight : Form
    {
        private DataGridView a;
        private delegate void myDelegate(string s);
        SerialPort serialPort1 = new SerialPort("COM3", 2400, Parity.None, 8, StopBits.One);
        int Row, Colume;
        private string units="";
        public ReWeight(DataGridView a,int Row,int Colume,string units)
        {
            InitializeComponent();
            this.a=a;
            this.Row = Row;
            this.Colume = Colume;
            this.units = units;


            if (this.units == "斤")
            {
                ////关键 为 serialPort1绑定事件句柄
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                serialPort1.Open();
            }

            if (this.units == "个")
            {
                label1.Text = "请重新输入个数";
                label3.Text = "个";
                label2.Text = "数量";

            
            }
        }

        private void ReWeight_Load(object sender, EventArgs e)
        {

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
                }
                else
                {
                    serialPort1.DiscardOutBuffer();
                }
            }





        }

        private void SetText(string s)
        {

            textBox1.Text = (Convert.ToDecimal(s) * 2).ToString();
           


        }

        private void button1_Click(object sender, EventArgs e)
        {
            a.Rows[Row].Cells["DrugWeight"].Value = textBox1.Text;
            serialPort1.Close();
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
