using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IncoMasterAPIService.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Models.CategoriesModel, GrpcService.Common.Category>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));

            CreateMap<Models.UserModel, GrpcService.Common.User>()
                .ForMember(d => d.FName, op => op.MapFrom(src => src.FirstName))
                .ForMember(d => d.LName, op => op.MapFrom(src => src.LastName))
                .ForMember(d => d.Income, op => op.MapFrom(src => src.IncomeList))
                .ForMember(d => d.Expenses, op => op.MapFrom(src => src.ExpensesList))
                .ForMember(d => d.Savings, op => op.MapFrom(src => src.SavingsList))
                .ForMember(d => d.Loans, op => op.MapFrom(src => src.LoansList));
        }
    }
}
