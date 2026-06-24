using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SistemaMatriculaCibertec.Models;

namespace SistemaMatriculaCibertec.Controllers
{
    public class RetiroesController : Controller
    {
        private SistemaMatriculaDBEntities db = new SistemaMatriculaDBEntities();

        // GET: Index
        public ActionResult Index()
        {
            var retiro = db.Retiro.Include(r => r.Matricula);
            return View(retiro.ToList());
        }

        // DETAILS
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Retiro retiro = db.Retiro.Find(id);

            if (retiro == null)
                return HttpNotFound();

            return View(retiro);
        }

        // CREATE GET
        public ActionResult Create()
        {
            ViewBag.IdMatricula = new SelectList(db.Matricula, "IdMatricula", "CodigoMatricula");
            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Retiro retiro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int ultimoId = db.Retiro.Any()
                        ? db.Retiro.Max(r => r.IdRetiro) + 1
                        : 1;

                    retiro.CodigoRetiro = "RET-" + ultimoId.ToString("000");

                    db.Retiro.Add(retiro);
                    db.SaveChanges();

                    TempData["Success"] = "Retiro registrado correctamente";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            ViewBag.IdMatricula = new SelectList(db.Matricula, "IdMatricula", "CodigoMatricula", retiro.IdMatricula);
            return View(retiro);
        }

        // EDIT GET
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Retiro retiro = db.Retiro.Find(id);

            if (retiro == null)
                return HttpNotFound();

            ViewBag.IdMatricula = new SelectList(db.Matricula, "IdMatricula", "CodigoMatricula", retiro.IdMatricula);
            return View(retiro);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Retiro retiro)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(retiro).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["Success"] = "Retiro actualizado correctamente";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            ViewBag.IdMatricula = new SelectList(db.Matricula, "IdMatricula", "CodigoMatricula", retiro.IdMatricula);
            return View(retiro);
        }

        // DELETE GET
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Retiro retiro = db.Retiro.Find(id);

            if (retiro == null)
                return HttpNotFound();

            return View(retiro);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Retiro retiro = db.Retiro.Find(id);

                db.Retiro.Remove(retiro);
                db.SaveChanges();

                TempData["Success"] = "Retiro eliminado correctamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}