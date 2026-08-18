using System.ComponentModel.DataAnnotations;

namespace CatalogoApi.API.DTO
{
    public class CategoriaDTO
    {
        [Required]
        public int CategoriaId { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public string ImagemURL { get; set; }
    }
}
