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
    public class EquipoTipoController : Controller
    {
        private Lojack_PruebaEntities2 db = new Lojack_PruebaEntities2();

        //
        // GET: /EquipoTipo/

        public ActionResult Index()
        {
            return View(db.EquipoTipoes.ToList());
        }

        //
        // GET: /EquipoTipo/Details/5

        public ActionResult Details(int id = 0)
        {
            EquipoTipo equipotipo = db.EquipoTipoes.Find(id);
            if (equipotipo == null)
            {
                return HttpNotFound();
            }
            return View(equipotipo);
        }

        //
        // GET: /EquipoTipo/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /EquipoTipo/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EquipoTipo equipotipo)
        {
            if (ModelState.IsValid)
            {
                db.EquipoTipoes.Add(equipotipo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(equipotipo);
        }

        //
        // GET: /EquipoTipo/Edit/5

        public ActionResult Edit(int id = 0)
        {
            EquipoTipo equipotipo = db.EquipoTipoes.Find(id);
            if (equipotipo == null)
            {
                return HttpNotFound();
            }
            return View(equipotipo);
        }

        //
        // POST: /EquipoTipo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EquipoTipo equipotipo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipotipo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(equipotipo);
        }

        //
        // GET: /EquipoTipo/Delete/5

        public ActionResult Delete(int id = 0)
        {
            EquipoTipo equipotipo = db.EquipoTipoes.Find(id);
            if (equipotipo == null)
            {
                return HttpNotFound();
            }
            return View(equipotipo);
        }

        //
        // POST: /EquipoTipo/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EquipoTipo equipotipo = db.EquipoTipoes.Find(id);
            db.EquipoTipoes.Remove(equipotipo);
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