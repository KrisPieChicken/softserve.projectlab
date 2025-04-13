using API.Data;
using API.Data.Entities;
using softserve.projectlabs.Shared.DTOs;
using API.Models.IntAdmin;
using Microsoft.EntityFrameworkCore;
using softserve.projectlabs.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Implementations.Domain
{
    public class CategoryDomain
    {
        private readonly ApplicationDbContext _context;

        public CategoryDomain(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new category in the database.
        /// </summary>
        public async Task<Result<Category>> CreateCategoryAsync(CategoryDto categoryDto)
        {
            try
            {
                var categoryEntity = new CategoryEntity
                {
                    CategoryName = categoryDto.CategoryName,
                    CategoryStatus = categoryDto.CategoryStatus
                };

                _context.CategoryEntities.Add(categoryEntity);
                await _context.SaveChangesAsync();

                var category = await MapToCategory(categoryEntity.CategoryId);
                return Result<Category>.Success(category);
            }
            catch (Exception ex)
            {
                return Result<Category>.Failure($"Error creating category: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        public async Task<Result<Category>> UpdateCategoryAsync(int categoryId, CategoryDto categoryDto)
        {
            try
            {
                var categoryEntity = await _context.CategoryEntities
                    .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
                if (categoryEntity == null)
                    return Result<Category>.Failure("Category not found.");

                categoryEntity.CategoryName = categoryDto.CategoryName;
                categoryEntity.CategoryStatus = categoryDto.CategoryStatus;

                await _context.SaveChangesAsync();

                var category = await MapToCategory(categoryId);
                return Result<Category>.Success(category);
            }
            catch (Exception ex)
            {
                return Result<Category>.Failure($"Error updating category: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a category by its unique ID.
        /// </summary>
        public async Task<Result<Category>> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                var category = await MapToCategory(categoryId);
                if (category == null)
                    return Result<Category>.Failure("Category not found.");
                return Result<Category>.Success(category);
            }
            catch (Exception ex)
            {
                return Result<Category>.Failure($"Error retrieving category: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        public async Task<Result<List<Category>>> GetAllCategoriesAsync()
        {
            try
            {
                var categoryEntities = await _context.CategoryEntities.ToListAsync();
                var categories = new List<Category>();
                foreach (var entity in categoryEntities)
                {
                    var category = await MapToCategory(entity.CategoryId);
                    if (category != null)
                        categories.Add(category);
                }
                return Result<List<Category>>.Success(categories);
            }
            catch (Exception ex)
            {
                return Result<List<Category>>.Failure($"Error retrieving categories: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a category by its unique ID.
        /// </summary>
        public async Task<Result<bool>> DeleteCategoryAsync(int categoryId)
        {
            try
            {
                var categoryEntity = await _context.CategoryEntities
                    .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
                if (categoryEntity == null)
                    return Result<bool>.Failure("Category not found.");

                _context.CategoryEntities.Remove(categoryEntity);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error deleting category: {ex.Message}");
            }
        }

        /// <summary>
        /// Maps a CategoryEntity to the domain model Category.
        /// Includes mapping of items associated to the category.
        /// </summary>
        private async Task<Category> MapToCategory(int categoryId)
        {
            var categoryEntity = await _context.CategoryEntities
                .Include(c => c.ItemEntities)  // Incluye los items asociados.
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
            if (categoryEntity == null)
                return null;

            var category = new Category
            {
                CategoryId = categoryEntity.CategoryId,
                CategoryName = categoryEntity.CategoryName,
                CategoryStatus = categoryEntity.CategoryStatus,
                Items = new List<Item>()
            };
            return category;
        }
    }
}
