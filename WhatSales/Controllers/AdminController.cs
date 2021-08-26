using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WhatSales.Models;
using WhatSales.Services;
using WhatSales.Utils;
namespace WhatSales.Controllers
{
    public class AdminController : Controller
    {
        UserServices admin = new UserServices();   

        private bool checkStr(string str)
        {
            return Regex.IsMatch(str, @"^[A-Za-z0-9_.&!@#]+$");
        }

        private bool IsUserName(string UserName)
        {
            List<User> userList = admin.FindByName(UserName);
            if (userList.Count > 0)
            {
                return true;   
            }
            return false;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FindAll()
        {
            List<User> userList = admin.FindAllUser();
            if (userList == null)
            {
                return Json(RespBean.fail("查询失败"), JsonRequestBehavior.AllowGet);
            }

            return Json(RespBean.success("查询成功", userList), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddUser()
        {
            User user = new User();
            try
            {
                user.UserName = Request.Params["UserName"].ToString().Trim();
                if (IsUserName(user.UserName))
                {
                    return Json(RespBean.fail("此用户已经存在"), JsonRequestBehavior.DenyGet);
                }
                user.PassWord = Request.Params["PassWord"].Trim().ToString();
                user.Active = Convert.ToInt32(Request.Params["Active"].ToString().Trim());
                user.Role = (Role)Enum.Parse(typeof(Role), Request.Params["Role"].ToString().Trim());
                if (!checkStr(user.UserName) || !checkStr(user.PassWord))
                {
                    return Json(RespBean.fail("添加失败"), JsonRequestBehavior.DenyGet);
                }
            }
            catch
            {
                return Json(RespBean.fail("添加失败"), JsonRequestBehavior.DenyGet);
            }
            if (admin.AddUser(user))
            {
                return Json(RespBean.success("添加成功"), JsonRequestBehavior.DenyGet);
            }
            return Json(RespBean.fail("添加失败"), JsonRequestBehavior.DenyGet);
        }

        public ActionResult AddUserView()
        {
            return View("addUser");
        }

        [HttpPost]
        public ActionResult FindOne()
        {
            int id ;
            try
            {
                id = int.Parse(Request.Params["id"].ToString().Trim());
            }
            catch (Exception e)
            {
                return Json(RespBean.fail("参数不能被解析或者参数不存在"), JsonRequestBehavior.AllowGet);
            }
            User user = admin.FindOne(id);
            if (null == user)
            {
                return Json(RespBean.fail("查询失败", user), JsonRequestBehavior.AllowGet);
            }

            return Json(RespBean.success("查询成功", user), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UpdateUserView()
        {
            return View("updateUser");
        }

        [HttpPost]
        public ActionResult UpdateUser()
        {
            int id;
            try
            {
                id = int.Parse(Request.Params["id"].ToString().Trim());
            }
            catch (Exception e)
            {
                return Json(RespBean.fail("参数不能被解析或者参数不存在"), JsonRequestBehavior.AllowGet);
            }

            User user = new User();
            try
            {
                user.UserName = Request.Params["UserName"].ToString().Trim();
                user.Active = Convert.ToInt32(Request.Params["Active"].ToString().Trim());
                user.Role = (Role)int.Parse(Request.Params["Role"].ToString().Trim());
                if (!checkStr(user.UserName))
                {
                    return Json(RespBean.fail("添加失败"), JsonRequestBehavior.DenyGet);
                }
            }
            catch
            {
                return Json(RespBean.fail("更新失败"), JsonRequestBehavior.DenyGet);
            }
            if (admin.UpdateUser(id, user))
            {
                return Json(RespBean.success("更新成功"), JsonRequestBehavior.DenyGet);
            }
            return Json(RespBean.fail("更新失败"), JsonRequestBehavior.DenyGet);
        }

        public ActionResult DeleteUser()
        {
            int id;
            try
            {
                id = int.Parse(Request.Params["id"].ToString().Trim());
            }
            catch (Exception e)
            {
                return Json(RespBean.fail("参数不能被解析或者参数不存在"), JsonRequestBehavior.AllowGet);
            }
            if (admin.DeleteUser(id))
            {
                return Json(RespBean.success("删除成功"), JsonRequestBehavior.AllowGet);
            }
            return Json(RespBean.fail("删除失败"), JsonRequestBehavior.AllowGet);
        }
    }
}
