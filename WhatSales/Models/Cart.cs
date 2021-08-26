using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatSales.Models
{
    public class Cart
    {
        public int id { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
    }
}