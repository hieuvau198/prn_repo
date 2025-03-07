using AutoMapper;
using PRN222.ProductStore.Repository.Models;
using PRN222.ProductStore.Service.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.ProductStore.Service.Mappers
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<ProductModel, Product>().ReverseMap();
            CreateMap<CategoryModel, Category>().ReverseMap();
            CreateMap<AccountMemberModel, AccountMember>().ReverseMap();
        }
    }
}
