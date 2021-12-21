using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDatVe.Models;

namespace WebsiteDatVe.Controllers
{
    public class UserController : Controller
    {

        private DatVeDB db = new DatVeDB();

        public ActionResult Login()
        {
            return PartialView();
        }

        //login
        public JsonResult CheckAccount(string email, string password)
        {
            try
            {
                Boolean thanhcong = false;
                TaiKhoan user = (from u in db.TaiKhoans where u.Email == email && u.MatKhau == password select u).FirstOrDefault();
                string quyen = "";
                if(user != null)
                {
                    thanhcong = true;
                    Session["TaiKhoan"] = user;
                    quyen = user.Quyen;
                }
                return Json(new { code = 200, user = user, thanhcong = thanhcong , quyen = quyen}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Đã lưu
        public ActionResult Saved()
        {
            List<VeDaLuu> list = db.VeDaLuus.ToList();
            return View(list);
        }

        public JsonResult SavedTicket(int id)
        {
            try
            {
                TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
                VeDaLuu ve = new VeDaLuu();
                ChuyenBay chuyen = (from v in db.ChuyenBays where v.MaChuyenBay == id select v).SingleOrDefault();
                ve.MaChuyenBay = id;
                ve.MaTaiKhoan = taikhoan.MaTaiKhoan;
                db.VeDaLuus.Add(ve);
                db.SaveChanges();
                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        //Lịch sử đặt vé
        public ActionResult MyBooking()
        {
            TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
            List<Ve> lstVe = (from v in db.Ves where v.MaTaiKhoan == taikhoan.MaTaiKhoan orderby v.NgayDat descending select v).ToList();
            return View(lstVe);
        }

        public ActionResult Logout()
        {
            Session["Taikhoan"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}