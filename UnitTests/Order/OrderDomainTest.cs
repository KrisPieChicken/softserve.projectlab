using API.Data;
using API.Data.Entities;
using API.Data.Repositories.LogisticsRepositories.Interfaces;
using API.Implementations.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;

namespace UnitTests.Order
{
    [TestFixture]
    public class OrderDomainTest
    {
        private Mock<IOrderRepository> _orderRepoMock;
        private Mock<WarehouseDomain> _warehouseDomainMock;

        [SetUp]
        public void SetUp()
        {
            _orderRepoMock = new Mock<IOrderRepository>();
            _warehouseDomainMock = new Mock<WarehouseDomain>(MockBehavior.Strict, null);
        }

        private DbContextOptions<ApplicationDbContext> GetInMemoryOptions(string dbName)
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        }

        [Test]
        public async Task GetOrderById_ReturnsSuccess_WhenOrderExists()
        {
            // Arrange
            var options = GetInMemoryOptions("GetOrderById_Success");
            using var context = new ApplicationDbContext(options);
            context.OrderEntities.Add(new OrderEntity { OrderId = 1, OrderStatus = "Pending" });
            context.SaveChanges();

            var domain = new OrderDomain(_orderRepoMock.Object, context, _warehouseDomainMock.Object);
            _orderRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(context.OrderEntities.First());

            // Act
            var result = await domain.GetOrderById(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, OrderId={result.Data?.OrderId}");
            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public async Task GetOrderById_ReturnsFailure_WhenOrderNotFound()
        {
            // Arrange
            var options = GetInMemoryOptions("GetOrderById_Failure");
            using var context = new ApplicationDbContext(options);

            var domain = new OrderDomain(_orderRepoMock.Object, context, _warehouseDomainMock.Object);
            _orderRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((OrderEntity)null);

            // Act
            var result = await domain.GetOrderById(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Order not found."));
        }

        [Test]
        public async Task GetAllOrders_ReturnsSuccess()
        {
            // Arrange
            var options = GetInMemoryOptions("GetAllOrders_Success");
            using var context = new ApplicationDbContext(options);
            context.OrderEntities.Add(new OrderEntity { OrderId = 1, OrderStatus = "Pending" });
            context.SaveChanges();

            var domain = new OrderDomain(_orderRepoMock.Object, context, _warehouseDomainMock.Object);

            // Act
            var result = await domain.GetAllOrders();

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Count={result.Data?.Count}");
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllOrders_ReturnsFailure_OnException()
        {
            // Arrange
            var options = GetInMemoryOptions("GetAllOrders_Failure");
            var context = new ApplicationDbContext(options);
            context.Dispose();

            var domain = new OrderDomain(_orderRepoMock.Object, context, _warehouseDomainMock.Object);

            // Act
            var result = await domain.GetAllOrders();

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("Failed to retrieve orders"));
        }

        [Test]
        public async Task UpdateOrderFromCart_ReturnsFailure_WhenOrderNotFound()
        {
            // Arrange
            var options = GetInMemoryOptions("UpdateOrderFromCart_Failure");
            using var context = new ApplicationDbContext(options);

            var domain = new OrderDomain(_orderRepoMock.Object, context, _warehouseDomainMock.Object);

            // Act
            var result = await domain.UpdateOrderFromCart(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Order not found."));
        }

        [Test]
        public async Task DeleteOrder_ReturnsSuccess()
        {
            // Arrange
            var options = GetInMemoryOptions("DeleteOrder_Success");
            using var context = new ApplicationDbContext(options);

            var domain = new OrderDomain(_orderRepoMock.Object, context, _warehouseDomainMock.Object);
            _orderRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await domain.DeleteOrder(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.True);
        }

        [Test]
        public async Task DeleteOrder_ReturnsFailure_OnException()
        {
            // Arrange
            var options = GetInMemoryOptions("DeleteOrder_Failure");
            using var context = new ApplicationDbContext(options);

            var domain = new OrderDomain(_orderRepoMock.Object, context, _warehouseDomainMock.Object);
            _orderRepoMock.Setup(r => r.DeleteAsync(1)).ThrowsAsync(new Exception("fail"));

            // Act
            var result = await domain.DeleteOrder(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("fail"));
        }

        [Test]
        public async Task FulfillOrder_ReturnsSuccess()
        {
            // Arrange
            var options = GetInMemoryOptions("FulfillOrder_Success");
            using var context = new ApplicationDbContext(options);
            context.OrderEntities.Add(new OrderEntity { OrderId = 1, OrderStatus = "Pending" });
            context.SaveChanges();

            var domain = new OrderDomain(_orderRepoMock.Object, context, _warehouseDomainMock.Object);
            var orderEntity = context.OrderEntities.First();
            _orderRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(orderEntity);
            _orderRepoMock.Setup(r => r.UpdateAsync(orderEntity)).Returns(Task.CompletedTask);

            // Act
            var result = await domain.FulfillOrder(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.True);
        }

        [Test]
        public async Task FulfillOrder_ReturnsFailure_WhenOrderNotFound()
        {
            // Arrange
            var options = GetInMemoryOptions("FulfillOrder_Failure");
            using var context = new ApplicationDbContext(options);

            var domain = new OrderDomain(_orderRepoMock.Object, context, _warehouseDomainMock.Object);
            _orderRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((OrderEntity)null);

            // Act
            var result = await domain.FulfillOrder(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Order not found."));
        }

        [Test]
        public async Task FulfillOrder_ReturnsFailure_WhenAlreadyFulfilled()
        {
            // Arrange
            var options = GetInMemoryOptions("FulfillOrder_AlreadyFulfilled");
            using var context = new ApplicationDbContext(options);
            context.OrderEntities.Add(new OrderEntity { OrderId = 1, OrderStatus = "Fulfilled" });
            context.SaveChanges();

            var domain = new OrderDomain(_orderRepoMock.Object, context, _warehouseDomainMock.Object);
            var orderEntity = context.OrderEntities.First();
            _orderRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(orderEntity);

            // Act
            var result = await domain.FulfillOrder(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Order is already fulfilled."));
        }

        [Test]
        public async Task SaveUnsavedOrders_SavesOrders()
        {
            // Arrange
            var options = GetInMemoryOptions("SaveUnsavedOrders");
            using var context = new ApplicationDbContext(options);

            context.CustomerEntities.Add(new CustomerEntity
            {
                CustomerId = 1,
                CustomerContactEmail = "test@example.com",
                CustomerContactNumber = "1234567890",
                CustomerFirstName = "Test Customer",
                CartEntities = new List<CartEntity>(),
                OrderEntities = new List<OrderEntity>()
            });

            var item = new ItemEntity
            {
                ItemId = 100,
                ItemPrice = 10.0m,
                ItemName = "Test Item",
                ItemDescription = "Test Desc",
                ItemCurrency = "USD",
                CategoryId = 1,
                OriginalStock = 10,
                CurrentStock = 10,
                ItemUnitCost = 5.0m,
                ItemMarginGain = 2.0m,
                ItemStatus = true
            };
            context.ItemEntities.Add(item);

            context.CartEntities.Add(new CartEntity
            {
                CartId = 1,
                CustomerId = 1,
                CartItemEntities = new List<CartItemEntity>
                {
                    new CartItemEntity
                    {
                        CartId = 1,
                        SkuNavigation = item,
                        ItemQuantity = 2
                    }
                }
            });
            context.SaveChanges();

            var orderRepoMock = new Mock<IOrderRepository>();
            var warehouseDomainMock = new Mock<WarehouseDomain>(MockBehavior.Strict, Mock.Of<IWarehouseRepository>());

            var domain = new OrderDomain(orderRepoMock.Object, context, warehouseDomainMock.Object);

            // Act
            var result = await domain.SaveUnsavedOrders();

            // Assert
            if (!result.IsSuccess)
                TestContext.WriteLine("Domain error: " + result.ErrorMessage);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(context.OrderEntities.Count(), Is.GreaterThan(0));
        }

        [Test]
        public async Task RetrieveOrderByCartId_ReturnsOrder_WhenCartExists()
        {
            // Arrange
            var options = GetInMemoryOptions("RetrieveOrderByCartId");
            using var context = new ApplicationDbContext(options);

            context.CustomerEntities.Add(new CustomerEntity
            {
                CustomerId = 1,
                CustomerContactEmail = "test@example.com",
                CustomerContactNumber = "1234567890",
                CustomerFirstName = "Test Customer",
                CartEntities = new List<CartEntity>(),
                OrderEntities = new List<OrderEntity>()
            });

            var item = new ItemEntity
            {
                ItemId = 100,
                ItemPrice = 10.0m,
                ItemName = "Test Item",
                ItemDescription = "Test Desc",
                ItemCurrency = "USD",
                CategoryId = 1,
                OriginalStock = 10,
                CurrentStock = 10,
                ItemUnitCost = 5.0m,
                ItemMarginGain = 2.0m,
                ItemStatus = true
            };
            context.ItemEntities.Add(item);

            context.CartEntities.Add(new CartEntity
            {
                CartId = 1,
                CustomerId = 1,
                CartItemEntities = new List<CartItemEntity>
                {
                    new CartItemEntity
                    {
                        CartId = 1,
                        SkuNavigation = item,
                        ItemQuantity = 2
                    }
                }
            });
            context.SaveChanges();

            var orderRepoMock = new Mock<IOrderRepository>();
            var warehouseDomainMock = new Mock<WarehouseDomain>(MockBehavior.Strict, Mock.Of<IWarehouseRepository>());

            var domain = new OrderDomain(orderRepoMock.Object, context, warehouseDomainMock.Object);

            // Act
            var result = await domain.RetrieveOrderByCartId(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, DataIsNull={result.Data == null}");
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public async Task RetrieveOrderByCartId_ReturnsFailure_WhenCartNotFound()
        {
            // Arrange
            var options = GetInMemoryOptions("RetrieveOrderByCartId_Failure");
            using var context = new ApplicationDbContext(options);

            var orderRepoMock = new Mock<IOrderRepository>();
            var warehouseDomainMock = new Mock<WarehouseDomain>(MockBehavior.Strict, null);

            var domain = new OrderDomain(orderRepoMock.Object, context, warehouseDomainMock.Object);

            // Act
            var result = await domain.RetrieveOrderByCartId(999);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Cart not found."));
        }

        [Test]
        public async Task RetrieveAndSaveAllUnsavedOrders_SavesOrders()
        {
            // Arrange
            var options = GetInMemoryOptions("RetrieveAndSaveAllUnsavedOrders");
            using var context = new ApplicationDbContext(options);

            context.CustomerEntities.Add(new CustomerEntity
            {
                CustomerId = 1,
                CustomerContactEmail = "test@example.com",
                CustomerContactNumber = "1234567890",
                CustomerFirstName = "Test Customer",
                CartEntities = new List<CartEntity>(),
                OrderEntities = new List<OrderEntity>()
            });

            var item = new ItemEntity
            {
                ItemId = 100,
                ItemPrice = 10.0m,
                ItemName = "Test Item",
                ItemDescription = "Test Desc",
                ItemCurrency = "USD",
                CategoryId = 1,
                OriginalStock = 10,
                CurrentStock = 10,
                ItemUnitCost = 5.0m,
                ItemMarginGain = 2.0m,
                ItemStatus = true
            };
            context.ItemEntities.Add(item);

            context.CartEntities.Add(new CartEntity
            {
                CartId = 1,
                CustomerId = 1,
                CartItemEntities = new List<CartItemEntity>
                {
                    new CartItemEntity
                    {
                        CartId = 1,
                        SkuNavigation = item,
                        ItemQuantity = 2
                    }
                }
            });
            context.SaveChanges();

            var orderRepoMock = new Mock<IOrderRepository>();
            var warehouseDomainMock = new Mock<WarehouseDomain>(MockBehavior.Strict, null);

            var domain = new OrderDomain(orderRepoMock.Object, context, warehouseDomainMock.Object);

            // Act
            var result = await domain.RetrieveAndSaveAllUnsavedOrders();

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, OrderCount={context.OrderEntities.Count()}");
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(context.OrderEntities.Count(), Is.GreaterThan(0));
        }
    }
}
