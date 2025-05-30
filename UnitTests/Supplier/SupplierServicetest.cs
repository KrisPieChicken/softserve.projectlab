using API.Data.Repositories.IntAdministrationRepository.Interfaces;
using API.Data.Repositories.LogisticsRepositories.Interfaces;
using API.Implementations.Domain;
using API.Services.Logistics;
using Moq;
using softserve.projectlabs.Shared.DTOs;
using softserve.projectlabs.Shared.DTOs.Supplier;

namespace UnitTests.Supplier
{
    [TestFixture]
    public class SupplierServicetest
    {
        private Mock<ISupplierRepository> _supplierRepoMock;
        private Mock<IItemRepository> _itemRepoMock;
        private SupplierDomain _domain;
        private SupplierService _service;

        [SetUp]
        public void SetUp()
        {
            _supplierRepoMock = new Mock<ISupplierRepository>();
            _itemRepoMock = new Mock<IItemRepository>();
            _domain = new SupplierDomain(_supplierRepoMock.Object, _itemRepoMock.Object);
            _service = new SupplierService(_domain);
        }

        [Test]
        public async Task CreateSupplierAsync_ReturnsSuccess()
        {
            // Arrange
            var dto = new SupplierDto { SupplierId = 1, Name = "Test", ContactNumber = "123", ContactEmail = "test@test.com", Address = "Addr" };
            _supplierRepoMock.Setup(r => r.AddAsync(It.IsAny<API.Data.Entities.SupplierEntity>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateSupplierAsync(dto);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Name={result.Data?.Name}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that supplier creation succeeded.");
        }

        [Test]
        public async Task CreateSupplierAsync_ReturnsFailure()
        {
            // Arrange
            var dto = new SupplierDto { SupplierId = 1, Name = "Test", ContactNumber = "123", ContactEmail = "test@test.com", Address = "Addr" };
            _supplierRepoMock.Setup(r => r.AddAsync(It.IsAny<API.Data.Entities.SupplierEntity>())).ThrowsAsync(new Exception("fail"));

            // Act
            var result = await _service.CreateSupplierAsync(dto);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that supplier creation failed as expected.");
            Assert.That(result.ErrorMessage, Does.Contain("fail"));
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task GetSupplierByIdAsync_ReturnsSuccess()
        {
            // Arrange
            var entity = new API.Data.Entities.SupplierEntity { SupplierId = 1, SupplierName = "Test" };
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

            // Act
            var result = await _service.GetSupplierByIdAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Name={result.Data?.Name}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that supplier retrieval succeeded.");
        }

        [Test]
        public async Task GetSupplierByIdAsync_ReturnsFailure()
        {
            // Arrange
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((API.Data.Entities.SupplierEntity)null);

            // Act
            var result = await _service.GetSupplierByIdAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that supplier retrieval failed as expected.");
            Assert.That(result.ErrorMessage, Is.EqualTo("Supplier not found."));
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task GetAllSuppliersAsync_ReturnsSuccess()
        {
            // Arrange
            var entities = new List<API.Data.Entities.SupplierEntity>
            {
                new API.Data.Entities.SupplierEntity { SupplierId = 1, SupplierName = "Test" }
            };
            _supplierRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);

            // Act
            var result = await _service.GetAllSuppliersAsync();

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Count={result.Data?.Count}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that suppliers retrieval succeeded.");
            Assert.That(result.Data.Count, Is.EqualTo(1));
            TestContext.WriteLine("Asserted that the correct number of suppliers is returned.");
        }

        [Test]
        public async Task GetAllSuppliersAsync_ReturnsFailure()
        {
            // Arrange
            _supplierRepoMock.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("fail"));

            // Act
            var result = await _service.GetAllSuppliersAsync();

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that suppliers retrieval failed as expected.");
            Assert.That(result.ErrorMessage, Does.Contain("fail"));
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task UpdateSupplierAsync_ReturnsSuccess()
        {
            // Arrange
            var dto = new SupplierDto { SupplierId = 1, Name = "Test", ContactNumber = "123", ContactEmail = "test@test.com", Address = "Addr" };
            var entity = new API.Data.Entities.SupplierEntity { SupplierId = 1, SupplierName = "Test" };
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
            _supplierRepoMock.Setup(r => r.UpdateAsync(entity)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateSupplierAsync(dto);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Name={result.Data?.Name}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that supplier update succeeded.");
        }

        [Test]
        public async Task UpdateSupplierAsync_ReturnsFailure()
        {
            // Arrange
            var dto = new SupplierDto { SupplierId = 1, Name = "Test", ContactNumber = "123", ContactEmail = "test@test.com", Address = "Addr" };
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((API.Data.Entities.SupplierEntity)null);

            // Act
            var result = await _service.UpdateSupplierAsync(dto);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that supplier update failed as expected.");
            Assert.That(result.ErrorMessage, Is.EqualTo("Supplier not found."));
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task DeleteSupplierAsync_ReturnsSuccess()
        {
            // Arrange
            _supplierRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteSupplierAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that supplier deletion succeeded.");
            Assert.That(result.Data, Is.True);
            TestContext.WriteLine("Asserted that delete result is true.");
        }

        [Test]
        public async Task DeleteSupplierAsync_ReturnsFailure()
        {
            // Arrange
            _supplierRepoMock.Setup(r => r.DeleteAsync(1)).ThrowsAsync(new Exception("fail"));

            // Act
            var result = await _service.DeleteSupplierAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that supplier deletion failed as expected.");
            Assert.That(result.ErrorMessage, Does.Contain("fail"));
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task UndeleteSupplierAsync_ReturnsSuccess()
        {
            // Arrange
            var entity = new API.Data.Entities.SupplierEntity { SupplierId = 1, SupplierName = "Test", IsDeleted = true };
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
            _supplierRepoMock.Setup(r => r.UpdateAsync(entity)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.UndeleteSupplierAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that supplier undelete succeeded.");
            Assert.That(result.Data, Is.True);
            TestContext.WriteLine("Asserted that undelete result is true.");
        }

        [Test]
        public async Task UndeleteSupplierAsync_ReturnsFailure()
        {
            // Arrange
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((API.Data.Entities.SupplierEntity)null);

            // Act
            var result = await _service.UndeleteSupplierAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that supplier undelete failed as expected.");
            Assert.That(result.ErrorMessage, Is.EqualTo("Supplier not found."));
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task AddItemToSupplierAsync_ReturnsSuccess()
        {
            // Arrange
            var supplierEntity = new API.Data.Entities.SupplierEntity { SupplierId = 1, SupplierName = "Test" };
            var itemEntity = new API.Data.Entities.ItemEntity { Sku = 100 };
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(supplierEntity);
            _itemRepoMock.Setup(r => r.GetBySkuAsync(100)).ReturnsAsync(itemEntity);
            _supplierRepoMock.Setup(r => r.SupplierHasItem(1, 100)).ReturnsAsync(false);
            _supplierRepoMock.Setup(r => r.LinkItemToSupplier(1, 100)).ReturnsAsync(true);

            // Act
            var result = await _service.AddItemToSupplierAsync(1, 100);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that item was added to supplier.");
            Assert.That(result.Data, Is.True);
            TestContext.WriteLine("Asserted that add item result is true.");
        }

        [Test]
        public async Task AddItemToSupplierAsync_ReturnsFailure()
        {
            // Arrange
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((API.Data.Entities.SupplierEntity)null);

            // Act
            var result = await _service.AddItemToSupplierAsync(1, 100);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that add item to supplier failed as expected.");
            Assert.That(result.ErrorMessage, Is.EqualTo("Supplier not found."));
            TestContext.WriteLine("Asserted that error message is correct.");
        }
    }
}
