using AutoMapper;
using CleanIsClean.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CleanIsClean.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(AuthenticationService authenticationService, UserService userService, IMapper mapper) : ControllerBase
{
    private readonly AuthenticationService _authenticationService = authenticationService;
    private readonly UserService _userService = userService;
    private readonly IMapper _mapper = mapper;

    [HttpPost("Login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        string? token = await _authenticationService.Login(email, password);
        if (token == null) return NotFound("Login failed");
        return Ok(token);
    }
    [HttpPost("Register")]
    public async Task<ActionResult> Register(UserView userView)
    {
        if (await _userService.GetUserByEmailAsync(userView.Email) != null) return BadRequest("User already exists");
        User? newUser = _mapper.Map<User>(userView);
        await _userService.AddUserAsync(newUser);
        string? url = Url.Action("GetUserById", "UserController", new { id = newUser.Id }, Request.Scheme);
        return Created(url, _mapper.Map<UserView>(newUser));
    }
}