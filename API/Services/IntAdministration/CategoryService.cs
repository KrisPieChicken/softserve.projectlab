using API.Data.Entities;
using API.Implementations.Domain;
using API.Models;
using API.Models.IntAdmin;
using System.Collections.Generic;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.DTOs;

namespace API.Services.IntAdmin
{
    /// <summary>
    /// Service class for category operations. Delegates business logic to the CategoryDomain.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly CategoryDomain _categoryDomain;
        public CategoryService(CategoryDomain categoryDomain)
        {
            _categoryDomain = categoryDomain;
        }
        public async Task<Result<Category>> CreateCategoryAsync(CategoryDto categoryDto)
        {
            return await _categoryDomain.CreateCategoryAsync(categoryDto);
        }
        public async Task<Result<Category>> UpdateCategoryAsync(int categoryId, CategoryDto categoryDto)
        {
            return await _categoryDomain.UpdateCategoryAsync(categoryId, categoryDto);
        }
        public async Task<Result<Category>> GetCategoryByIdAsync(int categoryId)
        {
            return await _categoryDomain.GetCategoryByIdAsync(categoryId);
        }
        public async Task<Result<List<Category>>> GetAllCategoriesAsync()
        {
            return await _categoryDomain.GetAllCategoriesAsync();
        }
        public async Task<Result<bool>> DeleteCategoryAsync(int categoryId)
        {
            return await _categoryDomain.DeleteCategoryAsync(categoryId);
        }
    }
}
