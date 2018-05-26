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
    public partial class Login : Form
    {
        Test.WebService1 SQL = new Test.WebService1();
       
        User user;
        Main mainView;   //主窗口
        
        public Login()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            
            if (SQL.verifyPassWord(ID.Text.ToString(), PS.Text.ToString()))
            {
                if (SQL.verifyUserRole(ID.Text.ToString()))
                {
                    user = new User(ID.Text.ToString());
                    user.getTelAndName();

                    mainView = new Main(user);


                    this.Hide();
                    mainView.ShowDialog();
                    this.ID.Text = string.Empty;
                    this.PS.Text = string.Empty;

                    this.Show();
                }
                else
                {
                    MessageBox.Show("没有登陆权限");
                }

            }
            else
            {
                MessageBox.Show("账户或密码错误");

            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void dmButton1_Click(object sender, EventArgs e)
        {
            if (SQL.verifyPassWord(ID.Text.ToString(), PS.Text.ToString()))
            {
                if (SQL.verifyUserRole(ID.Text.ToString()))
                {
                    user = new User(ID.Text.ToString());
                    user.getTelAndName();

                    mainView = new Main(user);


                    this.Hide();
                    mainView.ShowDialog();
                    this.ID.Text = string.Empty;
                    this.PS.Text = string.Empty;

                    this.Show();
                }
                else
                {
                    MessageBox.Show("没有登陆权限");
                }

            }
            else
            {
                MessageBox.Show("账户或密码错误");

            }
        }
    }
}
