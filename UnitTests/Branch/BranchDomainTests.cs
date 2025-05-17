using Moq;
using API.Implementations.Domain;
using API.Data.Entities;
using API.Data.Repositories.LogisticsRepositories.Interfaces;
using softserve.projectlabs.Shared.DTOs;
using BranchTest = API.Models.Logistics.Branch.Branch;


namespace UnitTests.Branch
{
    [TestFixture]
    public class BranchDomainTests
    {
        private Mock<IBranchRepository> _repoMock;
        private BranchDomain _domain;


        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<IBranchRepository>();
            _domain = new BranchDomain(_repoMock.Object);
        }

        [Test]
        public async Task CreateBranchAsync_ReturnsCreatedBranch()
        {
            var dto = new BranchDto
            {
                BranchName = "Main",
                BranchCity = "City",
                BranchRegion = "Region",
                BranchContactNumber = "123",
                BranchContactEmail = "main@branch.com",
                BranchAddress = "123 Main St"
            };

            BranchEntity? addedEntity = null;
            _repoMock.Setup(r => r.AddAsync(It.IsAny<BranchEntity>()))
                .Callback<BranchEntity>(e => addedEntity = e)
                .Returns(Task.CompletedTask);

            // Simulate ToDomain mapping
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(addedEntity);

            var result = await _domain.CreateBranchAsync(dto);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, BranchName={result.Data?.BranchName}");
            Assert.That(result.IsSuccess, Is.True, "Should succeed when branch is created.");
            TestContext.WriteLine("Asserted that branch creation succeeded.");

            Assert.That(result.Data.BranchName, Is.EqualTo("Main"), "Created branch should have correct name.");
            TestContext.WriteLine("Asserted that created branch name is correct.");
        }

        [Test]
        public async Task GetBranchById_ReturnsBranch_WhenFound()
        {
            var entity = new BranchEntity { BranchId = 1, BranchName = "Main", IsDeleted = false };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

            var result = await _domain.GetBranchById(1);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
            Assert.That(result.IsSuccess, Is.True, "Branch should be found.");
            TestContext.WriteLine("Asserted that branch is found.");

            Assert.That(result.Data, Is.Not.Null, "Returned branch data should not be null.");
            TestContext.WriteLine("Asserted that returned branch data is not null.");

            Assert.That(result.Data.BranchId, Is.EqualTo(1), "BranchId should match the requested ID.");
            TestContext.WriteLine("Asserted that returned branch ID matches expected value.");
        }

        [Test]
        public async Task GetBranchById_ReturnsFailure_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((BranchEntity)null);

            var result = await _domain.GetBranchById(1);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False, "Branch should not be found.");
            TestContext.WriteLine("Asserted that branch is not found.");

            Assert.That(result.ErrorMessage, Is.EqualTo("Branch not found."), "Error message should indicate not found.");
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task GetAllBranches_ReturnsBranches()
        {
            var entities = new List<BranchEntity>
            {
                new BranchEntity { BranchId = 1, BranchName = "Main", IsDeleted = false }
            };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);

            var result = await _domain.GetAllBranches();

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Count={result.Data?.Count}");
            Assert.That(result.IsSuccess, Is.True, "Should succeed when branches are found.");
            TestContext.WriteLine("Asserted that branches are found.");

            Assert.That(result.Data.Count, Is.EqualTo(1), "Should return one branch.");
            TestContext.WriteLine("Asserted that the correct number of branches is returned.");
        }

        [Test]
        public async Task UpdateBranch_ReturnsSuccess_WhenFound()
        {
            var branch = new BranchTest(
                1, "Main", "City", "Region", "123 Main St", "123", "main@branch.com", DateTime.UtcNow, DateTime.UtcNow, false
            );
            var entity = new BranchEntity { BranchId = 1, BranchName = "Main", IsDeleted = false };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.UpdateAsync(entity)).Returns(Task.CompletedTask);

            var result = await _domain.UpdateBranch(branch);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
            Assert.That(result.IsSuccess, Is.True, "Should succeed when branch is updated.");
            TestContext.WriteLine("Asserted that branch update succeeded.");

            Assert.That(result.Data, Is.Not.Null, "Data should not be null indicating update success.");
            TestContext.WriteLine("Asserted that update result is not null.");
        }

        [Test]
        public async Task UpdateBranch_ReturnsFailure_WhenNotFound()
        {
            var branch = new BranchTest(
                1, "Main", "City", "Region", "123 Main St", "123", "main@branch.com", DateTime.UtcNow, DateTime.UtcNow, false
            );
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((BranchEntity)null);

            var result = await _domain.UpdateBranch(branch);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False, "Should fail when branch is not found.");
            TestContext.WriteLine("Asserted that update failed as expected.");

            Assert.That(result.ErrorMessage, Is.EqualTo("Branch not found."), "Should return correct error message.");
            TestContext.WriteLine("Asserted that error message is correct.");
        }

        [Test]
        public async Task RemoveBranch_ReturnsSuccess()
        {
            _repoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _domain.RemoveBranch(1);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data={result.Data}");
            Assert.That(result.IsSuccess, Is.True, "Should succeed when branch is removed.");
            TestContext.WriteLine("Asserted that branch removal succeeded.");

            Assert.That(result.Data, Is.True, "Data should be true indicating remove success.");
            TestContext.WriteLine("Asserted that remove result is true.");
        }

        [Test]
        public async Task RemoveBranch_ReturnsFailure_OnException()
        {
            _repoMock.Setup(r => r.DeleteAsync(1)).ThrowsAsync(new Exception("DB error"));

            var result = await _domain.RemoveBranch(1);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, ErrorMessage={result.ErrorMessage}");
            Assert.That(result.IsSuccess, Is.False, "Should fail when exception is thrown.");
            TestContext.WriteLine("Asserted that remove failed as expected.");

            Assert.That(result.ErrorMessage, Does.Contain("Failed to remove branch"), "Should return correct error message.");
            TestContext.WriteLine("Asserted that error message is correct.");
        }
    }
}
