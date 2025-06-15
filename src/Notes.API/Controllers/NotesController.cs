using Microsoft.AspNetCore.Mvc;
using Notes.Application.Interfaces;
using Notes.Application.Models.Requests;

namespace Notes.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var notes = await _noteService.GetAllNotesAsync(cancellationToken);
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
        {
            var note = await _noteService.GetNoteByIdAsync(id, cancellationToken);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NoteRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newNoteId = await _noteService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = newNoteId }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NoteRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedNote = await _noteService.UpdateAsync(id, request, cancellationToken);

            if (updatedNote == null)
            {
                return NotFound();
            }

            return Ok(updatedNote);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] NoteRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedNote = await _noteService.PatchAsync(id, request, cancellationToken);

            if (updatedNote == null)
            {
                return NotFound();
            }

            return Ok(updatedNote);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _noteService.DeleteAsync(id, cancellationToken);    

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
