using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BookStoreOnline.Core
{
    public static class Extension
    { 
        public static string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Chuyển đổi chuỗi đầu vào thành mảng byte và tính toán mã MD5
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Tạo một StringBuilder để chứa các byte đã mã hóa
                StringBuilder sBuilder = new StringBuilder();

                // Lặp qua mỗi byte của mã MD5 và định dạng nó thành dạng hex
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }
    }
}