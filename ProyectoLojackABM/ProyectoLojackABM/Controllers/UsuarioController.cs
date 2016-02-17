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
    public class UsuarioController : Controller
    {
        private DataContextLoJack_Prueba db = new DataContextLoJack_Prueba();

        //
        // GET: /Usuario/

        public ActionResult Index(int? Order, string currentFilter, string searchString, int? page)
        {
            ViewBag.filtro = false;

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var query = from usuarios in db.Usuarios select usuarios;

            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.nombre.Contains(searchString));
                ViewBag.filtro = true;
            }

            switch (Order)
            {
                case 0:
                    query = query.OrderBy(s => s.idUsuario);
                    break;
                case 1:
                    query = query.OrderBy(s => s.Cliente.nombre);
                    break;
                case 2:
                    query = query.OrderBy(s => s.userLogin);
                    break;
                case 3:
                    query = query.OrderBy(s => s.nombre);
                    break;
                case 4:
                    query = query.OrderBy(s => s.apellido);
                    break;
                case 5:
                    query = query.OrderBy(s => s.Sector.nombre);
                    break;
                case 6:
                    query = query.OrderBy(s => s.password);
                    break;
                case 7:
                    query = query.OrderBy(s => s.operador);
                    break;
                case 8:
                    query = query.OrderBy(s => s.operadorSeguridad);
                    break;
                case 9:
                    query = query.OrderBy(s => s.supervisor);
                    break;
                case 10:
                    query = query.OrderBy(s => s.email);
                    break;
                case 11:
                    query = query.OrderBy(s => s.nivelAuditoria);
                    break;
                case 12:
                    query = query.OrderBy(s => s.perfilWindows);
                    break;
                case 13:
                    query = query.OrderBy(s => s.perfilWeb);
                    break;
                case 14:
                    query = query.OrderBy(s => s.multipais);
                    break;
                case 15:
                    query = query.OrderBy(s => s.demo);
                    break;
                case 16:
                    query = query.OrderBy(s => s.Cuenta.nombre);
                    break;
                default:
                    query = query.OrderBy(s => s.idUsuario);
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

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlCommand("INSERT INTO Usuario (idCliente, userLogin, nombre, apellido, idSector,password,operador,operadorSeguridad,supervisor,email,fechaAlta,usuarioAlta,fechaModificacion,usuarioModificacion,nivelAuditoria,perfilWindows,perfilWeb,multipais,demo,diasDemo,idCuenta)VALUES(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17,@p18)", /*p0*/usuario.idCliente,/*p1*/ usuario.userLogin,/*p2*/ usuario.nombre,/*p3*/ usuario.apellido,/*p4*/ usuario.idSector,/*p5*/ usuario.password,/*p6*/ usuario.operador,/*p7*/ usuario.operadorSeguridad,/*p8*/ usuario.supervisor,/*p9*/ usuario.email,/*p10*/DateTime.Now.ToString("yyyy-MM-dd h:mm:ss.f"),/*p11*/ 66/*TEMPORAL*/,/*p12*/ usuario.nivelAuditoria,/*p13*/ usuario.perfilWindows,/*p14*/ usuario.perfilWeb,/*p15*/usuario.multipais,/*p16*/usuario.demo,/*p17*/usuario.diasDemo,/*p18*/usuario.idCuenta);
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
        public ActionResult Edit(Usuario usuario,int id )
        {
            if (ModelState.IsValid)
            {
                db.Database.ExecuteSqlCommand("UPDATE Usuario SET idCliente= @p0, userLogin= @p1, nombre= @p2, apellido= @p3, idSector= @p4,password= @p5,operador= @p6,operadorSeguridad= @p7,supervisor= @p8,email= @p9,fechaModificacion= @p10,usuarioModificacion= @p11,nivelAuditoria= @p12,perfilWindows= @p13,perfilWeb= @p14,multipais= @p15,demo= @p16,diasDemo= @p17,idCuenta= @p18 WHERE idCliente= @p19", /*p0*/usuario.idCliente,/*p1*/ usuario.userLogin,/*p2*/ usuario.nombre,/*p3*/ usuario.apellido,/*p4*/ usuario.idSector,/*p5*/ usuario.password,/*p6*/ usuario.operador,/*p7*/ usuario.operadorSeguridad,/*p8*/ usuario.supervisor,/*p9*/ usuario.email,/*p10*/DateTime.Now.ToString("yyyy-MM-dd h:mm:ss.f"),/*p11*/ 66/*TEMPORAL*/,/*p12*/ usuario.nivelAuditoria,/*p13*/ usuario.perfilWindows,/*p14*/ usuario.perfilWeb,/*p15*/usuario.multipais,/*p16*/usuario.demo,/*p17*/usuario.diasDemo,/*p18*/usuario.idCuenta, id);
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
            db.Database.ExecuteSqlCommand("UPDATE Usuario SET fechaBaja = @P0, usuarioBaja = @p1 WHERE idUsuario = @p2", DateTime.Now.ToString("yyyy-MM-dd h:mm:ss.f"), 66/*TEMPORAL*/, id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}