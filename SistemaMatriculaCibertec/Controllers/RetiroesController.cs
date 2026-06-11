using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaMatriculaCibertec.Models;

namespace SistemaMatriculaCibertec.Controllers
{
    public class RetiroesController : Controller
    {
        private SistemaMatriculaDBEntities db = new SistemaMatriculaDBEntities();

        // GET: Retiroes
        public ActionResult Index()
        {
            var retiro = db.Retiro.Include(r => r.Matricula);
            return View(retiro.ToList());
        }

        // GET: Retiroes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Retiro retiro = db.Retiro.Find(id);
            if (retiro == null)
            {
                return HttpNotFound();
            }
            return View(retiro);
        }

        // GET: Retiroes/Create
        public ActionResult Create()
        {
            ViewBag.IdMatricula = new SelectList(db.Matricula, "IdMatricula", "CodigoMatricula");
            return View();
        }

        // POST: Retiroes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdRetiro,IdMatricula,FechaRetiro,Motivo")] Retiro retiro)
        {
            if (ModelState.IsValid)
            {
                db.Retiro.Add(retiro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdMatricula = new SelectList(db.Matricula, "IdMatricula", "CodigoMatricula", retiro.IdMatricula);
            return View(retiro);
        }

        // GET: Retiroes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Retiro retiro = db.Retiro.Find(id);
            if (retiro == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdMatricula = new SelectList(db.Matricula, "IdMatricula", "CodigoMatricula", retiro.IdMatricula);
            return View(retiro);
        }

        // POST: Retiroes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdRetiro,IdMatricula,FechaRetiro,Motivo")] Retiro retiro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(retiro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdMatricula = new SelectList(db.Matricula, "IdMatricula", "CodigoMatricula", retiro.IdMatricula);
            return View(retiro);
        }

        // GET: Retiroes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Retiro retiro = db.Retiro.Find(id);
            if (retiro == null)
            {
                return HttpNotFound();
            }
            return View(retiro);
        }

        // POST: Retiroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Retiro retiro = db.Retiro.Find(id);
            db.Retiro.Remove(retiro);
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
