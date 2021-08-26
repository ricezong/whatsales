using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatSales.Models
{
    public class UserInfo
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public string Ip { get; set; }
    }
}