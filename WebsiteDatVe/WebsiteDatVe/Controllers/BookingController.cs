using MoMo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteDatVe.Controllers
{
    public class BookingController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string serectkey;
        public static string amount;
        public ActionResult Checkout(int id)
        {
            return View();
        }

        public ActionResult Ticket()
        {
            return View();
        }

        public void ThanhToanMomo()
        {
            //request params need to request to MoMo system
            string endpoint = "https://payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMODDI520210624";
            string accessKey = "xryFOk958utQJR3T";
            //string serectkey = "art4TPJhFphYnpVLIDX9pIWKcXybGJw3";
            serectkey = "art4TPJhFphYnpVLIDX9pIWKcXybGJw3";
            string orderInfo = "Thanh toán vé máy bay";
            string returnUrl = "https://localhost:44387/Home/returnUrl/";
            string notifyurl = "https://localhost:44387/Home/notifyurl";

            amount = "1985000" ;
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
            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);
            //log.Debug("Return from MoMo: " + jmessage.ToString());

            //yes...
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