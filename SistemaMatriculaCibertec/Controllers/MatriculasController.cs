using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaMatriculaCibertec.Models;

namespace SistemaMatriculaCibertec.Controllers
{
    public class MatriculasController : Controller
    {
        private SistemaMatriculaDBEntities db = new SistemaMatriculaDBEntities();

        // GET: Matriculas
        public ActionResult Index()
        {
            var matricula = db.Matricula.Include(m => m.Alumno).Include(m => m.Curso);
            return View(matricula.ToList());
        }

        // GET: Details
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Matricula matricula = db.Matricula.Find(id);

            if (matricula == null)
                return HttpNotFound();

            return View(matricula);
        }

        // GET: Create
        public ActionResult Create()
        {
            ViewBag.IdAlumno = new SelectList(db.Alumno, "IdAlumno", "CodigoAlumno");
            ViewBag.IdCurso = new SelectList(db.Curso, "IdCurso", "CodigoCurso");
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Matricula matricula)
        {
            try
            {
                if (matricula.IdAlumno == 0 || matricula.IdCurso == 0)
                {
                    TempData["Error"] = "Debe seleccionar alumno y curso";
                    return RedirectToAction("Create");
                }

                if (ModelState.IsValid)
                {
                    int ultimoId = db.Matricula.Any()
                        ? db.Matricula.Max(m => m.IdMatricula) + 1
                        : 1;

                    matricula.CodigoMatricula = "MAT-" + ultimoId.ToString("000");

                    db.Matricula.Add(matricula);
                    db.SaveChanges();

                    TempData["Success"] = "Matrícula registrada correctamente";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al registrar matrícula: " + ex.Message;
            }

            ViewBag.IdAlumno = new SelectList(db.Alumno, "IdAlumno", "CodigoAlumno", matricula.IdAlumno);
            ViewBag.IdCurso = new SelectList(db.Curso, "IdCurso", "CodigoCurso", matricula.IdCurso);

            return View(matricula);
        }

        // GET: Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Matricula matricula = db.Matricula.Find(id);

            if (matricula == null)
                return HttpNotFound();

            ViewBag.IdAlumno = new SelectList(db.Alumno, "IdAlumno", "CodigoAlumno", matricula.IdAlumno);
            ViewBag.IdCurso = new SelectList(db.Curso, "IdCurso", "CodigoCurso", matricula.IdCurso);

            return View(matricula);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Matricula matricula)
        {
            try
            {
                var matDB = db.Matricula.Find(matricula.IdMatricula);

                if (matDB == null)
                {
                    TempData["Error"] = "Matrícula no encontrada";
                    return RedirectToAction("Index");
                }

                if (matricula.IdAlumno == 0 || matricula.IdCurso == 0)
                {
                    TempData["Error"] = "Debe seleccionar alumno y curso";
                    return RedirectToAction("Index");
                }

                matDB.IdAlumno = matricula.IdAlumno;
                matDB.IdCurso = matricula.IdCurso;
                matDB.FechaMatricula = matricula.FechaMatricula;
                matDB.Estado = matricula.Estado;

                db.SaveChanges();

                TempData["Success"] = "Matrícula actualizada correctamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al actualizar: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // GET: Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Matricula matricula = db.Matricula.Find(id);

            if (matricula == null)
                return HttpNotFound();

            return View(matricula);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Matricula matricula = db.Matricula.Find(id);

                if (matricula == null)
                {
                    TempData["Error"] = "Matrícula no encontrada";
                    return RedirectToAction("Index");
                }

                // 🔥 VALIDACIÓN: evitar borrar si tiene retiro
                bool tieneRetiro = db.Retiro.Any(r => r.IdMatricula == id);

                if (tieneRetiro)
                {
                    TempData["Error"] = "No se puede eliminar: la matrícula tiene un retiro asociado";
                    return RedirectToAction("Index");
                }

                db.Matricula.Remove(matricula);
                db.SaveChanges();

                TempData["Success"] = "Matrícula eliminada correctamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar: " + ex.Message;
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