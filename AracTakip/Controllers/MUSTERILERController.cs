using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AracTakip.Models;
using MongoDB.Bson;
using MongoDB.Driver.Core;
using System.Configuration;
using AracTakip.App_Start;
using MongoDB.Driver;
using System.Data.SqlClient;


namespace AracTakip.Controllers
{
    public class MUSTERILERController : Controller
    {
        SqlConnection bagla = new SqlConnection(@"Data Source= (LocalDB)\MSSQLLocalDB;Initial Catalog=MUSTERIGIRIS;Integrated Security=True");
        public static string girenKullanıcıAd;
        private MUSTERIGIRISEntities db = new MUSTERIGIRISEntities();
        

        public ActionResult GIRIS()
        {
          
            return View();
        }
        public ActionResult CIKIS()
        {
           
            bagla.Open();
            string kayit = "insert into CIKIS_ZAMAN(ID,CIKIS_ZAMAN) values (@ID,@CIKIS_DATE)";
            SqlCommand komut = new SqlCommand(kayit, bagla);
            int a = Convert.ToInt32(Session["ID"]);
            komut.Parameters.AddWithValue("@ID", a);
            komut.Parameters.AddWithValue("@CIKIS_DATE", DateTime.Now);
            komut.ExecuteNonQuery();
            bagla.Close();
            Session["KULLANICI_ADI"] = null;
            return View();

        }
        [HttpPost]
        public ActionResult GIRIS(MUSTERILER model)
        {
            girenKullanıcıAd = model.KULLANICI_ADI;
            var MUSTERI = db.MUSTERILER.FirstOrDefault(x => x.KULLANICI_ADI == model.KULLANICI_ADI && x.SIFRE == model.SIFRE);
            if (MUSTERI != null)
            { 
                bagla.Open();
                string kayit = "insert into GIRIS_ZAMAN(ID,GIRIS_DATE) values (@ID,@GIRIS_DATE)";
                SqlCommand komut = new SqlCommand(kayit, bagla);
                komut.Parameters.AddWithValue("@ID", MUSTERI.ID);
                komut.Parameters.AddWithValue("@GIRIS_DATE", DateTime.Now);
                komut.ExecuteNonQuery();
                bagla.Close();
                Session["KULLANICI_ADI"] = MUSTERI;
                Session["ID"] = MUSTERI.ID;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.HATA = "KULLANICI ADI VEYA SİFRE YANLIŞ";
            return View();
        }
       
        public ActionResult Index()
        {
            return View(db.MUSTERILER.ToList());
        }

        // GET: MUSTERILER/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MUSTERILER mUSTERILER = db.MUSTERILER.Find(id);
            if (mUSTERILER == null)
            {
                return HttpNotFound();
            }
            return View(mUSTERILER);
        }

       
        public ActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,KULLANICI_ADI,SIFRE")] MUSTERILER mUSTERILER)
        {
            if (ModelState.IsValid)
            {
                db.MUSTERILER.Add(mUSTERILER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mUSTERILER);
        }

        // GET: MUSTERILER/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MUSTERILER mUSTERILER = db.MUSTERILER.Find(id);
            if (mUSTERILER == null)
            {
                return HttpNotFound();
            }
            return View(mUSTERILER);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,KULLANICI_ADI,SIFRE")] MUSTERILER mUSTERILER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mUSTERILER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mUSTERILER);
        }

   
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MUSTERILER mUSTERILER = db.MUSTERILER.Find(id);
            if (mUSTERILER == null)
            {
                return HttpNotFound();
            }
            return View(mUSTERILER);
        }

   
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MUSTERILER mUSTERILER = db.MUSTERILER.Find(id);
            db.MUSTERILER.Remove(mUSTERILER);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
