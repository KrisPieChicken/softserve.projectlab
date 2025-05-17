using API.Implementations.Domain;
using API.Data.Entities;
using API.Data.Repositories.LogisticsRepositories.Interfaces;
using API.Models.Logistics.Warehouses;
using softserve.projectlabs.Shared.DTOs;
using Moq;
using API.Models.IntAdmin;

namespace UnitTests.WarehouseTests;

    [TestFixture]
    public class WarehouseDomainTests
    {
        private Mock<IWarehouseRepository> _repoMock;
        private WarehouseDomain _domain;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<IWarehouseRepository>();
            _domain = new WarehouseDomain(_repoMock.Object);
        }

        [Test]
        public async Task GetWarehouseByIdAsync_ReturnsWarehouse_WhenFound()
        {
            // Arrange
            var entity = new WarehouseEntity { WarehouseId = 1, WarehouseLocation = "Loc", WarehouseCapacity = 10, BranchId = 1, WarehouseItemEntities = new List<WarehouseItemEntity>() };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

            // Act
            var result = await _domain.GetWarehouseByIdAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
            Assert.That(result.IsSuccess, Is.True, "Warehouse should be found.");
            TestContext.WriteLine("Asserted that warehouse is found.");

            Assert.That(result.Data, Is.Not.Null, "Returned warehouse data should not be null.");
            TestContext.WriteLine("Asserted that returned warehouse data is not null.");

            Assert.That(result.Data.WarehouseId, Is.EqualTo(1), "WarehouseId should match the requested ID.");
            TestContext.WriteLine("Asserted that returned warehouse ID matches expected value.");
        }

        [Test]
        public async Task GetWarehouseByIdAsync_ReturnsFailure_WhenNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((WarehouseEntity)null);

            // Act
            var result = await _domain.GetWarehouseByIdAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False, "Warehouse should not be found.");
            TestContext.WriteLine("Asserted that warehouse is not found.");

            Assert.That(result.ErrorMessage, Is.EqualTo("Warehouse not found."), "Error message should indicate not found.");
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task GetAllWarehousesAsync_ReturnsWarehouses()
        {
            // Arrange
            var entities = new List<WarehouseEntity>
            {
                new WarehouseEntity { WarehouseId = 1, WarehouseLocation = "Loc", WarehouseCapacity = 10, BranchId = 1, WarehouseItemEntities = new List<WarehouseItemEntity>() }
            };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);

            // Act
            var result = await _domain.GetAllWarehousesAsync();

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Count={result.Data?.Count}");
            Assert.That(result.IsSuccess, Is.True, "Should succeed when warehouses are found.");
            TestContext.WriteLine("Asserted that warehouses are found.");

            Assert.That(result.Data.Count, Is.EqualTo(1), "Should return one warehouse.");
            TestContext.WriteLine("Asserted that the correct number of warehouses is returned.");
        }

        [Test]
        public async Task CreateWarehouseAsync_ReturnsCreatedWarehouse()
        {
            // Arrange
            var dto = new WarehouseDto { Location = "Loc", Capacity = 10, BranchId = 1 };
            _repoMock.Setup(r => r.AddAsync(It.IsAny<WarehouseEntity>())).Returns(Task.CompletedTask);

            // Act
            var result = await _domain.CreateWarehouseAsync(dto);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Location={result.Data?.Location}");
            Assert.That(result.IsSuccess, Is.True, "Should succeed when warehouse is created.");
            TestContext.WriteLine("Asserted that warehouse creation succeeded.");

            Assert.That(result.Data.Location, Is.EqualTo("Loc"), "Created warehouse should have correct location.");
            TestContext.WriteLine("Asserted that created warehouse location is correct.");
        }

        [Test]
        public async Task UpdateWarehouseAsync_ReturnsSuccess()
        {
            // Arrange
            var warehouse = new Warehouse(1, "Name", "Loc", 10, 1, new List<Item>());
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<WarehouseEntity>())).Returns(Task.CompletedTask);

            // Act
            var result = await _domain.UpdateWarehouseAsync(warehouse);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
            Assert.That(result.IsSuccess, Is.True, "Should succeed when warehouse is updated.");
            TestContext.WriteLine("Asserted that warehouse update succeeded.");

            Assert.That(result.Data, Is.True, "Data should be true indicating update success.");
            TestContext.WriteLine("Asserted that update result is true.");
        }

        [Test]
        public async Task SoftDeleteWarehouseAsync_ReturnsSuccess_WhenFound()
        {
            // Arrange
            var entity = new WarehouseEntity { WarehouseId = 1, IsDeleted = false, WarehouseItemEntities = new List<WarehouseItemEntity>() };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.UpdateAsync(entity)).Returns(Task.CompletedTask);

            // Act
            var result = await _domain.SoftDeleteWarehouseAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}, IsDeleted={entity.IsDeleted}");
            Assert.That(result.IsSuccess, Is.True, "Should succeed when warehouse is found and soft deleted.");
            TestContext.WriteLine("Asserted that soft delete succeeded.");

            Assert.That(result.Data, Is.True, "Data should be true indicating soft delete success.");
            TestContext.WriteLine("Asserted that soft delete result is true.");

            Assert.That(entity.IsDeleted, Is.True, "Entity should be marked as deleted.");
            TestContext.WriteLine("Asserted that entity is marked as deleted.");
        }

        [Test]
        public async Task SoftDeleteWarehouseAsync_ReturnsFailure_WhenNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((WarehouseEntity)null);

            // Act
            var result = await _domain.SoftDeleteWarehouseAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False, "Should fail when warehouse is not found.");
            TestContext.WriteLine("Asserted that soft delete failed as expected.");

            Assert.That(result.ErrorMessage, Is.EqualTo("Warehouse not found."), "Should return correct error message.");
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task UndeleteWarehouseAsync_ReturnsSuccess_WhenFound()
        {
            // Arrange
            var entity = new WarehouseEntity { WarehouseId = 1, IsDeleted = true, WarehouseItemEntities = new List<WarehouseItemEntity>() };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.UpdateAsync(entity)).Returns(Task.CompletedTask);

            // Act
            var result = await _domain.UndeleteWarehouseAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}, IsDeleted={entity.IsDeleted}");
            Assert.That(result.IsSuccess, Is.True, "Should succeed when warehouse is found and undeleted.");
            TestContext.WriteLine("Asserted that undelete succeeded.");

            Assert.That(result.Data, Is.True, "Data should be true indicating undelete success.");
            TestContext.WriteLine("Asserted that undelete result is true.");

            Assert.That(entity.IsDeleted, Is.False, "Entity should be marked as not deleted.");
            TestContext.WriteLine("Asserted that entity is marked as not deleted.");
        }

        [Test]
        public async Task GetLowStockItemsAsync_ReturnsLowStockItems()
        {
            // Arrange
            var entity = new WarehouseEntity
            {
                WarehouseId = 1,
                WarehouseItemEntities = new List<WarehouseItemEntity>
                {
                    new WarehouseItemEntity { Sku = 1, ItemQuantity = 2 },
                    new WarehouseItemEntity { Sku = 2, ItemQuantity = 10 }
                }
            };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

            // Act
            var result = await _domain.GetLowStockItemsAsync(1, 5);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, LowStockCount={result.Data?.Count}");
            Assert.That(result.IsSuccess, Is.True, "Should succeed when low stock items are found.");
            TestContext.WriteLine("Asserted that low stock items retrieval succeeded.");

            Assert.That(result.Data.Count, Is.EqualTo(1), "Only one item should be low stock.");
            TestContext.WriteLine("Asserted that only one item is low stock.");

            Assert.That(result.Data[0].Sku, Is.EqualTo(1), "The low stock item should have Sku 1.");
            TestContext.WriteLine("Asserted that the low stock item has the correct SKU.");
        }

        [Test]
        public async Task GetTotalInventoryValueAsync_ReturnsSum()
        {
            // Arrange
            var entity = new WarehouseEntity
            {
                WarehouseId = 1,
                WarehouseItemEntities = new List<WarehouseItemEntity>
                {
                    new WarehouseItemEntity
                    {
                        Sku = 1,
                        ItemQuantity = 2,
                        SkuNavigation = new ItemEntity { Sku = 1, ItemPrice = 5 }
                    },
                    new WarehouseItemEntity
                    {
                        Sku = 2,
                        ItemQuantity = 3,
                        SkuNavigation = new ItemEntity { Sku = 2, ItemPrice = 10 }
                    }
                }
            };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

            // Act
            var result = await _domain.GetTotalInventoryValueAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, TotalValue={result.Data}");
            Assert.That(result.IsSuccess, Is.True, "Should succeed when total inventory value is calculated.");
            TestContext.WriteLine("Asserted that inventory value calculation succeeded.");

            Assert.That(result.Data, Is.EqualTo(2 * 5 + 3 * 10), "Should return the correct sum of item values.");
            TestContext.WriteLine("Asserted that total inventory value is correct.");
        }

        [Test]
        public async Task GenerateInventoryReportAsync_ReturnsJson()
        {
            // Arrange
            var entity = new WarehouseEntity
            {
                WarehouseId = 1,
                WarehouseItemEntities = new List<WarehouseItemEntity>
                {
                    new WarehouseItemEntity { Sku = 1, ItemQuantity = 2 }
                }
            };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

            // Act
            var result = await _domain.GenerateInventoryReportAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Report={result.Data}");
            Assert.That(result.IsSuccess, Is.True, "Should succeed when report is generated.");
            TestContext.WriteLine("Asserted that report generation succeeded.");

            StringAssert.Contains("\"Sku\":1", result.Data, "JSON should contain Sku 1.");
            TestContext.WriteLine("Asserted that report contains the correct SKU.");
        }

        [Test]
        public async Task ReserveStockForOrderAsync_Success()
        {
            // Arrange
            var entity = new WarehouseEntity
            {
                WarehouseId = 1,
                WarehouseItemEntities = new List<WarehouseItemEntity>
                {
                    new WarehouseItemEntity { Sku = 1, ItemQuantity = 10 }
                }
            };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

            WarehouseEntity? updatedEntity = null;
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<WarehouseEntity>()))
                     .Callback<WarehouseEntity>(e => updatedEntity = e)
                     .Returns(Task.CompletedTask);

            // Act
            var result = await _domain.ReserveStockForOrderAsync(1, 1, 5);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, UpdatedQuantity={updatedEntity?.WarehouseItemEntities.First(i => i.Sku == 1).ItemQuantity}");
            Assert.That(result.IsSuccess, Is.True, "Should succeed when stock is reserved.");
            TestContext.WriteLine("Asserted that stock reservation succeeded.");

            Assert.That(updatedEntity, Is.Not.Null, "Updated entity should not be null.");
            TestContext.WriteLine("Asserted that updated entity is not null.");

            Assert.That(updatedEntity.WarehouseItemEntities.First(i => i.Sku == 1).ItemQuantity, Is.EqualTo(5), "Stock should be reduced by 5.");
            TestContext.WriteLine("Asserted that stock was reduced correctly.");
        }

        [Test]
        public async Task ReserveStockForOrderAsync_Fails_WhenInsufficientStock()
        {
            // Arrange
            var entity = new WarehouseEntity
            {
                WarehouseId = 1,
                WarehouseItemEntities = new List<WarehouseItemEntity>
                {
                    new WarehouseItemEntity { Sku = 1, ItemQuantity = 2 }
                }
            };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

            // Act
            var result = await _domain.ReserveStockForOrderAsync(1, 1, 5);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False, "Should fail when not enough stock.");
            TestContext.WriteLine("Asserted that stock reservation failed as expected.");

            Assert.That(result.ErrorMessage, Is.EqualTo("Insufficient stock for reservation."), "Should return correct error message.");
            TestContext.WriteLine("Asserted that error message is correct.");
        }
    }

