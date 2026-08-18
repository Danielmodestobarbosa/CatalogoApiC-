namespace CatalogoApi.Infra.Data.Pagination
{
    public class CategoriaFiltroNome : QueryStringParameters
    {
        public string? Nome { get; set; }
    }
}
