using CatalogoApiNovo.Data;
using CatalogoApiNovo.Filters;
using CatalogoApiNovo.Model;
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

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CategoriaController(AppDbContext context, IConfiguration configuration, ILogger<CategoriaController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("LerArquivoConfiguracao")]
        public string GetValores()
        {
            var valor1 = _configuration["chave1"];
            var valor2 = _configuration["chave2"];

            var secao1 = _configuration["secao1:chave2"];

            return $"Chave1 = {valor1} \nChave2 = {valor2} \nSeção1 => Chave2 = {secao1}";
           
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaModel> ListaCategoriaPorId (int id)
        {

            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

            if(categoria == null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada");
                return NotFound($"Categoria com id= {id} não encontrada");
            }

            return Ok(categoria);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLogginFilter))]
        public async Task<ActionResult<IEnumerable<CategoriaModel>>> ListaTodasCategorias()
        {
            return await _context.Categorias.AsNoTracking().ToListAsync();
        }

        [HttpPost]


    }
}
