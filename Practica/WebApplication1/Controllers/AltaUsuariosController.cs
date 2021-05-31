using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Practica.Models;

namespace Practica.Controllers
{
    public class AltaUsuariosController : Controller
    {
        // GET: AltaUsuarios
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Guardar(UsuarioViewModel user)
        {
            try
            {
                // Si el formulario es válido
                if (ModelState.IsValid)
                {

                    if (ConfigurationManager.AppSettings["almacenamiento"] == "persistente")
                    {
                        // Base de datos
                        using (EntityCrud db = new EntityCrud())
                        {
                            var usuario = new usuario();
                            usuario.Email = user.Email;
                            usuario.Nombre = user.Nombre;
                            usuario.Password = user.Password;

                            db.usuario.Add(usuario);
                            db.SaveChanges();
                        }
                    }
                    else if (ConfigurationManager.AppSettings["almacenamiento"] == "volatil")
                    {
                        // Almacenamiento en sesión
                        List<UsuarioViewModel> listaUsuarios;

                        if(Session["Usuarios"]!= null)
                        {
                            listaUsuarios = Session["Usuarios"] as List<UsuarioViewModel>;
                        }
                        else
                        {
                            listaUsuarios = new List<UsuarioViewModel>();
                        }
                        listaUsuarios.Add(user);
                        Session["Usuarios"] = listaUsuarios;
                    }
                    
                    // Redirige al listado de usuarios
                    return Redirect("/Home/Listado");
                }
                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Redirect("/AltaUsuarios");
        }
    }
}