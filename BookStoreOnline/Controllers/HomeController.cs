using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStoreOnline.Models;
using System.Data.Entity; // Để sử dụng phương thức Include

namespace BookStoreOnline.Controllers
{
    public class HomeController : Controller
    {
        NhaSachEntities1 db = new NhaSachEntities1();

        public ActionResult Index()
        {
            // Lấy 8 sản phẩm đầu tiên cùng với các đánh giá của chúng
            var books = db.SANPHAM
                          .Include(b => b.DANHGIA) // Bao gồm các đánh giá
                          .Take(8) // Lấy 8 sản phẩm
                          .ToList();

            return View(books);
        }
    }
}