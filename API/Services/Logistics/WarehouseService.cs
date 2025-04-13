using API.Implementations.Domain;
using API.Models.IntAdmin;
using API.Models.Logistics.Interfaces;
using API.Models.Logistics;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using API.Data.Entities;
using API.Data;
using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.Interfaces;
using softserve.projectlabs.Shared.DTOs;

public class WarehouseService : IWarehouseService
{
    private readonly ApplicationDbContext _context;
    private readonly WarehouseDomain _warehouseDomain;
    private readonly IMapper _mapper;

    public WarehouseService(WarehouseDomain warehouseDomain, IMapper mapper, ApplicationDbContext applicationDbContext)
    {
        _context = applicationDbContext;
        _warehouseDomain = warehouseDomain;
        _mapper = mapper;
    }

    public async Task<List<WarehouseResponseDto>> GetWarehousesAsync()
    {
        var result = await _warehouseDomain.GetAllWarehousesAsync();
        return result.IsSuccess
            ? _mapper.Map<List<WarehouseResponseDto>>(result.Data)
            : new List<WarehouseResponseDto>();
    }

    public async Task<Result<WarehouseResponseDto>> GetWarehouseByIdAsync(int warehouseId)
    {
        var result = await _warehouseDomain.GetWarehouseByIdAsync(warehouseId);
        return result.IsSuccess
            ? Result<WarehouseResponseDto>.Success(_mapper.Map<WarehouseResponseDto>(result.Data))
            : Result<WarehouseResponseDto>.Failure(result.ErrorMessage, result.ErrorCode);
    }

    public async Task<Result<bool>> AddItemToWarehouseAsync(int warehouseId, AddItemToWarehouseDTO itemDto)
    {
        var item = _mapper.Map<Item>(itemDto);
        var result = await _warehouseDomain.AddItemToWarehouseAsync(warehouseId, itemDto);
        return result.IsSuccess
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(result.ErrorMessage, result.ErrorCode);
    }

    public async Task<Result<List<ItemDto>>> GetLowStockItemsAsync(int warehouseId, int threshold)
    {
        var result = await _warehouseDomain.GetWarehouseByIdAsync(warehouseId);
        if (!result.IsSuccess)
            return Result<List<ItemDto>>.Failure(result.ErrorMessage, result.ErrorCode);

        var lowStockResult = await result.Data.GetLowStockItemsAsync(threshold);
        return lowStockResult.IsSuccess
            ? Result<List<ItemDto>>.Success(_mapper.Map<List<ItemDto>>(lowStockResult.Data))
            : Result<List<ItemDto>>.Failure(lowStockResult.ErrorMessage, lowStockResult.ErrorCode);
    }

    public async Task<Result<bool>> RemoveItemFromWarehouseAsync(int warehouseId, int itemId)
    {
        var result = await _warehouseDomain.RemoveItemFromWarehouseAsync(warehouseId, itemId);

        if (!result.IsSuccess)
        {
            return Result<bool>.Failure(result.ErrorMessage, result.ErrorCode, result.StackTrace);
        }

        // If the operation is successful, return a Result<bool> with true
        return Result<bool>.Success(true);
    }



    public async Task<Result<int>> CheckWarehouseStockAsync(int warehouseId, int sku)
    {
        var result = await _warehouseDomain.GetWarehouseByIdAsync(warehouseId);
        if (!result.IsSuccess)
            return Result<int>.Failure(result.ErrorMessage, result.ErrorCode);

        var stockResult = await result.Data.CheckItemStockAsync(sku);
        return stockResult;
    }

    public async Task<Result<bool>> TransferItemAsync(
        int sourceWarehouseId,
        int sku,
        int quantity,
        int targetWarehouseId)
    {
        var sourceResult = await _warehouseDomain.GetWarehouseByIdAsync(sourceWarehouseId);
        var targetResult = await _warehouseDomain.GetWarehouseByIdAsync(targetWarehouseId);

        if (!sourceResult.IsSuccess || !targetResult.IsSuccess)
            return Result<bool>.Failure("One or both warehouses not found", 404);

        var transferResult = await sourceResult.Data.TransferItemAsync(
            sku,
            quantity,
            targetResult.Data);

        return transferResult;
    }

    public async Task<Result<decimal>> CalculateTotalInventoryValueAsync(int warehouseId)
    {
        var result = await _warehouseDomain.GetWarehouseByIdAsync(warehouseId);
        if (!result.IsSuccess)
            return Result<decimal>.Failure(result.ErrorMessage, result.ErrorCode);

        var valueResult = await result.Data.GetTotalInventoryValueAsync();
        return valueResult;
    }

    public async Task<Result<string>> GenerateInventoryReportAsync(int warehouseId)
    {
        var result = await _warehouseDomain.GetWarehouseByIdAsync(warehouseId);
        if (!result.IsSuccess)
            return Result<string>.Failure(result.ErrorMessage, result.ErrorCode);

        var reportResult = await result.Data.GenerateInventoryReportAsync();
        return reportResult;
    }

    public async Task<Result<bool>> DeleteWarehouseAsync(int warehouseId)
    {
        return await _warehouseDomain.SoftDeleteWarehouseAsync(warehouseId);
    }

    public async Task<Result<bool>> UndeleteWarehouseAsync(int warehouseId)
    {
        return await _warehouseDomain.UndeleteWarehouseAsync(warehouseId);
    }


}
