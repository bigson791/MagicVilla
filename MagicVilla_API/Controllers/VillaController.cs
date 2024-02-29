using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")] // ruta
    [ApiController] // controlador API
    public class VillaController : ControllerBase
    {

        // inyeccion de la dependencia
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        //se crea un constructor con ctor
        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }
        // inyeccion de la dependencia 

        [HttpGet] //tipo de verbo
        [ProducesResponseType(StatusCodes.Status200OK)] // documentando el codigo de respuesta
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task< ActionResult<IEnumerable<villaDto>>> GetVillas()
        {
            //con loger se puede enviar mensajes de error.
            _logger.LogInformation("Obtener las villas");
            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<villaDto>>(villaList)); //_db.Villas.ToList() es como hacer un select * from

        }

        [HttpGet("id: int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)] // documentando el codigo de respuesta
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<villaDto>>GetVilla(int id)
        {
            if (id == 0) // verificando que el id sea 0
            {
                _logger.LogInformation("Error al obtener la villa"+id);
                return BadRequest(); // se retorna el error
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa =  await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null) // verificando que el no exista
            {
                return NotFound(); // se retorna el error
            }
            return Ok(_mapper.Map<villaDto>(villa)); // si todo bien se regresa el resultado
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        // el atriburo FromBody indica que recibira información.
        // VillaDto hace referencia a la clase del modelo.
        // villaDto es una instancia de la Clase VillaDto.
        public async Task <ActionResult<villaDto>> crearVilla([FromBody] villaCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                //verifica que lo que los datos enviados este dentro de los parametros indicados
                return BadRequest(ModelState);
            }

            if (await _db.Villas.FirstOrDefaultAsync(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "la villa con ese nombre ya existe!");
                return BadRequest(ModelState);
            }

            if (createDto == null)
            {
                return BadRequest(createDto);
            }

            /*if (villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }*/
            Villa modelo = _mapper.Map<Villa>(createDto);

            await _db.Villas.AddAsync(modelo);
            await _db.SaveChangesAsync();
           // villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
           // VillaStore.villaList.Add(villaDto);
            //cuando en la api se crea un nuevo recurso se debe indicar la url de este
            // el primer parametro llama al endpoint get, el segundo parametro es el id que nos pide el endopoint y el tercer parametro es todo el objeto 
            return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        //IActionResult no necesita el modelo, cuando se trabaja con delete se devuelve un no content
        public async Task <IActionResult> DeleteVilla(int id) 
        {

            if (id == 0) // verificando que el id sea 0
            {
                return BadRequest(); // se retorna el error
            }
            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null) // verificando que el no exista
            {
                return NotFound(); // se retorna el error
            }

            // se elimina la villa de la lista
            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();
            //VillaStore.villaList.Remove(villa);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task <IActionResult> UpdateVilla(int id, [FromBody] villaUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.Id)
            {
                return BadRequest();
            }

            Villa modelo = _mapper.Map<Villa>(updateDto);


            _db.Villas.Update(modelo);
            await _db.SaveChangesAsync();
            return NoContent();
        }




        //permite actualizar un solo objeto, para este caso se usa en path se usa /nombre, en op se usa "replace" en value se agrega el valor que se desea, 
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //recibe el id a actualizar, y recibe el objeto
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<villaUpdateDto> patchDto)
        {
            // se valida que lo recibido por no este nulo y 
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return BadRequest();
            }
            villaUpdateDto villaDto = _mapper.Map<villaUpdateDto>(villa);


            if (villa == null)
            {
                return BadRequest();
            }
            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo = _mapper.Map<Villa>(villaDto);

            _db.Villas.Update(modelo);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
