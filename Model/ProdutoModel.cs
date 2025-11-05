using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogoApiNovo.Model
{
    [Table("Produtos")]
    public class ProdutoModel
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required]
        [StringLength(200)]
        public string? Nome { get; set; }

        [Required]
        [StringLength(100)]
        public string Descricao { get; set;}

        [Required]
        [Column(TypeName ="decimal(10,2)")]
        public decimal Preco { get; set; }

        [Required]
        [StringLength(100)]
        public string ImagemUrl { get; set; }
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }

        
        public int CategoriaId {  get; set; }

    }
}
