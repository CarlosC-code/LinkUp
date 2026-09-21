
using AutoMapper;
using LinkUp.Core.Application.Dtos.User;
using LinkUp.Core.Application.ViewModels.User;

namespace LinkUp.Core.Application.Mappings.DtosAndViewModels
{
    public class UserDtoMappingProfile : Profile
    {
        public UserDtoMappingProfile()
        {
            // UserDto <-> UserViewModel
            CreateMap<UserDto, UserViewModel>()
                .ReverseMap();

            // UserDto <-> DeleteUserViewModel
            CreateMap<UserDto, DeleteUserViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ReverseMap()
                // Al volver a UserDto desde DeleteUserViewModel, no tenemos estos campos:
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Phone, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore())
                .ForMember(dest => dest.isVerified, opt => opt.Ignore());

            // UserDto <-> UpdateUserViewModel
            CreateMap<UserDto, UpdateUserViewModel>()
                // Nunca debes mapear password a la vista de edición
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImageFile, opt => opt.Ignore())
                .ReverseMap()
                // Del VM a UserDto no enviamos el archivo; la imagen final suele
                // resolverse por FileManager y luego se pasa como string (ProfileImage)
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());

            // SaveUserDto <-> UpdateUserViewModel
            CreateMap<SaveUserDto, UpdateUserViewModel>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore())
                .ForMember(dest => dest.ProfileImageFile, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());

            // SaveUserDto <-> CreateUserViewModel
            CreateMap<SaveUserDto, CreateUserViewModel>()
                .ForMember(dest => dest.ProfileImageFile, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());

            // SaveUserDto <-> RegisterUserViewModel
            CreateMap<SaveUserDto, RegisterUserViewModel>()
                .ForMember(dest => dest.ProfileImageFile, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());

            // LoginDto <-> LoginViewModel
            CreateMap<LoginDto, LoginViewModel>()
                .ReverseMap();
        }
    }
}
