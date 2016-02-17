using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoLojackABM.Models;
using PagedList;
using PagedList;

namespace ProyectoLojackABM.Controllers
{
    public class ClienteController : Controller
    {
        private DataContextLoJack_Prueba db = new DataContextLoJack_Prueba();

        //
        // GET: /Cliente/

        public ActionResult Index(int? Order, string currentFilter, string searchString, int? page)
        {
            ViewBag.filtro = false;

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var query = from clientes in db.Clientes select clientes;

            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.nombre.Contains(searchString));
                ViewBag.filtro = true;
            }

            switch (Order)
            {
                case 0:
                    query = query.OrderBy(s => s.idCliente);
                    break;
                case 1:
                    query = query.OrderBy(s => s.nombre);
                    break;
                case 2:
                    query = query.OrderBy(s => s.direccion);
                    break;
                case 3:
                    query = query.OrderBy(s => s.telefono);
                    break;
                case 4:
                    query = query.OrderBy(s => s.localidad);
                    break;
                case 5:
                    query = query.OrderBy(s => s.partido);
                    break;
                case 6:
                    query = query.OrderBy(s => s.provincia);
                    break;
                case 7:
                    query = query.OrderBy(s => s.cuit);
                    break;
                case 8:
                    query = query.OrderBy(s => s.email);
                    break;
                case 9:
                    query = query.OrderBy(s => s.fax);
                    break;
                case 10:
                    query = query.OrderBy(s => s.inhabilitado);
                    break;
                case 11:
                    query = query.OrderBy(s => s.idPais);
                    break;
                case 12:
                    query = query.OrderBy(s => s.idCuenta);
                    break;
                case 13:
                    query = query.OrderBy(s => s.vistaInternoDesvios);
                    break;
                default:
                    query = query.OrderBy(s => s.idCliente);
                    break;
            }

            if (query.Count() == 0 && ViewBag.filtro == true)
            {
                return Content("<p>NO SE HAN ENCONTRADO RESULTADOS</p>");
            }

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(query.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Cliente/Details/5

        public ActionResult Details(int id = 0)
        {
            
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);

        }

        //
        // GET: /Cliente/Create

        public ActionResult Create()
        {
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre");
            return View();
        }

        //
        // POST: /Cliente/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cliente cliente)
        {
            var query = db.Cuentas.OrderByDescending(u => u.idCuenta).Select(u => u.idCuenta).Take(1).FirstOrDefault();

            if (ModelState.IsValid)
            {
                cliente.idCuenta = Convert.ToInt32(query.ToString()) + 1;
                db.Clientes.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", cliente.idCuenta);
            return View(cliente);
        }

        //
        // GET: /Cliente/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", cliente.idCuenta);
            return View(cliente);
        }

        //
        // POST: /Cliente/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlCommand("UPDATE Cliente SET nombre = @p0, direccion= @p1, telefono= @p2, localidad= @p3, partido= @p4, provincia= @p5, cuit= @p6,email= @p7 ,fax= @p8, inhabilitado= @p9, usuarioAlta= @p10,fechaAlta= @p11,idPais= @p12,idCuenta= @p13, vistaInternoDesvios= @p14 , fechaModificacion = p@15, usuarioModificacion = @p16WHERE idCliente = @p17", cliente.nombre, cliente.direccion, cliente.direccion, cliente.telefono, cliente.localidad, cliente.partido, cliente.provincia, cliente.cuit, cliente.email, cliente.fax, cliente.inhabilitado, 66/*TEMPORAL*/ ,DateTime.Now.ToString("yyyy-MM-dd h:mm:ss.f"),cliente.idPais,cliente.idCuenta,cliente.vistaInternoDesvios,cliente.idCliente);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.idCuenta = new SelectList(db.Cuentas, "idCuenta", "nombre", cliente.idCuenta);
            return View(cliente);
        }

        //
        // GET: /Cliente/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        //
        // POST: /Cliente/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            db.Database.ExecuteSqlCommand("UPDATE Sector SET fechaBaja = @P0, usuarioBaja = @p1 WHERE idCliente = @p2", DateTime.Now.ToString("yyyy-MM-dd h:mm:ss.f"), 66/*TEMPORAL*/, cliente.idCliente);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}