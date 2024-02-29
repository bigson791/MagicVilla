using System.ComponentModel.DataAnnotations;

// se crea el DTO para evitar interacturar directamente con el modelo
namespace MagicVilla_API.Modelos.Dto
{
    public class villaUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        [Required]
        public double Tarifa { get; set; }
        [Required]
        public double MetrosCuadrados { get; set; }
        [Required]
        public int Ocupantes { get; set; }
        [Required]
        public string ImagenUrl { get; set; }
        public string Amenidad { get; set; }
    }
}
