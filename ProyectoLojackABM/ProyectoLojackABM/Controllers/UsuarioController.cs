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
    public class UsuarioController : Controller
    {
        private DataContextLoJack_Prueba db = new DataContextLoJack_Prueba();

        //
        // GET: /Usuario/

        public ActionResult Index()
        {
            var usuarios = db.Usuarios.Include(u => u.Cliente).Include(u => u.Cuenta).Include(u => u.Sector);
            return View(usuarios.ToList());
        }

        //
        // GET: /Usuario/Details/5

        public ActionResult Details(int id = 0)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        //
        // GET: /Usuario/Create

        public ActionResult Create()
        {
            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "nombre");
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre");
            ViewBag.idSector = new SelectList(db.Sectors, "idSector", "nombre");
            return View();
        }

        //
        // POST: /Usuario/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "nombre", usuario.idCliente);
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", usuario.idCuenta);
            ViewBag.idSector = new SelectList(db.Sectors, "idSector", "nombre", usuario.idSector);
            return View(usuario);
        }

        //
        // GET: /Usuario/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "nombre", usuario.idCliente);
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", usuario.idCuenta);
            ViewBag.idSector = new SelectList(db.Sectors, "idSector", "nombre", usuario.idSector);
            return View(usuario);
        }

        //
        // POST: /Usuario/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCliente = new SelectList(db.Clientes, "idCliente", "nombre", usuario.idCliente);
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", usuario.idCuenta);
            ViewBag.idSector = new SelectList(db.Sectors, "idSector", "nombre", usuario.idSector);
            return View(usuario);
        }

        //
        // GET: /Usuario/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        //
        // POST: /Usuario/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuario);
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