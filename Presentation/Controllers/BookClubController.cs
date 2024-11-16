using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers;

/// <summary>
/// The controller for handling all endpoints related to book clubs
/// </summary>
[ApiController]
[Route("[controller]")]
public class BookClubController(IServiceManager serviceManager) : ControllerBase
{
    /// <summary>
    /// Get all book clubs
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookClubDto>>> GetBookClubs(
        CancellationToken cancellationToken
    )
    {
        var bookClubs = await serviceManager.BookClubService.GetAllAsync(cancellationToken);
        return Ok(bookClubs);
    }

    /// <summary>
    /// Get a specific book club based on Id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<BookClubDto>> GetBookClubById(
        int id,
        CancellationToken cancellationToken
    )
    {
        var bookClubDto = await serviceManager.BookClubService.GetByIdAsync(id, cancellationToken);
        return Ok(bookClubDto);
    }

    /// <summary>
    /// Creates a new book club
    /// </summary>
    /// <response code="201">Book Club created</response>
    /// <response code="400">Book Club has missing/invalid values</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookClubDto>> CreateBookClub(
        [FromBody] BookClubForCreationDto bookClubForCreationDto
    )
    {
        var bookClubDto = await serviceManager.BookClubService.CreateAsync(bookClubForCreationDto);
        return CreatedAtAction(nameof(GetBookClubById), new { id = bookClubDto.Id }, bookClubDto);
    }

    /// <summary>
    /// Updates a Book Club
    /// </summary>
    /// <response code="200">Book Club update</response>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBookClub(
        int id,
        [FromBody] BookClubForUpdateDto bookClubForUpdateDto,
        CancellationToken cancellationToken
    )
    {
        var bookClub = await serviceManager.BookClubService.UpdateAsync(
            id,
            bookClubForUpdateDto,
            cancellationToken
        );
        return Ok(bookClub);
    }

    /// <summary>
    /// Deletes a Book Club
    /// </summary>
    /// <response code="204">Book Club deleted</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteBookClub(int id, CancellationToken cancellationToken)
    {
        await serviceManager.BookClubService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
