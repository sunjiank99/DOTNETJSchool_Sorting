using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Sorting.net.f3322.scyueve1;
//using NoTest = Sorting.net.f3322.scyueve1;

using Test = Sorting.cn.com.sngoo.app7;

namespace Sorting
{
    public class User
    {
        public string ID;   //登入账号

        public string UserTel;   //电话

        public string UserName;  //姓名

        public User(string id)
        {
            ID = id;
        
        }
        public User()
        {
            

        }

        public void getTelAndName()
        {
            Test.WebService1 SQL = new Test.WebService1();
            UserTel=SQL.searchContent("MobilePhone", "UserLogin", ID, "ES_User");
            UserName = SQL.searchContent("UserName", "UserLogin", ID, "ES_User");
        
        
        }


        
    }
}
