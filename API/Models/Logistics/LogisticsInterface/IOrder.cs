using API.Data.Entities;
using API.Models.Customers;
using API.Models.IntAdmin;
using API.Models.Logistics.Interfaces;
using softserve.projectlabs.Shared.Utilities;


namespace API.Models.Logistics.Interfaces
{
    public interface IOrder
    {
        int OrderId { get; set; }
        Customer Customer { get; set; }  
        DateTime OrderDate { get; set; }
        List<Item> Items { get; set; }
        string Status { get; set; }
        decimal TotalAmount { get; set; }

        Result<IOrder> AddOrder(IOrder order);
        Result<IOrder> UpdateOrder(IOrder order);
        Result<IOrder> GetOrderById(int orderId);
        Result<List<IOrder>> GetAllOrders();
        Result<bool> RemoveOrder(int orderId);
        Result<bool> AddItemToOrder(Item item);
        Result<bool> RemoveItemFromOrder(Item item);
    }
}
