using CatalogoApiNovo.Controllers;
using CatalogoApiNovo.Data;
using CatalogoApiNovo.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace CatalogoApiNovo.Repositories
{
    public class ProdutoRepository : Repository<ProdutoModel>, IProdutoRepository
    {

        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<ProdutoModel> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c=> c.CategoriaId == id);
        }
    }
}
