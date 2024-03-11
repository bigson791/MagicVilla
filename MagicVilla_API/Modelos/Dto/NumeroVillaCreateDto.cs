using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Modelos.Dto
{
    public class NumeroVillaCreateDto
    {
        [Required] // permite al usuario ingresar manualmente el id 
        public int VillaNo { get; set; }
        [Required]
        public int VillaId { get; set; }
        [ForeignKey("VillId")]
        public string DetallesEspeciales { get; set; }

    }
}
