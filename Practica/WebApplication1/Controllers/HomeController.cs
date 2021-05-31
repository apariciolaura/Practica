using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Practica.Models;

namespace Practica.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(UsuarioViewModel user)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UsuarioViewModel user)
        {
            if(ConfigurationManager.AppSettings["almacenamiento"] == "persistente")
            {
                // Base de datos
                using (EntityCrud contexto = new EntityCrud())
                {
                    var query = contexto.usuario.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);

                    if (query != null)
                    {
                        return Redirect("/Home/Listado");
                    }
                    else
                    {
                        // Alerta que no hay ningún usuario y contraseña con esos datos
                        TempData["Message"] = "Usuario o contraseña incorrectos";
                        // Redirige al formulario de login
                        return RedirectToAction("Index", "Home");
                    }
                }
            }else if(ConfigurationManager.AppSettings["almacenamiento"] == "volatil")
            {
                // Almacenamiento en sesión 
                List<UsuarioViewModel> listaUsuarios = Session["Usuarios"] as List<UsuarioViewModel>;
                try
                {
                    var query = listaUsuarios.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
                    // Redirige al listado
                    return Redirect("/Home/Listado");
                }
                catch(Exception ex)
                {
                    // Alerta que no hay ningún usuario y contraseña con esos datos
                    TempData["Message"] = "Usuario o contraseña incorrectos";
                    // Redirige al formulario de login
                    return RedirectToAction("Index", "Home");
                }
            }
            // Redirige al formulario de login
            return Redirect("/Home/Index");
        }


        public ActionResult Listado()
        {
            List<UsuarioViewModel> listaUsuarios = new List<UsuarioViewModel>();

            if (ConfigurationManager.AppSettings["almacenamiento"] == "persistente")
            {
                // Base de datos
                using (EntityCrud contexto = new EntityCrud())
                {
                    listaUsuarios = (from d in contexto.usuario
                                     select new UsuarioViewModel
                                     {
                                         Id = d.Id,
                                         Nombre = d.Nombre,
                                         Email = d.Email,
                                         Password = d.Password
                                     }).ToList();
                }
            }
            else if (ConfigurationManager.AppSettings["almacenamiento"] == "volatil")
            {
                // Almacenamiento en sesión 
                listaUsuarios = Session["Usuarios"] as List<UsuarioViewModel>;
            }
            if (listaUsuarios != null)
            {
                // Si hay usuarios, devuelve el listado con ellos
                return View(listaUsuarios);
            }
            else
            {
                // Si no hay usuario, alerta de que no hay datos
                return Content("No hay datos");
            }
        }
    }
}