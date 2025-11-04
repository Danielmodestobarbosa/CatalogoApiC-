using CatalogoApiNovo.Data;
using CatalogoApiNovo.Model;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApiNovo.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public CategoriaModel ListaCategoriaPorId(int id)
        {
           return _context.Categorias.FirstOrDefault(c=> c.CategoriaId == id);
        }

        public IEnumerable<CategoriaModel> ListaTodasCategorias()
        {
            return _context.Categorias.ToList();
        }

        public CategoriaModel AdicionaCategoria(CategoriaModel categoria)
        {
            if(categoria is null)
            {
                throw new ArgumentException(nameof(categoria));
            }

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return categoria;
        }
        public CategoriaModel AtualizaCategoria(CategoriaModel categoria)
        {
            if(categoria is null)
            {
                throw new ArgumentException(nameof (categoria));
            }

            _context.Categorias.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return categoria;
        }

        public CategoriaModel DeletaCategoria(int id)
        {
            var categoria = _context.Categorias.Find(id);
            if (categoria is null)
            {
                throw new ArgumentException(nameof(categoria));
            }

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return categoria;
        }

       
    }
}
