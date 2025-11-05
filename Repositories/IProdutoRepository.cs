using CatalogoApiNovo.Model;

namespace CatalogoApiNovo.Repositories
{
    public interface IProdutoRepository : IRepository<ProdutoModel>
    {
        IEnumerable<ProdutoModel> GetProdutosPorCategoria(int id);
    }
}
