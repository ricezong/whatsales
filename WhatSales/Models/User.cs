using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WhatSales.Models
{
    public enum Role
    {
        user = 1,
        admin = 2,
    }

    public class User
    {
        public int id { get; set; }
        [Required(ErrorMessage = "请输入用户名")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "请输入密码")]
        [Display(Name = "密码")]
        public string PassWord { get; set; }
        public int Active { get; set; }
        public string Salt { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime DeleteTime { get; set; }
        public Role Role { get; set; }

    }
}