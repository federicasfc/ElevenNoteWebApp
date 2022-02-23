using ElevenNoteWebApp.Server.Services.NoteServices;
using ElevenNoteWebApp.Shared.Models.Note;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElevenNoteWebApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase //General question: difference between API controller and MVC controller (ControllerBase vs Controller); Why we did an API Controller here instead of an MVC one...
    {
        //Field
        
        private readonly INoteService _noteService;

        //Constructor
        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        //GetAll

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!SetUserIdInService())
                return Unauthorized();

            var notes = await _noteService.GeAllNotesAsync();

            return Ok(notes); //In the module (4.01), it has notes.ToList, but that's already established in GetAllNotesAsync, so not sure why it's reiterated?
        }

        //GetById

        [HttpGet("{id}")]
        
        public async Task<IActionResult> Note(int id) //don't love this name; might change to Detail...again
        {
            if (!SetUserIdInService())
                return Unauthorized();

            var note = await _noteService.GetNoteByIdAsync(id);

            if (note is null)
                return NotFound();

            return Ok(note);

        }

        //GetNoteByCategory

        [HttpGet("category/{id}")] //probably will move this to category controller? haven't decided

        public async Task<IActionResult> CategoryIndex(int id)
        {
            if (!SetUserIdInService())
                return Unauthorized();

            var notesByCategory = await _noteService.GetNotesByCategory(id);

            if (!notesByCategory.Any())
                return NoContent();

            return Ok(notesByCategory);

        } //might actually put link to this in the Category detail page...not sure yet

        //CreateNote

        [HttpPost]

        public async Task<IActionResult> Create(NoteCreate model)
        {
            if (!SetUserIdInService())
                return Unauthorized();

            if (!ModelState.IsValid || model is null) //both of these checks are necesssary because a null ModelState will return as IsValid
                return BadRequest(ModelState);


            bool wasSuccessful = await _noteService.CreateNoteAsync(model); //kind of redundant because method already returns a bool, but leaving it for now

            if (!wasSuccessful)
                return UnprocessableEntity();

            return Ok("Note created successfully");



        }

        //UpdateNote

        [HttpPut("edit/{id}")]

        public async Task<IActionResult> Edit(int id, NoteEdit model)
        {
            if (!SetUserIdInService())
                return Unauthorized();

            if (!ModelState.IsValid || model is null)
                return BadRequest(ModelState);

            if (model.Id != id)
                return BadRequest();

            if (!await _noteService.UpdateNoteAsync(model)) //changed from creating the wasSuccessful bool (see above), if something breaks, look here
                return BadRequest();

            return Ok("Note updated successfully");



        }

        //DeleteNote

        [HttpDelete("delete/{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            if (!SetUserIdInService())
                return Unauthorized(); //next time shove this into another helper method maybe; annoyed at copy pasting it

            var note = await _noteService.GetNoteByIdAsync(id);

            if (note is null)
                return NotFound();

            if (!await _noteService.DeleteNoteAsync(id))
                return BadRequest();

            return Ok($"Note {id} deleted successfully");

        }



        //Helper Methods:

        //Helper method to set user Id

        private bool SetUserIdInService()
        {
            var userId = GetUserId();

            if(userId == null)
                return false;

            _noteService.SetUserId(userId);
            return true;
        }

        //Helper method to get user Id (from auth token sent from API after user logs in)
        private string GetUserId()
        {
            string userIdClaim = User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value;

            if (userIdClaim == null)
                return null;

            return userIdClaim;
        }
     }
}
