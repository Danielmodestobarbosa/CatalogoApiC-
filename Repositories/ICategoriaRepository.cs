using CatalogoApiNovo.Model;

namespace CatalogoApiNovo.Repositories
{
    public interface ICategoriaRepository
    {

        IEnumerable<CategoriaModel> ListaTodasCategorias();
        CategoriaModel ListaCategoriaPorId (int id);
        CategoriaModel AdicionaCategoria(CategoriaModel categoria);
        CategoriaModel AtualizaCategoria(CategoriaModel categoria);
        CategoriaModel DeletaCategoria (int id);

    }
}
