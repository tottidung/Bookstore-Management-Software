using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStoreOnline.Models;

namespace BookStoreOnline.Controllers
{
    public class CategoryController : Controller
    {
        NhaSachEntities1 db = new NhaSachEntities1();

        // GET: Category
        public ActionResult Index(int id)
        {
            ViewBag.CategoryName = db.LOAI.FirstOrDefault(n => n.Maloai == id).Tenloai;
            return View(db.SANPHAM.Where(book => book.MaLoai == id).ToList());
        }

        public ActionResult GetAllBook()
        {
            return View(db.SANPHAM.ToList());
        }

        public ActionResult Search(string inputString)
        {
            ViewBag.TextSeatch = inputString;
            var result = db.SANPHAM
                .Where(s => s.TenSanPham.Contains(inputString) || s.TacGia.Contains(inputString))
                .ToList();

            return View("Search", result); // Render the Search view with the result
        }
    }
}
