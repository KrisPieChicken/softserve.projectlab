using API.Data.Entities;
using API.Data.Repositories.LogisticsRepositories.Interfaces;
using API.Implementations.Domain;
using API.Services.Logistics;
using Moq;
using softserve.projectlabs.Shared.DTOs;
using softserve.projectlabs.Shared.DTOs.Branch;

namespace UnitTests.Branch
{
    [TestFixture]
    public class BranchServiceTests
    {
        private Mock<IBranchRepository> _repoMock;
        private BranchDomain _domain;
        private BranchService _service;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<IBranchRepository>();
            _domain = new BranchDomain(_repoMock.Object);
            _service = new BranchService(_domain);
        }

        [Test]
        public async Task AddBranchAsync_ReturnsSuccess()
        {
            var dto = new BranchDto { BranchName = "Main" };
            var entity = new BranchEntity
            {
                BranchId = 1,
                BranchName = "Main",
                BranchCity = "City",
                BranchRegion = "Region",
                BranchContactNumber = "123",
                BranchContactEmail = "mail@mail.com",
                BranchAddress = "Addr",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            _repoMock.Setup(r => r.AddAsync(It.IsAny<BranchEntity>()))
                .Callback<BranchEntity>(e =>
                {
                    e.BranchId = 1;
                    e.CreatedAt = entity.CreatedAt;
                    e.UpdatedAt = entity.UpdatedAt;
                })
                .Returns(Task.CompletedTask);

            var result = await _service.AddBranchAsync(dto);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, BranchName={result.Data?.BranchName}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that branch creation succeeded.");
            Assert.That(result.Data.BranchName, Is.EqualTo("Main"));
            TestContext.WriteLine("Asserted that created branch name is correct.");
        }

        [Test]
        public async Task AddBranchAsync_ReturnsFailure()
        {
            var dto = new BranchDto { BranchName = "Main" };
            _repoMock.Setup(r => r.AddAsync(It.IsAny<BranchEntity>()))
                .ThrowsAsync(new Exception("fail"));

            var result = await _service.AddBranchAsync(dto);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that branch creation failed as expected.");
            Assert.That(result.ErrorMessage, Does.Contain("fail"));
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task UpdateBranchAsync_ReturnsSuccess()
        {
            var dto = new BranchDto { BranchId = 1, BranchName = "Main" };
            var entity = new BranchEntity
            {
                BranchId = 1,
                BranchName = "Main",
                BranchCity = "City",
                BranchRegion = "Region",
                BranchContactNumber = "123",
                BranchContactEmail = "mail@mail.com",
                BranchAddress = "Addr",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<BranchEntity>())).Returns(Task.CompletedTask);

            var result = await _service.UpdateBranchAsync(dto);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, BranchName={result.Data?.BranchName}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that branch update succeeded.");
            Assert.That(result.Data.BranchName, Is.EqualTo("Main"));
            TestContext.WriteLine("Asserted that updated branch name is correct.");
        }

        [Test]
        public async Task UpdateBranchAsync_ReturnsFailure()
        {
            var dto = new BranchDto { BranchId = 1, BranchName = "Main" };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((BranchEntity)null);

            var result = await _service.UpdateBranchAsync(dto);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that branch update failed as expected.");
            Assert.That(result.ErrorMessage, Is.EqualTo("Branch not found."));
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task GetBranchByIdAsync_ReturnsSuccess()
        {
            var entity = new BranchEntity
            {
                BranchId = 1,
                BranchName = "Main",
                BranchCity = "City",
                BranchRegion = "Region",
                BranchContactNumber = "123",
                BranchContactEmail = "mail@mail.com",
                BranchAddress = "Addr",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

            var result = await _service.GetBranchByIdAsync(1);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, BranchId={result.Data?.BranchId}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that branch retrieval succeeded.");
            Assert.That(result.Data.BranchId, Is.EqualTo(1));
            TestContext.WriteLine("Asserted that returned branch ID matches expected value.");
        }

        [Test]
        public async Task GetBranchByIdAsync_ReturnsFailure()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((BranchEntity)null);

            var result = await _service.GetBranchByIdAsync(1);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that branch retrieval failed as expected.");
            Assert.That(result.ErrorMessage, Is.EqualTo("Branch not found."));
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task GetAllBranchesAsync_ReturnsSuccess()
        {
            var entities = new List<BranchEntity>
            {
                new BranchEntity
                {
                    BranchId = 1,
                    BranchName = "Main",
                    BranchCity = "City",
                    BranchRegion = "Region",
                    BranchContactNumber = "123",
                    BranchContactEmail = "mail@mail.com",
                    BranchAddress = "Addr",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                }
            };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);

            var result = await _service.GetAllBranchesAsync();

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Count={result.Data?.Count}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that branches retrieval succeeded.");
            Assert.That(result.Data.Count, Is.EqualTo(1));
            TestContext.WriteLine("Asserted that the correct number of branches is returned.");
        }

        [Test]
        public async Task GetAllBranchesAsync_ReturnsFailure()
        {
            _repoMock.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("fail"));

            var result = await _service.GetAllBranchesAsync();

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that branches retrieval failed as expected.");
            Assert.That(result.ErrorMessage, Does.Contain("fail"));
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task RemoveBranchAsync_ReturnsSuccess()
        {
            _repoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _service.RemoveBranchAsync(1);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
            Assert.That(result.IsSuccess, Is.True);
            TestContext.WriteLine("Asserted that branch removal succeeded.");
            Assert.That(result.Data, Is.True);
            TestContext.WriteLine("Asserted that remove result is true.");
        }

        [Test]
        public async Task RemoveBranchAsync_ReturnsFailure()
        {
            _repoMock.Setup(r => r.DeleteAsync(1)).ThrowsAsync(new Exception("fail"));

            var result = await _service.RemoveBranchAsync(1);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False);
            TestContext.WriteLine("Asserted that branch removal failed as expected.");
            Assert.That(result.ErrorMessage, Does.Contain("fail"));
            TestContext.WriteLine("Asserted that error message is correct.");
        }
    }
}
