using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoLojackABM.Models;
using PagedList;

namespace ProyectoLojackABM.Controllers
{
    public class CuentaController : Controller
    {
        private DataContextLoJack_Prueba db = new DataContextLoJack_Prueba();

        //
        // GET: /Cuenta/

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IDSortParm = "id_desc";
            ViewBag.CuentaNombre = "nombre_desc";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var cuentas = from s in db.Cuentas
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                cuentas = cuentas.Where(s => s.nombre.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "id_desc":
                    cuentas = cuentas.OrderBy(s => s.idCuenta);
                    break;
                case "nombre_desc":
                    cuentas = cuentas.OrderBy(s => s.nombre);
                    break;
                default:
                    cuentas = cuentas.OrderBy(s => s.idCuenta);
                    break;
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(cuentas.ToPagedList(pageNumber, pageSize));

        }

        //
        // GET: /Cuenta/Details/5

        public ActionResult Details(int id = 0)
        {
            Cuenta cuenta = db.Cuentas.Find(id);
            if (cuenta == null)
            {
                return HttpNotFound();
            }
            return View(cuenta);
        }

        //
        // GET: /Cuenta/Create

        public ActionResult Create()
        {
            ViewBag.idClienteControlador = new SelectList(db.Clientes, "idCliente", "nombre");
            return View();
        }

        //
        // POST: /Cuenta/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                db.Cuentas.Add(cuenta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idClienteControlador = new SelectList(db.Clientes, "idCliente", "nombre", cuenta.idClienteControlador);
            return View(cuenta);
        }

        //
        // GET: /Cuenta/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Cuenta cuenta = db.Cuentas.Find(id);
            if (cuenta == null)
            {
                return HttpNotFound();
            }
            ViewBag.idClienteControlador = new SelectList(db.Clientes, "idCliente", "nombre", cuenta.idClienteControlador);
            return View(cuenta);
        }

        //
        // POST: /Cuenta/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cuenta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idClienteControlador = new SelectList(db.Clientes, "idCliente", "nombre", cuenta.idClienteControlador);
            return View(cuenta);
        }

        //
        // GET: /Cuenta/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Cuenta cuenta = db.Cuentas.Find(id);
            if (cuenta == null)
            {
                return HttpNotFound();
            }
            return View(cuenta);
        }

        //
        // POST: /Cuenta/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cuenta cuenta = db.Cuentas.Find(id);
            db.Cuentas.Remove(cuenta);
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