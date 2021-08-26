using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhatSales.Models;
using WhatSales.Utils;
using System.Data;
using System.Data.SqlClient;

namespace WhatSales.Services
{
    public class UserServices
    {
        public const int NUMLENGTH = 1;

        private bool CheckId(int id)
        {
            if (id != null || (id > 0 && id < int.MaxValue))
            {
                return true;
            }
            return false;
        }

        public User Login(string password)
        {
            string sql = "select * from ws_user where PassWord=@password";
            SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@password",password)
                };
            DataSet ds = DBHelper.executeQuery(sql,parms);
            if (ds != null && ds.Tables[0].Rows.Count == 1)
            {
                User user = new User();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    user.id = Convert.ToInt32(dr["id"]);
                    user.UserName = dr["UserName"].ToString();
                    user.Active = Convert.ToInt32(dr["Active"]);
                }
                return user;
            }
            else
            {
                return null;
            }
        }

        public User FindNameIsExist(string username)
        {
            string sql = "select * from ws_user where UserName=@username";
            SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@username",username),
                };
            DataSet ds = DBHelper.executeQuery(sql, parms);
            if (ds != null && ds.Tables[0].Rows.Count == 1)
            {
                User user = new User();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    user.Salt = dr["Salt"].ToString();
                }
                return user;
            }
            else
            {
                return null;
            }
        }

        public int SaveUserInfo(UserInfo info)
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@userId",info.UserId),
                new SqlParameter("@ip",info.Ip)
            };
            string sql = "insert into ws_userInfo (userId,Ip) values(@userId,@ip)";
            int result = DBHelper.executeNonQuery(sql,parms);
            return result;
        }

        public bool AddUser(User user)
        {
            user.Salt = Guid.NewGuid().ToString();
            user.PassWord = MD5Util.GetMD5(user.PassWord + user.Salt);
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@UserName",user.UserName),
                new SqlParameter("@PassWord",user.PassWord),
                new SqlParameter("@Role",user.Role.ToString()),
                new SqlParameter("@Salt",user.Salt),
                new SqlParameter("@Active",user.Active)
            };
            string sql = "insert into ws_user (UserName,PassWord,Active,Salt,Role) values(@UserName,@PassWord,@Active,@Salt,@Role)";

            return DBHelper.executeNonQuery(sql, parameters) > 0;
        }

        public List<User> FindAllUser()
        {
            string sql = "select id,UserName,Active,Salt,Role,CreateTime,UpdateTime,DeleteTime from ws_user";
            DataSet ds = DBHelper.executeQuery(sql);
            if (ds == null)
            {
                return null;
            }
            List<User> userList = new List<User>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                User user = new User();
                user.id = Convert.ToInt32(dr["id"].ToString());
                user.UserName = dr["UserName"].ToString();
                user.Active = Convert.ToInt32(dr["Active"].ToString());
                user.Role = (Role)Enum.Parse(typeof(Role), dr["Role"].ToString());
                user.Salt = dr["Salt"].ToString();
                DateTime time;
                if (DateTime.TryParse(dr["CreateTime"].ToString(), out time))
                {
                    user.CreateTime = time;
                }
                if (DateTime.TryParse(dr["UpdateTime"].ToString(), out time))
                {
                    user.UpdateTime = time;
                }
                if (DateTime.TryParse(dr["DeleteTime"].ToString(), out time))
                {
                    user.DeleteTime = time;
                }
                userList.Add(user);
            }
            return userList;
        }

        public bool UpdateUser(int id, User user)
        {
            if (!CheckId(id))
            {
                return false;
            }
            user.Salt = Guid.NewGuid().ToString();
            user.PassWord = MD5Util.GetMD5(user.PassWord + user.Salt);
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@id",id),
                new SqlParameter("@UserName",user.UserName),
                new SqlParameter("@PassWord",user.PassWord),
                new SqlParameter("@Role",user.Role.ToString()),
                new SqlParameter("@Salt",user.Salt),
                new SqlParameter("@Active",user.Active),
                new SqlParameter("@UpdateTime",DateTime.Now)
            };
            int result = DBHelper.executeNonQuery("update ws_user set UserName=@UserName,Role=@Role,Salt=@Salt,Active=@Active,UpdateTime=@UpdateTime where id=@id", parameters);
            //PassWord=@PassWord,
            return result > 0;
        }

        public bool DeleteUser(int id)
        {
            if (!CheckId(id))
            {
                return false;
            }
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@deleteTime",DateTime.Now),
                new SqlParameter("@id", id)
            };
            int result = DBHelper.executeNonQuery("update ws_user set DeleteTime = @deleteTime where id=@id", parameters);
            //SqlParameter parameter = new SqlParameter("@id", id);
            //int result = DBHelper.executeNonQuery("delete from ws_user where id=@id",parameter);

            return result > 0;
        }

        public User FindOne(int id)
        {
            if (!CheckId(id))
            {
                new Exception("所给id存在错误");
            }
            SqlParameter parameter = new SqlParameter("@id", id);
            DataSet ds = DBHelper.executeQuery("select id,UserName,PassWord,Active,Salt,Role,CreateTime,UpdateTime,DeleteTime from ws_user where id=@id", parameter);
            User user = new User();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                user.id = Convert.ToInt32(dr["id"].ToString());
                user.UserName = dr["UserName"].ToString();
                user.Active = Convert.ToInt32(dr["Active"].ToString());
                user.Role = (Role)Enum.Parse(typeof(Role), dr["Role"].ToString());
                user.Salt = dr["Salt"].ToString();
                DateTime time;
                if (DateTime.TryParse(dr["CreateTime"].ToString(), out time))
                {
                    user.CreateTime = time;
                }
                if (DateTime.TryParse(dr["UpdateTime"].ToString(), out time))
                {
                    user.UpdateTime = time;
                }
                if (DateTime.TryParse(dr["DeleteTime"].ToString(), out time))
                {
                    user.DeleteTime = time;
                }
            }

            return user;
        }

        public List<User> FindByName(string UserName)
        {
            SqlParameter[] parameter = new SqlParameter[]{
                    new SqlParameter("@UserName", UserName),
            };
            string sql = "select id from ws_user where UserName=@UserName";
            DataSet ds = DBHelper.executeQuery(sql, parameter);
            if (ds == null)
            {
                return null;
            }
            List<User> userList = new List<User>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                User user = new User();
                user.id = Convert.ToInt32(dr["id"].ToString());
                userList.Add(user);
            }

            return userList;
        }
    }
}