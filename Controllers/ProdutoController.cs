using CatalogoApiNovo.Data;
using CatalogoApiNovo.Filters;
using CatalogoApiNovo.Model;
using CatalogoApiNovo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApiNovo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        //Injeção de dependência
        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;

        public ProdutoController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<ProdutoModel>> GetProdutosCategorias(int id)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);
            if(produtos == null)
            {
                _logger.LogWarning("Produto é nulo");
                return NotFound("Produito é nulo");
            }

            return Ok(produtos);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public ActionResult<IEnumerable<ProdutoModel>> ListaTodosProdutos()
        {
            var produto = _uof.ProdutoRepository.GetAll().ToList(); 
            if(produto is null)
            {
                _logger.LogWarning("O produto é nulo");
                return NotFound("O produto é nulo");
            }

            return Ok(produto);

        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<ProdutoModel> ObterProdutoPorId(int id)
        {
            var produto = _uof.ProdutoRepository.Get(c => c.ProdutoId == id);

            if(produto is null)
            {
                _logger.LogWarning($"Produto com id= {id} não encontrado");
                return NotFound($"Produto com id= {id} não encontrado");
            }

            return Ok(produto);
        }

        [HttpPost]
        public ActionResult  AdicionaProduto (ProdutoModel produto)
        {
            if (produto is null)
            {
                _logger.LogWarning($"Dados inválidos");
                  return StatusCode(StatusCodes.Status500InternalServerError, "Dados inválidos");
            }

             var produtoCriado = _uof.ProdutoRepository.Create(produto);
            _uof.Commit();

            return Created($"/api/Produtos/{produtoCriado.ProdutoId}", produtoCriado);
        }

        [HttpPut("{id:int}")]
        public ActionResult AtualizaProduto (int id, ProdutoModel produto)
        {
            if(id != produto.ProdutoId)
            {
                _logger.LogWarning($"Produto com id= {id} não encontrado");
                return StatusCode(StatusCodes.Status400BadRequest, $"\"Produto com id= {id} não encontrado");
            }

          var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
         _uof.Commit();

         return Ok(produtoAtualizado);
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeletarProduto(int id)
        {
            var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);
            if (id != produto.ProdutoId)
            {
                _logger.LogWarning($"Produto com id= {id} não encontrado");
                return StatusCode(StatusCodes.Status400BadRequest, $"\"Produto com id= {id} não encontrado");
            }

           var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();

            return Ok(produtoDeletado);
        }
    }
}
