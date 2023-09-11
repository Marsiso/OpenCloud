using AutoMapper;
using OpenCloud.Core.Commands.Users;
using OpenCloud.Domain.DataTransferObjects.Authentication;
using OpenCloud.Domain.Models;

namespace OpenCloud.Application.Mappings;

public class UserMappingConfiguration : Profile
{
	public UserMappingConfiguration()
	{
		CreateMap<User, User>()
			.ForMember(destination => destination.UserCreatedBy, options => options.Ignore())
			.ForMember(destination => destination.UserUpdatedBy, options => options.Ignore());

		CreateMap<User, CreateUserCommand>().ReverseMap();
		CreateMap<User, UpdateUserCommand>().ReverseMap();
		CreateMap<GoogleCloudIdentityToken, User>().ReverseMap();
		CreateMap<GoogleCloudIdentityToken, CreateUserCommand>().ReverseMap();
		CreateMap<GoogleCloudIdentityToken, UpdateUserCommand>().ReverseMap();
	}
}