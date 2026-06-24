using SistemaMatriculaCibertec.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

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

            ViewBag.Alumno = null;
            ViewBag.Curso = null;
            ViewBag.Matricula = null;
            ViewBag.Retiro = null;

            ViewBag.Matriculas = null;
            ViewBag.AlumnosCurso = null;

            if (!string.IsNullOrWhiteSpace(codigo))
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
                                .Include(m => m.Curso)
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
                                .Include(m => m.Alumno)
                                .Where(m => m.IdCurso == curso.IdCurso)
                                .ToList();
                        }

                        break;

                    case "matricula":

                        var matricula = db.Matricula
                            .Include(m => m.Alumno)
                            .Include(m => m.Curso)
                            .FirstOrDefault(m => m.CodigoMatricula == codigo);

                        ViewBag.Matricula = matricula;

                        break;

                    case "retiro":

                        var retiro = db.Retiro
                            .Include(r => r.Matricula)
                            .FirstOrDefault(r => r.CodigoRetiro == codigo);

                        ViewBag.Retiro = retiro;

                        break;
                }
            }

            return View();
        }
    }
}