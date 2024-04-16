using AutoMapper;
using CleanIsClean.Domain.Interfaces;
using CleanIsClean.Domain.ViewModels;
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
        string? token = await _authenticationService.Login(loginView);
        if (token == null) return NotFound("Login failed");
        return Ok(token);
    }
    [HttpPost("Register")]
    public async Task<ActionResult> Register(RegisterView registerView)
    {
        if (await _userService.GetUserByEmailAsync(registerView.Email) != null) return BadRequest("User already exists");
        User? newUser = _mapper.Map<User>(registerView);
        await _userService.AddUserAsync(newUser);
        // get user information after adding to response
        User? users = await _userService.GetUserByIdAsync(newUser.Id);
        UserView userView = _mapper.Map<UserView>(users);
        userView.RoleName = await _userService.GetUserRoleNameByUserId(newUser.Id) ?? "";
        return Ok(userView);
        // string? url = Url.Action("GetUserById", "UserController", new { id = newUser.Id }, Request.Scheme);
        // return Created(url, _mapper.Map<UserView>(newUser));
    }
}