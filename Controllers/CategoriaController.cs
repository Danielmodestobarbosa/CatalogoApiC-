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
                var categoriasDto = categoria.ToCategoriaDTO();


                return Ok(categoriasDto);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public ActionResult<IEnumerable<CategoriaDTO>> ListaTodasCategorias()
        {
            var categorias = _uof.CategoriaRepository.GetAll();
            
            var categoriasDto = categorias.ToCategoriaDTOList();

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
                
                var categoria = categoriaDto.ToCategoria();

                var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
                _uof.Commit();

            var novaCategoriaDto = categoriaCriada.ToCategoriaDTO();

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

            var categoria = categoriaDto.ToCategoria();

                var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
                _uof.Commit();

                var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDTO

            return Ok(categoriaAtualizadaDto);
            }

        [HttpDelete("{id}")]
        public ActionResult<CategoriaDTO> DeletaCategoria (int id)
        {
                var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);
                if (categoria is null)
                {
                        _logger.LogWarning($"Categoria com id= {id} não encontrado");
                        return StatusCode(StatusCodes.Status404NotFound, $"Categoria com id= {id} não encontrado");
                }

                var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
                _uof.Commit();

                var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDTO ();

                return Ok(categoriaExcluidaDto);
        }
        }

    }

