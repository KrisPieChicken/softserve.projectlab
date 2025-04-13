using API.Data.Entities;
using API.Models;
using API.Models.IntAdmin;
using System.Collections.Generic;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.DTOs;

namespace API.Services.IntAdmin
{
    /// <summary>
    /// Service interface for category operations.
    /// </summary>
    public interface ICategoryService
    {
        Task<Result<Category>> CreateCategoryAsync(CategoryDto categoryDto);
        Task<Result<Category>> UpdateCategoryAsync(int categoryId, CategoryDto categoryDto);
        Task<Result<Category>> GetCategoryByIdAsync(int categoryId);
        Task<Result<List<Category>>> GetAllCategoriesAsync();
        Task<Result<bool>> DeleteCategoryAsync(int categoryId);
    }
}
