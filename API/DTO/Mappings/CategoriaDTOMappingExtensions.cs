using CatalogoApi.Domain;
using System.Runtime.CompilerServices;

namespace CatalogoApi.API.DTO.Mappings
{
    public static class CategoriaDTOMappingExtensions
    {
        public static CategoriaDTO? ToCategoriaDTO(this Categoria categoria)
        {
            if(categoria == null)
            
                return null;
            
            return new CategoriaDTO
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                Descricao = categoria.Descricao,
                ImagemURL = categoria.ImagemUrl,
            };
        }

        public static Categoria? ToCategoria (this CategoriaDTO categoriaDto)
        {
            if(categoriaDto == null)
            
                return null;

            return new Categoria
            {
                CategoriaId = categoriaDto.CategoriaId,
                Nome = categoriaDto.Nome,
                Descricao = categoriaDto.Descricao,
                ImagemUrl = categoriaDto.ImagemURL
            };
        }

        public static IEnumerable<CategoriaDTO> ToCategoriaDTOList (this IEnumerable<Categoria> categorias)
        {
             if(categorias == null || !categorias.Any())
            {
                return Enumerable.Empty<CategoriaDTO>();
            }

            return categorias.Select(categoria => new CategoriaDTO
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                Descricao = categoria.Descricao,
                ImagemURL = categoria.ImagemUrl
            }).ToList();

        }
    }
}
