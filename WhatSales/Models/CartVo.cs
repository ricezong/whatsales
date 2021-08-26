using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatSales.Models
{
    public class CartVo
    {
        public int id { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }
}