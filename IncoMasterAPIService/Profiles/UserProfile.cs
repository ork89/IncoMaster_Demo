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
            CreateMap<Models.CategoriesModel, GrpcService.Common.Category>();
            CreateMap<Models.UserModel, GrpcService.Common.User>()
                .ForMember(d => d.Income, op => op.MapFrom(src => src.IncomeList))
                .ForMember(d => d.Expenses, op => op.MapFrom(src => src.ExpensesList))
                .ForMember(d => d.Savings, op => op.MapFrom(src => src.SavingsList))
                .ForMember(d => d.Loans, op => op.MapFrom(src => src.LoansList))
                .ReverseMap();
        }
    }
}
