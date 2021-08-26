using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatSales.Utils
{
    public class Result
    {
        public string message { get; set; }
        public bool success { get; set; }
        public Object data { get; set; }

        public Result(string message,bool success)
        {
            this.message = message;
            this.success = success;
        }

        public Result(string message, bool success,Object data)
        {
            this.message = message;
            this.success = success;
            this.data = data;
        }
    }
}