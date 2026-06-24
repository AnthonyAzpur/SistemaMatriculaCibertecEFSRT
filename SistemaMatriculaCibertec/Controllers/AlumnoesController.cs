using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SistemaMatriculaCibertec.Models;

namespace SistemaMatriculaCibertec.Controllers
{
    public class AlumnoesController : Controller
    {
        private SistemaMatriculaDBEntities db = new SistemaMatriculaDBEntities();

        // INDEX
        public ActionResult Index()
        {
            return View(db.Alumno.ToList());
        }

        // DETAILS
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Alumno alumno = db.Alumno.Find(id);

            if (alumno == null)
                return HttpNotFound();

            return View(alumno);
        }

        // CREATE GET
        public ActionResult Create()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Alumno alumno)
        {
            try
            {
                if (alumno == null)
                    return View(alumno);

                // VALIDACIONES
                if (string.IsNullOrWhiteSpace(alumno.CodigoAlumno))
                    ModelState.AddModelError("CodigoAlumno", "Código obligatorio");

                if (string.IsNullOrWhiteSpace(alumno.Nombres))
                    ModelState.AddModelError("Nombres", "Nombres obligatorios");

                if (string.IsNullOrWhiteSpace(alumno.Apellidos))
                    ModelState.AddModelError("Apellidos", "Apellidos obligatorios");

                if (string.IsNullOrWhiteSpace(alumno.DNI))
                    ModelState.AddModelError("DNI", "DNI obligatorio");

                if (alumno.DNI != null && alumno.DNI.Length != 8)
                    ModelState.AddModelError("DNI", "El DNI debe tener 8 dígitos");

                if (string.IsNullOrWhiteSpace(alumno.Correo))
                    ModelState.AddModelError("Correo", "Correo obligatorio");

                // DUPLICADO DNI
                bool existe = db.Alumno.Any(a => a.DNI == alumno.DNI);
                if (existe)
                    ModelState.AddModelError("DNI", "Ya existe un alumno con este DNI");

                if (ModelState.IsValid)
                {
                    db.Alumno.Add(alumno);
                    db.SaveChanges();

                    TempData["Success"] = "Alumno registrado correctamente";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            return View(alumno);
        }

        // EDIT GET
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Alumno alumno = db.Alumno.Find(id);

            if (alumno == null)
                return HttpNotFound();

            return View(alumno);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Alumno alumno)
        {
            try
            {
                if (alumno == null)
                    return View(alumno);

                if (alumno.DNI != null && alumno.DNI.Length != 8)
                    ModelState.AddModelError("DNI", "DNI inválido");

                bool existe = db.Alumno.Any(a =>
                    a.DNI == alumno.DNI &&
                    a.IdAlumno != alumno.IdAlumno);

                if (existe)
                    ModelState.AddModelError("DNI", "DNI ya registrado");

                if (ModelState.IsValid)
                {
                    db.Entry(alumno).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["Success"] = "Alumno actualizado correctamente";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }

            return View(alumno);
        }

        // DELETE GET
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Alumno alumno = db.Alumno.Find(id);

            if (alumno == null)
                return HttpNotFound();

            return View(alumno);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Alumno alumno = db.Alumno.Find(id);

                if (alumno == null)
                {
                    TempData["Error"] = "Alumno no encontrado";
                    return RedirectToAction("Index");
                }

                // 🔥 VALIDAR SI TIENE MATRÍCULAS
                bool tieneMatriculas = db.Matricula.Any(m => m.IdAlumno == id);

                if (tieneMatriculas)
                {
                    TempData["Error"] = "No se puede eliminar el alumno esta Matriculado Actualmente";
                    return RedirectToAction("Index");
                }

                db.Alumno.Remove(alumno);
                db.SaveChanges();

                TempData["Success"] = "Alumno eliminado correctamente";
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocurrió un error al eliminar el alumno";
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