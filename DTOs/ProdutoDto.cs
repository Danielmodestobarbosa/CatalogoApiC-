using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogoApiNovo.DTOs
{
    public class ProdutoDto
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required]
        [StringLength(200)]
        public string? Nome { get; set; }

        [Required]
        [StringLength(100)]
        public string Descricao { get; set; }

        public decimal Preco { get; set; }

        [Required]
        [StringLength(100)]
        public string ImagemUrl { get; set; }

    }
}
