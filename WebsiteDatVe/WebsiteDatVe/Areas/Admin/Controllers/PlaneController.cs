using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDatVe.Models;

namespace WebsiteDatVe.Areas.Admin.Controllers
{
    public class PlaneController : Controller
    {
        DatVeDB db = new DatVeDB();
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View(db.MayBays.ToList());
        }

        //Quản lý máy bay
        //GET: Admin/Plane
        public ActionResult Plane()
        {
            return View(db.MayBays);
        }
    }
}