using CatalogoApi.Domain;
using CatalogoApi.Infra.Data.Pagination;

namespace CatalogoApi.Infra.Data.Repositories
{
    public interface ICategoriaRepository
    {
        Task<PagedList<Categoria>> GetCategorias(CategoriaParameters categoriaParams);
        Task<PagedList<Categoria>> GetCategoriasFiltroNome(CategoriaFiltroNome categoriaParams);
        Task<IEnumerable<Categoria>> GetAllAsync();
        Task<Categoria> GetAsync(int id);
        Task<Categoria> Create (Categoria categoria);
        Task<Categoria> Update (Categoria categoria);
        Task<Categoria> Delete (int id);

    }
}
