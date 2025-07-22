using Microsoft.AspNetCore.Mvc;
using Note_oriousWebApp.API.DTOs.Notes;
using Note_oriousWebApp.API.DTOs.NotesDTOs;
using Note_oriousWebApp.API.DTOs.UsersDTOs;
using Note_oriousWebApp.API.Helpers;
using Note_oriousWebApp.API.Services;

namespace note_oriouswebapp.api.controllers
{
    // Endpoint: api/Notes
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

        // CREATE a Note Method
        // POST /api/Notes
        [HttpPost("{id}")]
        public async Task<IActionResult> Create(int id, [FromBody] CreateNoteDTO createNoteDTO)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(createNoteDTO.Title))
                {
                    return BadRequest("Title is Required!");
                }
                else if (string.IsNullOrWhiteSpace(createNoteDTO.Content))
                {
                    return BadRequest("Content is Required!");
                }

                var create = await _notesService.Create(id, createNoteDTO);

                return CreatedAtAction(nameof(GetNoteByID), new { id = create.Id }, create);
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        // GET Notes Method
        // GET /api/Notes
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

        // GET a Note Method
        // GET /api/Notes/{id}
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

        // UPDATE a Note Method
        // PUT /api/Notes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateNoteDTO updateNoteDTO)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(updateNoteDTO.Title))
                {
                    return BadRequest("Title is Required!");
                }
                else if (string.IsNullOrWhiteSpace(updateNoteDTO.Content))
                {
                    return BadRequest("Content is Required");
                }

                var updatedNote = await _notesService.Update(id, updateNoteDTO);
                return Ok(updateNoteDTO);

            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        // SOFT-DELETE a Note Method
        // DELETE /api/Notes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id, [FromBody] SoftDeleteNoteDTO softDeleteNoteDTO)
        {
            try
            {
                var softDeletedUser = await _notesService.SoftDelete(id, softDeleteNoteDTO);
                return Ok($"Note {id} is now Deleted at {softDeletedUser.DeletedAt}.");
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

    }
}
