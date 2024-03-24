using AutoMapper;
using OLAP.API.Models.Identity;

namespace OLAP.API.Infrastructure.AutoMapperExtensions
{
    public class AccountProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public AccountProfile()
        {
            //CreateMap<RegisterPartnerCommand, ApplicationUser>();
            //CreateMap<RegisterUserCommand, ApplicationUser>();

            //CreateMap<ApplicationUser, ApplicationUserResponseModel>()
            //    .ForMember(d => d.Id, src => src.MapFrom(s => Guid.Parse(s.Id)));
        }
    }
}
