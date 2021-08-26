using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WhatSales.Models;
using WhatSales.Utils;
using System.Transactions;

namespace WhatSales.Services
{
    public class OrderServices
    {

        public bool AddOrder(Order order,int userId)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@Price",order.Price),
                new SqlParameter("@UserId",userId)
                };
                int result = 0;
                try
                {
                     result = DBHelper.executeNonQuery("insert into ws_order (Price,User_id) values(@Price,@UserId)", parameters);
                     parameters = new SqlParameter[]{
                       new SqlParameter("@userId",userId)
                       };
                     string sql = "update ws_order_product set Order_id = SCOPE_IDENTITY() where Order_id=0 and [User_id] = @userId";
                     result = DBHelper.executeNonQuery(sql, parameters);
                }
                catch
                {
                    throw;
                }
                ts.Complete();
                return true;
            }
        }

        public List<Order> FindAllOrder()
        {
            List<Order> OrderList = new List<Order>();
            DataSet dt = DBHelper.executeQuery("select * from ws_order");
            foreach (DataRow dr in dt.Tables[0].Rows)
            {
                Order o = new Order();
                o.id = int.Parse(dr["id"].ToString());
                o.Price = double.Parse(dr["Price"].ToString());
                o.UserId = int.Parse(dr["User_id"].ToString());
                o.CreateTime = DateTime.Parse(dr["CreateTime"].ToString());
                OrderList.Add(o);
            }
            return OrderList;
        }

        public Order FindOneOrder(int id)
        {
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@id",id)
            };
            DataSet dt = DBHelper.executeQuery("select * from ws_order where id=@id",parameters);
            Order o = new Order();
            foreach (DataRow dr in dt.Tables[0].Rows)
            {
                o.id = int.Parse(dr["id"].ToString());
                o.Price = double.Parse(dr["Price"].ToString());
                o.UserId = int.Parse(dr["User_id"].ToString());
                o.CreateTime = DateTime.Parse(dr["CreateTime"].ToString());
            }
            return o;
        }

        public bool DeleteOrder(int id)
        {
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@id",id),
            };
            int result = DBHelper.executeNonQuery("delete from ws_order where id = @id;", parameters);
            
            return result > 0;
        }

        public List<OrderProductVo> FindProductByOrderId(int orderId,int userId)
        {
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@orderId",orderId),
                new SqlParameter("@userId",userId)
            };
            List<OrderProductVo> orderProductList = new List<OrderProductVo>();
            string sql = "select op.id,op.Count,op.Price,p.ProductName,p.Property,u.UserName from ws_order_product op "
                        + " left join ws_product p on op.Product_id=p.id "
                        + " left join ws_user u on op.[User_id] = u.id "
                        + " where Order_id = @orderId and [User_id] = @userId";
            DataSet dt = DBHelper.executeQuery(sql, parameters);
            foreach (DataRow dr in dt.Tables[0].Rows)
            {
                OrderProductVo orderProductVo = new OrderProductVo();
                orderProductVo.id = int.Parse(dr["id"].ToString());
                orderProductVo.Price = float.Parse(dr["Price"].ToString());
                orderProductVo.Count = int.Parse(dr["Count"].ToString());
                orderProductVo.ProductName = dr["ProductName"].ToString();
                orderProductVo.UserName = dr["UserName"].ToString();
                orderProductVo.Property = dr["Property"].ToString();
                orderProductList.Add(orderProductVo);
            }

            return orderProductList;
        }

        public bool UpdateOrderProductByOrderId(int orderId, int orderProductId)
        {
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@orderId",orderId),
                new SqlParameter("@orderProductId",orderProductId)
            };
            string sql = "update ws_order_product set Order_id = @orderId where id =@orderProductId ";
            int result = DBHelper.executeNonQuery(sql,parameters);

            return result > 0;
        }

        public List<Order> OrderQuery(string property)
        {
            string sql = "select o.* from ws_order o left join ws_order_product ops on o.id=ops.Order_id inner join ws_product p on p.id=ops.Product_id where p.Property like @name";
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@name",String.Concat("%"+property+"%"))
                };
            DataSet ds = DBHelper.executeQuery(sql, parms);
            if (ds == null)
            {
                return null;
            }
            List<Order> list = new List<Order>();
            Order order = null;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                order = new Order();
                order.id = Convert.ToInt32(dr["id"]);
                order.Price = Convert.ToDouble(dr["Price"]);
                list.Add(order);
            }
            return list;
        }

        public List<Cart> FindCartListByUserId(int userId)
        {
            string sql = "select p.ProductName,p.Price,ops.[Count],ops.id  from ws_product p left join ws_order_product ops on p.id=ops.Product_id where ops.User_id=@id";
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