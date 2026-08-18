using CatalogoApi.Domain;
using CatalogoApi.Infra.Data.Context;
using CatalogoApi.Infra.Data.Pagination;
using Microsoft.EntityFrameworkCore;


namespace CatalogoApi.Infra.Data.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> GetAllAsync()
        {
            return await _context.Categorias.AsNoTracking().ToListAsync();
        }

        protected IQueryable<Categoria> GetAllAsyncPag()
        {
            return _context.Set<Categoria>().AsNoTracking();
        }

        public async Task<Categoria> GetAsync(int id)
        {
            return await _context.Categorias.FirstOrDefaultAsync(c => c.CategoriaId == id);
        }

        public async Task<Categoria> Create(Categoria categoria)
        {
            if (categoria == null)
            {
                throw new ArgumentNullException(nameof(categoria));
            }

            await _context.Categorias.AddAsync(categoria);
            return categoria;
        }

        public async Task<Categoria> Update(Categoria categoria)
        {
            if (categoria == null)
            {
                throw new ArgumentNullException(nameof(categoria));
            }

            _context.Categorias.Entry(categoria).State = EntityState.Modified;
            return categoria;
        }

        public async Task<Categoria> Delete(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                throw new ArgumentNullException(nameof(categoria));
            }
            _context.Categorias.Remove(categoria);
            return categoria;
        }

        //public async Task<IEnumerable<Categoria>> GetCategorias(CategoriaParameters categoriaParams)
        //{
        //    return await GetAllAsyncPag()
        //       .OrderBy(c => c.Nome)
        //        .Skip((categoriaParams.PageNumber - 1) * categoriaParams.PageSize)
        //        .Take(categoriaParams.PageSize).ToListAsync();
        //}

        public async Task<PagedList<Categoria>> GetCategorias(CategoriaParameters categoriaParameters)
        {
            var categorias = (await GetAllAsync()).OrderBy(c => c.CategoriaId).AsQueryable();
            var produtosOrdenados = await PagedList<Categoria>.ToPagedList(categorias, categoriaParameters.PageNumber, categoriaParameters.PageSize);

            return produtosOrdenados;
        }

        public async Task<PagedList<Categoria>> GetCategoriasFiltroNome(CategoriaFiltroNome categoriaParams)
        {
            var categorias = (await GetAllAsync()).AsQueryable();
            if (!string.IsNullOrEmpty(categoriaParams.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriaParams.Nome));
            }

            var categoriasOrdenadas = await PagedList<Categoria>.ToPagedList(
                        categorias,
                        categoriaParams.PageNumber,
                        categoriaParams.PageSize);

            return categoriasOrdenadas;
        }
    }
}
