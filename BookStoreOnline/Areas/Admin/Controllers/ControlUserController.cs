using BookStoreOnline.Core;
using BookStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BookStoreOnline.Areas.Admin.Controllers
{
    public class ControlUserController : Controller
    {
        private NhaSachEntities1 db = new NhaSachEntities1();
        // GET: Admin/ControlUser
        public ActionResult Index()
        {
            var users = db.KHACHHANG.ToList();
            return View(users);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG khachHang = db.KHACHHANG.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: Admin/Nguoidungs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KHACHHANG nguoidung = db.KHACHHANG.Find(id);

            db.KHACHHANG.Remove(nguoidung);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ResetPassword(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG khachHang = db.KHACHHANG.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        [HttpPost, ActionName("ResetPassword")]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(int id)
        {
            KHACHHANG nguoidung = db.KHACHHANG.Find(id);

            nguoidung.MatKhau = "123456";
               
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DisableAccount(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG KHACHHANGs = db.KHACHHANG.Find(id);
            if (KHACHHANGs == null)
            {
                return HttpNotFound();
            }
            return View(KHACHHANGs);
        }

        [HttpPost, ActionName("DisableAccount")]
        [ValidateAntiForgeryToken]
        public ActionResult DisableAccountConfirmed(int id)
        {
            KHACHHANG khachHang = db.KHACHHANG.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }

            khachHang.TrangThai = false; 
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult EnableAccount(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KHACHHANG KHACHHANGs = db.KHACHHANG.Find(id);
            if (KHACHHANGs == null)
            {
                return HttpNotFound();
            }
            return View(KHACHHANGs);
        }

        [HttpPost, ActionName("EnableAccount")]
        [ValidateAntiForgeryToken]
        public ActionResult EnableAccount(int id)
        {
            KHACHHANG khachHang = db.KHACHHANG.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }

            khachHang.TrangThai = true;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}