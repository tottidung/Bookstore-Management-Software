using System;
using System.Linq;
using System.Web.Mvc;
using BookStoreOnline.Models;

namespace BookStoreOnline.Controllers
{
    public class ReviewsController : Controller
    {
        private NhaSachEntities1 db = new NhaSachEntities1();

        // GET: Reviews/Create
        public ActionResult Create(int productId)
        {
            ViewBag.ProductId = productId;
            return View();
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MaKH,MaSanPham,NoiDung,NgayTao,SoSao")] DANHGIA review)
        {
            if (ModelState.IsValid)
            {
                review.NgayTao = DateTime.Now;
                db.DANHGIA.Add(review);
                db.SaveChanges();
                return RedirectToAction("Details", "Products", new { id = review.MaSanPham });
            }

            ViewBag.ProductId = review.MaSanPham;
            return View(review);
        }

        // GET: Reviews for a specific product
        public ActionResult ProductReviews(int productId)
        {
            var reviews = db.DANHGIA.Where(r => r.MaSanPham == productId).ToList();
            return PartialView("_ProductReviews", reviews);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
