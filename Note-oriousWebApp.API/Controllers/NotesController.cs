using Microsoft.AspNetCore.Mvc;
using Note_oriousWebApp.API.DTOs.Notes;
using Note_oriousWebApp.API.Services;

namespace note_oriouswebapp.api.controllers
{
    // Endpoint: api/notes
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        // Call NotesService Class
        private readonly NotesService _notesService;

        // Constructor
        public NotesController(NotesService notesService)
        {
            _notesService = notesService ?? throw new ArgumentNullException(nameof(notesService));
        }

        // POST /api/notes
        [HttpPost("{id}")]
        public async Task<IActionResult> Create(int id, [FromBody] CreateNoteDTO note)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(note.Title))
                {
                    return BadRequest("Title is Required!");
                }
                else if (string.IsNullOrWhiteSpace(note.Content))
                {
                    return BadRequest("Content is Required!");
                }

                var create = await _notesService.Create(id, note);

                return CreatedAtAction(nameof(GetNoteByID), new { id = create.Id }, create);
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        // GET /api/notes
        [HttpGet]
        public async Task<IActionResult> GetNotes()
        {
            try
            {
                var notes = await _notesService.GetNotes();
                if (notes == null || notes.Count == 0)
                    return NotFound("Notes not Found!");
                return Ok(notes);
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        // GET /api/notes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoteByID(int id)
        {
            try
            {
                var note = await _notesService.GetNoteByID(id);
                if (note == null)
                    return BadRequest("Note not Found!");

                return Ok(note);
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }


    }
}
