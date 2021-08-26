using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using WhatSales.Models;
using WhatSales.Utils;
using System.Data;

namespace WhatSales.Services
{
    public class OrderService
    {
        public List<Cart> FindCartListByUserId(int userId)
        {
            string sql = "select p.ProductName,p.Price,ops.[Count],ops.id  from ws_product p left join ws_order_product_user ops on p.id=ops.Product_id where ops.User_id=@id";
            SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@id",userId)
                };
            DataSet ds = DBHelper.executeQuery(sql, parms);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                List<Cart> list = new List<Cart>();
                Cart cart = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cart = new Cart();
                    cart.id = Convert.ToInt32(dr["id"]);
                    cart.Count = Convert.ToInt32(dr["Count"]);
                    cart.ProductName = dr["ProductName"].ToString();
                    cart.Price = Convert.ToDouble(dr["Price"]);
                    list.Add(cart);
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        public int AddToCart(CartVo cartVo)
        {
            string sql = "insert into ws_order_product(Order_id,Product_id,Price,[Count],User_id) values(@OrderId,@ProductId,@Price,@Count,@UserId)";
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@OrderId",cartVo.OrderId),
                new SqlParameter("@ProductId",cartVo.ProductId),
                new SqlParameter("@Price",cartVo.Price),
                new SqlParameter("@Count",cartVo.Count),
                new SqlParameter("@UserId",cartVo.UserId)
            };
            return DBHelper.executeNonQuery(sql, parms); 
        }

        public int UpdateCart(CartVo cartVo)
        {
            string sql = "update ws_order_product set Count=@Count where Product_id=@ProductId";
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@Count",cartVo.Count),
                new SqlParameter("@ProductId",cartVo.ProductId)
            };
            return DBHelper.executeNonQuery(sql, parms);
        }

        public CartVo FindCartVoByProductId(int id)
        {
            string sql = "select Count from ws_order_product ops where ops.Product_id=@id";
            SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@id",id)
                };
            DataSet ds = DBHelper.executeQuery(sql, parms);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                CartVo cartVo = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cartVo = new CartVo();
                    cartVo.Count = Convert.ToInt32(dr["Count"]);
                }
                return cartVo;
            }
            else
            {
                return null;
            }
        }

        public int DelCartById(int id)
        {
            string sql = "delete from ws_order_product where id=@id";
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@id",id),
            };
            return DBHelper.executeNonQuery(sql, parms);
        }
    }
}