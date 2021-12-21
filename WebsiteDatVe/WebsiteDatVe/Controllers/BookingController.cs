using MoMo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebsiteDatVe.Models;

namespace WebsiteDatVe.Controllers
{
    public class BookingController : Controller
    {
        private DatVeDB db = new DatVeDB();


        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Checkout(long id, double giave, long? idKhuHoi, double giaveKhuHoi)
        {
            ThongTinDatVe thongtin = (ThongTinDatVe) Session["ThongTinDatVe"];
            ViewBag.DiemDi = db.SanBays.Where(x => x.MaSanBay.Equals(thongtin.DiemDi)).Select(x => x.TenSanBay).SingleOrDefault();
            ViewBag.DiemDen = db.SanBays.Where(x => x.MaSanBay.Equals(thongtin.DiemDen)).Select(x => x.TenSanBay).SingleOrDefault();
            ViewBag.SoLuong = thongtin.NguoiLon + thongtin.TreEm + thongtin.EmBe;

            //double giave = (double)db.ChuyenBays.Where(x => x.MaChuyenBay == id).Select(x => x.Gia).SingleOrDefault();
            double giaHangVe = 1;
            double giavetong = giave + giaveKhuHoi;

            if(thongtin.HangGhe.Trim() == "Phổ thông")
            {
                giaHangVe = giavetong;
            }
            else if (thongtin.HangGhe.Trim() == "Phổ thông đặc biệt")
            {
                giaHangVe = giavetong * 1.2;
            }
            else if(thongtin.HangGhe.Trim() == "Thương gia")
            {
                giaHangVe = giavetong * 1.4;
            }
            else if(thongtin.HangGhe.Trim() == "Hạng nhất")
            {
                giaHangVe = giavetong * 1.8;
            }

            ViewBag.GiaVeNguoiLon = (giaHangVe * thongtin.NguoiLon);
            ViewBag.GiaVeTreEm = giaHangVe*0.75 * thongtin.TreEm;
            ViewBag.GiaVeEmBe = 110000 * thongtin.EmBe;
            double tongtam = giaHangVe * thongtin.NguoiLon + giaHangVe*0.75 * thongtin.TreEm + 110000 * thongtin.EmBe;
            ViewBag.TongTamTinh = tongtam;
            double thue = (double)(tongtam * 0.1);
            ViewBag.Thue = thue;
            ViewBag.TongCong = tongtam + thue;

            ViewBag.MaChuyenBay1 = id;
            ViewBag.MaChuyenBay2 = idKhuHoi;

            Session["TongTien1"] = TinhGiaVe(giave);
            Session["TongTien2"] = TinhGiaVe(giaveKhuHoi);

            return View();
        }

        public double TinhGiaVe(double gia)
        {
            ThongTinDatVe thongtin = (ThongTinDatVe)Session["ThongTinDatVe"];
            double giaHangVe = 1;
            if (thongtin.HangGhe.Trim() == "Phổ thông")
            {
                giaHangVe = gia;
            }
            else if (thongtin.HangGhe.Trim() == "Phổ thông đặc biệt")
            {
                giaHangVe = gia * 1.2;
            }
            else if (thongtin.HangGhe.Trim() == "Thương gia")
            {
                giaHangVe = gia * 1.4;
            }
            else if (thongtin.HangGhe.Trim() == "Hạng nhất")
            {
                giaHangVe = gia * 1.8;
            }
            double tongtien = giaHangVe * thongtin.NguoiLon + giaHangVe * 0.75 * thongtin.TreEm + 110000 * thongtin.EmBe;
            return tongtien + tongtien * 0.1;
        }

        public Ve CreateTicket(string arr, long? machuyenbay, double tongtien, string nguoidat)
        {
                var listCus = new JavaScriptSerializer().Deserialize<List<KhachHang>>(arr);
                var obj = new JavaScriptSerializer().Deserialize<NguoiDatVe>(nguoidat);

                TaiKhoan taikhoan = (TaiKhoan)Session["TaiKhoan"];
                ThongTinDatVe thongtin = (ThongTinDatVe)Session["ThongTinDatVe"];



                //Đặt vé
                Ve ve = new Ve();
                ve.MaVe = taikhoan.MaTaiKhoan + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                ve.MaChuyenBay = machuyenbay;
                ve.HangVe = thongtin.HangGhe;
                ve.SoLuongGhe = thongtin.NguoiLon + thongtin.TreEm;
                ve.MaTaiKhoan = taikhoan.MaTaiKhoan;
                ve.TinhTrang = "Processing";
                ve.NgayDat = DateTime.Now;
                ve.TongTien = tongtien;
                db.Ves.Add(ve);
                db.SaveChanges();

                //Tạo sesion 
                //Session["TongTien"] = tongtien;
                //Session["MaVe"] = ve.MaVe;

                //Thêm khách hàng
                foreach (var item in listCus)
                {
                    KhachHang khachhang = new KhachHang();
                    khachhang.Ho = item.Ho;
                    khachhang.Ten = item.Ten;
                    khachhang.SDT = item.SDT;
                    khachhang.NgaySinh = item.NgaySinh;
                    khachhang.CMND = item.CMND;
                    khachhang.QuocTich = item.QuocTich;
                    khachhang.MaVe = ve.MaVe;
                    khachhang.LoaiKhachHang = item.LoaiKhachHang;
                    db.KhachHangs.Add(khachhang);
                    db.SaveChanges();
                }

                //Thêm người đặt
                NguoiDatVe nguoiDatVe = new NguoiDatVe();
                nguoiDatVe.Ho = obj.Ho;
                nguoiDatVe.Ten = obj.Ten;
                nguoiDatVe.SDT = obj.SDT;
                nguoiDatVe.Email = obj.Email;
                nguoiDatVe.MaVe = ve.MaVe;
                db.NguoiDatVes.Add(nguoiDatVe);
                db.SaveChanges();


                return ve;
        }

        public JsonResult TaoVe(string arr, long machuyenbay1, long? machuyenbay2, string nguoidat)
        {
            try {
                double tongtien1 = (double) Session["TongTien1"];
                double tongtien2 = (double)Session["TongTien2"];

                Ve ve1 = CreateTicket(arr, machuyenbay1, tongtien1, nguoidat);
                Session["MaVe1"] = ve1.MaVe;

                if(machuyenbay2 != null)
                {
                    Ve ve2 = CreateTicket(arr, machuyenbay2, tongtien1, nguoidat);
                    Session["MaVe2"] = ve2.MaVe;
                }

                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Ticket(string id)
        {
            Ve ve = (from v in db.Ves where v.MaVe == id select v).SingleOrDefault();

            //Sân bay
            ViewBag.DiemDi = db.SanBays.Where(x => x.MaSanBay == ve.ChuyenBay.DiemDi).Select(x => x.TenSanBay).SingleOrDefault();
            ViewBag.DiemDen = db.SanBays.Where(x => x.MaSanBay == ve.ChuyenBay.DiemDen ).Select(x => x.TenSanBay).SingleOrDefault();

            return View(ve);
        }

        
        public async Task<ActionResult> ThanhToanMomo()
        {
            double tongtien = (double)Session["TongTien1"] + (double)Session["TongTien2"];

            string partnerCode = "MOMODDI520210624";
            string accessKey = "xc5ROj205YDvqrBn";
            string serectkey = "sSFqWXSq4QXW9MSr5SDAaGNBjL7FCrIk";
            string orderId = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";
            string orderInfo = "THANH TOÁN VÉ MÁY BAY";
            string amount = tongtien+"";
            string redirectUrl = "http://localhost:55480/Booking/returnUrl";
            string ipnUrl = "http://localhost:55480/Booking/notifyurl";
            string requestType = "captureWallet";
            //Before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + amount +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + redirectUrl +
                "&requestId=" + requestId +
                "&requestType=" + requestType
                ;


            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);


            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "Test" },
                { "storeId", "MomoTestStore" },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "en" },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }

            };
            var httpClient = new HttpClient();

            var httpContent = new StringContent(message.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync("https://test-payment.momo.vn/v2/gateway/api/create", httpContent);
            string a = await response.Content.ReadAsStringAsync();
            JObject jmessage = JObject.Parse(a);
            return Redirect(jmessage.GetValue("payUrl").ToString());
        }

        public ActionResult returnUrl()
        {
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);

            //Thành công

            string mave1 = Session["MaVe1"].ToString();
            
            if (Request.QueryString["message"].Equals("Successful."))
            {
                ViewBag.Message = "Đặt vé thành công! Vui lòng vào lịch sử đặt vé để kiểm tra";
                //Thay đổi trạng thái vé
                Ve ve1 = (from v in db.Ves where v.MaVe == mave1 select v).FirstOrDefault();
                ve1.TinhTrang = "Paid";
                db.SaveChanges();

                if (Session["MaVe2"] != null)
                {
                    string mave2 = Session["MaVe2"].ToString();
                    Ve ve2 = (from v in db.Ves where v.MaVe == mave2 select v).FirstOrDefault();
                    ve2.TinhTrang = "Paid";
                    db.SaveChanges();
                }
            }
            else
            {
                ViewBag.Message = "Thanh toán thất bại!";
                //Thay đổi trạng thái vé
                Ve ve1 = (from v in db.Ves where v.MaVe == mave1 select v).FirstOrDefault();
                ve1.TinhTrang = "Canceled";
                db.SaveChanges();
                if (Session["MaVe2"] != null)
                {
                    string mave2 = Session["MaVe2"].ToString();
                    Ve ve2 = (from v in db.Ves where v.MaVe == mave2 select v).FirstOrDefault();
                    ve2.TinhTrang = "Canceled";
                    db.SaveChanges();
                }
                return View();
                
            }
            return View();
        }
        public JsonResult notifyurl()
        {
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult getKhachHang(string mave)
        {
            try
            {
                var lstKhachHang = (from k in db.KhachHangs where k.MaVe == mave select new { 
                    HoTen = k.Ho + k.Ten,
                    LoaiKhachHang = k.LoaiKhachHang
                }).ToList();

                return Json(new { code = 200, lst = lstKhachHang }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(new { code = 500 }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult HuyVe(string mave)
        {
            try
            {
                Ve ve = (from v in db.Ves where v.MaVe == mave select v).SingleOrDefault();
                ve.TinhTrang = "request";
                db.SaveChanges();

                return Json(new { code = 200}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500 }, JsonRequestBehavior.AllowGet);
            }
        }
        
    }
}