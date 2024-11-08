using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class GioHangController : BookStoreController 
    {
        // GET: Giohang
        public ActionResult Index()
        {
            return View();
        }
        public List<Giohang> Laygiohang()
        {
            List<Giohang> IsGiohang = Session["Giohang"] as List<Giohang>;
            if (IsGiohang == null)
            {
                IsGiohang = new List<Giohang>();
                Session["Giohang"] = IsGiohang;
            }
            return IsGiohang;
        }
        public ActionResult ThemGiohang(int iMasach, string strURL) { 
            List<Giohang> IsGiohang = Laygiohang();
            Giohang sanpham = IsGiohang.Find(n => n.iMasach == iMasach);
            if (sanpham == null) { 
                sanpham = new Giohang(iMasach);
                IsGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int iTongSoluong = 0;
            List<Giohang> IsGiohang = Session["Giohang"] as List<Giohang>;
            if (IsGiohang!= null)
            {
                iTongSoluong = IsGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoluong;
        }
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> IsGiohang = Session["Giohang"] as List<Giohang>;
            if (IsGiohang != null)
            {
                iTongTien = IsGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;

        }
        public ActionResult GioHang() { 
            List<Giohang> IsGiohang = Laygiohang();
            if(IsGiohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(IsGiohang);
        }
        public ActionResult Giohangpartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        public ActionResult XoaGiohang(int iMaSP)
        {
            List<Giohang> IsGiohang = Laygiohang();
            Giohang sanpham = IsGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            if (sanpham != null)
            {
                IsGiohang.Remove(sanpham);
            }
            if (IsGiohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            List<Giohang> IsGiohang = Laygiohang();
            Giohang sanpham = IsGiohang.SingleOrDefault(n => n.iMasach == iMaSP);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"]);
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaTatcaGiohang() {
            List<Giohang> IsGiohang = Laygiohang();
            IsGiohang.Clear();
            return RedirectToAction("Index", "BookStore");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "BookStore");
            }
            List<Giohang> IsGiohang = Laygiohang();
            ViewBag.Tongsouong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(IsGiohang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao= DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            foreach (var item in gh) { 
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.Masach = item.iMasach;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal) item.dDongia;
                data.CHITIETDONTHANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang() { return View(); }
    }
}