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
                if(user != null)
                {
                    thanhcong = true;
                    Session["TaiKhoan"] = user;
                }
                return Json(new { code = 200, user = user, thanhcong = thanhcong }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Đã lưu
        public ActionResult Saved()
        {
            return View();
        }

        //Lịch sử đặt vé
        public ActionResult MyBooking()
        {
            return View();
        }
    }
}