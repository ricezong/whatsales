using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WhatSales.Models;
using WhatSales.Utils;
using System.Data;
using Newtonsoft.Json;

namespace WhatSales.Services
{
    public class ProductService
    {
        public const int AFFECTEDMIXNUM = 0;

        public List<Category> FindCategoryList()
        {
            string sql = "select * from ws_category";
            DataSet ds = DBHelper.executeQuery(sql);
            if (ds != null && ds.Tables[0].Rows.Count >0)
            {
                List<Category> list = new List<Category>();
                Category category = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    category=new Category();
                    category.id = Convert.ToInt32(dr["id"]);
                    category.TypeName = dr["TypeName"].ToString();
                    list.Add(category);
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        public List<Product> FindProductListByCatecoryId(int categoryId)
        {
            string sql = "select * from ws_product where categoryId=@id";
            SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@id",categoryId)
                };
            DataSet ds = DBHelper.executeQuery(sql,parms);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                List<Product> list = new List<Product>();
                Product product = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    product = new Product();
                    product.id = Convert.ToInt32(dr["id"]);
                    product.ProductName = dr["ProductName"].ToString();
                    product.Price = Convert.ToDouble(dr["Price"]);
                    product.Property = dr["Property"].ToString();
                    product.CategoryId = Convert.ToInt32(dr["CategoryId"]);
                    product.Active = Convert.ToInt32(dr["Active"]);
                    list.Add(product);
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        public List<Product> FindProductList()
        {
            string sql = "select * from ws_product";
            DataSet ds = DBHelper.executeQuery(sql);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                List<Product> list = new List<Product>();
                Product product = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    product = new Product();
                    product.id = Convert.ToInt32(dr["id"]);
                    product.ProductName = dr["ProductName"].ToString();
                    product.Price = Convert.ToDouble(dr["Price"]);
                    product.Property = dr["Property"].ToString();
                    product.CategoryId = Convert.ToInt32(dr["CategoryId"]);
                    product.Active = Convert.ToInt32(dr["Active"]);
                    list.Add(product);
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        public List<Product> FuzzyQuery(string Property)
        {
            string sql = "SELECT * FROM ws_product CROSS APPLY OPENJSON (ws_product.Property) where value like @name";
            SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@name",String.Concat("%"+Property+"%"))
                };
            DataSet ds = DBHelper.executeQuery(sql,parms);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                List<Product> list = new List<Product>();
                Product product = null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    product = new Product();
                    product.id = Convert.ToInt32(dr["id"]);
                    product.ProductName = dr["ProductName"].ToString();
                    product.Price = Convert.ToDouble(dr["Price"]);
                    product.Property = dr["Property"].ToString();
                    product.CategoryId = Convert.ToInt32(dr["CategoryId"]);
                    product.Active = Convert.ToInt32(dr["Active"]);
                    list.Add(product);
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        public List<String> GetHotProName()
        {
            string sql = "select Top 7 Property from ws_product";
            DataSet ds = DBHelper.executeQuery(sql);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                List<String> list = new List<String>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(dr["Property"].ToString());
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        public bool AddProduct(Product product)
        {
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@productName",product.ProductName),
                new SqlParameter("@property",product.Property),
                new SqlParameter("@active",product.Active),
                new SqlParameter("@price",product.Price),
                new SqlParameter("@categoryId",product.CategoryId)
            };
            string sql = "insert into ws_product (ProductName,Property,Active,Price,CategoryId) values(@productName,@property,@active,@price,@categoryId)";

            return DBHelper.executeNonQuery(sql, parameters) > AFFECTEDMIXNUM;
        }

        public bool UpdateProduct(int id, Product product)
        {
            if (!MD5Util.CheckId(id))
            {
                return false;
            }
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@id",id),
                new SqlParameter("@ProductName",product.ProductName),
                new SqlParameter("@Property",product.Property),
                new SqlParameter("@Active",product.Active),
                new SqlParameter("@Price",product.Price),
            };
            string sql = "update ws_product set ProductName=@ProductName,Property=@Property,Active=@Active,Price=@Price where id=@id ";

            return DBHelper.executeNonQuery(sql, parameters) > AFFECTEDMIXNUM;
        }

        public bool DeleteProduct(int id)
        {
            if (!MD5Util.CheckId(id))
            {
                return false;
            }
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@id",id)
            };
            string sql = "delete from ws_product where id=@id";

            return DBHelper.executeNonQuery(sql, parameters) > AFFECTEDMIXNUM;
        }

        public Product FindOneProduct(int id)
        {
            if (!MD5Util.CheckId(id))
            {
                return null;
            }
             SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@id",id)
            };
            string sql = "select id,ProductName,Property,Active,Price,CategoryId from ws_product where id=@id";
            DataSet ds = DBHelper.executeQuery(sql, parameters);
            if (ds == null)
            {
                return null;
            }
            Product product = new Product();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                try
                {
                    product.id = int.Parse(dr["id"].ToString());
                    product.ProductName = dr["ProductName"].ToString();
                    product.Property = dr["Property"].ToString();
                    product.Active = int.Parse(dr["Active"].ToString());
                    product.Price = float.Parse(dr["Price"].ToString());
                }
                catch
                {
                    return null;
                }
            }
            return product;
        }
    }
}