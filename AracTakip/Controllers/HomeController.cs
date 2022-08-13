using AracTakip.App_Start;
using AracTakip.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;


namespace AracTakip.Controllers
{
   
    public class HomeController : Controller
    {                                                                                       
        private MUSTERIGIRISEntities db = new MUSTERIGIRISEntities();
        private MongoDBIcerık dbbaglam;
        private IMongoCollection<aracModel> aracCollection;
        public HomeController()
        {
            dbbaglam = new MongoDBIcerık();
            aracCollection = dbbaglam.database.GetCollection<aracModel>("denemearac");
            
        }
        [HttpPost]
        public ActionResult KonumlarıAl(string Hour , string gelenId)
        {
            int idsi = Convert.ToInt32(gelenId);
            if(Hour == null)
            {
                List<aracModel> aracss = aracCollection.AsQueryable<aracModel>().ToList();
                DateTime simdiki_zamann = DateTime.Now;
                DateTime sorgulanacak_zamann = simdiki_zamann.AddMinutes(-30);
                IEnumerable<aracModel> aracss_yeni = aracss.Where(c => c.aracId == idsi);
                IEnumerable<aracModel> filtrelemee =
                   from car in aracss_yeni
                   where ((Convert.ToDateTime(car.tarih).Hour > sorgulanacak_zamann.Hour) || (Convert.ToDateTime(car.tarih).Hour == sorgulanacak_zamann.Hour && Convert.ToDateTime(car.tarih).Minute > sorgulanacak_zamann.Minute)) && ((DateTime.Now.Hour > Convert.ToDateTime(car.tarih).Hour) || (DateTime.Now.Hour == Convert.ToDateTime(car.tarih).Hour && DateTime.Now.Minute >= Convert.ToDateTime(car.tarih).Minute))
                   select car;
                return Json(filtrelemee, JsonRequestBehavior.AllowGet);
            }
            else
            {
                int hour = Convert.ToInt32(Hour);
                List<aracModel> aracs = aracCollection.AsQueryable<aracModel>().ToList();
                DateTime simdiki_zaman = DateTime.Now;
                DateTime sorgulanacak_zaman = simdiki_zaman.AddHours(-(hour));
                IEnumerable<aracModel> aracs_yeni = aracs.Where(c => c.aracId == idsi);
                IEnumerable<aracModel> filtreleme =
                   from car in aracs_yeni
                   where ((Convert.ToDateTime(car.tarih).Hour > sorgulanacak_zaman.Hour) || (Convert.ToDateTime(car.tarih).Hour == sorgulanacak_zaman.Hour && Convert.ToDateTime(car.tarih).Minute > sorgulanacak_zaman.Minute)) && ((DateTime.Now.Hour > Convert.ToDateTime(car.tarih).Hour) || (DateTime.Now.Hour == Convert.ToDateTime(car.tarih).Hour && DateTime.Now.Minute >= Convert.ToDateTime(car.tarih).Minute))
                   select car;
                return Json(filtreleme, JsonRequestBehavior.AllowGet);
            }
         
        }

        public ActionResult Index()
        {
            if(MUSTERILERController.girenKullanıcıAd == "cemre")
            {
                List<string> aracIDleri = new List<string> { "595", "274", "444", "222" };
                ViewBag.aracIDleri = aracIDleri;
            }
            if (MUSTERILERController.girenKullanıcıAd == "safak")
            {
                List<string> aracIDleri = new List<string> { "886", "474", "244", "520" };
                ViewBag.aracIDleri = aracIDleri;
            }

            return View();
        }
       
     
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
       
    }
}