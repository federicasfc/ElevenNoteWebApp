using ElevenNoteWebApp.Shared.Models.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElevenNoteWebApp.Server.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryListItem>> GetAllCategoriesAsync();

        Task<CategoryDetail> GetCategoryByIdAsync(int categoryId);

        Task<bool> CreateCategoryAsync(CategoryCreate model);

        Task<bool> UpdateCategoryAsync(CategoryEdit model);

        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}
