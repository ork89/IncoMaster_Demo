using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using GrpcService.Common;
using IncoMasterAPIService.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IncoMasterAPIService.Controllers
{
    public class CategoriesGrpcController : CategoryManagementService.CategoryManagementServiceBase
    {
        private readonly CategoriesService _categoriesService;
        private readonly IMapper _mapper;
        public CategoriesGrpcController(CategoriesService categoriesService, IMapper mapper)
        {
            _categoriesService = categoriesService;
            _mapper = mapper;
        }

        public async override Task<AddNewCategoryResponse> AddNewCategory(AddNewCategoryRequest request, ServerCallContext context)
        {
            try
            {
                if (request != null && request.Category != null)
                {
                    //var newCategory = _mapper.Map<CategoriesModel>(request.Category);
                    var newCategory = new CategoriesModel
                    {
                        Id = request.Category.Id,
                        Category = request.Category.Category,
                        Title = request.Category.Title,
                        Amount = request.Category.Amount,
                        SubmitDate = request.Category.SubmitDate.ToDateTime()
                    };
                    
                    var date = request.Category.SubmitDate.ToDateTime();
                    newCategory.SubmitDate = date;

                    var result = await _categoriesService.CreateAsync(newCategory);

                    if(result == null)
                        return new AddNewCategoryResponse { Error = "Unable to add new category. Result is null or empty" };

                    return new AddNewCategoryResponse
                    {
                        Success = true,
                        CategoryId = result.Id
                    };
                }
                else
                {
                    return new AddNewCategoryResponse { Error = "Unable to add new category. Request is null or empty" };
                }
            }
            catch (Exception ex)
            {
                return new AddNewCategoryResponse { Error = $"{ex.Message}" };
            }
        }

        public async override Task<UpdateCategoryResponse> UpdateCategory(UpdateCategoryRequest request, ServerCallContext context)
        {
            try
            {
                if (request.Category != null)
                {
                    var categoryToUpdate = await _categoriesService.GetByIdAsync(request.Category.Id);
                    if(categoryToUpdate != null)
                    {
                        categoryToUpdate.Category = request.Category.Category;
                        categoryToUpdate.Title = request.Category.Title;
                        categoryToUpdate.Amount = request.Category.Amount;
                        categoryToUpdate.SubmitDate = request.Category.SubmitDate.ToDateTime();
                        await _categoriesService.UpdateAsync(categoryToUpdate);
                    }
                        

                    return new UpdateCategoryResponse
                    {
                        Success = true
                    };
                }
                else
                {
                    return new UpdateCategoryResponse { Error = "Unable to register user" };
                }
            }
            catch (Exception ex)
            {
                return new UpdateCategoryResponse { Error = $"{ex.Message}" };
            }
        }

        public async override Task<DeleteCategoryResponse> DeleteCategory(DeleteCategoryRequest request, ServerCallContext context)
        {
            try
            {
                if (request != null && !string.IsNullOrEmpty(request.Id))
                {
                    await _categoriesService.DeleteAsync(request.Id);

                    return new DeleteCategoryResponse { Success = true };
                }
                else
                {
                    return new DeleteCategoryResponse { Error = "Unable to delete category" };
                }
            }
            catch (Exception ex)
            {
                return new DeleteCategoryResponse { Error = $"{ex.Message}" };
            }
        }

        //public override async Task<GetUserResponse> GetCategory(GetUserRequest request, ServerCallContext context)
        //{
        //    try
        //    {
        //        if (request.Id != null)
        //        {
        //            var user = await _categoriesService.GetByIdAsync(request.Id);

        //            GetUserResponse response = new GetUserResponse();
        //            response.User = _mapper.Map<User>(user);

        //            return response;
        //        }
        //        else
        //        {
        //            return new GetUserResponse
        //            {
        //                Error = "User ID is null or empty"
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return new GetUserResponse
        //        { Error = $"{ ex.Message }" };
        //    };

        //}
    }
}
