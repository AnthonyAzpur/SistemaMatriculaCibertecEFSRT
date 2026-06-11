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
    public class MatriculasController : Controller
    {
        private SistemaMatriculaDBEntities db = new SistemaMatriculaDBEntities();

        // GET: Matriculas
        public ActionResult Index()
        {
            var matricula = db.Matricula.Include(m => m.Alumno).Include(m => m.Curso);
            return View(matricula.ToList());
        }

        // GET: Matriculas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matricula matricula = db.Matricula.Find(id);
            if (matricula == null)
            {
                return HttpNotFound();
            }
            return View(matricula);
        }

        // GET: Matriculas/Create
        public ActionResult Create()
        {
            ViewBag.IdAlumno = new SelectList(db.Alumno, "IdAlumno", "CodigoAlumno");
            ViewBag.IdCurso = new SelectList(db.Curso, "IdCurso", "CodigoCurso");
            return View();
        }

        // POST: Matriculas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdMatricula,CodigoMatricula,IdAlumno,IdCurso,FechaMatricula,Estado")] Matricula matricula)
        {
            if (ModelState.IsValid)
            {
                db.Matricula.Add(matricula);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdAlumno = new SelectList(db.Alumno, "IdAlumno", "CodigoAlumno", matricula.IdAlumno);
            ViewBag.IdCurso = new SelectList(db.Curso, "IdCurso", "CodigoCurso", matricula.IdCurso);
            return View(matricula);
        }

        // GET: Matriculas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matricula matricula = db.Matricula.Find(id);
            if (matricula == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdAlumno = new SelectList(db.Alumno, "IdAlumno", "CodigoAlumno", matricula.IdAlumno);
            ViewBag.IdCurso = new SelectList(db.Curso, "IdCurso", "CodigoCurso", matricula.IdCurso);
            return View(matricula);
        }

        // POST: Matriculas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMatricula,CodigoMatricula,IdAlumno,IdCurso,FechaMatricula,Estado")] Matricula matricula)
        {
            if (ModelState.IsValid)
            {
                db.Entry(matricula).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdAlumno = new SelectList(db.Alumno, "IdAlumno", "CodigoAlumno", matricula.IdAlumno);
            ViewBag.IdCurso = new SelectList(db.Curso, "IdCurso", "CodigoCurso", matricula.IdCurso);
            return View(matricula);
        }

        // GET: Matriculas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matricula matricula = db.Matricula.Find(id);
            if (matricula == null)
            {
                return HttpNotFound();
            }
            return View(matricula);
        }

        // POST: Matriculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Matricula matricula = db.Matricula.Find(id);
            db.Matricula.Remove(matricula);
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
