using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using LoginRegistro.Models; // Asegúrate de que el namespace coincida con el nombre de tu proyecto

namespace LoginRegistro.Controllers
{
    public class AccesoController : Controller
    {
        // Cadena de conexión ajustada según tu base de datos
        private static readonly string connectionString = "Data Source=DESKTOP-GDFNNGE\\SQLEXPRESS;Initial Catalog=BDD_LoginEm;User ID=ga;Password=1976ga19";

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuario oUsuario)
        {
            // Validar si faltan campos
            if (string.IsNullOrEmpty(oUsuario.IdUsuario) || string.IsNullOrEmpty(oUsuario.Clave))
            {
                ViewBag.ErrorMessage = "Usuario y contraseña son requeridos.";
                return View();
            }

            try
            {
                // Usar bloque using para asegurar que la conexión se cierra
                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("sp_ValidarLogin", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre_usuario", oUsuario.IdUsuario);

                    // Convertir la contraseña a hash usando SHA-256
                    string hashedPassword = ComputeSha256Hash(oUsuario.Clave);
                    cmd.Parameters.AddWithValue("@contrasena", hashedPassword);

                    // Parámetros de salida
                    cmd.Parameters.Add(new SqlParameter("@es_autenticado", SqlDbType.Bit) { Direction = ParameterDirection.Output });
                    cmd.Parameters.Add(new SqlParameter("@mensaje", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output });

                    // Abrir la conexión y ejecutar el comando
                    con.Open();
                    cmd.ExecuteNonQuery();

                    bool isAuthenticated = Convert.ToBoolean(cmd.Parameters["@es_autenticado"].Value);
                    string message = cmd.Parameters["@mensaje"].Value.ToString();

                    if (isAuthenticated)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = message;
                        return View();
                    }
                }
            }
            catch (SqlException ex)
            {
                ViewBag.ErrorMessage = "Error en la base de datos: " + ex.Message;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Se produjo un error al intentar iniciar sesión: " + ex.Message;
                return View();
            }
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using (var sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        [HttpGet]
        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Usuario oUsuario)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Por favor, corrija los errores en el formulario.";
                return View(oUsuario);
            }

            try
            {
                string hashedPassword = ComputeSha256Hash(oUsuario.Clave);

                using (var con = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("sp_EmpleadoNuevo", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@nombre", oUsuario.Nombre);
                    cmd.Parameters.AddWithValue("@apellido", oUsuario.Apellido);
                    cmd.Parameters.AddWithValue("@genero", oUsuario.Genero);
                    cmd.Parameters.AddWithValue("@fecha_contratacion", oUsuario.FechaIngreso);
                    cmd.Parameters.AddWithValue("@documento_identidad", oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("@correo_electronico", oUsuario.Correo);
                    cmd.Parameters.AddWithValue("@usuario", oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("@contrasena", hashedPassword);

                    cmd.Parameters.Add(new SqlParameter("@resultado", SqlDbType.Bit) { Direction = ParameterDirection.Output });
                    cmd.Parameters.Add(new SqlParameter("@mensaje", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output });

                    con.Open();
                    cmd.ExecuteNonQuery();

                    bool isSuccess = Convert.ToBoolean(cmd.Parameters["@resultado"].Value);
                    string message = cmd.Parameters["@mensaje"].Value.ToString();

                    if (isSuccess)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = message;
                        return View(oUsuario);
                    }
                }
            }
            catch (SqlException ex)
            {
                ViewData["ErrorMessage"] = "Error en la base de datos: " + ex.Message;
                return View(oUsuario);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Se produjo un error al intentar registrar: " + ex.Message;
                return View(oUsuario);
            }
        }
    }
}
