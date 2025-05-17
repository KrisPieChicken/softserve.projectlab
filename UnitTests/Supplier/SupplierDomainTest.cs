using API.Data.Entities;
using API.Data.Repositories.IntAdministrationRepository.Interfaces;
using API.Data.Repositories.LogisticsRepositories.Interfaces;
using API.Implementations.Domain;
using Moq;
using SupplierModel = API.Models.Logistics.Supplier.Supplier;

namespace UnitTests.Supplier
{
    [TestFixture]
    public class SupplierDomainTest
    {
        private Mock<ISupplierRepository> _supplierRepoMock;
        private Mock<IItemRepository> _itemRepoMock;
        private SupplierDomain _domain;

        [SetUp]
        public void SetUp()
        {
            _supplierRepoMock = new Mock<ISupplierRepository>();
            _itemRepoMock = new Mock<IItemRepository>();
            _domain = new SupplierDomain(_supplierRepoMock.Object, _itemRepoMock.Object);
        }

        [Test]
        public async Task CreateSupplier_ReturnsSuccess()
        {
            // Arrange
            var supplier = new SupplierModel(1, "Test", "123", "test@test.com", "Test Address");
            var entity = new SupplierEntity { SupplierId = 1, SupplierName = "Test" };
            _supplierRepoMock.Setup(r => r.AddAsync(It.IsAny<SupplierEntity>())).Returns(Task.CompletedTask);
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

            // Act
            var result = await _domain.CreateSupplier(supplier);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Name={result.Data?.Name}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that supplier creation succeeded.");
        }

        [Test]
        public async Task CreateSupplier_ReturnsFailure_OnException()
        {
            // Arrange
            var supplier = new SupplierModel(1, "Test", "123", "test@test.com", "Test Address");
            _supplierRepoMock.Setup(r => r.AddAsync(It.IsAny<SupplierEntity>())).ThrowsAsync(new Exception("fail"));

            // Act
            var result = await _domain.CreateSupplier(supplier);

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
            var entity = new SupplierEntity { SupplierId = 1, SupplierName = "Test" };
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

            // Act
            var result = await _domain.GetSupplierByIdAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Name={result.Data?.Name}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that supplier retrieval succeeded.");
        }

        [Test]
        public async Task GetSupplierByIdAsync_ReturnsFailure_WhenNotFound()
        {
            // Arrange
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((SupplierEntity)null);

            // Act
            var result = await _domain.GetSupplierByIdAsync(1);

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
            var entities = new List<SupplierEntity> { new SupplierEntity { SupplierId = 1, SupplierName = "Test" } };
            _supplierRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);

            // Act
            var result = await _domain.GetAllSuppliersAsync();

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Count={result.Data?.Count}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that suppliers retrieval succeeded.");
            Assert.That(result.Data.Count, Is.EqualTo(1));
            TestContext.WriteLine("Asserted that the correct number of suppliers is returned.");
        }

        [Test]
        public async Task GetAllSuppliersAsync_ReturnsFailure_OnException()
        {
            // Arrange
            _supplierRepoMock.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("fail"));

            // Act
            var result = await _domain.GetAllSuppliersAsync();

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that suppliers retrieval failed as expected.");
            Assert.That(result.ErrorMessage, Does.Contain("fail"));
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task UpdateSupplier_ReturnsSuccess()
        {
            // Arrange
            var supplier = new SupplierModel(1, "Test", "123", "test@test.com", "Test Address");
            var entity = new SupplierEntity { SupplierId = 1, SupplierName = "Test" };
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
            _supplierRepoMock.Setup(r => r.UpdateAsync(entity)).Returns(Task.CompletedTask);

            // Act
            var result = await _domain.UpdateSupplier(supplier);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Name={result.Data?.Name}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that supplier update succeeded.");
        }

        [Test]
        public async Task UpdateSupplier_ReturnsFailure_WhenNotFound()
        {
            // Arrange
            var supplier = new SupplierModel(1, "Test", "123", "test@test.com", "Test Address");
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((SupplierEntity)null);

            // Act
            var result = await _domain.UpdateSupplier(supplier);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that supplier update failed as expected.");
            Assert.That(result.ErrorMessage, Is.EqualTo("Supplier not found."));
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task RemoveSupplierAsync_ReturnsSuccess()
        {
            // Arrange
            _supplierRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _domain.RemoveSupplierAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that supplier removal succeeded.");
            Assert.That(result.Data, Is.True);
            TestContext.WriteLine("Asserted that remove result is true.");
        }

        [Test]
        public async Task RemoveSupplierAsync_ReturnsFailure_OnException()
        {
            // Arrange
            _supplierRepoMock.Setup(r => r.DeleteAsync(1)).ThrowsAsync(new Exception("fail"));

            // Act
            var result = await _domain.RemoveSupplierAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that supplier removal failed as expected.");
            Assert.That(result.ErrorMessage, Does.Contain("fail"));
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task SupplierHasItem_ReturnsTrue()
        {
            // Arrange
            _supplierRepoMock.Setup(r => r.SupplierHasItem(1, 100)).ReturnsAsync(true);

            // Act
            var result = await _domain.SupplierHasItem(1, 100);

            // Assert
            TestContext.WriteLine($"Result: {result}");
            Assert.That(result, Is.True);
            TestContext.WriteLine("Asserted that supplier has item.");
        }

        [Test]
        public async Task LinkItemToSupplier_ReturnsSuccess()
        {
            // Arrange
            var supplierEntity = new SupplierEntity { SupplierId = 1, SupplierName = "Test" };
            var itemEntity = new ItemEntity { Sku = 100 };
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(supplierEntity);
            _itemRepoMock.Setup(r => r.GetBySkuAsync(100)).ReturnsAsync(itemEntity);
            _supplierRepoMock.Setup(r => r.SupplierHasItem(1, 100)).ReturnsAsync(false);
            _supplierRepoMock.Setup(r => r.LinkItemToSupplier(1, 100)).ReturnsAsync(true);

            // Act
            var result = await _domain.LinkItemToSupplier(1, 100);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that item was linked to supplier.");
            Assert.That(result.Data, Is.True);
            TestContext.WriteLine("Asserted that link result is true.");
        }

        [Test]
        public async Task LinkItemToSupplier_ReturnsFailure_WhenSupplierNotFound()
        {
            // Arrange
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((SupplierEntity)null);

            // Act
            var result = await _domain.LinkItemToSupplier(1, 100);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that link failed due to missing supplier.");
            Assert.That(result.ErrorMessage, Is.EqualTo("Supplier not found."));
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task LinkItemToSupplier_ReturnsFailure_WhenItemNotFound()
        {
            // Arrange
            var supplierEntity = new SupplierEntity { SupplierId = 1, SupplierName = "Test" };
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(supplierEntity);
            _itemRepoMock.Setup(r => r.GetBySkuAsync(100)).ReturnsAsync((ItemEntity)null);

            // Act
            var result = await _domain.LinkItemToSupplier(1, 100);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that link failed due to missing item.");
            Assert.That(result.ErrorMessage, Is.EqualTo("Item not found."));
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task LinkItemToSupplier_ReturnsFailure_WhenAlreadyLinked()
        {
            // Arrange
            var supplierEntity = new SupplierEntity { SupplierId = 1, SupplierName = "Test" };
            var itemEntity = new ItemEntity { Sku = 100 };
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(supplierEntity);
            _itemRepoMock.Setup(r => r.GetBySkuAsync(100)).ReturnsAsync(itemEntity);
            _supplierRepoMock.Setup(r => r.SupplierHasItem(1, 100)).ReturnsAsync(true);

            // Act
            var result = await _domain.LinkItemToSupplier(1, 100);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that link failed due to already linked item.");
            Assert.That(result.ErrorMessage, Is.EqualTo("Item is already linked to this supplier."));
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task UndeleteSupplierAsync_ReturnsSuccess()
        {
            // Arrange
            var entity = new SupplierEntity { SupplierId = 1, SupplierName = "Test", IsDeleted = true };
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
            _supplierRepoMock.Setup(r => r.UpdateAsync(entity)).Returns(Task.CompletedTask);

            // Act
            var result = await _domain.UndeleteSupplierAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that supplier was undeleted.");
            Assert.That(result.Data, Is.True);
            TestContext.WriteLine("Asserted that undelete result is true.");
        }

        [Test]
        public async Task UndeleteSupplierAsync_ReturnsFailure_WhenNotFound()
        {
            // Arrange
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((SupplierEntity)null);

            // Act
            var result = await _domain.UndeleteSupplierAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that undelete failed due to missing supplier.");
            Assert.That(result.ErrorMessage, Is.EqualTo("Supplier not found."));
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task UndeleteSupplierAsync_ReturnsFailure_WhenAlreadyActive()
        {
            // Arrange
            var entity = new SupplierEntity { SupplierId = 1, SupplierName = "Test", IsDeleted = false };
            _supplierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

            // Act
            var result = await _domain.UndeleteSupplierAsync(1);

            // Assert
            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that undelete failed due to already active supplier.");
            Assert.That(result.ErrorMessage, Is.EqualTo("Supplier is already active."));
            TestContext.WriteLine("Asserted that error message is correct.");
        }
    }
}
