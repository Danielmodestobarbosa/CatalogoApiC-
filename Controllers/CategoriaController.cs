using CatalogoApiNovo.Data;
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

        private readonly ICategoriaRepository _repository;
        private readonly ILogger<CategoriaController> _logger;

        public CategoriaController(ICategoriaRepository repository, ILogger<CategoriaController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaModel> ListaCategoriaPorId (int id)
        {

            var categoria = _repository.ListaCategoriaPorId(id);

            if(categoria == null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada");
                return NotFound($"Categoria com id= {id} não encontrada");
            }

            return Ok(categoria);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public ActionResult<IEnumerable<CategoriaModel>> ListaTodasCategorias()
        {
            var categorias = _repository.ListaTodasCategorias();
            return Ok(categorias);
        }

        [HttpPost]
        public ActionResult AdicionaCategoria(CategoriaModel categoria)
        {
            if (categoria is null)
            {
                _logger.LogWarning($"Dados inválidos");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Dados inválidos");
            }

            var CategoriaCriada = _repository.AdicionaCategoria(categoria);

            return new CreatedAtActionResult("ListaTodasCategorias", "CategoriaController", new { id = CategoriaCriada.CategoriaId }, CategoriaCriada);
        }

        [HttpPut("{id}")]
        public ActionResult AtualizaCategoria(int id,  CategoriaModel categoria)
        {
            if (id != categoria.CategoriaId)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrado");
                return StatusCode(StatusCodes.Status400BadRequest, $"Categoria com id= {id} não encontrado");
            }
            _repository.AtualizaCategoria(categoria);

            return Ok(categoria);
            }

        [HttpDelete("{id}")]
        public ActionResult DeletaCategoria (int id)
        {
            var categoria = _repository.ListaCategoriaPorId(id);
            if(categoria is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrado");
                return StatusCode(StatusCodes.Status404NotFound, $"Categoria com id= {id} não encontrado");
            }

           var categoriaExcluida = _repository.DeletaCategoria(id);

            return Ok(categoriaExcluida);
        }
        }

    }

