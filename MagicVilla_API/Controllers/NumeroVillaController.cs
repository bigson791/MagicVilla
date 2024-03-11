using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")] // ruta
    [ApiController] // controlador API
    public class NumeroVillaController : ControllerBase
    {

        // inyeccion de la dependencia
        private readonly ILogger<NumeroVillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly INumeroVillaRepositorio _numeroRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        //se crea un constructor con ctor
        public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepositorio villaRepo, INumeroVillaRepositorio numeroRepo,IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _mapper = mapper;
            _numeroRepo = numeroRepo;
            _response = new();
        }
        // inyeccion de la dependencia 

        [HttpGet] //tipo de verbo
        [ProducesResponseType(StatusCodes.Status200OK)] // documentando el codigo de respuesta
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task< ActionResult<APIResponse>> GetNumerosVillas()
        {
            try
            {
                //con loger se puede enviar mensajes de error.
                _logger.LogInformation("Obtener Numero de villas");
                IEnumerable<NumeroVilla> numeroVillaList = await _numeroRepo.ObtenerTodos();
                _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(numeroVillaList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response); //_db.Villas.ToList() es como hacer un select * from
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<String>() { ex.ToString() };
                return _response;
            }


        }

        [HttpGet("id: int", Name = "GeNumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)] // documentando el codigo de respuesta
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id)
        {
            try
            {
                if (id == 0) // verificando que el id sea 0
                {
                    _logger.LogInformation("Error al obtener el numero de villa" + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response); // se retorna el error
                }
                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                var numeroVilla = await _numeroRepo.Obtener(v => v.VillaNo == id);

                if (numeroVilla == null) // verificando que el no exista
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response); // se retorna el error
                }

                _response.Resultado = _mapper.Map<NumeroVillaDto>(numeroVilla);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response); // si todo bien se regresa el resultado
            }
            catch(Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<String>() { ex.ToString() };
                return _response;

            }           
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        // el atriburo FromBody indica que recibira información.
        // VillaDto hace referencia a la clase del modelo.
        // villaDto es una instancia de la Clase VillaDto.
        public async Task<ActionResult<APIResponse>> crearNumeroVilla([FromBody] NumeroVillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    //verifica que lo que los datos enviados este dentro de los parametros indicados
                    return BadRequest(ModelState);
                }

                if (await _numeroRepo.Obtener(v => v.VillaNo == createDto.VillaNo) != null)
                {
                    ModelState.AddModelError("NumeroVillaExiste", "la número de villa con ese numero ya existe!");
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.Obtener(v => v.Id == createDto.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea", "El id de villa proporcionado no existe!");
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
                NumeroVilla modelo = _mapper.Map<NumeroVilla>(createDto);
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;
                await _numeroRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsExitoso = true;
                // villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
                // VillaStore.villaList.Add(villaDto);
                //cuando en la api se crea un nuevo recurso se debe indicar la url de este
                // el primer parametro llama al endpoint get, el segundo parametro es el id que nos pide el endopoint y el tercer parametro es todo el objeto 
                return CreatedAtRoute("GetVilla", new { id = modelo.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<String>() { ex.ToString() };
                return _response;
            }
            
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        //IActionResult no necesita el modelo, cuando se trabaja con delete se devuelve un no content
        public async Task <IActionResult> DeleteNumeroVilla(int id) 
        {
            try
            {

                if (id == 0) // verificando que el id sea 0
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response); // se retorna el error
                }
                var NumeroVilla = await _numeroRepo.Obtener(v => v.VillaNo == id);

                if (NumeroVilla == null) // verificando que el no exista
                {
                    _response.IsExitoso = false;
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    return NotFound(_response); // se retorna el error
                }

                // se elimina la villa de la lista
                await _numeroRepo.Remover(NumeroVilla);
                //VillaStore.villaList.Remove(villa);
                _response.IsExitoso=true;
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok (_response);
                    
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<String>() { ex.ToString() };
                
            }
            return BadRequest(_response);

        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task <IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto updateDto)
        {

            if (updateDto == null || id != updateDto.VillaNo)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (await _villaRepo.Obtener(v => v.Id == updateDto.VillaId) == null)
            {
                ModelState.AddModelError("Clave Foranea", "El Id de la villa No existe");
                return BadRequest(ModelState);
            }
            NumeroVilla modelo = _mapper.Map<NumeroVilla>(updateDto);


            await _numeroRepo.Actualizar(modelo);
            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }

    }
}
