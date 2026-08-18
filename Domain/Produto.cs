using CatalogoApi.Infra.Data.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CatalogoApi.Domain
{
    [Table("Produtos")]
    public class Produto
    {

        [Key]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter entre 5 e 100 caracteres.", MinimumLength = 5)]
        [PrimeiraLetraMaiuscula]
        public string nome { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição deve ter entre 10 e 500 caracteres.", MinimumLength = 10)]
        public string descricao { get; set; }

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "A URL da imagem é obrigatória.")]
        [StringLength(200, ErrorMessage = "A URL da imagem deve ter entre 10 e 200 caracteres.", MinimumLength = 10)]
        public string ImagemUrl { get; set; }

        public float Estoque { get; set; }

        public DateTime DataCadastro { get; set; }

        public int CategoriaId { get; set; }

        [JsonIgnore]
        public Categoria? Categoria { get; set; }

    }
}
