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

        public List<ChuyenBay> TimKiem(long diemdi, long diemden, int nguoilon, int treem, DateTime ngaydi, string hangghe)
        {
            //Tìm chuyến bay có ngày phù hợp
            List<ChuyenBay> flights = (from c
                          in db.ChuyenBays
                                       where c.DiemDi == diemdi && c.DiemDen == diemden
                                       && EntityFunctions.TruncateTime(c.ThoiGianDi) == EntityFunctions.TruncateTime(ngaydi) && c.ThoiGianDi > DateTime.Now
                                       select c).ToList();
            //Kiểm tra chuyến bay còn vé
            //Tạo list mới lưu số chuyến bay còn vé
            List<ChuyenBay> chuyenbays = new List<ChuyenBay>();

            foreach (var item in flights)
            {

                //Lấy ra số lượng ghế ban đầu của hạng ghế đang tìm
                int socho = (int)item.MayBay.SoGhePhoThong;

                if (hangghe == "Phổ thông đặc biệt")
                {
                    socho = (int)item.MayBay.SoGhePhoThongDacBiet;
                }
                if (hangghe == "Thương gia")
                {
                    socho = (int)item.MayBay.SoGheThuongGia;
                }
                if (hangghe == "Hạng nhất")
                {
                    socho = (int)item.MayBay.SoGheHangNhat;
                }

                //Lấy ra số vé đã được đặt của hạng ghế đang tìm
                int slve = (from v in db.Ves where v.MaChuyenBay == item.MaChuyenBay && v.HangVe == hangghe && v.TinhTrang != "Canceled" select v).Count();

                //Kiểm tra có đủ chỗ nếu đặt thêm không

                if (socho > (slve + nguoilon + treem))
                {
                    chuyenbays.Add(item);
                }

            }
            return chuyenbays;
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

            List<ChuyenBay> chuyenbays = TimKiem(diemdi, diemden, nguoilon, treem, ngaydi, hangghe);


            return View(chuyenbays);
        }

        

        public ActionResult Search2Way(long diemdi, long diemden, int nguoilon, int treem, int embe, DateTime ngaydi, DateTime ngayve, string hangghe)
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
            thongTinDatVe.NgayVe = ngayve;
            thongTinDatVe.HangGhe = hangghe;
            Session["ThongTinDatVe"] = thongTinDatVe;

            return View();
        }

        public JsonResult GetListChuyenBay()
        {
            try
            {
                ThongTinDatVe thongtindatve = (ThongTinDatVe)Session["ThongTinDatVe"];

                //lấy list chuyến bay đi
                List<ChuyenBay> lstdi = TimKiem(thongtindatve.DiemDi, thongtindatve.DiemDen, thongtindatve.NguoiLon, thongtindatve.TreEm, thongtindatve.NgayDi, thongtindatve.HangGhe);

                var diemdi = db.SanBays.Where(x => x.MaSanBay.Equals(thongtindatve.DiemDi)).Select(x => x.TenSanBay).SingleOrDefault();
                var diemden = db.SanBays.Where(x => x.MaSanBay.Equals(thongtindatve.DiemDen)).Select(x => x.TenSanBay).SingleOrDefault();

                var lst1 = (from v in lstdi
                            select new
                            {
                                MaChuyenBay = v.MaChuyenBay,
                                DiemDi = diemdi,
                                DiemDen = diemden,
                                ThoiGianDi = v.ThoiGianDi.GetValueOrDefault().ToString("HH: mm"),
                                ThoiGianDen = v.ThoiGianDen.GetValueOrDefault().ToString("HH: mm"),
                                HangBay = v.MayBay.HangBay.Logo,
                                TongThoiGian = v.ThoiGianDen.GetValueOrDefault().Subtract(v.ThoiGianDi.GetValueOrDefault()).TotalHours,
                                Gia = v.Gia.GetValueOrDefault().ToString("N0")
                            }).ToList();

                //Lấy list chuyến bay về
                List<ChuyenBay> lstve = TimKiem(thongtindatve.DiemDen, thongtindatve.DiemDi, thongtindatve.NguoiLon, thongtindatve.TreEm, thongtindatve.NgayVe, thongtindatve.HangGhe);
                var lst2 = (from v in lstve
                            select new
                            {
                                MaChuyenBay = v.MaChuyenBay,
                                DiemDi = diemden,
                                DiemDen = diemdi,
                                ThoiGianDi = v.ThoiGianDi.GetValueOrDefault().ToString("HH: mm"),
                                ThoiGianDen = v.ThoiGianDen.GetValueOrDefault().ToString("HH: mm"),
                                HangBay = v.MayBay.HangBay.Logo,
                                TongThoiGian = v.ThoiGianDen.GetValueOrDefault().Subtract(v.ThoiGianDi.GetValueOrDefault()).TotalHours,
                                Gia = v.Gia.GetValueOrDefault().ToString("N0")
                            }).ToList();

                return Json(new { code = 200, lstdi = lst1, lstve = lst2}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetChuyenBay(int machuyenbay)
        {
            //try
            //{

                List<ChuyenBay> cb = (from c in db.ChuyenBays
                                 where c.MaChuyenBay == machuyenbay
                                 select c).ToList();
                var chuyenbay = (from c in cb select new{
                                     MaChuyenBay = c.MaChuyenBay,
                                     DiemDi = c.DiemDen,
                                     DiemDen = c.DiemDi,
                                     ThoiGianDi = c.ThoiGianDi.GetValueOrDefault().ToString("HH: mm"),
                                     ThoiGianDen = c.ThoiGianDen.GetValueOrDefault().ToString("HH: mm"),
                                     HangBay = c.MayBay.HangBay.Logo,
                                     TongThoiGian = c.ThoiGianDen.GetValueOrDefault().Subtract(c.ThoiGianDi.GetValueOrDefault()).TotalHours+ " giờ",
                                     Gia = c.Gia
                                 }).FirstOrDefault();
                ThongTinDatVe thongtindatve = (ThongTinDatVe)Session["ThongTinDatVe"];
                var diemdi = db.SanBays.Where(x => x.MaSanBay.Equals(thongtindatve.DiemDi)).Select(x => x.TenSanBay).SingleOrDefault();
                var diemden = db.SanBays.Where(x => x.MaSanBay.Equals(thongtindatve.DiemDen)).Select(x => x.TenSanBay).SingleOrDefault();
                return Json(new { code = 200, chuyenbay = chuyenbay, diemdi = diemdi, diemden = diemden
                }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception e)
            //{
            //    return Json(new { code = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            //}
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