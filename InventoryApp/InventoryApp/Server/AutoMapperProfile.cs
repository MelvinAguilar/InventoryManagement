using AutoMapper;
using InventoryApp.Shared.Dtos.CategoryDtos;
using InventoryApp.Shared.Dtos.CustomerDtos;
using InventoryApp.Shared.Dtos.EmployeeDtos;
using InventoryApp.Shared.Dtos.ProductDtos;
using InventoryApp.Shared.Dtos.ProviderDtos;
using InventoryApp.Shared.Dtos.PurchaseDetailDtos;
using InventoryApp.Shared.Dtos.PurchaseDtos;
using InventoryApp.Shared.Dtos.SupplyDetailDtos;
using InventoryApp.Shared.Dtos.SupplyDtos;

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