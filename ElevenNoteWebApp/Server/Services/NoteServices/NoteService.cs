using ElevenNoteWebApp.Server.Data;
using ElevenNoteWebApp.Server.Models;
using ElevenNoteWebApp.Shared.Models.Note;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenNoteWebApp.Server.Services.NoteServices
{
    public class NoteService : INoteService
    {
        //Field

        private readonly ApplicationDbContext _context;

        private string _userId;

        //Constructor

        public NoteService(ApplicationDbContext context)
        {
            _context = context; 

        }

        //Read/Gets

        //GetAllNotes associated with User
        public async Task<IEnumerable<NoteListItem>> GeAllNotesAsync()
        {
            var noteQuery = await _context.Notes
                .Where(n => n.OwnerId == _userId)
                .Select(n => new NoteListItem()
                {
                    Id = n.Id,
                    Title = n.Title,
                    CategoryName = n.Category.Name,
                    CreatedUtc = n.CreatedUtc
                }).ToListAsync();

            return noteQuery;

        }

        //GetById
        public async Task<NoteDetail> GetNoteByIdAsync(int noteId)
        {
            var noteEntity = await _context.Notes
                .Include(nameof(Category))
                .FirstOrDefaultAsync(n => n.Id == noteId && n.OwnerId == _userId);

            if (noteEntity is null)
                return null;

            var detail = new NoteDetail()
            {
                Id = noteEntity.Id,
                Title = noteEntity.Title,
                Content = noteEntity.Content,
                CreatedUtc = noteEntity.CreatedUtc,
                ModifiedUtc = noteEntity.ModifiedUtc,
                CategoryName = noteEntity.Category.Name,
                CatergoryId = noteEntity.Category.Id
            };

            return detail;

        }

        //GetNotesByCategory

        public async Task<IEnumerable<NoteListItem>> GetNotesByCategory(int categoryId)
        {
            var noteQuery = await _context.Notes
                .Include(nameof(Category))
                .Where(n => n.OwnerId == _userId && n.CategoryId == categoryId)
                .Select(n => new NoteListItem()
                {
                    Id = n.Id,
                    Title = n.Title,
                    CategoryName = n.Category.Name,
                    CreatedUtc = n.CreatedUtc
                }).ToListAsync();

            return noteQuery;
        }


        //Create 
        public async Task<bool> CreateNoteAsync(NoteCreate model)
        {
            var noteEntity = new Note()
            {
                Title = model.Title,
                Content = model.Content,
                OwnerId = _userId,
                CreatedUtc = DateTimeOffset.Now,
                CatergoryId = model.CatergoryId

            };

            _context.Notes.Add(noteEntity);

            return await _context.SaveChangesAsync() == 1;
        }

        
        //Update
        public async Task<bool> UpdateNoteAsync(NoteEdit model)
        {
            if (model is null)
                return false;
            var noteEntity = await _context.Notes.FindAsync(model.Id);

            if (noteEntity?.OwnerId != _userId) //if noteEntity is not null, check to see if ownerId == _userId, if either is null/false return false
                return false;

            noteEntity.Title = model.Title;
            noteEntity.Content = model.Content;
            noteEntity.CatergoryId = model.CatergoryId;
            noteEntity.ModifiedUtc = DateTimeOffset.Now;

            return await _context.SaveChangesAsync() == 1;
        }

        //Delete
        public async Task<bool> DeleteNoteAsync(int noteId)
        {
            var noteEntity = await _context.Notes.FindAsync(noteId);

            if (noteEntity?.OwnerId != _userId) //slightly different but for brain's sake, think that if(noteEntity == null || OwnerId !=  _userId) would accomplish the same thing except without hierarchy of checking both...would just take either
                return false;

            _context.Notes.Remove(noteEntity);  

            return await _context.SaveChangesAsync() == 1;
        }

        //Method to set the value of userId
        //if null, no user is logged in

        public void SetUserId(string userId) => _userId = userId;
    }
}
