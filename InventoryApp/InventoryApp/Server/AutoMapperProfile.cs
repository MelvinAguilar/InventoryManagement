using AutoMapper;
using InventoryApp.Server.Dtos.CategoryDtos;
using InventoryApp.Server.Dtos.CustomerDtos;
using InventoryApp.Server.Dtos.EmployeeDtos;
using InventoryApp.Server.Dtos.ProductDtos;
using InventoryApp.Server.Dtos.ProviderDtos;
using InventoryApp.Server.Dtos.PurchaseDtos;
using InventoryApp.Server.Dtos.SupplyDtos;

namespace InventoryApp.Server
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Map from Category
            CreateMap<Category, GetCategoryDto>().ReverseMap();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<AddCategoryDto, Category>();
            // Map from Customer
            CreateMap<Customer, GetCustomerDto>().ReverseMap();
            CreateMap<UpdateCustomerDto, Customer>();
            CreateMap<AddCustomerDto, Customer>();
            // Map from Employee
            CreateMap<Employee, GetEmployeeDto>().ReverseMap();
            CreateMap<UpdateEmployeeDto, Employee>();
            CreateMap<AddEmployeeDto, Employee>();
            // Map from Product
            CreateMap<Product, GetProductDto>().ReverseMap();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<AddProductDto, Product>();
            // Map from Provider
            CreateMap<Provider, GetProviderDto>().ReverseMap();
            CreateMap<UpdateProviderDto, Provider>();
            CreateMap<AddProviderDto, Provider>();
            // Map from Purchase
            CreateMap<Purchase, GetPurchaseDto>().ReverseMap();
            CreateMap<UpdatePurchaseDto, Purchase>();
            CreateMap<AddPurchaseDto, Purchase>();
            // Map from Supply
            CreateMap<Supply, GetSupplyDto>().ReverseMap();
            CreateMap<UpdateSupplyDto, Supply>();
            CreateMap<AddSupplyDto, Supply>();
        }
    }
}