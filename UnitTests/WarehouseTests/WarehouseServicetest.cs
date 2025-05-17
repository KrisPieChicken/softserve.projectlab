using API.Implementations.Domain;
using API.Services.Logistics;
using Moq;
using API.Data.Repositories.LogisticsRepositories.Interfaces;
using API.Data.Entities;

namespace UnitTests.WarehouseTests;

[TestFixture]
public class WarehouseServiceTests
{
    private Mock<IWarehouseRepository> _repoMock;
    private WarehouseDomain _domain;
    private WarehouseService _service;

    [SetUp]
    public void SetUp()
    {
        _repoMock = new Mock<IWarehouseRepository>();
        _domain = new WarehouseDomain(_repoMock.Object);
        _service = new WarehouseService(_domain);
    }

    [Test]
    public async Task GetAllWarehousesAsync_ReturnsMappedDtos_WhenDomainSuccess()
    {
        // Arrange
        var warehouseEntity = new WarehouseEntity
        {
            WarehouseId = 1,
            WarehouseLocation = "Loc",
            WarehouseCapacity = 10,
            BranchId = 1,
            WarehouseItemEntities = new List<WarehouseItemEntity>()
        };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<WarehouseEntity> { warehouseEntity });

        // Act
        var result = await _service.GetAllWarehousesAsync();

        // Assert
        TestContext.WriteLine($"Result count: {result.Count}");
        Assert.That(result.Count, Is.EqualTo(1), "Should return one warehouse DTO");
        TestContext.WriteLine("Asserted that one warehouse DTO is returned.");

        Assert.That(result[0].WarehouseId, Is.EqualTo(1), "WarehouseId should be 1");
        TestContext.WriteLine("Asserted that WarehouseId is 1.");

        Assert.That(result[0].Name, Is.Not.Null, "Name should not be null (mapping logic)");
        TestContext.WriteLine("Asserted that warehouse name is not null.");
    }

    [Test]
    public async Task GetAllWarehousesAsync_ReturnsEmptyList_WhenDomainFails()
    {
        // Arrange
        _repoMock.Setup(r => r.GetAllAsync()).ThrowsAsync(new System.Exception("error"));

        // Act
        var result = await _service.GetAllWarehousesAsync();

        // Assert
        TestContext.WriteLine($"Result is null: {result == null}");
        Assert.That(result, Is.Not.Null, "Should not be null even if domain fails");
        TestContext.WriteLine("Asserted that result is not null.");

        Assert.That(result.Count, Is.EqualTo(0), "Should return an empty list");
        TestContext.WriteLine("Asserted that result is an empty list.");
    }

    [Test]
    public async Task GetWarehouseByIdAsync_ReturnsMappedDto_WhenDomainSuccess()
    {
        // Arrange
        var warehouseEntity = new WarehouseEntity
        {
            WarehouseId = 1,
            WarehouseLocation = "Loc",
            WarehouseCapacity = 10,
            BranchId = 1,
            WarehouseItemEntities = new List<WarehouseItemEntity>()
        };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(warehouseEntity);

        // Act
        var result = await _service.GetWarehouseByIdAsync(1);

        // Assert
        TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, WarehouseId={result.Data?.WarehouseId}, Name={result.Data?.Name}");
        Assert.That(result.IsSuccess, Is.True, "Should succeed when warehouse is found");
        TestContext.WriteLine("Asserted that warehouse is found.");

        Assert.That(result.Data.WarehouseId, Is.EqualTo(1), "WarehouseId should be 1");
        TestContext.WriteLine("Asserted that WarehouseId is 1.");

        Assert.That(result.Data.Name, Is.Not.Null, "Name should not be null (mapping logic)");
        TestContext.WriteLine("Asserted that warehouse name is not null.");
    }

    [Test]
    public async Task GetWarehouseByIdAsync_ReturnsFailure_WhenDomainFails()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((WarehouseEntity)null);

        // Act
        var result = await _service.GetWarehouseByIdAsync(1);

        // Assert
        TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
        Assert.That(result.IsSuccess, Is.False, "Should fail when warehouse is not found");
        TestContext.WriteLine("Asserted that warehouse is not found.");

        Assert.That(result.ErrorMessage, Is.EqualTo("Warehouse not found."), "Should return correct error message");
        TestContext.WriteLine("Asserted that error message is correct.");
    }

    [Test]
    public async Task AddItemToWarehouseAsync_AddsItemAndUpdatesWarehouse()
    {
        // Arrange
        var warehouseEntity = new WarehouseEntity
        {
            WarehouseId = 1,
            WarehouseLocation = "Loc",
            WarehouseCapacity = 10,
            BranchId = 1,
            WarehouseItemEntities = new List<WarehouseItemEntity>()
        };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(warehouseEntity);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<WarehouseEntity>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.AddItemToWarehouseAsync(1, 100, 5);

        // Assert
        TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
        Assert.That(result.IsSuccess, Is.True, "Should succeed when item is added");
        TestContext.WriteLine("Asserted that item addition succeeded.");

        Assert.That(result.Data, Is.True, "Data should be true indicating update success");
        TestContext.WriteLine("Asserted that update result is true.");
    }

    [Test]
    public async Task AddItemToWarehouseAsync_ReturnsFailure_WhenWarehouseNotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((WarehouseEntity)null);

        // Act
        var result = await _service.AddItemToWarehouseAsync(1, 100, 5);

        // Assert
        TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
        Assert.That(result.IsSuccess, Is.False, "Should fail when warehouse is not found");
        TestContext.WriteLine("Asserted that item addition failed as expected.");

        Assert.That(result.ErrorMessage, Is.EqualTo("Warehouse not found."), "Should return correct error message");
        TestContext.WriteLine("Asserted that error message is correct.");
    }

    [Test]
    public async Task RemoveItemFromWarehouseAsync_RemovesItemAndUpdatesWarehouse()
    {
        // Arrange
        var warehouseEntity = new WarehouseEntity
        {
            WarehouseId = 1,
            WarehouseLocation = "Loc",
            WarehouseCapacity = 10,
            BranchId = 1,
            WarehouseItemEntities = new List<WarehouseItemEntity>
            {
                new WarehouseItemEntity { Sku = 100, ItemQuantity = 5 }
            }
        };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(warehouseEntity);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<WarehouseEntity>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.RemoveItemFromWarehouseAsync(1, 100);

        // Assert
        TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
        Assert.That(result.IsSuccess, Is.True, "Should succeed when item is removed");
        TestContext.WriteLine("Asserted that item removal succeeded.");

        Assert.That(result.Data, Is.True, "Data should be true indicating update success");
        TestContext.WriteLine("Asserted that update result is true.");
    }

    [Test]
    public async Task TransferItemAsync_TransfersItemBetweenWarehouses()
    {
        // Arrange
        var sourceEntity = new WarehouseEntity
        {
            WarehouseId = 1,
            WarehouseLocation = "Loc",
            WarehouseCapacity = 10,
            BranchId = 1,
            WarehouseItemEntities = new List<WarehouseItemEntity>
            {
                new WarehouseItemEntity { Sku = 100, ItemQuantity = 5 }
            }
        };
        var targetEntity = new WarehouseEntity
        {
            WarehouseId = 2,
            WarehouseLocation = "Loc2",
            WarehouseCapacity = 10,
            BranchId = 1,
            WarehouseItemEntities = new List<WarehouseItemEntity>()
        };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(sourceEntity);
        _repoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(targetEntity);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<WarehouseEntity>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.TransferItemAsync(1, 100, 2, 2);

        // Assert
        TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
        Assert.That(result.IsSuccess, Is.True, "Should succeed when transfer is successful");
        TestContext.WriteLine("Asserted that item transfer succeeded.");

        Assert.That(result.Data, Is.True, "Data should be true indicating transfer success");
        TestContext.WriteLine("Asserted that transfer result is true.");
    }
}
