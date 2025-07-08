using Microsoft.AspNetCore.Mvc;
using Note_oriousWebApp.API.DTOs.Notes;
using Note_oriousWebApp.API.Services;

namespace note_oriouswebapp.api.controllers
{
    // this controller exposes http endpoints for interacting with notes
    [ApiController]
    [Route("api/[controller]")] // endpoint: api/notes
    public class NotesController : ControllerBase
    {
        private readonly NotesService _notesService;

        // constructor injects the notesservice
        public NotesController(NotesService notesService)
        {
            _notesService = notesService ?? throw new ArgumentNullException(nameof(notesService));
        }

        // post /api/notes
        // create a new note
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNoteDTO note)
        {
            try
            {
                // simple validation
                if (string.IsNullOrWhiteSpace(note.Title))
                {
                    return BadRequest("Title is Required!");
                }
                else if (string.IsNullOrWhiteSpace(note.Content))
                {
                    return BadRequest("Content is Required!");
                }

                var create = await _notesService.Create(note.Title, note.Content);

                // return 201 created with location of the new note
                return CreatedAtAction(nameof(GetNoteByID), new { id = create.Id }, create);
            }
            catch (Exception)
            {
                // log the exception (not implemented here)
                return StatusCode(500, "An Error Occured!");
            }

        }

        // get /api/notes
        // return all notes
        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            try
            {
                var notes = await _notesService.GetAllNotes();
                if (notes == null || notes.Count == 0)
                    return NotFound("Notes not Found!");
                return Ok(notes);
            }
            catch (Exception)
            {
                // log the exception (not implemented here)
                return StatusCode(500, "An Error Occured!");
            }
        }

        // get /api/notes/{id}
        // return a specific note by id
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
            catch (Exception)
            {
                // log the exception (not implemented here)
                return StatusCode(500, "An Error Occured!");
            }
        }


    }
}
