using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDatVe.Models;

namespace WebsiteDatVe.Controllers
{
    public class HomeController : Controller
    {
        public DatVeDB db = new DatVeDB();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult getDiaDiem()
        {
            try
            {
                var lstDiaDiem = (from d in db.SanBays
                                 select new
                                 {
                                     MaSanBay = d.MaSanBay,
                                     NoiDung = d.TenSanBay + " (" + d.DiaChi + ")"
                                 }).ToList();

                return Json(new { code = 200, lstDiaDiem = lstDiaDiem }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(new { code = 500 , msg = e.Message}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}