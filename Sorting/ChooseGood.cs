using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using Sorting.net.f3322.scyueve1;
//using NoTest = Sorting.net.f3322.scyueve1;

using Test = Sorting.cn.com.sngoo.app7;
namespace Sorting
{
    public partial class ChooseGood : Form
    {
        public Action<Test.Goods> AfterChangeTextDel { get; set; }

        bool flag = false;//判断标志 true为称重分拣，false为按个分拣

        Test.Goods Goodsinfo = new Test.Goods();//选中货物的信息

        List<string> fruitClass = new List<string>();   //水果分类
        Test.WebService1 a = new Test.WebService1();

        public ChooseGood(bool flag)
        {
            InitializeComponent();
            this.flag = flag;
            
            fruitClass = a.GetGoodsClass().ToList();

            metroComboBox1.Items.AddRange(fruitClass.ToArray());  //添加选项
            
            //Test.Goods[] Goods = a.ReadGoods().ToArray();

            //if (flag == true) //按斤称重
            //{   int k=0;
            //    for (int i = 0; i < Goods.Length; i++)
            //    {
            //        if (Goods[i].Untis == "斤")
            //        {
            //            dataGridView1.Rows.Add();
            //            dataGridView1.Rows[k].Cells["GoodName"].Value = Goods[i].DrugName;
            //            dataGridView1.Rows[k].Cells["GoodsNode"].Value = Goods[i].DrugCode;
            //            dataGridView1.Rows[k].Cells["DrugPrice"].Value = Goods[i].DrugPrice;
            //            dataGridView1.Rows[k].Cells["Untis"].Value = Goods[i].Untis;
            //            dataGridView1.Rows[k].Cells["PuzzlePrice"].Value = Goods[i].PuzzlePrice;
            //            k++;

            //        }

            //    }
            //}
            //else
            //{   
            //    int k=0;
            //for (int i = 0; i < Goods.Length; i++)
            //{
            //    if (Goods[i].Untis == "个")
            //    {
            //        dataGridView1.Rows.Add();
            //        dataGridView1.Rows[k].Cells["GoodName"].Value = Goods[i].DrugName;
            //        dataGridView1.Rows[k].Cells["GoodsNode"].Value = Goods[i].DrugCode;
            //        dataGridView1.Rows[k].Cells["DrugPrice"].Value = Goods[i].DrugPrice;
            //        dataGridView1.Rows[k].Cells["Untis"].Value = Goods[i].Untis;
            //        dataGridView1.Rows[k].Cells["PuzzlePrice"].Value = Goods[i].PuzzlePrice;
            //        k++;

            //    }

            //}
            
            //}


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ChooseGood_Load(object sender, EventArgs e)
        {    
           





        }

        private void button2_Click(object sender, EventArgs e)
        {
            //int row = dataGridView1.SelectedRows.Count;
            //给对象富裕选中货物的信息
            
            Goodsinfo.DrugName = dataGridView1.CurrentRow.Cells["GoodName"].Value.ToString();
            Goodsinfo.DrugCode = dataGridView1.CurrentRow.Cells["GoodsNode"].Value.ToString();
            Goodsinfo.DrugPrice = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["DrugPrice"].Value);
            Goodsinfo.PuzzlePrice = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["PuzzlePrice"].Value);
            Goodsinfo.Untis = dataGridView1.CurrentRow.Cells["Untis"].Value.ToString();

            AfterChangeTextDel(Goodsinfo);//执行委托

            this.Close();
            




        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void metroComboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            Test.Goods[] Goods = a.ReadGoods(metroComboBox1.SelectedItem.ToString()).ToArray();

            if (flag == true) //按斤称重
            {
                int k = 0;
                for (int i = 0; i < Goods.Length; i++)
                {
                    if (Goods[i].Untis == "斤")
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[k].Cells["GoodName"].Value = Goods[i].DrugName;
                        dataGridView1.Rows[k].Cells["GoodsNode"].Value = Goods[i].DrugCode;
                        dataGridView1.Rows[k].Cells["DrugPrice"].Value = Goods[i].DrugPrice;
                        dataGridView1.Rows[k].Cells["Untis"].Value = Goods[i].Untis;
                        dataGridView1.Rows[k].Cells["PuzzlePrice"].Value = Goods[i].PuzzlePrice;
                        k++;

                    }

                }
            }
            else
            {
                int k = 0;
                for (int i = 0; i < Goods.Length; i++)
                {
                    if (Goods[i].Untis == "个")
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[k].Cells["GoodName"].Value = Goods[i].DrugName;
                        dataGridView1.Rows[k].Cells["GoodsNode"].Value = Goods[i].DrugCode;
                        dataGridView1.Rows[k].Cells["DrugPrice"].Value = Goods[i].DrugPrice;
                        dataGridView1.Rows[k].Cells["Untis"].Value = Goods[i].Untis;
                        dataGridView1.Rows[k].Cells["PuzzlePrice"].Value = Goods[i].PuzzlePrice;
                        k++;

                    }

                }

            }

        
        }
    }
}
