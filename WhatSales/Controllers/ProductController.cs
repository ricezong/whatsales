using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhatSales.Models;
using WhatSales.Services;
using WhatSales.Utils;

namespace WhatSales.Controllers
{
    public class ProductController : Controller
    {
        ProductService productService = new ProductService();

        public ActionResult Index()
        {
            User user = Session["User"] as User;
            return View(user);
        }

        public ActionResult FindCategoryList()
        {
            RespBean respBean = null;
            try
            {
                var list=productService.FindCategoryList();
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

        public ActionResult FindProductListByCatecoryId(int categoryId)
        {
            RespBean respBean = null;
            try
            {
                var list = productService.FindProductListByCatecoryId(categoryId);
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

        public ActionResult FindProductList()
        {
            RespBean respBean = null;
            try
            {
                var list = productService.FindProductList();
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

        public ActionResult FuzzyQuery(string Property)
        {
            RespBean respBean = null;
            try
            {
                var list = productService.FuzzyQuery(Property);
                if (list != null)
                {
                    respBean = RespBean.success("成功", list);
                }
                else
                    respBean = RespBean.fail("没有该商品");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Json(respBean, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHotProName()
        {
            RespBean respBean = null;
            try
            {
                var list = productService.GetHotProName();
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

        public ActionResult Detail()
        {
            User user = Session["User"] as User;
            return View(user);
        }

        public ActionResult ProductView()
        {
            return View();
        }

        public ActionResult AddProductView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct()
        {
            Product product = new Product();
            try
            {
                product.ProductName = Request.Params["ProductName"].ToString().Trim();
                product.Property = Request.Params["Property"].ToString().Trim();
                product.Active = int.Parse(Request.Params["active"].ToString().Trim());
                product.Price = float.Parse(Request.Params["Price"].ToString().Trim());
                product.CategoryId = int.Parse(Request.Params["categroyId"].ToString().Trim());
            }
            catch
            {
                return Json(RespBean.fail("您输入的数据不合法！"));
            }
            if (!productService.AddProduct(product))
            {
                return Json(RespBean.fail("添加失败!"));
            }
 
            return Json(RespBean.success("添加成功!"));
        }

        public ActionResult UpdateProductView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateProduct()
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
            Product product = new Product();
            try
            {
                product.ProductName = Request.Params["ProductName"].ToString().Trim();
                product.Property = Request.Params["Property"].ToString().Trim();
                product.Active = int.Parse(Request.Params["active"].ToString().Trim());
                product.Price = float.Parse(Request.Params["Price"].ToString().Trim());
            }
            catch
            {
                return Json(RespBean.fail("更新失败"), JsonRequestBehavior.DenyGet);
            }
            if (productService.UpdateProduct(id, product))
            {
                return Json(RespBean.success("更新成功"), JsonRequestBehavior.DenyGet);
            }
            return Json(RespBean.fail("更新失败"), JsonRequestBehavior.DenyGet);
        }

        public ActionResult FindOneProduct(int id)
        {
            
            Product product = productService.FindOneProduct(id);
            if (product == null)
            {
                return Json(RespBean.fail("查询失败"), JsonRequestBehavior.AllowGet);
            }
            return Json(RespBean.success("查询成功!", product), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteProduct()
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
            if (productService.DeleteProduct(id))
            {
                return Json(RespBean.success("删除成功"), JsonRequestBehavior.AllowGet);
            }
            return Json(RespBean.fail("删除失败"), JsonRequestBehavior.AllowGet);

        }
    }
}
