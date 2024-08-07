using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStoreOnline.Areas.Admin.Constants;
using BookStoreOnline.Core;
using BookStoreOnline.Models;
using static BookStoreOnline.Areas.Admin.Constants.Constants;

namespace BookStoreOnline.Areas.Admin.Controllers
{
    public class AdminAccountsController : Controller
    {
        private NhaSachEntities1 db = new NhaSachEntities1();

        // GET: Admin/NHANVIENs
        public ActionResult Index()
        {
            return View(db.NHANVIEN.ToList());
        }

        // GET: Admin/NHANVIENs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nhanVien = db.NHANVIEN.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: Admin/NHANVIENs/Create
        public ActionResult Create()
        {
            var roles = from AdminRole e in Enum.GetValues(typeof(AdminRole))
                        select new { Id = (int)e, Name = e.GetDescription() };

            ViewBag.Role = new SelectList(roles, "Id", "Name");
            return View();
        }

        // POST: Admin/NHANVIENs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ten,Email,Quyen")] NHANVIEN nhanVienMoi)
        {
            if (ModelState.IsValid)
            {
                // Extract the part before the '@' symbol
                var emailParts = nhanVienMoi.Email.Split('@');
                if (emailParts.Length > 0)
                {
                    nhanVienMoi.MatKhau = emailParts[0]; // Set the default password to the part before the '@'
                }
                else
                {
                    nhanVienMoi.MatKhau = "defaultPassword"; // Fallback in case email is invalid, adjust as needed
                }

                nhanVienMoi.NgayTao = DateTime.Now;
                nhanVienMoi.TrangThai = true;
                db.NHANVIEN.Add(nhanVienMoi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var roles = from AdminRole e in Enum.GetValues(typeof(AdminRole))
                        select new { Id = (int)e, Name = e.GetDescription() };
            ViewBag.Role = new SelectList(roles, "Id", "Name");

            return View(nhanVienMoi);
        }

        // GET: Admin/NHANVIENs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nhanVien = db.NHANVIEN.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            var roles = from AdminRole e in Enum.GetValues(typeof(AdminRole))
                        select new { Id = (int)e, Name = e.GetDescription() };

            ViewBag.Role = new SelectList(roles, "Id", "Name");
            return View(nhanVien);
        }

        // POST: Admin/NHANVIENs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,Ten,Email,Quyen,TrangThai")] NHANVIEN nhanVien)
        {
            if (ModelState.IsValid)
            {
                NHANVIEN admAccount = db.NHANVIEN.Find(nhanVien.MaNV);
                admAccount.Ten = nhanVien.Ten;
                admAccount.Quyen = nhanVien.Quyen;
                admAccount.Email = nhanVien.Email;
                admAccount.TrangThai= nhanVien.TrangThai;
                db.Entry(admAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var roles = from AdminRole e in Enum.GetValues(typeof(AdminRole))
                        select new { Id = (int)e, Name = e.GetDescription() };
            ViewBag.Role = new SelectList(roles, "Id", "Name");

            return View(nhanVien);
        }

        // GET: Admin/NHANVIENs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nhanVien = db.NHANVIEN.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: Admin/NHANVIENs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Tìm khách hàng cần xóa
            KHACHHANG khachHang = db.KHACHHANG.Find(id);

            if (khachHang != null)
            {
                // Xóa các bản ghi liên quan trong DONHANG
                var donHangList = db.DONHANG.Where(dh => dh.ID == id).ToList();
                foreach (var donHang in donHangList)
                {
                    // Xóa chi tiết đơn hàng
                    var chiTietDonHangList = db.CHITIETDONHANG.Where(ct => ct.MaDonHang == donHang.MaDonHang).ToList();
                    foreach (var chiTiet in chiTietDonHangList)
                    {
                        db.CHITIETDONHANG.Remove(chiTiet);
                    }
                    db.DONHANG.Remove(donHang);
                }

                // Xóa khách hàng
                db.KHACHHANG.Remove(khachHang);
                db.SaveChanges();
            }

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

        public ActionResult DisableAccount(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nhanVien = db.NHANVIEN.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        [HttpPost, ActionName("DisableAccount")]
        [ValidateAntiForgeryToken]
        public ActionResult DisableAccountConfirmed(int id)
        {
            NHANVIEN nhanVien = db.NHANVIEN.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }

            nhanVien.TrangThai = false; // Assuming there is a property 'TrangThai' in the NHANVIEN model to indicate if the account is active or not
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
