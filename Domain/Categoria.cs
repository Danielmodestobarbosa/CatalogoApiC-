using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogoApi.Domain
{
    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(50, ErrorMessage = "O nome deve ter entre 5 e 50 caracteres.", MinimumLength = 5)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]


        [StringLength(200, ErrorMessage = "A descrição deve ter entre 10 e 200 caracteres.", MinimumLength = 10)]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A URL da imagem é obrigatória.")]
        [StringLength(200, ErrorMessage = "A URL da imagem deve ter entre 10 e 200 caracteres.", MinimumLength = 10)]
        public string ImagemUrl { get; set; }
        public ICollection<Produto>? Produtos { get; set; }

    }
}
