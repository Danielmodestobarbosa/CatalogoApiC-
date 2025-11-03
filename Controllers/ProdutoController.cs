using CatalogoApiNovo.Data;
using CatalogoApiNovo.Filters;
using CatalogoApiNovo.Model;
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
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public ProdutoController(AppDbContext context, ILogger<ProdutoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public async Task <IEnumerable<ProdutoModel>> BuscarTodosProdutos()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<ProdutoModel> ObterProdutoPorId(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p=> p.ProdutoId == id);

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

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return Created($"/api/Produtos/{produto.ProdutoId}", produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult AtualizaProduto (int id, ProdutoModel produto)
        {
            if(id != produto.ProdutoId)
            {
                _logger.LogWarning($"Produto com id= {id} não encontrado");
                return StatusCode(StatusCodes.Status400BadRequest, $"\"Produto com id= {id} não encontrado");
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeletarProduto(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (produto is null)
            {
                _logger.LogWarning($"Produco com {id} não encotrado");
                return StatusCode(StatusCodes.Status404NotFound, $"Produco com {id} não encotrado");
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
