using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatSales.Models
{
    public class OrderProductVo
    {
        public int id { get; set; }
        public string ProductName { get; set; }
        public string Property { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public string UserName { get; set; }
    }
}