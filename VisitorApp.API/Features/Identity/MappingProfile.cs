using AutoMapper;
using VisitorApp.API.Features.Identity.Users.Create;
using VisitorApp.API.Features.Identity.Users.Update;
using VisitorApp.API.Features.Identity.Users.Delete;
using VisitorApp.API.Features.Identity.Users.GetById;
using VisitorApp.API.Features.Identity.Users.GetPaginated;
using VisitorApp.API.Features.Identity.Users.ChangeState;
using VisitorApp.API.Features.Identity.Roles.Create;
using VisitorApp.API.Features.Identity.Roles.Update;
using VisitorApp.API.Features.Identity.Roles.Delete;
using VisitorApp.API.Features.Identity.Roles.GetById;
using VisitorApp.API.Features.Identity.Roles.GetPaginated;
using VisitorApp.API.Features.Identity.Roles.ChangeState;
using VisitorApp.Application.Features.Identity.Users.Create;
using VisitorApp.Application.Features.Identity.Users.Update;
using VisitorApp.Application.Features.Identity.Users.Delete;
using VisitorApp.Application.Features.Identity.Users.GetById;
using VisitorApp.Application.Features.Identity.Users.GetPaginated;
using VisitorApp.Application.Features.Identity.Users.ChangeState;
using VisitorApp.Application.Features.Identity.Roles.Create;
using VisitorApp.Application.Features.Identity.Roles.Update;
using VisitorApp.Application.Features.Identity.Roles.Delete;
using VisitorApp.Application.Features.Identity.Roles.GetById;
using VisitorApp.Application.Features.Identity.Roles.GetPaginated;
using VisitorApp.Application.Features.Identity.Roles.ChangeState;

namespace VisitorApp.API.Features.Identity;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings - API to Application
        CreateMap<CreateUserRequest, CreateUserCommandRequest>();
        CreateMap<UpdateUserRequest, UpdateUserCommandRequest>();
        CreateMap<DeleteUserRequest, DeleteUserCommandRequest>();
        CreateMap<GetByIdUserRequest, GetByIdUserQueryRequest>();
        CreateMap<GetPaginatedUserRequest, GetPaginatedUserQueryRequest>();
        CreateMap<ChangeStateUserRequest, ChangeStateUserCommandRequest>();
        
        // Filter mappings
        CreateMap<VisitorApp.API.Features.Identity.Users.GetPaginated.GetPaginatedUserFilter, VisitorApp.Application.Features.Identity.Users.GetPaginated.GetPaginatedUserFilter>();

        // Role mappings - API to Application
        CreateMap<CreateRoleRequest, CreateRoleCommandRequest>();
        CreateMap<UpdateRoleRequest, UpdateRoleCommandRequest>();
        CreateMap<DeleteRoleRequest, DeleteRoleCommandRequest>();
        CreateMap<GetByIdRoleRequest, GetByIdRoleQueryRequest>();
        CreateMap<GetPaginatedRoleRequest, GetPaginatedRoleQueryRequest>();
        CreateMap<ChangeStateRoleRequest, ChangeStateRoleCommandRequest>();
        
        // Filter mappings
        CreateMap<VisitorApp.API.Features.Identity.Roles.GetPaginated.GetPaginatedRoleFilter, VisitorApp.Application.Features.Identity.Roles.GetPaginated.GetPaginatedRoleFilter>();
    }
} 