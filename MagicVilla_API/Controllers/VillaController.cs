using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")] // ruta
    [ApiController] // controlador API
    public class VillaController : ControllerBase
    {
        [HttpGet] //tipo de verbo
        [ProducesResponseType(StatusCodes.Status200OK)] // documentando el codigo de respuesta
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            return Ok(VillaStore.villaList);

        }
        [HttpGet("id: int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)] // documentando el codigo de respuesta
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0) // verificando que el id sea 0
            {
                return BadRequest(); // se retorna el error
            }
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if (villa == null) // verificando que el no exista
            {
                return NotFound(); // se retorna el error
            }
            return Ok(villa); // si todo bien se regresa el resultado
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        // el atriburo FromBody indica que recibira información.
        // VillaDto hace referencia a la clase del modelo.
        // villaDto es una instancia de la Clase VillaDto.
        public ActionResult<VillaDto> crearVilla([FromBody] VillaDto villaDto)
        {
            if (!ModelState.IsValid)
            {
                //verifica que lo que los datos enviados este dentro de los parametros indicados
                return BadRequest(ModelState);
            }

            if (VillaStore.villaList.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "la villa con ese nombre ya existe!");
                return BadRequest(ModelState);
            }

            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }

            if (villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDto);
            //cuando en la api se crea un nuevo recurso se debe indicar la url de este
            // el primer parametro llama al endpoint get, el segundo parametro es el id que nos pide el endopoint y el tercer parametro es todo el objeto 
            return CreatedAtRoute("GetVilla", new { id = villaDto.Id }, villaDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        //IActionResult no necesita el modelo, cuando se trabaja con delete se devuelve un no content
        public IActionResult DeleteVilla(int id) 
        {

            if (id == 0) // verificando que el id sea 0
            {
                return BadRequest(); // se retorna el error
            }
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if (villa == null) // verificando que el no exista
            {
                return NotFound(); // se retorna el error
            }

            // se elimina la villa de la lista
            VillaStore.villaList.Remove(villa);

            return NoContent();
        }

        [HttpPut("{id: int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //recibe el id a actualizar, y recibe el objeto
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {
            // se valida que lo recibido por no este nulo y 
            if (villaDto == null || id != villaDto.Id)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(v=>v.Id == id);
            villa.Nombre = villaDto.Nombre;
            villa.Ocupantes = villaDto.Ocupantes;
            villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            return NoContent();
        }
    }
}
