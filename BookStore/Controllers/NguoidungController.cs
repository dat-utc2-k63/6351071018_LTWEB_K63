using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class NguoidungController : Controller
    {
        private readonly dbQLBanSachDataContext db;
        public NguoidungController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["QLBANSACHConnectionString"].ConnectionString;
            db = new dbQLBanSachDataContext(connectionString);
        }
        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);

            bool isValid = true;

            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            else if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Tên đăng nhập không được để trống";
            }

            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Mật khẩu không được để trống";
            }
            else if (matkhau != matkhaunhaplai)
            {
                ViewData["Loi4"] = "Mật khẩu nhập lại không khớp";
            }
            else if (string.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được để trống";
            }
            else if (string.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Điện thoại không được để trống";
            }
            else if (string.IsNullOrEmpty(diachi))
            {
                ViewData["Loi7"] = "diachi không được để trống";
            }
            else
            {
                kh.HoTen = hoten;
                kh.Taikhoan = tendn;
                kh.Matkhau = matkhau;
                kh.Email = dienthoai;
                kh.DiachiKH = diachi;
                kh.DienthoaiKH = dienthoai;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();

                return RedirectToAction("Dangnhap");
            }

            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Giohang", "Giohang");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
    }
}