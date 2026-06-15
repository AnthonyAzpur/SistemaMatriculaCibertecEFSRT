using SistemaMatriculaCibertec.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaMatriculaCibertec.Controllers
{
    public class ConsultasController : Controller
    {
        private SistemaMatriculaDBEntities db =
            new SistemaMatriculaDBEntities();

        public ActionResult Index(string tipo = "alumno", string codigo = "")
        {
            ViewBag.Tipo = tipo;
            ViewBag.CodigoBuscado = codigo;

            if (!string.IsNullOrEmpty(codigo))
            {
                switch (tipo.ToLower())
                {
                    case "alumno":

                        var alumno = db.Alumno
                            .FirstOrDefault(a => a.CodigoAlumno == codigo);

                        ViewBag.Alumno = alumno;

                        if (alumno != null)
                        {
                            ViewBag.Matriculas = db.Matricula
                                .Include("Curso")
                                .Where(m => m.IdAlumno == alumno.IdAlumno)
                                .ToList();
                        }

                        break;

                    case "curso":

                        var curso = db.Curso
                            .FirstOrDefault(c => c.CodigoCurso == codigo);

                        ViewBag.Curso = curso;

                        if (curso != null)
                        {
                            ViewBag.AlumnosCurso = db.Matricula
                                .Include("Alumno")
                                .Where(m => m.IdCurso == curso.IdCurso)
                                .ToList();
                        }

                        break;

                    case "matricula":

                        var matricula = db.Matricula
                            .Include("Alumno")
                            .Include("Curso")
                            .FirstOrDefault(m => m.CodigoMatricula == codigo);

                        ViewBag.Matricula = matricula;

                        break;

                    case "retiro":

                        var retiro = db.Retiro
                            .Include("Matricula")
                            .FirstOrDefault(r => r.CodigoRetiro == codigo);

                        ViewBag.Retiro = retiro;

                        break;
                }
            }

            return View();
        }
    }
}