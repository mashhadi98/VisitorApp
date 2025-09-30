using AutoMapper;
using VisitorApp.Contract.Features.Catalog.Categories.Create;
using VisitorApp.Contract.Features.Catalog.Categories.Update;
using VisitorApp.Contract.Features.Catalog.Categories.Delete;
using VisitorApp.Contract.Features.Catalog.Categories.GetById;
using VisitorApp.Contract.Features.Catalog.Categories.GetPaginated;
using VisitorApp.Contract.Features.Catalog.Categories.GetDropdown;
using VisitorApp.Contract.Features.Catalog.Categories.ChangeState;
using VisitorApp.Application.Features.Catalog.Categories.Create;
using VisitorApp.Application.Features.Catalog.Categories.Update;
using VisitorApp.Application.Features.Catalog.Categories.Delete;
using VisitorApp.Application.Features.Catalog.Categories.GetById;
using VisitorApp.Application.Features.Catalog.Categories.GetPaginated;
using VisitorApp.Application.Features.Catalog.Categories.GetDropdown;
using VisitorApp.Application.Features.Catalog.Categories.ChangeState;

namespace VisitorApp.API.Features.Catalog;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Category mappings - API to Application
        CreateMap<CreateCategoryRequest, CreateCategoryCommandRequest>();
        CreateMap<UpdateCategoryRequest, UpdateCategoryCommandRequest>();
        CreateMap<DeleteCategoryRequest, DeleteCategoryCommandRequest>();
        CreateMap<GetByIdCategoryRequest, GetByIdCategoryQueryRequest>();
        CreateMap<GetPaginatedCategoryRequest, GetPaginatedCategoryQueryRequest>();
        CreateMap<GetDropdownCategoryRequest, GetDropdownCategoryQueryRequest>();
        CreateMap<ChangeStateCategoryRequest, ChangeStateCategoryCommandRequest>();
    }
}