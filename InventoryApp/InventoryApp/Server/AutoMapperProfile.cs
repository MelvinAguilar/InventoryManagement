using AutoMapper;
using InventoryApp.Server.Dtos.CategoryDtos;
using InventoryApp.Server.Dtos.CustomerDtos;
using InventoryApp.Server.Dtos.EmployeeDtos;
using InventoryApp.Server.Dtos.ProductDtos;
using InventoryApp.Server.Dtos.ProviderDtos;
using InventoryApp.Server.Dtos.PurchaseDetailDtos;
using InventoryApp.Server.Dtos.PurchaseDtos;
using InventoryApp.Server.Dtos.SupplyDetailDtos;
using InventoryApp.Server.Dtos.SupplyDtos;

namespace InventoryApp.Server
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Map from Category
            CreateMap<Category, GetCategoryDto>().ReverseMap();
            CreateMap<AddCategoryDto, Category>();
            // Map from Customer
            CreateMap<Customer, GetCustomerDto>().ReverseMap();
            CreateMap<AddCustomerDto, Customer>();
            // Map from Employee
            CreateMap<Employee, GetEmployeeDto>().ReverseMap();
            CreateMap<AddEmployeeDto, Employee>();
            // Map from Product
            CreateMap<Product, GetProductDto>().ReverseMap();
            CreateMap<AddProductDto, Product>();
            // Map from Provider
            CreateMap<Provider, GetProviderDto>().ReverseMap();
            CreateMap<AddProviderDto, Provider>();
            // Map from Purchase
            CreateMap<Purchase, GetPurchaseDto>().ReverseMap();
            CreateMap<AddPurchaseDto, Purchase>();
            // Map from Supply
            CreateMap<Supply, GetSupplyDto>().ReverseMap();
            CreateMap<AddSupplyDto, Supply>();
            // Map from SupplyDetail
            CreateMap<SupplyDetail, GetSupplyDetailDto>().ReverseMap();
            CreateMap<AddSupplyDetailDto, SupplyDetail>();
            CreateMap<UpdateSupplyDetailDto, SupplyDetail>().ReverseMap();
            // Map from PurchaseDetail
            CreateMap<PurchaseDetail, GetPurchaseDetailDto>().ReverseMap();
            CreateMap<AddPurchaseDetailDto, PurchaseDetail>();
            CreateMap<UpdatePurchaseDetailDto, PurchaseDetail>().ReverseMap();
        }
    }
}