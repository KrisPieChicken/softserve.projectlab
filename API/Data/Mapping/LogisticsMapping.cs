using API.Data.Entities;
using API.Models.IntAdmin;
using API.Models.Logistics.Order;
using API.Models.Logistics.Warehouse;
using AutoMapper;
using softserve.projectlabs.Shared.DTOs.Branch;
using softserve.projectlabs.Shared.DTOs.Item;
using softserve.projectlabs.Shared.DTOs.Order;
using softserve.projectlabs.Shared.DTOs.Supplier;
using softserve.projectlabs.Shared.DTOs.Warehouse;

public class LogisticsMapping : Profile
{
    public LogisticsMapping()
    {
        // Map from WarehouseDto to WarehouseEntity and vice versa
        CreateMap<WarehouseDto, WarehouseEntity>().ReverseMap();

        // Map from WarehouseEntity to WarehouseResponseDto
        CreateMap<WarehouseEntity, WarehouseResponseDto>()
            .ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => src.WarehouseId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"Warehouse {src.WarehouseId}"))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.WarehouseLocation))
            .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.WarehouseCapacity))
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
            .ForMember(dest => dest.Items, opt => opt.Ignore()); // Items are handled manually if needed

        // Map from Warehouse to WarehouseResponseDto
        //CreateMap<Warehouse, WarehouseResponseDto>()
        //    .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items.ToList()));

        // Map from WarehouseEntity to Warehouse
        //CreateMap<WarehouseEntity, Warehouse>()
        //    .ConstructUsing(src => new Warehouse(
        //        src.WarehouseId,
        //        $"Warehouse {src.WarehouseId}",
        //        src.WarehouseLocation,
        //        src.WarehouseCapacity,
        //        src.BranchId,
        //        null
        //    ))
        //    .ForMember(dest => dest.Items, opt => opt.Ignore());


        // Map from Item to ItemDto
        CreateMap<Item, ItemDto>()
            .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku))
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.ItemName))
            .ForMember(dest => dest.ItemDescription, opt => opt.MapFrom(src => src.ItemDescription))
            .ForMember(dest => dest.ItemPrice, opt => opt.MapFrom(src => src.ItemPrice))
            .ForMember(dest => dest.CurrentStock, opt => opt.MapFrom(src => src.CurrentStock));

        // Map from AddItemToWarehouseDto to Item and vice versa
        CreateMap<AddItemToWarehouseDto, Item>()
            .ForMember(dest => dest.Sku, opt => opt.MapFrom(src => src.Sku))
            .ReverseMap();

        // Explicit mapping from BranchEntity to BranchDto
        CreateMap<BranchEntity, BranchDto>()
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BranchName))
            .ForMember(dest => dest.BranchCity, opt => opt.MapFrom(src => src.BranchCity))
            .ForMember(dest => dest.BranchAddress, opt => opt.MapFrom(src => src.BranchAddress))
            .ForMember(dest => dest.BranchRegion, opt => opt.MapFrom(src => src.BranchRegion))
            .ForMember(dest => dest.BranchContactNumber, opt => opt.MapFrom(src => src.BranchContactNumber))
            .ForMember(dest => dest.BranchContactEmail, opt => opt.MapFrom(src => src.BranchContactEmail))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));

        // Explicit mapping from BranchDto to BranchEntity
        CreateMap<BranchDto, BranchEntity>()
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BranchName))
            .ForMember(dest => dest.BranchCity, opt => opt.MapFrom(src => src.BranchCity))
            .ForMember(dest => dest.BranchAddress, opt => opt.MapFrom(src => src.BranchAddress))
            .ForMember(dest => dest.BranchRegion, opt => opt.MapFrom(src => src.BranchRegion))
            .ForMember(dest => dest.BranchContactNumber, opt => opt.MapFrom(src => src.BranchContactNumber))
            .ForMember(dest => dest.BranchContactEmail, opt => opt.MapFrom(src => src.BranchContactEmail))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        // Add mapping for SupplierDto to SupplierEntity
        CreateMap<SupplierDto, SupplierEntity>()
            .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.SupplierAddress, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.SupplierContactNumber, opt => opt.MapFrom(src => src.ContactNumber))
            .ForMember(dest => dest.SupplierContactEmail, opt => opt.MapFrom(src => src.ContactEmail))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted == false ? false : src.IsDeleted))
            .ReverseMap();

        // Map OrderEntity to OrderDto
        CreateMap<OrderEntity, OrderDto>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.OrderTotalAmount))
            .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItemEntities)); // Map items

        // Map OrderItemEntity to OrderItemDto
        CreateMap<OrderItemEntity, OrderItemDto>()
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.SkuNavigation.ItemId))
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.SkuNavigation.ItemName))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.ItemQuantity))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.SkuNavigation.ItemPrice))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.ItemQuantity * src.SkuNavigation.ItemPrice));

        // Map OrderDto to Order (domain model)
        CreateMap<OrderDto, Order>()
            .ConstructUsing(src => new Order(
                src.OrderId,
                src.CustomerId,
                src.OrderDate,
                src.OrderStatus,
                null 
            ));


        CreateMap<OrderEntity, OrderDto>()
    .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItemEntities));
        CreateMap<OrderItemEntity, OrderItemDto>();
        CreateMap<OrderDto, OrderEntity>()
    .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
    .ForMember(dest => dest.OrderItemEntities, opt => opt.MapFrom(src => src.Items));

        CreateMap<OrderItemDto, OrderItemEntity>();

    }
}


