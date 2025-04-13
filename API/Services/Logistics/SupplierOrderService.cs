using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models.Logistics.Interfaces;
using API.Domain.Logistics;
using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.Interfaces;
using softserve.projectlabs.Shared.DTOs;
using AutoMapper;

namespace API.Services.Logistics
{
    public class SupplierOrderService : ISupplierOrderService
    {
        private readonly SupplierOrderDomain _domain;
        private readonly IMapper _mapper;

        public SupplierOrderService(SupplierOrderDomain domain, IMapper mapper)
        {
            _domain = domain;
            _mapper = mapper;
        }

        public async Task<List<SupplierOrderDto>> GetAllSupplierOrdersAsync()
        {
            var orders = await _domain.GetAllSupplierOrdersAsync();
            return _mapper.Map<List<SupplierOrderDto>>(orders);
        }

        public async Task<SupplierOrderDto> GetSupplierOrderByIdAsync(int orderId)
        {
            var order = await _domain.GetSupplierOrderByIdAsync(orderId);
            return _mapper.Map<SupplierOrderDto>(order);
        }

        public async Task<SupplierOrderDto> AddSupplierOrderAsync(SupplierOrderDto orderDto)
        {
            var order = _mapper.Map<ISupplierOrder>(orderDto);
            var createdOrder = await _domain.AddSupplierOrderAsync(order);
            return _mapper.Map<SupplierOrderDto>(createdOrder);
        }

        public async Task<bool> UpdateSupplierOrderAsync(SupplierOrderDto orderDto)
        {
            var order = _mapper.Map<ISupplierOrder>(orderDto);
            return await _domain.UpdateSupplierOrderAsync(order);
        }

        public async Task<bool> DeleteSupplierOrderAsync(int orderId)
        {
            return await _domain.DeleteSupplierOrderAsync(orderId);
        }
    }
}

