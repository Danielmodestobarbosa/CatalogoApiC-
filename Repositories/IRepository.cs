using System.Linq.Expressions;

namespace CatalogoApiNovo.Repositories
{
    public interface IRepository<T>
    {
           //Não violar os principios ISP
           IEnumerable<T> GetAll(); 
           T? Get (Expression<Func<T, bool>> predicate);
           T Create (T entity);
           T Update (T entity);
           T Delete (T entity);
            
    }
}
