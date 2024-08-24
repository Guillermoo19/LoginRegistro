using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace LoginRegistro.Models
{
    public class Usuario
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(14, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 14 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es requerido.")]
        [StringLength(14, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 14 caracteres.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El género es requerido.")]
        [RegularExpression("^[MF]$", ErrorMessage = "El género debe ser 'M' o 'F'.")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "La fecha de ingreso es requerida.")]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        [Required(ErrorMessage = "El ID de usuario es requerido.")]
        [StringLength(10, ErrorMessage = "El ID de usuario no puede tener más de 10 caracteres.")]
        public string IdUsuario { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        [StringLength(100, ErrorMessage = "El correo electrónico no puede tener más de 100 caracteres.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [StringLength(150, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Clave { get; set; }

        public int Id { get; set; }
        public int CodigoEmpleado { get; set; }

        // Método para obtener la cadena de conexión desde la configuración
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
    }
}
