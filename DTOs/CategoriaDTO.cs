using System.ComponentModel.DataAnnotations;

namespace CatalogoApiNovo.DTOs
{
    public class CategoriaDTO
    {

        [Key]
        public int CategoriaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(200)]
        public string ImagemUrl { get; set; }

    }
}
