using ElevenNoteWebApp.Shared.Models.Note;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElevenNoteWebApp.Server.Services.NoteServices
{
    public interface INoteService
    {
        Task<IEnumerable<NoteListItem>> GeAllNotesAsync();
        Task<NoteDetail> GetNoteByIdAsync(int noteId);

        Task<IEnumerable<NoteListItem>> GetNotesByCategory(int categoryId);

        Task<bool> CreateNoteAsync(NoteCreate model);

        Task<bool> UpdateNoteAsync(NoteEdit model);

        Task<bool> DeleteNoteAsync(int noteId);

        void SetUserId(string userId);
    }
}
