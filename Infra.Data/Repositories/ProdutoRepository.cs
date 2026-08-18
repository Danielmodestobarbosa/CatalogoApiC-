using CatalogoApi.Domain;
using CatalogoApi.Infra.Data.Context;
using CatalogoApi.Infra.Data.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApi.Infra.Data.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Produto>> GetProdutosAsync()
        {
            return await _context.Produtos.ToListAsync();
        }

        public Produto GetProduto(int id)
        {
            return _context.Produtos.FirstOrDefault (p => p.ProdutoId == id);
        }

        public Produto Create(Produto produto)
        {
            if(produto == null)
            {
                throw new ArgumentNullException(nameof(produto));
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return produto;
        }

        public Produto Update(Produto produto)
        {
            if (produto == null)
            {
                throw new ArgumentNullException(nameof(produto));
            }

            _context.Produtos.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();
            return produto;
        }

        public Produto Delete(int id)
        {
            var produto = _context.Produtos.Find(id);
            if (produto == null)
            {
                throw new ArgumentNullException(nameof(produto));
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return produto;
        }

        public async Task<PagedList<Produto>> GetProdutosFiltroPedro(ProdutosFiltroPreco produtosFiltroParams)
        {
            var produtos = (await GetProdutosAsync()).AsQueryable();
            if(produtosFiltroParams.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroParams.PrecoCriterio))
            {
                if(produtosFiltroParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco > produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
                }
                else if(produtosFiltroParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco < produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
                }
                else if(produtosFiltroParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco == produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco); 
                }
            }
            var produtosFiltrados = await PagedList<Produto>.ToPagedList(produtos, produtosFiltroParams.PageNumber, produtosFiltroParams.PageSize);
            
            return produtosFiltrados;
        }
    }
}
