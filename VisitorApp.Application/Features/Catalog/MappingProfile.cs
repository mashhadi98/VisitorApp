using AutoMapper;
using VisitorApp.Application.Features.Catalog.Products.Create;
using VisitorApp.Domain.Features.Catalog.Entities;

namespace VisitorApp.Application.Features.Catalog;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product mappings
        CreateMap<CreateProductCommandRequest, Product>()
            .ConstructUsing(src => new Product(src.Title, src.Description ?? string.Empty))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
            
        CreateMap<Product, CreateProductCommandResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
    }
}