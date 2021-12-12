using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDatVe.Models;

namespace WebsiteDatVe.Areas.Admin.Controllers
{
    public class VeDatController : Controller
    {
        DatVeDB db = new DatVeDB();
        // GET: Admin/VeDat
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            var listVe = db.Ves.Where(v => v.TinhTrang.Equals("sold")).ToList();

            var data = (from v in listVe
                        join k in db.KhachHangs on v.MaKhachHang equals k.MaKhachHang
                        select new
                        {
                            MaVe = v.MaVe.ToString(),
                            MaChuyenBay = v.MaChuyenBay,
                            HangVe = v.HangVe.ToString(),
                            SoGhe = v.SoGhe,
                            TenKhachHang = k.Ten,
                            NgayDat = v.NgayDat.ToString(),
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}