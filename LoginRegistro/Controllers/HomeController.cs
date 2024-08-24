using System.Web.Mvc;

namespace LoginRegistro.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Página de descripción de la aplicación.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Página de contacto.";

            return View();
        }

        // Nueva acción para cerrar sesión
        public ActionResult Logout()
        {
            // Eliminar cualquier variable de sesión o autenticación
            Session.Clear();
            Session.Abandon();

            // Redirigir a la página de inicio de sesión
            return RedirectToAction("Login", "Acceso");
        }
    }
}
