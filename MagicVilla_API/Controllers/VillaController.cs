using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")] // ruta
    [ApiController] // controlador API
    public class VillaController : ControllerBase
    {
        [HttpGet] //tipo de verbo
        public IEnumerable<VillaDto> GetVillas()
        {
            return VillaStore.villaList;
          
        }
        [HttpGet( "id")]
        public VillaDto GetVilla(int id) 
        {
            return VillaStore.villaList.FirstOrDefault(v=>v.Id==id);
        }
    }
}
