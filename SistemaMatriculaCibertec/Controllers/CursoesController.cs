using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SistemaMatriculaCibertec.Models;

namespace SistemaMatriculaCibertec.Controllers
{
    public class CursoesController : Controller
    {
        private SistemaMatriculaDBEntities db = new SistemaMatriculaDBEntities();

        public ActionResult Index()
        {
            return View(db.Curso.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Curso curso = db.Curso.Find(id);

            if (curso == null)
                return HttpNotFound();

            return View(curso);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Curso curso)
        {
            try
            {
                if (curso == null)
                    return View(curso);

                // VALIDACIONES
                if (string.IsNullOrWhiteSpace(curso.CodigoCurso))
                    ModelState.AddModelError("CodigoCurso", "El código es obligatorio");

                if (string.IsNullOrWhiteSpace(curso.NombreCurso))
                    ModelState.AddModelError("NombreCurso", "El nombre es obligatorio");

                if (curso.Creditos <= 0)
                    ModelState.AddModelError("Creditos", "Créditos deben ser mayores a 0");

                if (curso.Vacantes <= 0)
                    ModelState.AddModelError("Vacantes", "Vacantes deben ser mayores a 0");

                // DUPLICADO
                bool existe = db.Curso.Any(x => x.CodigoCurso == curso.CodigoCurso);
                if (existe)
                    ModelState.AddModelError("CodigoCurso", "Ya existe este código");

                if (ModelState.IsValid)
                {
                    db.Curso.Add(curso);
                    db.SaveChanges();

                    TempData["Success"] = "Curso registrado correctamente";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
            }

            return View(curso);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Curso curso = db.Curso.Find(id);

            if (curso == null)
                return HttpNotFound();

            return View(curso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Curso curso)
        {
            try
            {
                if (curso == null)
                    return View(curso);

                if (curso.Creditos <= 0)
                    ModelState.AddModelError("Creditos", "Créditos inválidos");

                if (curso.Vacantes <= 0)
                    ModelState.AddModelError("Vacantes", "Vacantes inválidas");

                bool existe = db.Curso.Any(x =>
                    x.CodigoCurso == curso.CodigoCurso &&
                    x.IdCurso != curso.IdCurso);

                if (existe)
                    ModelState.AddModelError("CodigoCurso", "Código ya usado");

                if (ModelState.IsValid)
                {
                    db.Entry(curso).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["Success"] = "Curso actualizado correctamente";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
            }

            return View(curso);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Curso curso = db.Curso.Find(id);

            if (curso == null)
                return HttpNotFound();

            return View(curso);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Curso curso = db.Curso.Find(id);

                db.Curso.Remove(curso);
                db.SaveChanges();

                TempData["Success"] = "Curso eliminado correctamente";
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