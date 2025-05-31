using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using API.Implementations.Domain;
using API.Data.Entities;
using API.Data.Repositories.LogisticsRepositories.Interfaces;
using softserve.projectlabs.Shared.DTOs;
using CatalogTest = API.Models.Logistics.Catalog.Catalog;
using softserve.projectlabs.Shared.DTOs.Catalog;

namespace UnitTests.Catalog
{
    [TestFixture]
    public class CatalogDomainTests
    {
        private Mock<ICatalogRepository> _repoMock;
        private CatalogDomain _domain;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<ICatalogRepository>();
            _domain = new CatalogDomain(_repoMock.Object);
        }

        [Test]
        public async Task CreateCatalogAsync_ReturnsCreatedCatalog()
        {
            var dto = new CatalogDto
            {
                CatalogName = "Electronics",
                CatalogDescription = "All electronic items",
                CatalogStatus = true
            };

            CatalogEntity? addedEntity = null;
            _repoMock.Setup(r => r.AddAsync(It.IsAny<CatalogEntity>()))
                .Callback<CatalogEntity>(e => addedEntity = e)
                .Returns(Task.CompletedTask);

            // Simular el mapeo y recuperación tras creación
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(addedEntity);

            var result = await _domain.CreateCatalogAsync(dto);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, CatalogName={result.Data?.CatalogName}");
            Assert.That(result.IsSuccess, Is.True, "Debe tener éxito al crear el catálogo.");
            TestContext.WriteLine("Asserted que la creación del catálogo tuvo éxito.");

            Assert.That(result.Data.CatalogName, Is.EqualTo("Electronics"), "El nombre del catálogo creado debe ser correcto.");
            TestContext.WriteLine("Asserted que el nombre del catálogo creado es correcto.");
        }

        [Test]
        public async Task GetCatalogById_ReturnsCatalog_WhenFound()
        {
            var entity = new CatalogEntity
            {
                CatalogId = 10,
                CatalogName = "Books",
                CatalogDescription = "All kinds of books",
                CatalogStatus = true
            };
            _repoMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(entity);

            var result = await _domain.GetCatalogById(10);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, Data.Id={result.Data?.CatalogId}");
            Assert.That(result.IsSuccess, Is.True, "Debe encontrar el catálogo.");
            TestContext.WriteLine("Asserted que el catálogo fue encontrado.");

            Assert.That(result.Data, Is.Not.Null, "Los datos devueltos no deben ser nulos.");
            TestContext.WriteLine("Asserted que los datos retornados no son nulos.");

            Assert.That(result.Data.CatalogId, Is.EqualTo(10), "El ID del catálogo debe coincidir con el solicitado.");
            TestContext.WriteLine("Asserted que el ID del catálogo retornado coincide con el esperado.");
        }
    }
}
