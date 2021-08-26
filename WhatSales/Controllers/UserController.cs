using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhatSales.Models;
using WhatSales.Services;
using WhatSales.Utils;
using System.Net;

namespace WhatSales.Controllers
{
    public class UserController : Controller
    {
        UserServices userServices = new UserServices();
        RespBean respBean=null;

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult DoLogin()
        {
            try
            {
                var username = Request.Params["username"].Trim().ToString();
                var password = Request.Params["password"].Trim().ToString();
                if (username.Equals("") || username == null || password.Equals("") || password == null)
                {
                    respBean = RespBean.fail("用户名和密码禁止为空");
                }
                else
                {
                    //ModelState.IsValid指示是否可以将请求中的传入值正确绑定到模型，以及在模型绑定期间是否违反了任何明确指定的验证规则过程.
                    if (ModelState.IsValid == true)
                    {
                        var user = userServices.FindNameIsExist(username);
                        if (user != null)
                        {
                            var loginUser = userServices.Login(MD5Util.GetMD5(password + user.Salt));
                            if (loginUser != null)
                            {
                                if (loginUser.Active == 1)
                                {
                                    UserInfo info = new UserInfo()
                                    {
                                        UserId = loginUser.id,
                                        Ip = GetIp()
                                    };
                                    userServices.SaveUserInfo(info);
                                    User u = new User()
                                    {
                                        id = loginUser.id,
                                        UserName=loginUser.UserName
                                    };
                                    Session["User"] = u;
                                    respBean = RespBean.success("登录成功，登录信息已保存后台");
                                }
                                else
                                {
                                    respBean = RespBean.fail("该账户已被锁定");
                                }
                            }
                            else
                            {
                                respBean = RespBean.fail("密码错误");
                            }
                        }
                        else
                        {
                            respBean = RespBean.fail("用户不存在");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Json(respBean, JsonRequestBehavior.AllowGet);
        }

        //获取本地ip
        public string GetIp()
        {
            string ip = "";
            System.Net.IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            for (int i = 0; i < addressList.Length; i++)
            {
                ip = addressList[i].ToString();
            }
            return ip;
        }


    }
}
