using CatalogoApiNovo.Model;

namespace CatalogoApiNovo.DTOs
{
    public static class CategoriaDTOMappingExtensions
    {

        public static CategoriaDTO? ToCategoriaDTO (this CategoriaModel categoria)
        {
            if (categoria is null)
                return null;

            return new CategoriaDTO
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl,
            };
        }

        public static CategoriaModel? ToCategoria (this CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO is null) return null;

            return new CategoriaModel
            {
                CategoriaId = categoriaDTO.CategoriaId,
                Nome = categoriaDTO.Nome,
                ImagemUrl = categoriaDTO.ImagemUrl
            };
        }

        public static IEnumerable<CategoriaDTO> ToCategoriaDTOList (this IEnumerable<CategoriaModel> categorias)
        {
            if(categorias is null || !categorias.Any())
            {
                return new List<CategoriaDTO>();
            }

            return categorias.Select(categoria => new CategoriaDTO
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl= categoria.ImagemUrl,
            }).ToList();
        }

    }
}
