using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers;

/// <summary>
/// The controller for handling all endpoints related to users
/// </summary>
[ApiController]
[Route("[controller]")]
public class UserController(IServiceManager serviceManager) : ControllerBase
{
    /// <summary>
    /// Get a specific user
    /// </summary>
    [HttpGet("{userId}")]
    public async Task<ActionResult<UserDto>> GetUserById(
        int userId,
        CancellationToken cancellationToken
    )
    {
        var userDto = await serviceManager.UserService.GetByIdAsync(userId, cancellationToken);

        return Ok(userDto);
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// /// <response code="201">Book Club created</response>
    /// <response code="400">Book Club has missing/invalid values</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> CreateUser(
        int ownerId,
        [FromBody] UserForCreationDto userForCreationDto,
        CancellationToken cancellationToken
    )
    {
        var response = await serviceManager.UserService.CreateAsync(
            ownerId,
            userForCreationDto,
            cancellationToken
        );

        return CreatedAtAction(nameof(GetUserById), new { userId = response.Id }, response);
    }

    /// <summary>
    /// Deletes a user
    /// </summary>
    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteUser(int userId, CancellationToken cancellationToken)
    {
        await serviceManager.UserService.DeleteAsync(userId, cancellationToken);

        return NoContent();
    }
}
