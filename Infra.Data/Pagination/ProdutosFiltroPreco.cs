namespace CatalogoApi.Infra.Data.Pagination
{
    public class ProdutosFiltroPreco : QueryStringParameters
    {
        public decimal? Preco { get; set; }
        public string? PrecoCriterio { get; set; } // "maior" ou "menor" ou "igual"
    }
}
