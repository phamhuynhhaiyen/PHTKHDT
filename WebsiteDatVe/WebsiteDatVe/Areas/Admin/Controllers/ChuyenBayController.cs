﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDatVe.Models;

namespace WebsiteDatVe.Areas.Admin.Controllers
{
    public class ChuyenBayController : Controller
    {
        DatVeDB db = new DatVeDB();
        // GET: Admin/ChuyenBay
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            var listChuyenBay = db.ChuyenBays.ToList();
            var listSanBay = db.SanBays.ToList();

            var data = (from c in listChuyenBay
                        join a in listSanBay on c.DiemDi equals a.MaSanBay
                        join b in listSanBay on c.DiemDen equals b.MaSanBay
                        select new
                        {
                            MaChuyenBay = c.MaChuyenBay,
                            DiemDi = a.TenSanBay,
                            DiemDen = b.TenSanBay,
                            ThoiGianDi = c.ThoiGianDi.ToString(),
                            ThoiGianDen = c.ThoiGianDen.ToString(),
                            MaMayBay = c.MaMayBay,
                            Gia = c.Gia
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Add(ChuyenBay chuyenBay)
        {
            db.ChuyenBays.Add(chuyenBay);
            db.SaveChanges();
            return Json(new {result = 1, log = chuyenBay.Gia }, JsonRequestBehavior.AllowGet);
        }

        /*public JsonResult GetById(long Id)
        {
            var chuyenBay = db.ChuyenBays.Where(c => c.MaChuyenBay == Id).ToList();
            var listSanBay = db.SanBays.ToList();
            var result = (from c in chuyenBay
                          join a in listSanBay on c.DiemDi equals a.MaSanBay
                          join b in listSanBay on c.DiemDi equals b.MaSanBay
                          select new
                          {
                              MaChuyenBay = c.MaChuyenBay,
                              DiemDi = a.TenSanBay,
                              DiemDen = b.TenSanBay,
                              ThoiGianDi = c.ThoiGianDi.ToString(),
                              ThoiGianDen = c.ThoiGianDen.ToString(),
                              MaMayBay = c.MaMayBay,
                              Gia = c.Gia
                          }).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }*/
        
        public ActionResult getById(long Id)
        {
            var chuyenBay = db.ChuyenBays.Where(c => c.MaChuyenBay == Id).FirstOrDefault();
            return View(chuyenBay);
        }


        public JsonResult Update(ChuyenBay chuyenBay)
        {
            var preData = db.ChuyenBays.Where(c => c.MaChuyenBay == chuyenBay.MaChuyenBay).FirstOrDefault();
            preData.DiemDi = chuyenBay.DiemDi;
            preData.DiemDen = chuyenBay.DiemDen;
            preData.ThoiGianDi = chuyenBay.ThoiGianDi;
            preData.ThoiGianDen = chuyenBay.ThoiGianDen;
            preData.MaMayBay = chuyenBay.MaMayBay;
            preData.Gia = chuyenBay.Gia;

            return Json(db.SaveChanges(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(long Id)
        {
            var chuyenBay = db.ChuyenBays.Where(c => c.MaChuyenBay == Id).FirstOrDefault();
            db.ChuyenBays.Remove(chuyenBay);

            return Json(db.SaveChanges(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
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
            catch (Exception e)
            {
                return Json(new { code = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getMaMayBay()
        {
            try
            {
                var lstMayBay = (from d in db.MayBays
                                  join h in db.HangBays on d.MahangBay equals h.MaHangBay
                                 select new
                                  {
                                      MaMayBay = d.MaMayBay,
                                      NoiDung = d.MaMayBay + " (" + h.TenHangBay + ")"
                                  }).ToList();

                return Json(new { code = 200, lstMayBay = lstMayBay }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}