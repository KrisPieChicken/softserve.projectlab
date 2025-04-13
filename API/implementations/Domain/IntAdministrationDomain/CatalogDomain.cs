using API.Data;
using API.Data.Entities;
using API.Models;
using softserve.projectlabs.Shared.DTOs;
using API.Models.IntAdmin;
using API.Models.IntAdmin.Interfaces;
using Microsoft.EntityFrameworkCore;
using softserve.projectlabs.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Implementations.Domain
{
    public class CatalogDomain
    {
        private readonly ApplicationDbContext _context;
        public CatalogDomain(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new catalog in the database. If category IDs are provided, adds entries to CatalogCategoryEntity.
        /// </summary>
        public async Task<Result<Catalog>> CreateCatalogAsync(CatalogDto catalogDto)
        {
            try
            {
                // Create CatalogEntity from DTO.
                var catalogEntity = new CatalogEntity
                {
                    CatalogName = catalogDto.CatalogName,
                    CatalogDescription = catalogDto.CatalogDescription,
                    CatalogStatus = catalogDto.CatalogStatus
                };

                _context.CatalogEntities.Add(catalogEntity);
                await _context.SaveChangesAsync();

                // If CategoryIds are provided, add CatalogCategoryEntity rows.
                if (catalogDto.CategoryIds != null && catalogDto.CategoryIds.Any())
                {
                    foreach (var catId in catalogDto.CategoryIds)
                    {
                        // Aquí podrías validar si la categoría existe, si lo deseas.
                        var catalogCategoryEntity = new CatalogCategoryEntity
                        {
                            CatalogId = catalogEntity.CatalogId,
                            CategoryId = catId
                        };
                        _context.CatalogCategoryEntities.Add(catalogCategoryEntity);
                    }
                    await _context.SaveChangesAsync();
                }

                var catalog = await MapToCatalog(catalogEntity.CatalogId);
                return Result<Catalog>.Success(catalog);
            }
            catch (Exception ex)
            {
                return Result<Catalog>.Failure($"Error creating catalog: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing catalog in the database and synchronizes its category associations.
        /// </summary>
        public async Task<Result<Catalog>> UpdateCatalogAsync(int catalogId, CatalogDto catalogDto)
        {
            try
            {
                var catalogEntity = await _context.CatalogEntities
                    .Include(c => c.CatalogCategoryEntities)
                    .FirstOrDefaultAsync(c => c.CatalogId == catalogId);

                if (catalogEntity == null)
                    return Result<Catalog>.Failure("Catalog not found.");

                // Update basic properties.
                catalogEntity.CatalogName = catalogDto.CatalogName;
                catalogEntity.CatalogDescription = catalogDto.CatalogDescription;
                catalogEntity.CatalogStatus = catalogDto.CatalogStatus;

                // Synchronize category associations.
                if (catalogDto.CategoryIds != null)
                {
                    // Remove associations not in the DTO.
                    var toRemove = catalogEntity.CatalogCategoryEntities
                        .Where(cc => !catalogDto.CategoryIds.Contains(cc.CategoryId))
                        .ToList();
                    _context.CatalogCategoryEntities.RemoveRange(toRemove);

                    // Determine which category IDs to add.
                    var currentCategoryIds = catalogEntity.CatalogCategoryEntities.Select(cc => cc.CategoryId).ToList();
                    var toAdd = catalogDto.CategoryIds.Except(currentCategoryIds).ToList();
                    foreach (var catId in toAdd)
                    {
                        var newCatalogCategory = new CatalogCategoryEntity
                        {
                            CatalogId = catalogEntity.CatalogId,
                            CategoryId = catId
                        };
                        _context.CatalogCategoryEntities.Add(newCatalogCategory);
                    }
                }

                await _context.SaveChangesAsync();
                var updatedCatalog = await MapToCatalog(catalogEntity.CatalogId);
                return Result<Catalog>.Success(updatedCatalog);
            }
            catch (Exception ex)
            {
                return Result<Catalog>.Failure($"Error updating catalog: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a catalog by its ID including its associated categories.
        /// </summary>
        public async Task<Result<Catalog>> GetCatalogByIdAsync(int catalogId)
        {
            try
            {
                var catalog = await MapToCatalog(catalogId);
                if (catalog == null)
                    return Result<Catalog>.Failure("Catalog not found.");
                return Result<Catalog>.Success(catalog);
            }
            catch (Exception ex)
            {
                return Result<Catalog>.Failure($"Error retrieving catalog: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all catalogs from the database.
        /// </summary>
        public async Task<Result<List<Catalog>>> GetAllCatalogsAsync()
        {
            try
            {
                var catalogEntities = await _context.CatalogEntities.ToListAsync();
                var catalogs = new List<Catalog>();
                foreach (var entity in catalogEntities)
                {
                    var catalog = await MapToCatalog(entity.CatalogId);
                    if (catalog != null)
                        catalogs.Add(catalog);
                }
                return Result<List<Catalog>>.Success(catalogs);
            }
            catch (Exception ex)
            {
                return Result<List<Catalog>>.Failure($"Error retrieving catalogs: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a catalog from the database.
        /// </summary>
        public async Task<Result<bool>> DeleteCatalogAsync(int catalogId)
        {
            try
            {
                var catalogEntity = await _context.CatalogEntities.FirstOrDefaultAsync(c => c.CatalogId == catalogId);
                if (catalogEntity == null)
                    return Result<bool>.Failure("Catalog not found.");

                _context.CatalogEntities.Remove(catalogEntity);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error deleting catalog: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a list of category IDs to a catalog by creating CatalogCategoryEntity entries.
        /// </summary>
        public async Task<Result<bool>> AddCategoriesToCatalogAsync(int catalogId, List<int> categoryIds)
        {
            try
            {
                var catalogEntity = await _context.CatalogEntities
                    .Include(c => c.CatalogCategoryEntities)
                    .FirstOrDefaultAsync(c => c.CatalogId == catalogId);
                if (catalogEntity == null)
                    return Result<bool>.Failure("Catalog not found.");

                foreach (var catId in categoryIds)
                {
                    if (!catalogEntity.CatalogCategoryEntities.Any(cc => cc.CategoryId == catId))
                    {
                        var categoryEntity = await _context.CategoryEntities
                            .FirstOrDefaultAsync(c => c.CategoryId == catId);
                        if (categoryEntity == null)
                            return Result<bool>.Failure($"Category with id {catId} not found.");

                        // Adjunta la categoría al contexto para que EF sepa que es existente.
                        _context.Attach(categoryEntity);

                        var newCatalogCategory = new CatalogCategoryEntity
                        {
                            CatalogId = catalogId,
                            CategoryId = catId,
                            CategoryName = categoryEntity.CategoryName,
                            Category = categoryEntity // Opcional, pero útil para mantener la relación de navegación.
                        };
                        _context.CatalogCategoryEntities.Add(newCatalogCategory);
                    }
                }
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error adding categories: {ex.Message}");
            }
        }


        /// <summary>
        /// Removes a category association from a catalog.
        /// </summary>
        public async Task<Result<bool>> RemoveCategoryFromCatalogAsync(int catalogId, int categoryId)
        {
            try
            {
                var catalogCategory = await _context.CatalogCategoryEntities
                    .FirstOrDefaultAsync(cc => cc.CatalogId == catalogId && cc.CategoryId == categoryId);
                if (catalogCategory == null)
                    return Result<bool>.Failure("Category not found in catalog.");
                _context.CatalogCategoryEntities.Remove(catalogCategory);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error removing category: {ex.Message}");
            }
        }

        // Helper: Maps a CatalogEntity (including its associated categories) to the domain model Catalog.
        private async Task<Catalog> MapToCatalog(int catalogId)
        {
            var catalogEntity = await _context.CatalogEntities
                .Include(c => c.CatalogCategoryEntities)
                .FirstOrDefaultAsync(c => c.CatalogId == catalogId);
            if (catalogEntity == null)
                return null;

            var catalog = new Catalog
            {
                CatalogID = catalogEntity.CatalogId,
                CatalogName = catalogEntity.CatalogName,
                CatalogDescription = catalogEntity.CatalogDescription,
                CatalogStatus = catalogEntity.CatalogStatus,
                Categories = new List<ICategory>()
            };

            var catalogCategories = await _context.CatalogCategoryEntities
                .Where(cc => cc.CatalogId == catalogId)
                .ToListAsync();
            foreach (var cc in catalogCategories)
            {
                var categoryEntity = await _context.CategoryEntities.FirstOrDefaultAsync(cat => cat.CategoryId == cc.CategoryId);
                if (categoryEntity != null)
                {
                    // Se asume que existe una clase Category que implementa ICategory.
                    var category = new Category
                    {
                        CategoryId = categoryEntity.CategoryId,
                        CategoryName = categoryEntity.CategoryName
                        // Agrega otras propiedades si es necesario.
                    };
                    catalog.Categories.Add(category);
                }
            }
            return catalog;
        }
    }
}
