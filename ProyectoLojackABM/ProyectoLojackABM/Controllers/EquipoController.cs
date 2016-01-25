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
    public class EquipoController : Controller
    {
        private DataContextLoJack_Prueba db = new DataContextLoJack_Prueba();

        //
        // GET: /Equipo/

        public ActionResult Index()
        {
            var equipoes = db.Equipoes.Include(e => e.Cuenta).Include(e => e.EquipoTipo);
            return View(equipoes.ToList());
        }

        //
        // GET: /Equipo/Details/5

        public ActionResult Details(int id = 0)
        {
            Equipo equipo = db.Equipoes.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            return View(equipo);
        }

        //
        // GET: /Equipo/Create

        public ActionResult Create()
        {
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre");
            ViewBag.idEquipoTipo = new SelectList(db.EquipoTipoes, "idEquipoTipo", "descripcion");
            return View();
        }

        //
        // POST: /Equipo/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                db.Equipoes.Add(equipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", equipo.idCuenta);
            ViewBag.idEquipoTipo = new SelectList(db.EquipoTipoes, "idEquipoTipo", "descripcion", equipo.idEquipoTipo);
            return View(equipo);
        }

        //
        // GET: /Equipo/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Equipo equipo = db.Equipoes.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", equipo.idCuenta);
            ViewBag.idEquipoTipo = new SelectList(db.EquipoTipoes, "idEquipoTipo", "descripcion", equipo.idEquipoTipo);
            return View(equipo);
        }

        //
        // POST: /Equipo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Equipo equipo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", equipo.idCuenta);
            ViewBag.idEquipoTipo = new SelectList(db.EquipoTipoes, "idEquipoTipo", "descripcion", equipo.idEquipoTipo);
            return View(equipo);
        }

        //
        // GET: /Equipo/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Equipo equipo = db.Equipoes.Find(id);
            if (equipo == null)
            {
                return HttpNotFound();
            }
            return View(equipo);
        }

        //
        // POST: /Equipo/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Equipo equipo = db.Equipoes.Find(id);
            db.Equipoes.Remove(equipo);
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