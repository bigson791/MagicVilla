﻿using AutoMapper;
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
    public class VillaController : ControllerBase
    {

        // inyeccion de la dependencia
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        //se crea un constructor con ctor
        public VillaController(ILogger<VillaController> logger, IVillaRepositorio villaRepo, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _mapper = mapper;
            _response = new();
        }
        // inyeccion de la dependencia 

        [HttpGet] //tipo de verbo
        [ProducesResponseType(StatusCodes.Status200OK)] // documentando el codigo de respuesta
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task< ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                //con loger se puede enviar mensajes de error.
                _logger.LogInformation("Obtener las villas");
                IEnumerable<Villa> villaList = await _villaRepo.ObtenerTodos();
                _response.Resultado = _mapper.Map<IEnumerable<villaDto>>(villaList);
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

        [HttpGet("id: int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)] // documentando el codigo de respuesta
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0) // verificando que el id sea 0
                {
                    _logger.LogInformation("Error al obtener la villa" + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response); // se retorna el error
                }
                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                var villa = await _villaRepo.Obtener(v => v.Id == id);

                if (villa == null) // verificando que el no exista
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response); // se retorna el error
                }

                _response.Resultado = _mapper.Map<villaDto>(villa);
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
        public async Task<ActionResult<APIResponse>> crearVilla([FromBody] villaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    //verifica que lo que los datos enviados este dentro de los parametros indicados
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.Obtener(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
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
                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;
                await _villaRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsExitoso = true;
                // villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
                // VillaStore.villaList.Add(villaDto);
                //cuando en la api se crea un nuevo recurso se debe indicar la url de este
                // el primer parametro llama al endpoint get, el segundo parametro es el id que nos pide el endopoint y el tercer parametro es todo el objeto 
                return CreatedAtRoute("GetVilla", new { id = modelo.Id }, _response);
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
        public async Task <IActionResult> DeleteVilla(int id) 
        {
            try
            {

                if (id == 0) // verificando que el id sea 0
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response); // se retorna el error
                }
                var villa = await _villaRepo.Obtener(v => v.Id == id);

                if (villa == null) // verificando que el no exista
                {
                    _response.IsExitoso = false;
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    return NotFound(_response); // se retorna el error
                }

                // se elimina la villa de la lista
                await _villaRepo.Remover(villa);
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

        public async Task <IActionResult> UpdateVilla(int id, [FromBody] villaUpdateDto updateDto)
        {

            if (updateDto == null || id != updateDto.Id)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Villa modelo = _mapper.Map<Villa>(updateDto);


            await _villaRepo.Actualizar(modelo);
            _response.IsExitoso = true;
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
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

            var villa = await _villaRepo.Obtener(v => v.Id == id, tracked:false);


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

            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
