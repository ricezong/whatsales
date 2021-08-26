using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WhatSales.Models
{
    public class MyActionFilterAttribute : ActionFilterAttribute,IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            var user = filterContext.HttpContext.Session["User"];
            if (user != null)
                OnActionExecuting(filterContext);
            else
                filterContext.HttpContext.Response.Redirect("/User/Login");
        }

    }
}