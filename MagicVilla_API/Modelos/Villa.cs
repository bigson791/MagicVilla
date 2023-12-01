using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Modelos
{
    public class Villa
    {
        [Key] //data notation que identifica la primary key 
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Detalle { get; set; }
        [Required] // agrega la propiedad required
        public double Tarifa { get; set; }
        public int Ocupantes { get; set; }
        public double MetrosCuadrados { get; set; }
        public string ImagenUrl{ get; set; }
        public string Amenidad{ get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
