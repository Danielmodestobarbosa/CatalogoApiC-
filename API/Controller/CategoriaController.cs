using AutoMapper;
using CatalogoApi.API.DTO;
using CatalogoApi.API.DTO.Mappings;
using CatalogoApi.Domain;
using CatalogoApi.Infra.Data.Context;
using CatalogoApi.Infra.Data.Pagination;
using CatalogoApi.Infra.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CatalogoApi.API.NovaPasta
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {

        private readonly IUnitOfWork _uof;
        private readonly ILogger<CategoriaController> _logger;
        private readonly IMapper _mapper;

        public CategoriaController(IUnitOfWork uof, ILogger<CategoriaController> logger, IMapper mapper)
        {
            _uof = uof;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
      public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoria()
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetAllAsync();
                if (categoria == null)
                {
                    _logger.LogWarning("Nenhuma categoria encontrada");
                    return NotFound("Categoria não encontrada");
                }
                var categoriaDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categoria);

                return Ok(categoriaDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao obter categorias do banco de dados");
            }
            
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public async Task<ActionResult <CategoriaDTO>> GetCategoriaPorId (int id)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetAsync(id);
                if (categoria == null)
                {
                    _logger.LogWarning($"Categoria com id= {id} não encontrada");
                    return NotFound("Categoria não econtrada");
                }

                //var destino = _mapper.Map<CategoriaDTO>(categoria);
                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return Ok(categoriaDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao obter categorias do banco de dados");
            }
            
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasPaginadas([FromQuery] CategoriaParameters categoriaParams)
        {
            try
            {
                var categorias = await _uof.CategoriaRepository.GetCategorias(categoriaParams);
                if (categorias == null || !categorias.Any())
                {
                    _logger.LogWarning("Nenhuma categoria encontrada");
                    return NotFound("Categoria não encontrada");
                }
                return await ObterCategorias(categorias);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao obter categorias do banco de dados");
            }
        }

        [HttpGet("filter/nome/pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradas([FromQuery] CategoriaFiltroNome categoriasFiltro)
        {
            var categoriasFiltradas = await _uof.CategoriaRepository.GetCategoriasFiltroNome(categoriasFiltro);
            return await ObterCategorias(categoriasFiltradas);
        }

       private async Task<ActionResult<IEnumerable<CategoriaDTO>>> ObterCategorias(PagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
            return Ok(categoriasDto);
        }

        [HttpPost]
        public async Task<ActionResult <Categoria>> Post (CategoriaDTO categoriaDto)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDto);
                var categoriaCriada = await _uof.CategoriaRepository.Create(categoria);

                _uof.Commit();

                var novaCategoriaDto = _mapper.Map<CategoriaDTO>(categoriaCriada);

                return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    "Erro ao obter categorias do banco de dados");
            }
           
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult <Categoria>> Put (int id, CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.CategoriaId)
                {
                    _logger.LogWarning($"O ID informado é diferente do ID da categoria. ID informado: " +
                                       $" {id}, ID da categoria: {categoriaDto.CategoriaId}");
                    return BadRequest("O ID informado é diferente do ID da categoria.");
                }

                var categoria = _mapper.Map<Categoria>(categoriaDto);
                var categoriaExistente = await _uof.CategoriaRepository.Update(categoria);

                _uof.Commit();

                var categoriaAtualizadaDto = _mapper.Map<Categoria>(categoriaExistente);

                return Ok(categoriaAtualizadaDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       "Erro ao obter categorias do banco de dados");
            }
            
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult <Categoria>> Delete (int id)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetAsync(id);

                if (categoria == null)
                {
                    _logger.LogWarning($"Categoria com id= {id} não encontrada");
                    return NotFound("Categoria não encontrada");
                }

                var categoriaExcluida = await _uof.CategoriaRepository.Delete(id);
                _uof.Commit();

                var categoriaExcluidaDto = _mapper.Map<CategoriaDTO>(categoriaExcluida);

                return Ok(categoriaExcluidaDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                       "Erro ao obter categorias do banco de dados");
            }
            
        }

    }
    
}
