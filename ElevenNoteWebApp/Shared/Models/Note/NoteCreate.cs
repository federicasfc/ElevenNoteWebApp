using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNoteWebApp.Shared.Models.Note
{
    public class NoteCreate //why is the default for this internal and not public? intenral means accessible within the assembly/solution and public means accessible from anywhere, but where else is there if not the assembly?
    {
        [Required] 
        public string Title { get; set; }


        [Required]

        public string Content { get; set; }

        
        public int CatergoryId { get; set; }
    }
}
