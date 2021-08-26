using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatSales.Models
{
    public class Order
    {
        public int id { get; set; }
        public double Price { get; set; }
        public int UserId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}