using API.Data.Entities;
using API.Models.DTOs;
using API.Models.IntAdmin;
using API.Models.Logistics.Interfaces;
using API.Models.Logistics;
using AutoMapper;
using softserve.projectlabs.Shared.DTOs;
using Logistics.Models;

public class LogisticsMapping : Profile
{
    public LogisticsMapping()
    {
        // Map from WarehouseEntity to Warehouse (concrete class)
        CreateMap<WarehouseEntity, Warehouse>()
            .ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => src.WarehouseId))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.WarehouseLocation))
            .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.WarehouseCapacity))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"Warehouse {src.WarehouseId}"));

        // Map from Warehouse to IWarehouse
        CreateMap<Warehouse, IWarehouse>().ConvertUsing(src => src); // Use Warehouse as the implementation

        CreateMap<Warehouse, WarehouseResponseDto>();
        CreateMap<Item, ItemDto>();
        CreateMap<AddItemToWarehouseDTO, Item>();
        CreateMap<Branch, BranchDto>().ReverseMap();
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<Supplier, SupplierDto>().ReverseMap();
        CreateMap<SupplierOrder, SupplierOrderDto>().ReverseMap();
        CreateMap<SupplierOrder, SupplierOrderDto>().ReverseMap();
        CreateMap<OrderItemRequest, OrderItemRequestDto>();
        CreateMap<Item, AddItemToWarehouseDTO>().ReverseMap();
        CreateMap<Item, AddItemToWarehouseDTO>().ReverseMap();
        CreateMap<BranchEntity, BranchDto>().ReverseMap();
        CreateMap<BranchEntity, Branch>().ReverseMap();



        // Map from AddItemToWarehouseDTO to Item
        CreateMap<AddItemToWarehouseDTO, Item>()
            .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku));
           

        CreateMap<Warehouse, WarehouseResponseDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items.ToList())); // Convert to List


        CreateMap<Item, ItemDto>();



    }
}
