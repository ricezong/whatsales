using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatSales.Models
{
    public class Product
    {
        public int id { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Property { get; set; }
        public int CategoryId { get; set; }
        public int Active { get; set; }
    }
}