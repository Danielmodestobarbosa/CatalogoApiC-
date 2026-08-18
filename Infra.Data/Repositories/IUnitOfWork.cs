namespace CatalogoApi.Infra.Data.Repositories
{
    public interface IUnitOfWork
    {

        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }

        void Commit();


    }
}
