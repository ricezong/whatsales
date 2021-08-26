using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WhatSales.Utils
{
    public static class MD5Util
    {
        public static string GetMD5(string str)
        {
            MD5 md = MD5.Create();
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            byte[] MD5Buffer = md.ComputeHash(buffer);
            string strNew = "";
            for (int i = 0; i < MD5Buffer.Length; i++)
            {
                strNew += MD5Buffer[i].ToString("x");
            }
            return strNew;
        }

        public static bool CheckId(int id)
        {
            if (id != null || (id > 0 && id < int.MaxValue))
            {
                return true;
            }
            return false;
        }
    }
}