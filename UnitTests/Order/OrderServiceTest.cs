using API.Implementations.Domain;
using API.Models.Logistics.Order;
using API.Services.OrderService;
using Moq;
using softserve.projectlabs.Shared.DTOs;

// Type aliases to avoid namespace/type confusion
using OrderModel = API.Models.Logistics.Order.Order;
using OrderItemModel = API.Models.Logistics.Order.OrderItem;
using softserve.projectlabs.Shared.DTOs.Order;

namespace UnitTests.Order
{
    [TestFixture]
    public class OrderServiceTest
    {
        private Mock<API.Data.Repositories.LogisticsRepositories.Interfaces.IOrderRepository> _orderRepoMock;
        private Mock<API.Implementations.Domain.WarehouseDomain> _warehouseDomainMock;
        private OrderDomain _orderDomain;
        private OrderService _service;

        [SetUp]
        public void SetUp()
        {
            _orderRepoMock = new Mock<API.Data.Repositories.LogisticsRepositories.Interfaces.IOrderRepository>();
            _warehouseDomainMock = new Mock<API.Implementations.Domain.WarehouseDomain>(Mock.Of<API.Data.Repositories.LogisticsRepositories.Interfaces.IWarehouseRepository>());
            _orderDomain = new OrderDomain(_orderRepoMock.Object, null, _warehouseDomainMock.Object);
            _service = new OrderService(_orderDomain);
        }

        [Test]
        public async Task GetOrderByIdAsync_ReturnsSuccess()
        {
            // Arrange
            var orderEntity = new API.Data.Entities.OrderEntity
            {
                OrderId = 1,
                CustomerId = 1,
                OrderStatus = "Pending",
                OrderItemEntities = new List<API.Data.Entities.OrderItemEntity>()
            };
            _orderRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(orderEntity);

            // Act
            var result = await _service.GetOrderByIdAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, OrderId={result.Data?.OrderId}");
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data.OrderId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetOrderByIdAsync_ReturnsFailure()
        {
            // Arrange
            _orderRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((API.Data.Entities.OrderEntity?)null);

            // Act
            var result = await _service.GetOrderByIdAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Order not found."));
        }

        [Test]
        public async Task GetAllOrdersAsync_ReturnsFailure()
        {
            // Arrange
            _orderRepoMock.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("Failed to retrieve orders"));

            // Act
            var result = await _service.GetAllOrdersAsync();

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("Failed to retrieve orders"));
        }

        [Test]
        public async Task DeleteOrderAsync_ReturnsSuccess()
        {
            // Arrange
            _orderRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteOrderAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.True);
        }

        [Test]
        public async Task DeleteOrderAsync_ReturnsFailure()
        {
            // Arrange
            _orderRepoMock.Setup(r => r.DeleteAsync(1)).ThrowsAsync(new Exception("fail"));

            // Act
            var result = await _service.DeleteOrderAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("fail"));
        }

        [Test]
        public async Task UpdateOrderAsync_ReturnsFailure()
        {
            // Arrange
            var orderDto = new OrderDto { OrderId = 1 };
            _orderRepoMock.Setup(r => r.GetByIdAsync(orderDto.OrderId)).ReturnsAsync((API.Data.Entities.OrderEntity?)null);

            // Act
            var result = await _service.UpdateOrderAsync(orderDto);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}");
            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public async Task FulfillOrderAsync_ReturnsSuccess()
        {
            // Arrange
            _orderRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new API.Data.Entities.OrderEntity
            {
                OrderId = 1,
                OrderStatus = "Pending",
                OrderItemEntities = new List<API.Data.Entities.OrderItemEntity>()
            });
            _orderRepoMock.Setup(r => r.UpdateAsync(It.IsAny<API.Data.Entities.OrderEntity>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.FulfillOrderAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.True);
        }

        [Test]
        public async Task FulfillOrderAsync_ReturnsFailure()
        {
            // Arrange
            _orderRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((API.Data.Entities.OrderEntity?)null);

            // Act
            var result = await _service.FulfillOrderAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Order not found."));
        }
    }
}
