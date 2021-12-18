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
        public static string serectkey;
        public static string amount;
        public ActionResult Checkout(long id)
        {
            ThongTinDatVe thongtin = (ThongTinDatVe) Session["ThongTinDatVe"];
            ViewBag.DiemDi = db.SanBays.Where(x => x.MaSanBay.Equals(thongtin.DiemDi)).Select(x => x.TenSanBay).SingleOrDefault();
            ViewBag.DiemDen = db.SanBays.Where(x => x.MaSanBay.Equals(thongtin.DiemDen)).Select(x => x.TenSanBay).SingleOrDefault();
            ViewBag.SoLuong = thongtin.NguoiLon + thongtin.TreEm + thongtin.EmBe;

            double giave = (double)db.ChuyenBays.Where(x => x.MaChuyenBay == id).Select(x => x.Gia).SingleOrDefault();
            double giaHangVe = 1;
            if(thongtin.HangGhe.Trim() == "Phổ thông")
            {
                giaHangVe = giave;
            }
            else if (thongtin.HangGhe.Trim() == "Phổ thông đặc biệt")
            {
                giaHangVe = giave * 1.2;
            }
            else if(thongtin.HangGhe.Trim() == "Thương gia")
            {
                giaHangVe = giave * 1.4;
            }
            else if(thongtin.HangGhe.Trim() == "Hạng nhất")
            {
                giaHangVe = giave * 1.8;
            }

            ViewBag.GiaVeNguoiLon = (giaHangVe * thongtin.NguoiLon);
            ViewBag.GiaVeTreEm = giaHangVe * thongtin.TreEm;
            ViewBag.GiaVeEmBe = 110000 * thongtin.EmBe;
            double tongtam = giaHangVe * thongtin.NguoiLon + giaHangVe * thongtin.TreEm + 110000 * thongtin.EmBe;
            ViewBag.TongTamTinh = tongtam;
            double thue = (double)(tongtam * 0.1);
            ViewBag.Thue = thue;
            ViewBag.TongCong = tongtam + thue;

            ViewBag.MaChuyenBay = id;

            return View();
        }

        public JsonResult CreateTicket(string arr, long machuyenbay, double tongtien)
        {
            try
            {
                var listCus = new JavaScriptSerializer().Deserialize<List<KhachHang>>(arr);

                TaiKhoan taikhoan = (TaiKhoan) Session["TaiKhoan"];
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

                //Thêm khách hàng
                foreach(var item in listCus)
                {
                    KhachHang khachhang = new KhachHang();
                    khachhang.Ho = item.Ho;
                    khachhang.Ten = item.Ten;
                    khachhang.SDT = item.SDT;
                    khachhang.NgaySinh = item.NgaySinh;
                    khachhang.CMND = item.CMND;
                    khachhang.QuocTich = item.QuocTich;
                    khachhang.MaVe = ve.MaVe;
                    db.KhachHangs.Add(khachhang);
                    db.SaveChanges();
                }


                return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
            }catch(Exception e)
            {
                return Json(new { code = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Ticket()
        {
            return View();
        }

        public async Task ThanhToanMomo()
        {
            //request params need to request to MoMo system
            string endpoint = "https://payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMODDI520210624";
            string accessKey = "xryFOk958utQJR3T";
            //string serectkey = "art4TPJhFphYnpVLIDX9pIWKcXybGJw3";
            serectkey = "art4TPJhFphYnpVLIDX9pIWKcXybGJw3";
            string orderInfo = "Thanh toán vé máy bay";
            string returnUrl = "https://localhost:44387/Booking/returnUrl/";
            string notifyurl = "https://localhost:44387/Booking/notifyurl/";

            amount = "1985000";
            string orderid = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            log.Debug("rawHash = " + rawHash);

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);
            log.Debug("Signature = " + signature);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };
            //log.Debug("Json request to MoMo: " + message.ToString());
            var httpClient = new HttpClient();

            var httpContent = new StringContent(message.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync("https://test-payment.momo.vn/v2/gateway/api/create", httpContent);
            string a = await response.Content.ReadAsStringAsync();
            //  var json_serializer = new JavaScriptSerializer();
            //  object routes_list = json_serializer.DeserializeObject(a);
            JObject jmessage = JObject.Parse(a);
            System.Diagnostics.Process.Start(jmessage.GetValue("payUrl").ToString());
        }

        public ActionResult returnUrl()
        {
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            //log.Debug(param);
            param = Server.UrlDecode(param);
            MoMoSecurity crypto = new MoMoSecurity();
            //string serectkey = ConfigurationManager.AppSettings["serectkey"].ToString();
            string serectKey = serectkey.ToString();
            string signature = crypto.signSHA256(param, serectKey);
            if (signature != Request["signature"].ToString())
            {
                ViewBag.Message = "Thông tin không hợp lệ!";
                return View();
            }
            if (!Request.QueryString["errorCode"].Equals("0"))
            {
                ViewBag.Message = "Thanh toán thất bại!";
                return View();
            }
            else
            {
                //DateTime now = DateTime.Now;
                //DateTime ngayhethan = new DateTime();
                //if (amount == "1000")
                //{
                //    ngayhethan = now.AddDays(1);
                //}
                //else if (amount == "3000")
                //{
                //    ngayhethan = now.AddDays(3);
                //}
                //else if (amount == "5000")
                //{
                //    ngayhethan = now.AddDays(5);
                //}
                ViewBag.Message = "Thanh toán thành công!";
            }
            return View();
        }
        public JsonResult notifyurl(int id)
        {
            //Thanh cong
            //DateTime now = DateTime.Now;
            //DateTime ngayhethan = new DateTime();
            //if (amount == "1000")
            //{
            //    ngayhethan = now.AddDays(1);
            //}
            //else if (amount == "3000")
            //{
            //    ngayhethan = now.AddDays(3);
            //}
            //else if (amount == "5000")
            //{
            //    ngayhethan = now.AddDays(5);
            //}
            //QuangCao qc = new QuangCao();
            //qc.MaBaiDang = id;
            //qc.NgayHetHan = ngayhethan;
            //db.QuangCaos.Add(qc);
            //db.SaveChanges();
            return Json(new { code = 200 }, JsonRequestBehavior.AllowGet);
        }
    }
}