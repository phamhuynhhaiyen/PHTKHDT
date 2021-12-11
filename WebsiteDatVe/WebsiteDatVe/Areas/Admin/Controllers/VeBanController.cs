using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteDatVe.Models;

namespace WebsiteDatVe.Areas.Admin.Controllers
{
    public class VeBanController : Controller
    {
        DatVeDB db = new DatVeDB();
        //Giao diện vé bán chỉ nên cho admin có quyền xem và tạo
        //ra vé cho một chuyến bay nào đó, admin không nên có quyền sửa
        //bởi vì những thay đổi trên vé chỉ nằm ở thuộc tính Mã Khách hàng,
        //Tình trạng và Ngày đặt, Mã KH và ngày đặt sẽ đc update khi khách hàng
        //thực hiện đặt vé, còn tình trạng vé cũng sẽ thay đổi sau khi khách hàng
        //đã đặt ( từ sell -> sold), nếu người dùng yêu cầu huỷ vé ( trong thời gian cho phép)
        //thì ta thực hiện update lại trạng thái của vé trong giao diện khác của admin
        //(có thể là quản lý huỷ vé)
        // GET: Admin/VeBan
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            var listVe = db.Ves.Where(v => v.TinhTrang.Equals("sell")).ToList();

            var data = (from v in listVe
                        select new
                        {
                            MaVe = v.MaVe.ToString(),
                            MaChuyenBay = v.MaChuyenBay,
                            HangVe = v.HangVe.ToString()
                        }).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        public JsonResult getMaChuyenBay()
        {
           /* try
            {*/
                var lstChuyenBay = (from c in db.ChuyenBays
                                 join s1 in db.SanBays on c.DiemDi equals s1.MaSanBay
                                 join s2 in db.SanBays on c.DiemDen equals s2.MaSanBay
                                 join m in db.MayBays on c.MaMayBay equals m.MaMayBay
                                 join h in db.HangBays on m.MahangBay equals h.MaHangBay
                                 select new
                                 {
                                     MaChuyenBay = c.MaChuyenBay,
                                     NoiDung = c.MaChuyenBay + " (" + s1.TenSanBay + " đến " + s2.TenSanBay + ") bởi "
                                     + m.MaMayBay + " của " + h.TenHangBay
                                 }).ToList();

                return Json(new { code = 200, listChuyenBay = lstChuyenBay }, JsonRequestBehavior.AllowGet);
            /*}
            catch (Exception e)
            {
                return Json(new { code = 500, msg = e.Message }, JsonRequestBehavior.AllowGet);
            }*/
        }

        public JsonResult Add(Ve veBan)
        {
            //Dựa vào mã chuyến bay, tìm được mã máy bay của chuyến bay
            //Sau khi có được mã máy bay thì ta có thể tìm được
            //số lượng ghế theo hạng của loại máy bay đó và tự động
            //generate ra vé tương ứng theo từng hạng vé

            var maChuyenBay = veBan.MaChuyenBay;
            //Tìm thử xem trong CSDL đã có vé đã tạo cho chuyến bay này chưa
            //nếu có rồi thì ko cần tạo nữa
            var listVeCheck = db.Ves.Where(v => v.MaChuyenBay == maChuyenBay).ToList();
            if(listVeCheck.Count > 0)
            {
                return Json(new { log = "ERR" }, JsonRequestBehavior.AllowGet);
            }
            var maMayBay = db.ChuyenBays.Where(c => c.MaChuyenBay == maChuyenBay).FirstOrDefault().MaMayBay;
            var soGhePhoThong = db.MayBays.Where(m => m.MaMayBay == maMayBay)
                .FirstOrDefault().SoGhePhoThong.GetValueOrDefault(0);
            var soGheThuongGia = db.MayBays.Where(m => m.MaMayBay == maMayBay)
                .FirstOrDefault().SoGheThuongGia.GetValueOrDefault(0);
            var soGheHangNhat = db.MayBays.Where(m => m.MaMayBay == maMayBay)
                .FirstOrDefault().SoGheHangNhat.GetValueOrDefault(0);
            var soGhePhoThongDatBiet = db.MayBays.Where(m => m.MaMayBay == maMayBay)
                .FirstOrDefault().SoGhePhoThongDacBiet.GetValueOrDefault(0);
            var listVePhoThong = new List<Ve>();
            var listVeThuongGia = new List<Ve>();
            var listVeHangNhat = new List<Ve>();
            var listVePhoThongDatBiet = new List<Ve>();
            if (soGhePhoThong != 0)
            {
                for(int i = 0; i < soGhePhoThong; i++)
                {
                    Ve vePhoThong = new Ve()
                    {
                        //Mã vé: Chữ cái đầu tiên của hạng vé + số thứ tự + mã máy bay + mã chuyến bay
                        MaVe = "E" + i + "" + maMayBay + "" + maChuyenBay,
                        MaChuyenBay = maChuyenBay,
                        HangVe = "Economy",
                        //Số ghế: Chữ cái đầu tiên của hạng vé + số thứ tự
                        SoGhe = "E" + i,
                        MaKhachHang = null,
                        TinhTrang = "sell",
                        NgayDat = null,
                    };

                    db.Ves.Add(vePhoThong);
                }
            }
            if (soGheHangNhat != 0)
            {
                for (int i = 0; i < soGheHangNhat; i++)
                {
                    Ve veHangNhat = new Ve()
                    {
                        //Mã vé: Chữ cái đầu tiên của hạng vé + số thứ tự + mã máy bay + mã chuyến bay
                        MaVe = "F" + i + "" + maMayBay + "" + maChuyenBay,
                        MaChuyenBay = maChuyenBay,
                        HangVe = "First",
                        //Số ghế: Chữ cái đầu tiên của hạng vé + số thứ tự
                        SoGhe = "F" + i,
                        MaKhachHang = null,
                        TinhTrang = "sell",
                        NgayDat = null,
                    };

                    db.Ves.Add(veHangNhat);
                }
            }
            if (soGheThuongGia != 0)
            {
                for (int i = 0; i < soGheThuongGia; i++)
                {
                    Ve veThuongGia = new Ve()
                    {
                        //Mã vé: Chữ cái đầu tiên của hạng vé + số thứ tự + mã máy bay + mã chuyến bay
                        MaVe = "B" + i + "" + maMayBay + "" + maChuyenBay,
                        MaChuyenBay = maChuyenBay,
                        HangVe = "Business",
                        //Số ghế: Chữ cái đầu tiên của hạng vé + số thứ tự
                        SoGhe = "B" + i,
                        MaKhachHang = null,
                        TinhTrang = "sell",
                        NgayDat = null,
                    };

                    db.Ves.Add(veThuongGia);
                }
            }
            if (soGhePhoThongDatBiet != 0)
            {
                for (int i = 0; i < soGhePhoThongDatBiet; i++)
                {
                    Ve vePhoThongDB = new Ve()
                    {
                        //Mã vé: Chữ cái đầu tiên của hạng vé + số thứ tự + mã máy bay + mã chuyến bay
                        MaVe = "P" + i + "" + maMayBay + "" + maChuyenBay,
                        MaChuyenBay = maChuyenBay,
                        HangVe = "Premium",
                        //Số ghế: Chữ cái đầu tiên của hạng vé + số thứ tự
                        SoGhe = "P" + i,
                        MaKhachHang = null,
                        TinhTrang = "sell",
                        NgayDat = null,
                    };

                    db.Ves.Add(vePhoThongDB);
                }
            }

            db.SaveChanges();
            return Json(new { log = "OK" }, JsonRequestBehavior.AllowGet);
        }
    }
}