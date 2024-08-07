using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStoreOnline.Models;

namespace BookStoreOnline.Models
{
    public class CartItem
    {
        NhaSachEntities1 db = new NhaSachEntities1();
        public int ProductID { get; set; }
        public string NamePro { get; set; }
        public string ImagePro { get; set; }
        public decimal Price { get; set; }
        public int Number { get; set; }


        public decimal FinalPrice()
        {
            return Number * Price;
        }
        public CartItem(int ProductID)
        {
            this.ProductID = ProductID;
            var productDB = db.SANPHAM.Single(s => s.MaSanPham == this.ProductID);
            this.NamePro = productDB.TenSanPham;
            this.ImagePro = productDB.Anh;
            this.Price = (decimal)productDB.Gia;
            this.Number = 1;
        }
    }
}