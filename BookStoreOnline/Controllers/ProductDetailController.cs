using System;
using System.Linq;
using System.Web.Mvc;
using BookStoreOnline.Models;

namespace BookStoreOnline.Controllers
{
    public class ProductDetailController : Controller
    {
        NhaSachEntities1 db = new NhaSachEntities1();

        // GET: ProductDetail
        public ActionResult Index(int id)
        {
            var product = db.SANPHAM.FirstOrDefault(p => p.MaSanPham == id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var moreBooks = db.SANPHAM.Where(p => p.MaSanPham != id).Take(3).ToList();
            var user = Session["TaiKhoan"] as KHACHHANG;

            ViewBag.Book = product;
            ViewBag.MoreBook = moreBooks;
            ViewBag.User = user;

            return View(product); // Pass the product model to the view
        }

        [HttpPost]
        public ActionResult AddReview(int ProductID, string NoiDung, int SoSao)
        {
            var user = Session["TaiKhoan"] as KHACHHANG;
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            var review = new DANHGIA
            {
                MaKH = user.MaKH,
                MaSanPham = ProductID,
                NoiDung = NoiDung,
                NgayTao = DateTime.Now,
                SoSao = SoSao
            };

            db.DANHGIA.Add(review);
            db.SaveChanges();

            return RedirectToAction("Index", new { id = ProductID });
        }
    }
}
