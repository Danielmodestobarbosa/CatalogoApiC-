using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogoApiNovo.Model
{
    [Table("Categorias")]
    public class CategoriaModel
    {
        public CategoriaModel()
        {
            Produtos = new Collection<ProdutoModel>();
        }
        [Key]
        public int CategoriaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(200)]
        public string ImagemUrl { get; set; }
        
        public ICollection<ProdutoModel> Produtos { get; set; }

    }
}
