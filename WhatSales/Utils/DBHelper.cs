using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//命名空间的引入;
using System.Data;
using System.Data.SqlClient;
//需要调用web.config命名空间
using System.Configuration;
//集合的命名空间
using System.Collections;

namespace WhatSales.Utils
{
    public class DBHelper
    {
        //编写数据库连接串
        private static readonly string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;


        // 查询操作
        public static DataSet executeQuery(String sql, params SqlParameter[] parms)
        {
            //创建SqlConnection的实例
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                //打开数据库连接
                conn.Open();
                //创建SqlCommand对象执行SQL语句
                SqlCommand cmd = new SqlCommand(sql, conn);
                //将传过来的参数添加到SQL中
                cmd.Parameters.AddRange(parms);
                //创建 SQLDataAdapter 类的对象
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    //创建DataSet类的对象
                    DataSet ds = new DataSet();
                    try
                    {
                        //将查询结果填充到Dataset对象ds中
                        sda.Fill(ds);
                        //返回查询结果
                        return ds;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        // 执行增删改
        public static int executeNonQuery(string sql, params SqlParameter[] parms)
        {
            //创建SqlConnection的实例
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                //创建SqlCommand对象执行SQL语句
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //打开数据库连接
                    conn.Open();
                    try
                    {
                        //将传过来的参数添加到SQL中
                        cmd.Parameters.AddRange(parms);
                        var result = cmd.ExecuteNonQuery();
                        //返回修改结果
                        return result;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        // 返回一条记录的第一列
        public static Object executeScalar(string sql, params SqlParameter[] parms)
        {
            //创建SqlConnection的实例
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                //创建SqlCommand对象执行SQL语句
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    //打开数据库连接
                    conn.Open();
                    try
                    {
                        //将传过来的参数添加到SQL中
                        cmd.Parameters.AddRange(parms);
                        Object result = cmd.ExecuteScalar();
                        //返回修改结果
                        if (result != null)
                        {
                            return result;
                        }
                        else
                        {
                            return null;
                        }
                        
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

    }
}
