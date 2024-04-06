using AutoMapper;
using CleanIsClean.Application.ViewModels;
using CleanIsClean.Domain.Interfaces;
using CleanIsClean.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanIsClean.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(IAuthenticationService authenticationService, IUserService userService, IMapper mapper) : ControllerBase
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly IUserService _userService = userService;
    private readonly IMapper _mapper = mapper;

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginView loginView)
    {
        string? token = await _authenticationService.Login(loginView.Email, loginView.Password);
        if (token == null) return NotFound("Login failed");
        return Ok(token);
    }
    [HttpPost("Register")]
    public async Task<ActionResult> Register(UserView userView)
    {
        if (await _userService.GetUserByEmailAsync(userView.Email) != null) return BadRequest("User already exists");
        User? newUser = _mapper.Map<User>(userView);
        newUser.Status = true;
        await _userService.AddUserAsync(newUser);
        string? url = Url.Action("GetUserById", "UserController", new { id = newUser.Id }, Request.Scheme);
        return Created(url, _mapper.Map<UserView>(newUser));
    }
}