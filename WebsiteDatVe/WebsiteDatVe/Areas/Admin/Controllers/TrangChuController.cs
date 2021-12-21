using System;
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
            int nam = DateTime.Now.Year;
            ViewBag.tonghangbay = slhangbay();
            ViewBag.tongchuyenbay = slchuyenbay();
            ViewBag.tongkhachhang = slkh();
            ViewBag.tongve = slve();
            ViewBag.tongsanbay = slsb();
            ViewBag.T1 = tongVE(1, nam);
            ViewBag.T2 = tongVE(2, nam);
            ViewBag.T3 = tongVE(3, nam);
            ViewBag.T4 = tongVE(4, nam);

            ViewBag.T5 = tongVE(5, nam);
            ViewBag.T6 = tongVE(6, nam);
            ViewBag.T7 = tongVE(7, nam);
            ViewBag.T8 = tongVE(8, nam);
            ViewBag.T9 = tongVE(9, nam);
            ViewBag.T10 = tongVE(10, nam);
            ViewBag.T11 = tongVE(11, nam);
            ViewBag.T12 = tongVE(12, nam);

            ViewBag.tongtien1 = tongdoanhthuthang(1,nam);
            ViewBag.tongtien2 = tongdoanhthuthang(2,nam);
            ViewBag.tongtien3 = tongdoanhthuthang(3,nam);
            ViewBag.tongtien4 = tongdoanhthuthang(4,nam);
            ViewBag.tongtien5 = tongdoanhthuthang(5,nam);
            ViewBag.tongtien6 = tongdoanhthuthang(6,nam);
            ViewBag.tongtien7 = tongdoanhthuthang(7,nam);
            ViewBag.tongtien8 = tongdoanhthuthang(8,nam);
            ViewBag.tongtien9 = tongdoanhthuthang(9,nam);
            ViewBag.tongtien10 = tongdoanhthuthang(10,nam);
            ViewBag.tongtien11 = tongdoanhthuthang(11,nam);
            ViewBag.tongtien12 = tongdoanhthuthang(12,nam);
            return View();
        }

        public int slhangbay()
        {
            int tonghang = db.HangBays.Count();
            ViewBag.tonghangbay = tonghang;
            return tonghang;

        }
        public int slchuyenbay()
        {
            int tongchuyen = db.ChuyenBays.Count();
            ViewBag.tongchuyenbay = tongchuyen;
            return tongchuyen;

        }
        public int slkh()
        {
            int tongkh = db.KhachHangs.Count();
            ViewBag.tongkhachhang = tongkh;
            return tongkh;

        }
        public int slve()
        {
            int tongve = db.Ves.Count();
            ViewBag.tongve = tongve;
            return tongve;

        }
        public int slsb()
        {
            int tongsb = db.SanBays.Count();
            ViewBag.tongsanbay = tongsb;
            return tongsb;

        }
        public int tongVE(int thang, int nam)
        {

            int result = db.Ves.Where(n => n.NgayDat.Value.Month == thang && n.NgayDat.Value.Year == nam).Count();


            return result;
        }

        //Doanh thu
        public float tongdoanhthuthang(int thang, int nam)
        {
            var lst = (db.Ves.Where(n => n.NgayDat.Value.Month == thang && n.NgayDat.Value.Year == nam));
            if(lst.Count() > 0)
            {
                var tongtien = lst.Sum(c => c.TongTien).GetValueOrDefault().ToString();
                return float.Parse(tongtien);
            }
            return 0;
        }
    }
}