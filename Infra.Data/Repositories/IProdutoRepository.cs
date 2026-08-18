using CatalogoApi.Domain;
using CatalogoApi.Infra.Data.Pagination;

namespace CatalogoApi.Infra.Data.Repositories
{
    public interface IProdutoRepository 
    {

        Task<IEnumerable<Produto>> GetProdutosAsync();
        Task<PagedList<Produto>> GetProdutosFiltroPedro (ProdutosFiltroPreco produtosFiltroParams);
        Produto GetProduto(int id);
        Produto Create (Produto produto);
        Produto Update (Produto produto);
        Produto Delete (int id);

    }
}
