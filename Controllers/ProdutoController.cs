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
        private readonly IProdutoRepository _produtoRepository;
        private readonly ILogger _logger;

        public ProdutoController(IProdutoRepository produtoRepository, ILogger<ProdutoController> logger)
        {
            _produtoRepository = produtoRepository;
            _logger = logger;
        }

        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<ProdutoModel>> GetProdutosCategorias(int id)
        {
            var produtos = _produtoRepository.GetProdutosPorCategoria(id);
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
            var produto = _produtoRepository.GetAll().ToList(); 
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
            var produto = _produtoRepository.Get(c => c.ProdutoId == id);

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

             var produtoCriado = _produtoRepository.Create(produto);

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

          var produtoAtualizado = _produtoRepository.Update(produto);

         return Ok(produtoAtualizado);
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeletarProduto(int id)
        {
            var produto = _produtoRepository.Get(p => p.ProdutoId == id);
            if (id != produto.ProdutoId)
            {
                _logger.LogWarning($"Produto com id= {id} não encontrado");
                return StatusCode(StatusCodes.Status400BadRequest, $"\"Produto com id= {id} não encontrado");
            }

           var produtoDeletado = _produtoRepository.Delete(produto);

            return Ok(produtoDeletado);
        }
    }
}
