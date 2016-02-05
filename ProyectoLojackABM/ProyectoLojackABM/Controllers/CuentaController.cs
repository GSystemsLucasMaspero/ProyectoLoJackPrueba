﻿using System;
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
        private static int last_edit_id = 0;
        private static int last_delete_id = 0;
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

            var cuentas = from s in db.Cuentas where s.idCuenta != 0 select s ;
            
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
            //Svar cuentas = from s in db.Cuentas select s;
            //SELECT TOP 1 idCuenta FROM Cuenta ORDER BY idCuenta DESC
            //var cuentas = (from s in db.Cuentas orderby "idCuenta" select "idCuenta").Take(1);
            var query = db.Cuentas.OrderByDescending(u => u.idCuenta).Select(u => u.idCuenta).Take(1).FirstOrDefault();

            if (ModelState.IsValid)
            {
                    cuenta.idCuenta = Convert.ToInt32(query.ToString()) + 1;
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
            Cuenta query = db.Cuentas.Find(id);
            if (query == null || id == 0)
            {
                return HttpNotFound();
            }
            ViewBag.idClienteControlador = new SelectList(db.Clientes, "idCliente", "nombre", query.idClienteControlador);
            last_edit_id = id;
            return View(query);
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
            if (cuenta == null || id == 0)
            {
                return HttpNotFound();
            }
            return View(cuenta);
        }

        //
        // POST: /Cuenta/Delete/5

        [HttpPost]
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