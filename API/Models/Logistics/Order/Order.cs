
namespace API.Models.Logistics.Order
{
    public class Order
    {
        public int OrderId { get; private set; }
        public int CustomerId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public decimal TotalAmount { get; private set; }
        public string OrderStatus { get; private set; }
        public List<OrderItem> Items { get; private set; } = new();

        public Order(int orderId, int customerId, DateTime orderDate, string orderStatus, List<OrderItem>? items = null)
        {
            OrderId = orderId;
            CustomerId = customerId;
            OrderDate = orderDate;
            OrderStatus = orderStatus;
            if (items != null)
                Items = items;
            TotalAmount = CalculateTotal();
        }

        public void AddItem(OrderItem item)
        {
            Items.Add(item);
            TotalAmount = CalculateTotal();
        }

        public void RemoveItem(int sku)
        {
            var item = Items.FirstOrDefault(i => i.Sku == sku);
            if (item != null)
            {
                Items.Remove(item);
                TotalAmount = CalculateTotal();
            }
        }

        public void UpdateStatus(string status)
        {
            OrderStatus = status;
        }

        public void SetOrderDate(DateTime date)
        {
            OrderDate = date;
        }

        private decimal CalculateTotal()
        {
            return Items.Sum(i => i.UnitPrice * i.Quantity);
        }
    }

}
