using System.Linq;
using AutoMapper;
using Demo.DTOs;
using Demo.Models;

namespace Demo.Helpers {
    public class AutoMapperProfiles : Profile {
        public AutoMapperProfiles () {
            CreateMap<User, UserForListDto> ()
                .ForMember (dest => dest.PhotoUrl, opt => {
                    opt.MapFrom (src => src.Photos.FirstOrDefault (p => p.IsMain).Url);
                })
                .ForMember (dest => dest.Age, opt => {
                    opt.ResolveUsing (d => d.DateOfBirth.CalculateAge ());
                });
            CreateMap<User, UserForDetailedDto> ()
                .ForMember (dest => dest.PhotoUrl, opt => {
                    opt.MapFrom (src => src.Photos.FirstOrDefault (p => p.IsMain).Url);
                })
                .ForMember (dest => dest.Age, opt => {
                    opt.ResolveUsing (d => d.DateOfBirth.CalculateAge ());
                });
            CreateMap<Photo, PhotoForDetailsDto> ();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<Photo, PhotoFromReturnDto>();
            CreateMap<UserForRegisterDto, User>();

        }
    }
}