using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDatVe.Models;

namespace WebsiteDatVe.Controllers
{
    public class HomeController : Controller
    {
        private DatVeDB db = new DatVeDB();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(long diemdi, long diemden, int nguoilon, int treem, int embe, DateTime ngaydi, string hangghe)
        {
            ViewBag.DiemDi = db.SanBays.Where(x => x.MaSanBay.Equals(diemdi)).Select(x => x.TenSanBay).SingleOrDefault();
            ViewBag.DiemDen = db.SanBays.Where(x => x.MaSanBay.Equals(diemden)).Select(x => x.TenSanBay).SingleOrDefault();
            ViewBag.SoLuong = nguoilon + treem + embe;

            ThongTinDatVe thongTinDatVe = new ThongTinDatVe();
            thongTinDatVe.DiemDen = diemden;
            thongTinDatVe.DiemDi = diemdi;
            thongTinDatVe.NguoiLon = nguoilon;
            thongTinDatVe.TreEm = treem;
            thongTinDatVe.EmBe = embe;
            thongTinDatVe.NgayDi = ngaydi;
            thongTinDatVe.HangGhe = hangghe;
            Session["ThongTinDatVe"] = thongTinDatVe;
            //search
            List<ChuyenBay> flights = (from c
                          in db.ChuyenBays
                           where c.DiemDi == diemdi && c.DiemDen == diemden
                           && EntityFunctions.TruncateTime(c.ThoiGianDi) == EntityFunctions.TruncateTime(ngaydi)
                           select c).ToList();
            //Kiểm tra vé trống
            return View(flights);
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