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

        //
        // GET: /NivelServicio/

        public ActionResult Index()
        {
            return View(db.NivelServicios.ToList());
        }

        //
        // GET: /NivelServicio/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /NivelServicio/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NivelServicio nivelservicio)
        {
            // La fecha de alta es la de "ahora"
            nivelservicio.fechaAlta = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.NivelServicios.Add(nivelservicio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nivelservicio);
        }

        //
        // GET: /NivelServicio/Edit/5

        public ActionResult Edit(int id = 0)
        {
            NivelServicio nivelservicio = db.NivelServicios.Find(id);
            if (nivelservicio == null)
            {
                return HttpNotFound();
            }
            return View(nivelservicio);
        }

        //
        // POST: /NivelServicio/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NivelServicio nivelservicio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nivelservicio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nivelservicio);
        }

        //
        // GET: /NivelServicio/Delete/5

        public ActionResult Delete(int id = 0)
        {
            NivelServicio nivelservicio = db.NivelServicios.Find(id);
            if (nivelservicio == null)
            {
                return HttpNotFound();
            }
            return View(nivelservicio);
        }

        //
        // POST: /NivelServicio/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NivelServicio nivelservicio = db.NivelServicios.Find(id);
            db.NivelServicios.Remove(nivelservicio);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}