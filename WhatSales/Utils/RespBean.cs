using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatSales.Utils
{
    public class RespBean
    {
        //状态码
        public int status { get; set; }
        //错误信息
        public string msg { get; set; }
        //返回对象
        public Object obj { get; set; }

        public RespBean(int status,string msg, Object obj)
        {
            this.status = status;
            this.msg = msg;
            this.obj = obj;
        }

        //成功
        public static RespBean success(String msg)
        {
            return new RespBean(200, msg, null);
        }

        public static RespBean success(String msg, Object obj)
        {
            return new RespBean(200, msg, obj);
        }
        //失败
        public static RespBean fail(String msg)
        {
            return new RespBean(500, msg, null);
        }

        public static RespBean fail(String msg, Object obj)
        {
            return new RespBean(500, msg, obj);
        }
    }
}