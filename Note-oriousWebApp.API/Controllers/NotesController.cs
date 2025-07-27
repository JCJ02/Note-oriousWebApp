using Microsoft.AspNetCore.Mvc;
using Note_oriousWebApp.API.DTOs.Notes;
using Note_oriousWebApp.API.DTOs.NotesDTOs;
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
        [HttpPost("{userId}")]
        public async Task<ActionResult> Create(int userId, [FromBody] CreateNoteDTO createNoteDTO)
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

                var create = await _notesService.Create(userId, createNoteDTO);

                return Ok(create);
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        // GET Notes Method
        // GET /api/Notes
        [HttpGet("list/{userId}")]
        public async Task<IActionResult> GetNotes(int userId)
        {
            try
            {
                var notes = await _notesService.GetNotes(userId);
                return Ok(notes);
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        // GET Archive Notes Method
        // GET /api/Notes
        [HttpGet("archive-list/{userId}")]
        public async Task<IActionResult> GetArchiveNotes(int userId)
        {
            try
            {
                var notes = await _notesService.GetArchiveNotes(userId);
                if (notes == null)
                    return Ok(new { message = "No Archive Notes.", data = new List<object>() });
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
        // DELETE /api/Notes/soft-delete/{id}
        [HttpDelete("soft-delete/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            try
            {
                var softDeleteNote = await _notesService.SoftDelete(id);
                return Ok($"Note {id} is now Deleted at {softDeleteNote.DeletedAt}.");
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        // DELETE a Note Method
        // DELETE /api/Notes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var noteDeleted = await _notesService.Delete(id);
                if (!noteDeleted)
                    return NotFound("Note not Found!");

                return Ok(new { message = "Note Deleted Permanently!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET Soft-Deleted Notes Method
        // GET /api/Notes/soft-deleted-list/{userId}
        [HttpGet("soft-deleted-list/{userId}")]
        public async Task<IActionResult> GetSoftDeletedNotes(int userId)
        {
            try
            {
                var softDeletedNotes = await _notesService.GetSoftDeletedNotes(userId);
                return Ok(softDeletedNotes);
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

        // ARCHIVE a Note Method
        // PUT /api/Notes/{id}
        [HttpPut("archive/{id}")]
        public async Task<IActionResult> ArchiveNote(int id, [FromBody] ArchiveNoteDTO archiveNoteDTO)
        {
            try
            {
                var archiveNote = await _notesService.ArchiveNote(id, archiveNoteDTO);
                return Ok($"Note with {id} has Archive Status: {archiveNote.IsArchive}.");
            }
            catch (Exception error)
            {
                return StatusCode(500, error.Message);
            }
        }

    }
}
