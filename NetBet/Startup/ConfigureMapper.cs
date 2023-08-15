using AutoMapper;
using NetBet.DataAccess.Models;
using NetBet.Models.Dtos;
using NetBet.Services.Validations;

namespace NetBet.Startup;

public class ConfigureMapper : Profile
{
    public ConfigureMapper()
    {
        CreateMap<AddCarRequest, Car>();
        CreateMap<Car, AddCarResponse>().ForMember(des => des.CarId, op => op.MapFrom(src => src.Id));
        CreateMap<Car, CarItem>();
        CreateMap<Car, GetCarQueryResponse>();


        


        //CreateMap<NetBetException, AddCarResponse>();

        CreateMap<CarRentalRequest, RentDetails>();
        CreateMap<RentDetails, CarRentalResponse>().
            ForMember(des => des.RentId, op => op.MapFrom(src => src.Id));


        CreateMap<RentDetails, CarRentalItem>()
            .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.From.ToShortDateString()))
            .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.To.ToShortDateString()));
    }
}
