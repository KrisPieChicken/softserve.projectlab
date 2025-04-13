using API.Data.Entities;
using API.Implementations.Domain;
using Logistics.Models;
using API.Models;
using API.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.Interfaces;
using softserve.projectlabs.Shared.DTOs;

namespace API.Services.Logistics
{
    public class SupplierService : ISupplierService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SupplierService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<SupplierDto>> CreateSupplierAsync(SupplierDto supplierDto)
        {
            var entity = _mapper.Map<SupplierEntity>(supplierDto);
            _context.SupplierEntities.Add(entity);
            await _context.SaveChangesAsync();
            return Result<SupplierDto>.Success(_mapper.Map<SupplierDto>(entity));
        }

        public async Task<Result<SupplierDto>> GetSupplierByIdAsync(int supplierId)
        {
            var entity = await _context.SupplierEntities.FindAsync(supplierId);
            if (entity == null)
            {
                return Result<SupplierDto>.Failure("Supplier not found.");
            }
            return Result<SupplierDto>.Success(_mapper.Map<SupplierDto>(entity));
        }

        public async Task<Result<List<SupplierDto>>> GetAllSuppliersAsync()
        {
            var entities = await _context.SupplierEntities.ToListAsync();
            return Result<List<SupplierDto>>.Success(_mapper.Map<List<SupplierDto>>(entities));
        }

        public async Task<Result<SupplierDto>> UpdateSupplierAsync(SupplierDto supplierDto)
        {
            var entity = await _context.SupplierEntities.FindAsync(supplierDto.SupplierId);
            if (entity == null)
            {
                return Result<SupplierDto>.Failure("Supplier not found.");
            }
            _mapper.Map(supplierDto, entity);
            await _context.SaveChangesAsync();
            return Result<SupplierDto>.Success(_mapper.Map<SupplierDto>(entity));
        }

        public async Task<Result<bool>> DeleteSupplierAsync(int supplierId)
        {
            var entity = await _context.SupplierEntities.FindAsync(supplierId);
            if (entity == null)
            {
                return Result<bool>.Failure("Supplier not found.");
            }
            _context.SupplierEntities.Remove(entity);
            await _context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
    }
}

