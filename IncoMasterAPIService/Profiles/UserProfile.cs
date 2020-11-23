using AutoMapper;
using Google.Protobuf.WellKnownTypes;

namespace IncoMasterAPIService.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Models.CategoriesModel, GrpcService.Common.SingleCategory>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount)).ReverseMap();
            //.ForMember(dest => dest.SubmitDate, opt => opt.MapFrom(src => src.SubmitDate.ToTimestamp()))

            CreateMap<Models.CategoriesModel, GrpcService.Common.Category>()
                .ForMember(dest => dest.Category_, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.SubmitDate, opt => opt.MapFrom(src => src.SubmitDate.ToTimestamp()));


            CreateMap<Models.UserModel, GrpcService.Common.User>()
                .ForMember(d => d.FName, op => op.MapFrom(src => src.FirstName))
                .ForMember(d => d.LName, op => op.MapFrom(src => src.LastName))
                .ForMember(d => d.Email, op => op.MapFrom(src => src.Email))
                .ForMember(d => d.Password, op => op.MapFrom(src => src.Password))
                .ForMember(d => d.Income, op => op.MapFrom(src => src.IncomeList))
                .ForMember(d => d.Expenses, op => op.MapFrom(src => src.ExpensesList))
                .ForMember(d => d.Savings, op => op.MapFrom(src => src.SavingsList))
                .ForMember(d => d.Loans, op => op.MapFrom(src => src.LoansList)).ReverseMap();
        }
    }
}
