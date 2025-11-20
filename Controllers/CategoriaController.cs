using CatalogoApiNovo.Data;
using CatalogoApiNovo.DTOs;
using CatalogoApiNovo.Filters;
using CatalogoApiNovo.Model;
using CatalogoApiNovo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CatalogoApiNovo.Controllers
{
    [Route("(categorias)")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {

        private readonly IUnitOfWork _uof;
        private readonly ILogger<CategoriaController> _logger;

        public CategoriaController(IUnitOfWork uof, ILogger<CategoriaController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> ListaCategoriaPorId (int id)
        {

            var categoria = _uof.CategoriaRepository.Get(c=> c.CategoriaId == id);


            if (categoria == null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada");
                return NotFound($"Categoria com id= {id} não encontrada");
            }
            //Mapeando o DTO
            var categoriaDto = new CategoriaDTO()
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            };


            return Ok(categoriaDto);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public ActionResult<IEnumerable<CategoriaDTO>> ListaTodasCategorias()
        {
            var categorias = _uof.CategoriaRepository.GetAll();

            var categoriasDto = new List<CategoriaDTO>();
            foreach (var categoria in categorias)
            {
                var categoriaDto = new CategoriaDTO
                {
                    CategoriaId = categoria.CategoriaId,
                    Nome = categoria.Nome,
                    ImagemUrl = categoria.ImagemUrl
                };
            }

            return Ok(categoriasDto);

        }

        [HttpPost]
        public ActionResult<CategoriaDTO> AdicionaCategoria(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning($"Dados inválidos");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Dados inválidos");
            }

            var categoria = new CategoriaModel()
            {
                CategoriaId = categoriaDto.CategoriaId,
                Nome = categoriaDto.Nome,
                ImagemUrl = categoriaDto.ImagemUrl
            };

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            var novaCategoriaDto = new CategoriaDTO()
            {
                CategoriaId = categoriaCriada.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            };

            return new CreatedAtActionResult("ListaTodasCategorias", "CategoriaController", new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
        }

        [HttpPut("{id}")]
        public ActionResult<CategoriaDTO> AtualizaCategoria(int id,  CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrado");
                return StatusCode(StatusCodes.Status400BadRequest, $"Categoria com id= {id} não encontrado");
            }

            var categoria = new CategoriaModel()
            {
                CategoriaId = categoriaDto.CategoriaId,
                Nome = categoriaDto.Nome,
                ImagemUrl = categoriaDto.ImagemUrl
            };

           var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            var categoriaAtualizadaDto = new CategoriaDTO()
            {
                CategoriaId = categoriaAtualizada.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            };

            return Ok(categoriaAtualizadaDto);
            }

        [HttpDelete("{id}")]
        public ActionResult<CategoriaDTO> DeletaCategoria (int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);
            if(categoria is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrado");
                return StatusCode(StatusCodes.Status404NotFound, $"Categoria com id= {id} não encontrado");
            }

           var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            var categoriaExcluidaDto = new CategoriaDTO()
            {
                CategoriaId = categoriaExcluida.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl
            };

            return Ok(categoriaExcluidaDto);
        }
        }

    }

