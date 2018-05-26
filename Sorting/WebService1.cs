using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using System.Xml.Linq;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;

namespace Sorting
{
   public  class WebService1
    {

        public class Goods
        {
            public string DrugCode;
            public string DrugName;
            public Decimal DrugPrice;

        }

        public class GoodsView
        {
            public string UserTel;
            public string UserName;
            public string DrugCode;
            public string DrugName;
            public Decimal DrugWeight;
            public string FJ_id;
            public DateTime FJ_time;
            public int packed;
            public Decimal DrugPrice;
            public Decimal TotalPrice;
        }
       /// <summary>
       /// 验证用户名密码
       /// </summary>
       /// <param name="id">用户名</param>
       /// <param name="ps">密码</param>
       /// <returns>true正确 false 错误</returns>
        public bool verifyPassWord(string id, string ps)
        {
            string ConnString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=SCYueve;Data Source=(local)";
            string inputPs, readPS;
            bool returnVal;
            string ConnQuery = "select " + "UserPwd" + " from " + "ES_User" + " where " + "UserLogin" + "='" + id + "'";
            SqlConnection connection = new SqlConnection(ConnString);
            connection.Open();
            SqlCommand lo_cmd = new SqlCommand(ConnQuery, connection);
            SqlDataReader reader = lo_cmd.ExecuteReader();
            reader.Read();
            if (!reader.HasRows) //读取的行数
            {
                returnVal = false;
            }
            else
            {
                readPS = reader[0].ToString();
                inputPs = FormsAuthentication.HashPasswordForStoringInConfigFile(ps, "MD5");
                //string strMd5 = HashPasswordForStoringInConfigFile("123", "md5"); 


                if (readPS == inputPs)
                {
                    returnVal = true;

                }
                else
                {
                    returnVal = false;
                }



            }

            reader.Close();
            connection.Close();
            connection.Dispose();

            //ConnString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=sngoo712;Data Source=(local)";
            return returnVal;



        }



        /// <summary>
        /// 读取数据库数据
        /// </summary>
        /// <param name="classname">要读取得列名</param>
        /// <param name="indexclass">索引列名</param>
        /// <param name="indexcontent">索引内容</param>
        /// <param name="tablename">要读取得表名</param>
        /// <returns></returns>
        public string searchContent(string classname, string indexclass, string indexcontent, string tablename)
        {
            string ConnString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=SCYueve;Data Source=(local)";
            string consequence;
            string ConnQuery = "select " + classname + " from " + tablename + " where " + indexclass + "='" + indexcontent + "'";
            SqlConnection connection = new SqlConnection(ConnString);
            connection.Open();
            SqlCommand lo_cmd = new SqlCommand(ConnQuery, connection);
            SqlDataReader reader = lo_cmd.ExecuteReader();
            reader.Read();

            consequence = reader[0].ToString();
            if (consequence == null)
            {
                consequence = "-1";
            }
            reader.Close();
            connection.Close();
            connection.Dispose();

            //ConnString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=sngoo712;Data Source=(local)";
            return consequence;




        }




      /// <summary>
      /// 读取货物信息
      /// </summary>
      /// <returns></returns>
        public List<Goods> ReadGoods()
        {
            string ConnString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=SCYueve;Data Source=(local)";
            Goods cache=new Goods();
            List<Goods> cacheList = new List<Goods>();

            string ConnQuery = "select " + "*" + " from " + "物品信息_主表" ;
            SqlConnection connection = new SqlConnection(ConnString);
            connection.Open();
            SqlCommand lo_cmd = new SqlCommand(ConnQuery, connection);
            SqlDataReader reader = lo_cmd.ExecuteReader();
            while (reader.Read())
            {   

                cache.DrugCode = reader["物品编号"].ToString();
                cache.DrugName = reader["物品名称"].ToString();
                cache.DrugPrice = Convert.ToDecimal(reader["物品单价"]);
             
                cacheList.Add(cache);




            }



            reader.Close();
            connection.Close();
            connection.Dispose();

            return cacheList;






        }


        /// <summary>
        /// 写入日分拣数据表
        /// </summary>
        /// <param name="cache"> 写入的分拣信息</param>
        public void WriteIntoFJ(GoodsView info)
        {

            string ConnString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=sngoo712;Data Source=(local)";
            string ConnQuery = " insert into fj_GoodsInfo (UserTel,UserName,DrugCode,DrugName,DrugWeight,FJ_id,FJ_time,packed,DrugPrice,TotalPrice)values('" + info.UserTel + "','" + info.UserName + "','" + info.DrugCode + "','" + info.DrugName + "','" + info.DrugWeight + "','" + info.FJ_id + "','" + info.FJ_time + "','" + info.packed + "','"+info.DrugPrice+"','"+info.TotalPrice+"')";
            SqlConnection connection = new SqlConnection(ConnString);
            connection.Open();
            SqlCommand lo_cmd = new SqlCommand(ConnQuery, connection);
            lo_cmd.ExecuteNonQuery();




            connection.Close();
            connection.Dispose();




        }




        /// <summary>
        /// 读取分拣订单信息
        /// </summary>
        /// <param name="StarTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>

        public List<GoodsView> readFJINFO(string StarTime, string EndTime)
        {

            string ConnString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=sngoo712;Data Source=(local)";
            List<GoodsView> cacheRead = new List<GoodsView>();
            
           
            string ConnQuery = "select * from fj_GoodsInfo where FJ_time between'" + StarTime + "' and'" + EndTime + "'";
            SqlConnection connection = new SqlConnection(ConnString);
            connection.Open();
            SqlCommand lo_cmd = new SqlCommand(ConnQuery, connection);
            SqlDataReader reader = lo_cmd.ExecuteReader();

            if (!reader.HasRows) //读取的行数
            {
                cacheRead = null;
            }
            else
            {
                
                while (reader.Read())
                {
                    GoodsView cacheItem = new GoodsView();
                    cacheItem.DrugCode = reader["DrugCode"].ToString();
                    cacheItem.DrugName = reader["DrugName"].ToString();
                    cacheItem.DrugWeight = Convert.ToDecimal(reader["DrugWeight"]);
                    cacheItem.FJ_id = reader["FJ_id"].ToString();
                    cacheItem.FJ_time = Convert.ToDateTime(reader["FJ_time"]);
                    cacheItem.packed = Convert.ToInt32(reader["packed"]);
                    cacheItem.UserName = reader["UserName"].ToString();
                    cacheItem.UserTel = reader["UserTel"].ToString();
                    
                    if (reader["DrugPrice"].ToString() != "")
                    {

                        cacheItem.DrugPrice = Convert.ToDecimal(reader["DrugPrice"]);
                        
                    }
                    else
                    {
                        cacheItem.DrugPrice = 0;
                    }

                    if (reader["TotalPrice"].ToString() != "")
                    {
                        cacheItem.TotalPrice = Convert.ToDecimal(reader["TotalPrice"]);
                    }
                    else
                    {
                        cacheItem.TotalPrice = 0;
                    }

                    cacheRead.Add(cacheItem);

                }





            }

            reader.Close();
            connection.Close();
            connection.Dispose();
            return cacheRead;

        }

       /// <summary>
       /// 从数据库删除指定的分拣信息
       /// </summary>
       /// <param name="FJ_id"></param>
        public void Delete(string FJ_id)
        {

            string ConnString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=sngoo712;Data Source=(local)";
            string ConnQuery = " delete  fj_GoodsInfo where FJ_id='" + FJ_id + "'";
            SqlConnection connection = new SqlConnection(ConnString);
            connection.Open();
            SqlCommand lo_cmd = new SqlCommand(ConnQuery, connection);
            lo_cmd.ExecuteNonQuery();




            connection.Close();
            connection.Dispose();
        
        
        
        }

       /// <summary>
       /// 修改重量
       /// </summary>
       /// <param name="FJ_id">分拣号码</param>
       /// <param name="weight">重量</param>
       /// <param name="TotalPrice">总计金额</param>

        public void ALterWeight(string FJ_id,double weight,double TotalPrice)
        {
            string ConnString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=sngoo712;Data Source=(local)";
            string ConnQuery = "  update fj_GoodsInfo set DrugWeight='"+weight+"' where FJ_id='"+FJ_id+"' \n";
            ConnQuery += "update fj_GoodsInfo set TotalPrice='"+TotalPrice+"'where FJ_id='"+FJ_id+"'";
            SqlConnection connection = new SqlConnection(ConnString);
            connection.Open();
            SqlCommand lo_cmd = new SqlCommand(ConnQuery, connection);
            lo_cmd.ExecuteNonQuery();




            connection.Close();
            connection.Dispose();
        
        
        }
     

    }
}
