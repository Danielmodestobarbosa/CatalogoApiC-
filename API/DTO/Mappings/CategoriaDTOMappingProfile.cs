using AutoMapper;
using CatalogoApi.Domain;

namespace CatalogoApi.API.DTO.Mappings
{
    public class CategoriaDTOMappingProfile : Profile
    {

        public CategoriaDTOMappingProfile()
        {
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        }

    }
}
