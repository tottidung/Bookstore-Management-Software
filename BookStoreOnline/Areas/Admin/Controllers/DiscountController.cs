using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BookStoreOnline.Models;
namespace BookStoreOnline.Areas.Admin.Controllers
{
    public class DiscountController : Controller
    {
        private NhaSachEntities1 db = new NhaSachEntities1();

        // GET: KhuyenMai
        public ActionResult Index()
        {
            return View(db.KHUYENMAI.ToList());
        }

        // GET: KhuyenMai/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHUYENMAI khuyenMai = db.KHUYENMAI.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMai);
        }

        // GET: KhuyenMai/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KhuyenMai/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TenChuongTrinh,MaKM,MoTa,NgayTao,NgayBatDau,NgayKetThuc,SoTienKM,SoTienMuaHangToiThieu,SoLanDung,KichHoat")] KHUYENMAI khuyenMai)
        {
            if (ModelState.IsValid)
            {
                db.KHUYENMAI.Add(khuyenMai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khuyenMai);
        }

        // GET: KhuyenMai/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHUYENMAI khuyenMai = db.KHUYENMAI.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMai);
        }

        // POST: KhuyenMai/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TenChuongTrinh,MaKM,MoTa,NgayTao,NgayBatDau,NgayKetThuc,SoTienKM,SoTienMuaHangToiThieu,SoLanDung,KichHoat")] KHUYENMAI khuyenMai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khuyenMai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khuyenMai);
        }

        // GET: KhuyenMai/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHUYENMAI khuyenMai = db.KHUYENMAI.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMai);
        }

        // POST: KhuyenMai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KHUYENMAI khuyenMai = db.KHUYENMAI.Find(id);
            db.KHUYENMAI.Remove(khuyenMai);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        public JsonResult ToggleActivation(int id, bool isActive)
        {
            try
            {
                var khuyenMai = db.KHUYENMAI.Find(id);
                if (khuyenMai == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy chương trình khuyến mãi." });
                }

                khuyenMai.KichHoat = isActive;
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception details (ex.Message) to a logging service or a file
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }
    }
}