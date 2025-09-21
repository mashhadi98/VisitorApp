using AutoMapper;
using VisitorApp.API.Features.Identity.Login;
using VisitorApp.API.Features.Identity.Register;
using VisitorApp.Application.Features.Identity.Login;
using VisitorApp.Application.Features.Identity.Register;

namespace VisitorApp.API.Features.Identity;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Register mappings
        CreateMap<RegisterRequest, RegisterCommandRequest>();
        CreateMap<RegisterCommandResponse, RegisterRequest>();

        // Login mappings
        CreateMap<LoginRequest, LoginCommandRequest>();
        CreateMap<LoginCommandResponse, LoginRequest>();
    }
} 