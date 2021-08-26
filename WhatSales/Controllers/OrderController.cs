using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhatSales.Models;
using WhatSales.Services;
using WhatSales.Utils;

namespace WhatSales.Controllers
{
    //[MyActionFilterAttribute]
    public class OrderController : Controller
    {
        private OrderServices orderServices = new OrderServices();
        RespBean respBean = null;

        private bool CheckId(int param)
        {
            if (param >= 0 && param < int.MaxValue)
            {
                return true;
            }
            return false;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FindAllOrder()
        {
            List<Order> OrderList = orderServices.FindAllOrder();
            if (OrderList != null)
            {
                respBean = RespBean.success("成功", OrderList);
            }
            else
            {
                respBean = RespBean.fail("失败");
            }

            return Json(respBean, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindOneOrder(int id)
        {
            if (!CheckId(id))
            {
                return Json(RespBean.fail("参数不合法"), JsonRequestBehavior.AllowGet);
            }
            Order order = orderServices.FindOneOrder(id);
            if (order == null)
            {
                return Json(RespBean.fail("查询失败"), JsonRequestBehavior.AllowGet);
            }
            return Json(RespBean.success("查询成功!", order), JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderDetail()
        {
            return View();
        }

        public ActionResult AddOrder(float Price)
        {
            Order order = new Order();
            User user = null;
            if (Price < 0 && Price > float.MaxValue)
            {
                return Json(RespBean.fail("您输入的参数不合法！"));
            }
            try
            {
                order.Price = Price;
                user = Session["User"] as User;
            }
            catch
            {
                return Json(RespBean.fail("您未登录"));
            }
            if (!orderServices.AddOrder(order, user.id))
            {
                return Json(RespBean.fail("添加失败!"));
            }

            return Json(RespBean.success("添加成功!"));
        }

        public ActionResult DeleteOrder(int id)
        {
            if (!CheckId(id))
            {
                return Json(RespBean.fail("参数不能被解析或者参数不存在"), JsonRequestBehavior.AllowGet);
            }
            if (orderServices.DeleteOrder(id))
            {
                return Json(RespBean.success("删除成功"), JsonRequestBehavior.AllowGet);
            }
            return Json(RespBean.fail("删除失败"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindOneOrderProduct(int OrderId)
        {
            if (!CheckId(OrderId))
            {
                return Json(RespBean.fail("参数不合法"), JsonRequestBehavior.AllowGet);
            }
            User user = Session["User"] as User;
            if (user == null)
            {
                return Json(RespBean.fail("请先登录"), JsonRequestBehavior.AllowGet);
            }
            List<OrderProductVo> orderProductVoList = orderServices.FindProductByOrderId(OrderId, 4);
            if (orderProductVoList == null)
            {
                return Json(RespBean.fail("查询失败"), JsonRequestBehavior.AllowGet);
            }
            return Json(RespBean.success("查询成功!", orderProductVoList), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindOneOrderByKey(string key)
        {
            if (key == null || key.Trim() == "")
            {
                return Json(RespBean.fail("参数不合法"), JsonRequestBehavior.AllowGet);
            }
            List<Order> orderList = orderServices.OrderQuery("重量");
            if (orderList == null)
            {
                return Json(RespBean.fail("查询失败"), JsonRequestBehavior.AllowGet);
            }

            return Json(RespBean.success("查询成功!", orderList), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Cart()
        {
            User user = Session["User"] as User;
            return View(user);
        }

        public ActionResult FindCartListByUserId()
        {
            User user = Session["User"] as User;
            RespBean respBean = null;
            try
            {
                var list = orderServices.FindCartListByUserId(user.id);
                if (list != null)
                {
                    respBean = RespBean.success("成功", list);
                }
                else
                    respBean = RespBean.fail("失败");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Json(respBean, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddToCart()
        {
            RespBean respBean = null;
            User user = Session["User"] as User;
            CartVo cartVo = new CartVo();
            cartVo.ProductId = Convert.ToInt32(Request.Params["ProductId"].Trim().ToString());
            cartVo.Price = Convert.ToDouble(Request.Params["Price"].Trim().ToString());
            cartVo.OrderId = 0;
            cartVo.UserId = user.id;
            CartVo vo = orderServices.FindCartVoByProductId(cartVo.ProductId);
            if (vo != null)
            {
                cartVo.Count = vo.Count + 1;
                var result = orderServices.UpdateCart(cartVo);
                if (result > 0)
                {
                    respBean = RespBean.success("添加成功");
                }
                else
                    respBean = RespBean.fail("添加失败");
            }
            else
            {
                cartVo.Count = 1;
                var result = orderServices.AddToCart(cartVo);
                if (result > 0)
                {
                    respBean = RespBean.success("添加成功");
                }
                else
                    respBean = RespBean.fail("添加失败");
            }
            return Json(respBean, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DelCartById(int opuId)
        {
            RespBean respBean = null;
            var result = orderServices.DelCartById(opuId);
            if (result > 0)
            {
                respBean = RespBean.success("删除成功");
            }
            else
                respBean = RespBean.fail("删除失败");

            return Json(respBean, JsonRequestBehavior.AllowGet);
        }

    }
}
