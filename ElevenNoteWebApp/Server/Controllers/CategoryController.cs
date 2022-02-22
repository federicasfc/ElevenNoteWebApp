using ElevenNoteWebApp.Server.Services.CategoryServices;
using ElevenNoteWebApp.Shared.Models.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ElevenNoteWebApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        //Field

        private readonly ICategoryService _categoryService;

        //Constructor

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        //GetAll

        [HttpGet]

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            //could add the .Any/NoContent validation- maybe later

            return Ok(categories);



        }

        //GetById

        [HttpGet("{id}")]

        public async Task<IActionResult> Category(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category is null)
                return NotFound();

            return Ok(category);
        }

        //CreateCategory

        [HttpPost]
        
        public async Task<IActionResult> Create(CategoryCreate model)
        {
            if (!ModelState.IsValid || model == null)
                return BadRequest(ModelState);

            if (!await _categoryService.CreateCategoryAsync(model))
                return UnprocessableEntity();

            return Ok("Category created successfully");

        }

        //UpdateCategory

        [HttpPut("edit/{id}")]

        public async Task<IActionResult> Edit(int id, CategoryEdit model)
        {
            if (!ModelState.IsValid || model == null)
                return BadRequest(ModelState);

            if (id != model.Id)
                return BadRequest();

            if (!await _categoryService.UpdateCategoryAsync(model)) 
                return BadRequest();

            return Ok("Category updated successfully");
        }

        //Delete

        [HttpDelete("delete/{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category is null)
                return NotFound();

            if (!await _categoryService.DeleteCategoryAsync(id))
                return BadRequest();

            return Ok($"Category {id} deleted successfully");
        }
    }
}
