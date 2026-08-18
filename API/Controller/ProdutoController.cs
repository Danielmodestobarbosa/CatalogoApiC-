using CatalogoApi.API.DTO;
using CatalogoApi.Domain;
using CatalogoApi.Infra.Data.Context;
using CatalogoApi.Infra.Data.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CatalogoApi.API.NovaPasta
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            try
            {
                var produtos = await _context.Produtos.AsNoTracking().Take(10).ToListAsync();

                if (produtos == null)
                {
                    return NotFound("Produtos não encontrados");
                }

                return Ok(produtos);
            }
            catch (Exception)
            {

                return StatusCode (StatusCodes.Status500InternalServerError, 
                    "Erro ao obter produtos do banco de dados");
            }
            
        }

        [HttpGet("filter/preco/pagination")]
        /*public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosFilterPreco([FromQuery] ProdutosFiltroPreco
                                                                                        produtosFilterParams)
        {
            var produtos = _uof.ProdutoRepository.GetProdutosFiltroPreco(produtosFilterParams);
            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtosDto = _mapper.Map<IEnumerable<CategoriaDTO>>(produtos);
            return Ok(produtosDto);
        }*/

        [HttpGet("{id}", Name= "ObterProduto")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            try
            {
                var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);

                if (produto == null)    
                {
                    return NotFound("Produto não encontrado");
                }

                return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       "Erro ao obter produtos do banco de dados");
            }
          
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> Post (Produto produto)
        {
            try
            {
                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       "Erro ao obter produtos do banco de dados");

            }

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Produto>> Put (int id, Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                {
                    return BadRequest();
                }

                _context.Entry(produto).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       "Erro ao obter produtos do banco de dados");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Produto>> Delete (int id)
        {
            try
            {
                var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);

                if (produto is null)
                {
                    return NotFound("Produto não encontrado");
                }

                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();

                return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       "Erro ao obter produtos do banco de dados");
            }
            
        }
        
    }
}
