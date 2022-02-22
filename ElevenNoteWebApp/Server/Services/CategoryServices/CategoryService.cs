using ElevenNoteWebApp.Server.Data;
using ElevenNoteWebApp.Server.Models;
using ElevenNoteWebApp.Shared.Models.Category;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenNoteWebApp.Server.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        //Field
        private readonly ApplicationDbContext _context;

        //Constructor

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Gets/Reads

        //GetAll

        public async Task<IEnumerable<CategoryListItem>> GetAllCategoriesAsync()
        {
            var categoryQuery = await _context.Categories
                .Select(categoryEntity => new CategoryListItem()
                {
                    Id = categoryEntity.Id,
                    Name = categoryEntity.Name


                }).ToListAsync();

            return categoryQuery; //Not doing it, but for future reference this line could be return await categoryQuery.ToListAsync(), instead of doing all of it above

        }

        //GetById
        public Task<CategoryDetail> GetCategoryByIdAsync(int categoryId)
        {
            throw new System.NotImplementedException();
        }

        //Create

        public async Task<bool> CreateCategoryAsync(CategoryCreate model)
        {
            if (model is null)
                return false;

            var categoryEntity = new Category()
            {
                Name = model.Name
            };

            _context.Categories.Add(categoryEntity);

            return await _context.SaveChangesAsync() == 1;
        }

        //Update

        public async Task<bool> UpdateCategoryAsync(CategoryEdit model)
        {
            if (model is null)
                return false;

            var categoryEntity = await _context.Categories.FindAsync(model.Id);

            if (categoryEntity is null)
                return false;

            categoryEntity.Name = model.Name;

            return await _context.SaveChangesAsync() == 1;

        }

        //Delete
        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var categoryEntity = await _context.Categories.FindAsync(categoryId);

            if (categoryEntity is null)
                return false;

            _context.Categories.Remove(categoryEntity);

            return await _context.SaveChangesAsync() == 1;

        }
    }
}
