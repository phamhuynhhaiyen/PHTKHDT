using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDatVe.Models;

namespace WebsiteDatVe.Areas.Admin.Controllers
{
    public class VeHuyController : Controller
    {
        DatVeDB db = new DatVeDB();
        // GET: Admin/VeHuy
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            var listVe = db.Ves.Where(v => v.TinhTrang.Equals("request")).ToList();

            var data = (from v in listVe
                        join k in db.KhachHangs on v.MaKhachHang equals k.MaKhachHang
                        select new
                        {
                            MaVe = v.MaVe,
                            MaChuyenBay = v.MaChuyenBay,
                            HangVe = v.HangVe.ToString(),
                            SoGhe = v.SoGhe,
                            TenKhachHang = k.Ten,
                            NgayDat = v.NgayDat.ToString(),
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Accept(Ve veHuy)
        {
            try
            {
                var preData = db.Ves.Where(v => v.MaVe == veHuy.MaVe).FirstOrDefault();
                preData.TinhTrang = "sell";
                db.SaveChanges();

                return Json(new { log = "OK" }, JsonRequestBehavior.AllowGet);
            }catch(Exception e)
            {
                return Json(new { log = "ERR" }, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}