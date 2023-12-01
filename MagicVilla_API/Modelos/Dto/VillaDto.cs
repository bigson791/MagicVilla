using System.ComponentModel.DataAnnotations;

// se crea el DTO para evitar interacturar directamente con el modelo
namespace MagicVilla_API.Modelos.Dto
{
    public class villaDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        [Required]
        public double Tarifa { get; set; }  
        public int MetrosCuadrados { get; set; }
        public int Ocupantes { get; set; }
        public string ImagenUrl { get; set; }
        public string Amenidad { get; set; }
        public int MyProperty { get; set; }
    }
}
