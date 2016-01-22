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
    public class EntidadController : Controller
    {
        private Lojack_PruebaEntities db = new Lojack_PruebaEntities();

        //
        // GET: /Entidad/

        public ActionResult Index()
        {
            var entidads = db.Entidads.Include(e => e.Cuenta).Include(e => e.NivelServicio);
            return View(entidads.ToList());
        }

        //
        // GET: /Entidad/Details/5

        public ActionResult Details(int id = 0)
        {
            Entidad entidad = db.Entidads.Find(id);
            if (entidad == null)
            {
                return HttpNotFound();
            }
            return View(entidad);
        }

        //
        // GET: /Entidad/Create

        public ActionResult Create()
        {
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre");
            ViewBag.idNivelServicio = new SelectList(db.NivelServicios, "idNivelServicio", "descripcion");
            return View();
        }

        //
        // POST: /Entidad/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Entidad entidad)
        {
            if (ModelState.IsValid)
            {
                db.Entidads.Add(entidad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", entidad.idCuenta);
            ViewBag.idNivelServicio = new SelectList(db.NivelServicios, "idNivelServicio", "descripcion", entidad.idNivelServicio);
            return View(entidad);
        }

        //
        // GET: /Entidad/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Entidad entidad = db.Entidads.Find(id);
            if (entidad == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", entidad.idCuenta);
            ViewBag.idNivelServicio = new SelectList(db.NivelServicios, "idNivelServicio", "descripcion", entidad.idNivelServicio);
            return View(entidad);
        }

        //
        // POST: /Entidad/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Entidad entidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", entidad.idCuenta);
            ViewBag.idNivelServicio = new SelectList(db.NivelServicios, "idNivelServicio", "descripcion", entidad.idNivelServicio);
            return View(entidad);
        }

        //
        // GET: /Entidad/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Entidad entidad = db.Entidads.Find(id);
            if (entidad == null)
            {
                return HttpNotFound();
            }
            return View(entidad);
        }

        //
        // POST: /Entidad/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Entidad entidad = db.Entidads.Find(id);
            db.Entidads.Remove(entidad);
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