using Application.Users.Queries.GetAllUsers;
using AtcMediator;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IAtcMediator _mediator;

    public UsersController(IAtcMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene la lista completa de usuarios
    /// </summary>
    /// <returns>Lista de usuarios</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _mediator.ExecuteAsync(new GetAllUsersQuery());
        return Ok(users);
    }
}
