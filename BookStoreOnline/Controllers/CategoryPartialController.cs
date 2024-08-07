using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStoreOnline.Models;

namespace BookStoreOnline.Controllers
{
    public class CategoryPartialController : Controller
    {
        NhaSachEntities1 db = new NhaSachEntities1();
        // GET: CategoryPartial
        public ActionResult GetPartialCategoryNav()
        {
            ViewBag.CateList = db.LOAI.ToList();
            return PartialView();
        }

        public ActionResult GetCategorySelection()
        {
            ViewBag.CateList = db.LOAI.ToList();
            return PartialView();
        }
    }
}