using CatalogoApiNovo.Data;
using CatalogoApiNovo.Model;
using Microsoft.EntityFrameworkCore;

namespace CatalogoApiNovo.Repositories
{
    public class CategoriaRepository : Repository<CategoriaModel>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }



    }
}
