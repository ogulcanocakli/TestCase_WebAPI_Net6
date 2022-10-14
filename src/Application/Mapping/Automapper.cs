using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class Automapper : Profile
    {
        public Automapper()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}
