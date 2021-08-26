using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhatSales.Models
{
    public class userDetails
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public string NickName { get; set; }
        public string Sex { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}