using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoLojackABM.Models;

namespace ProyectoLojackABM.Controllers
{
    public class NivelServicioController : Controller
    {
        private DataContextLoJack_Prueba db = new DataContextLoJack_Prueba();
        private static int last_edit_id = 0;
        private static int last_delete_id = 0;
        private static int last_id = 0;

        // Hasta que este hecho el log-in
        private static int usuarioPrueba = 20;

        public ActionResult Index()
        {
            return View(db.NivelServicios.ToList());
        }

        public ActionResult Create()
        {
            if (last_id == 0)
            {
                last_id = db.NivelServicios.ToArray().Last().idNivelServicio;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NivelServicio nivelservicio)
        {
            nivelservicio.fechaAlta = DateTime.Now;
            nivelservicio.idNivelServicio = ++last_id;
            if (ModelState.IsValid)
            {
                nivelservicio.usuarioAlta = usuarioPrueba;  // Hasta que este hecho el log-in
                db.NivelServicios.Add(nivelservicio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nivelservicio);
        }

        public ActionResult Edit(int id = 0)
        {
            NivelServicio nivelservicio = db.NivelServicios.Find(id);
            if (nivelservicio.fechaBaja == null)
                return View();
            if (nivelservicio == null)
            {
                return HttpNotFound();
            }
            last_edit_id = id;
            return View(nivelservicio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NivelServicio nivelservicio)
        {
            nivelservicio.idNivelServicio = last_edit_id;
            if (ModelState.IsValid)
            {
                var nivelServicioToUpdate = db.NivelServicios.SingleOrDefault(ns => ns.idNivelServicio == nivelservicio.idNivelServicio);
                if (nivelServicioToUpdate != null)
                {
                    nivelServicioToUpdate.descripcion = nivelservicio.descripcion;
                    nivelServicioToUpdate.usuarioAlta = nivelservicio.usuarioAlta;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(nivelservicio);
        }

        public ActionResult Delete(int id = 0)
        {
            NivelServicio nivelservicio = db.NivelServicios.Find(id);
            if (nivelservicio == null)
            {
                return HttpNotFound();
            }
            last_delete_id = id;
            return View(nivelservicio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(NivelServicio nivelservicio)
        {
            nivelservicio.idNivelServicio = last_delete_id;
            if (ModelState.IsValid)
            {
                var nivelServicioToUpdate = db.NivelServicios.SingleOrDefault(ns => ns.idNivelServicio == nivelservicio.idNivelServicio);
                if (nivelServicioToUpdate != null)
                {
                    nivelServicioToUpdate.usuarioBaja = usuarioPrueba; // Hasta que este hecho el log-in
                    nivelServicioToUpdate.fechaBaja = DateTime.Now;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(nivelservicio);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}