using CatalogoApiNovo.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogoApiNovo.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<T> GetAll()
        {
           return  _context.Set<T>().AsNoTracking().ToList();
        }
        public T? Get(Expression<Func<T, bool>> predicate)
        {
          return  _context.Set<T>().FirstOrDefault(predicate);
        }

        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
           //CHAMADO NO UNITOFWORK _context.SaveChanges();
            return entity;
        }
        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            //CHAMADO NO UNITOFWORK _context.SaveChanges();
            return entity;
        }

        public T Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            //CHAMADO NO UNITOFWORK _context.SaveChanges();
            return entity;
        }

    }
}
