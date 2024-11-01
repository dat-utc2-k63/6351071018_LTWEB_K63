using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class BookStoreController : Controller
    {
        // GET: BookStore

        private readonly dbQLBanSachDataContext data;

        public BookStoreController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["QLBANSACHConnectionString"].ConnectionString;
            data = new dbQLBanSachDataContext(connectionString);
        }
        public ActionResult Index()
        {
            var sachmoi = Laysachmoi(5);
            return View(sachmoi);
        }
        private List<SACH> Laysachmoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult Nhaxuatban()
        {
            var nhaxb = from cd in data.NHAXUATBANs select cd;
            return PartialView(nhaxb);
        }
        public ActionResult SPTheocd(int id)
        {
            var sach = from s in data.SACHes where s.MaCD==id select s;
            return View(sach);
        }
        public ActionResult SPTheoNXB(int id)
        {
            var sach = from s in data.SACHes where s.MaNXB == id select s;
            return View(sach);
        }
        public ActionResult Details(int id)
        {
            var sach = from s in data.SACHes where s.Masach == id select s;
            return View(sach.Single());
        }
    }
}