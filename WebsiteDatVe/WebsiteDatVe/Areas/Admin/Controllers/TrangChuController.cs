﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDatVe.Models;

namespace WebsiteDatVe.Areas.Admin.Controllers
{
    public class TrangChuController : Controller
    {
        DatVeDB db = new DatVeDB();
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }

<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
        //Quản lý máy bay
        //GET: Admin/Plane
        public ActionResult Plane()
        {
            return View(db.MayBays);
        }
<<<<<<< Updated upstream
=======

        //Tạo mới máy bay
        //GET: Admin/CreatePlane
        [HttpGet]
        public ActionResult CreatePhane()
        {
            ViewBag.MaHangBay = new SelectList(db.HangBays.OrderBy(c => c.TenHangBay), "MaHangBay", "TenHangBay");
            return View();
        }
        [HttpPost]
        public JsonResult AddPhane(string MaMayBay, int? GhePhoThong, int? GheThuongGia, int? GheHangNhat, int? GheDatBiet, long? MaHangBay)
        {
            try
            {
                MayBay maybay = new MayBay();
                maybay.MaMayBay = MaMayBay;
                maybay.SoGhePhoThong = GhePhoThong;
                maybay.SoGheThuongGia = GheThuongGia;
                maybay.SoGheHangNhat = GheHangNhat;
                maybay.SoGhePhoThongDacBiet = GheDatBiet;
                maybay.MahangBay = MaHangBay;
                db.MayBays.Add(maybay);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Đăng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Sửa máy bay
        [HttpGet]
        public ActionResult EditPlane(string id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View();
            }
            MayBay maybay = db.MayBays.SingleOrDefault(c => c.MaMayBay == id);
            if (maybay == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaHangBay = new SelectList(db.HangBays.OrderBy(c => c.TenHangBay), "MaHangBay", "TenHangBay", maybay.MahangBay);
            return View(maybay);
        }
        [HttpPost]
        public JsonResult UpdatePhane(string MaMayBay, int? GhePhoThong, int? GheThuongGia, int? GheHangNhat, int? GheDatBiet, long? MaHangBay)
        {
            try
            {
                var maybay = (from c in db.MayBays where c.MaMayBay == MaMayBay select c).FirstOrDefault();
                maybay.MaMayBay = MaMayBay;
                maybay.SoGhePhoThong = GhePhoThong;
                maybay.SoGheThuongGia = GheThuongGia;
                maybay.SoGheHangNhat = GheHangNhat;
                maybay.SoGhePhoThongDacBiet = GheDatBiet;
                maybay.MahangBay = MaHangBay;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Đăng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Xóa máy bay
        public JsonResult DeletePlane(string mamaybay)
        {
            try
            {
                var maybay = (from c in db.MayBays where c.MaMayBay == mamaybay select c).FirstOrDefault();
                db.MayBays.Remove(maybay);
                db.SaveChanges();
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        //Quản lý hãng bay
        //GET: Admin/Flight
        public ActionResult Airline()
        {
            return View(db.HangBays);
        }

        //Tạo mới hãng bay
        //GET: Admin/CreateAirline
        [HttpGet]
        public ActionResult CreateAirline()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AddAirline(string TenHangBay, string Hinh)
        {
            try
            {
                HangBay hangBay = new HangBay();
                hangBay.TenHangBay = TenHangBay;
                if (Hinh != "NULL")
                {
                    hangBay.Logo = Hinh;
                }
                db.HangBays.Add(hangBay);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Đăng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Sửa hãng bay
        [ValidateInput(false)]
        [HttpGet]
        public ActionResult EditAirline(long? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View();
            }
            HangBay hb = db.HangBays.SingleOrDefault(c => c.MaHangBay == id);
            if (hb == null)
            {
                return HttpNotFound();
            }
            return View(hb);
        }
        [HttpPost]
        public JsonResult EditAirline(int mahangbay, string TenHangBay, string Hinh)
        {
            try
            {
                var hangBay = (from c in db.HangBays where c.MaHangBay == mahangbay select c).FirstOrDefault();

                hangBay.TenHangBay = TenHangBay;

                hangBay.Logo = Hinh;

                db.SaveChanges();
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Xóa Hãng bay
        [HttpPost]
        public JsonResult DeleteAirline(int mahangbay)
        {
            try
            {
                var hb = (from c in db.HangBays where c.MaHangBay == mahangbay select c).FirstOrDefault();
                db.HangBays.Remove(hb);
                db.SaveChanges();

                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public string XuLyFile(HttpPostedFileBase file)
        {
            string path = Server.MapPath("~/Images/" + file.FileName);
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                file.SaveAs(path);
            }
            catch { }

            return "/Images/" + file.FileName;

        }
        
>>>>>>> Stashed changes
    }
}