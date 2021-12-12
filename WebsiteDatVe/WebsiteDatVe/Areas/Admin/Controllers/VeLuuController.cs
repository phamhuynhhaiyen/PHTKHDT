using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDatVe.Models;

namespace WebsiteDatVe.Areas.Admin.Controllers
{
    public class VeLuuController : Controller
    {
        DatVeDB db = new DatVeDB();
        // GET: Admin/VeLuu
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            var listVe = db.Ves.Where(v => v.TinhTrang.Equals("save")).ToList();

            var data = (from v in listVe
                        join k in db.KhachHangs on v.MaKhachHang equals k.MaKhachHang
                        select new
                        {
                            MaVe = v.MaVe.ToString(),
                            MaChuyenBay = v.MaChuyenBay,
                            HangVe = v.HangVe.ToString(),
                            SoGhe = v.SoGhe,
                            TenKhachHang = k.Ten
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchByMaChuyenBay(long? searchKey)
        {
            var resultSearch = db.Ves.Where(v => (v.MaChuyenBay == searchKey)
            && v.TinhTrang.Equals("save") || (searchKey == null) && v.TinhTrang.Equals("save")).ToList();
            var listVe = (from v in resultSearch
                          join k in db.KhachHangs on v.MaKhachHang equals k.MaKhachHang
                          select new
                          {
                              MaVe = v.MaVe.ToString(),
                              MaChuyenBay = v.MaChuyenBay,
                              HangVe = v.HangVe.ToString(),
                              SoGhe = v.SoGhe,
                              TenKhachHang = k.Ten
                          }).ToList();

            return Json(listVe, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchByHangVe(string searchKey)
        {
            var resultSearch = db.Ves.Where(v => (v.HangVe == searchKey)
            && v.TinhTrang.Equals("save") || (searchKey == null)
                && v.TinhTrang.Equals("save")).ToList();
            var listVe = (from v in resultSearch
                          join k in db.KhachHangs on v.MaKhachHang equals k.MaKhachHang
                          select new
                          {
                              MaVe = v.MaVe.ToString(),
                              MaChuyenBay = v.MaChuyenBay,
                              HangVe = v.HangVe.ToString(),
                              SoGhe = v.SoGhe,
                              TenKhachHang = k.Ten
                          }).ToList();

            return Json(listVe, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchByTenKH(string searchKey)
        {
            var listVeLuu = db.Ves.Where(v => v.TinhTrang.Equals("save")).ToList();
            var listKH = db.KhachHangs.Where(k => k.Ten.StartsWith(searchKey)
            || searchKey == "").ToList();
            var listVe = (from v in listVeLuu
                          join k in listKH on v.MaKhachHang equals k.MaKhachHang
                          select new
                          {
                              MaVe = v.MaVe.ToString(),
                              MaChuyenBay = v.MaChuyenBay,
                              HangVe = v.HangVe.ToString(),
                              SoGhe = v.SoGhe,
                              TenKhachHang = k.Ten
                          }).ToList();

            return Json(listVe, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchByMaVe(string searchKey)
        {
            var resultSearch = db.Ves.Where(v => (v.MaVe == searchKey)
            && v.TinhTrang.Equals("save") || (searchKey == null)
                && v.TinhTrang.Equals("save")).ToList();
            var listVe = (from v in resultSearch
                          join k in db.KhachHangs on v.MaKhachHang equals k.MaKhachHang
                          select new
                          {
                              MaVe = v.MaVe.ToString(),
                              MaChuyenBay = v.MaChuyenBay,
                              HangVe = v.HangVe.ToString(),
                              SoGhe = v.SoGhe,
                              TenKhachHang = k.Ten
                          }).ToList();

            return Json(listVe, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchBySoGhe(string searchKey)
        {
            var resultSearch = db.Ves.Where(v => (v.SoGhe == searchKey)
            && v.TinhTrang.Equals("save") || (searchKey == null)
                && v.TinhTrang.Equals("save")).ToList();
            var listVe = (from v in resultSearch
                          join k in db.KhachHangs on v.MaKhachHang equals k.MaKhachHang
                          select new
                          {
                              MaVe = v.MaVe.ToString(),
                              MaChuyenBay = v.MaChuyenBay,
                              HangVe = v.HangVe.ToString(),
                              SoGhe = v.SoGhe,
                              TenKhachHang = k.Ten
                          }).ToList();

            return Json(listVe, JsonRequestBehavior.AllowGet);
        }

        /*public JsonResult SearchByNgayDat(string searchKey)
        {
            DateTime dateToSearch = DateTime.Parse(searchKey);
            var resultSearch = db.Ves.Where(v => (v.NgayDat.Value.Day == dateToSearch.Day)
            && (v.NgayDat.Value.Month == dateToSearch.Month)
            && (v.NgayDat.Value.Year == dateToSearch.Year)
            && v.TinhTrang.Equals("save") || (searchKey == null)
                && v.TinhTrang.Equals("save")).ToList();
            var listVe = (from v in resultSearch
                          join k in db.KhachHangs on v.MaKhachHang equals k.MaKhachHang
                          select new
                          {
                              MaVe = v.MaVe.ToString(),
                              MaChuyenBay = v.MaChuyenBay,
                              HangVe = v.HangVe.ToString(),
                              SoGhe = v.SoGhe,
                              TenKhachHang = k.Ten
                          }).ToList();

            return Json(listVe, JsonRequestBehavior.AllowGet);
        }*/
    }
}