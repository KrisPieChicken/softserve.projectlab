using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using API.Data.Entities;
using API.Data.Repositories.LogisticsRepositories.Interfaces;
using API.Implementations.Domain;
using API.Services.Logistics;
using softserve.projectlabs.Shared.DTOs;
using CatalogTest = API.Models.Logistics.Catalog.Catalog;
using API.Services.IntAdmin;
using softserve.projectlabs.Shared.DTOs.Catalog;

namespace UnitTests.Catalog
{
    [TestFixture]
    public class CatalogServiceTests
    {
        private Mock<ICatalogRepository> _repoMock;
        private CatalogDomain _domain;
        private CatalogService _service;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<ICatalogRepository>();
            _domain = new CatalogDomain(_repoMock.Object);
            _service = new CatalogService(_domain);
        }

        [Test]
        public async Task AddCatalogAsync_ReturnsSuccess()
        {
            var dto = new CatalogDto { CatalogName = "Furniture" };
            var entity = new CatalogEntity
            {
                CatalogId = 5,
                CatalogName = "Furniture",
                CatalogDescription = "Home and office furniture",
                CatalogStatus = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _repoMock.Setup(r => r.AddAsync(It.IsAny<CatalogEntity>()))
                .Callback<CatalogEntity>(e =>
                {
                    e.CatalogId = entity.CatalogId;
                    e.CreatedAt = entity.CreatedAt;
                    e.UpdatedAt = entity.UpdatedAt;
                })
                .Returns(Task.CompletedTask);

            var result = await _service.AddCatalogAsync(dto);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, CatalogName={result.Data?.CatalogName}");
            Assert.That(result.IsSuccess, Is.True, "Debe tener éxito al agregar el catálogo.");
            TestContext.WriteLine("Asserted que la creación del catálogo via servicio tuvo éxito.");

            Assert.That(result.Data.CatalogName, Is.EqualTo("Furniture"), "El nombre del catálogo creado debe ser correcto.");
            TestContext.WriteLine("Asserted que el nombre del catálogo creado es correcto.");
        }

        [Test]
        public async Task GetCatalogByIdAsync_ReturnsSuccess()
        {
            var entity = new CatalogEntity
            {
                CatalogId = 7,
                CatalogName = "Clothing",
                CatalogDescription = "Apparel and garments",
                CatalogStatus = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _repoMock.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(entity);

            var result = await _service.GetCatalogByIdAsync(7);

            TestContext.WriteLine($"Result: IsSuccess={result.IsSuccess}, CatalogId={result.Data?.CatalogId}");
            Assert.That(result.IsSuccess, Is.True, "Debe tener éxito al obtener el catálogo por ID.");
            TestContext.WriteLine("Asserted que la obtención del catálogo via servicio tuvo éxito.");

            Assert.That(result.Data.CatalogId, Is.EqualTo(7), "El ID del catálogo retornado debe coincidir con el solicitado.");
            TestContext.WriteLine("Asserted que el ID del catálogo retornado coincide con el esperado.");
        }
    }
}
