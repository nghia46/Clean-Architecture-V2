using AutoMapper;
using CleanIsClean.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CleanIsClean.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController(UserService userService, IMapper mapper) : ControllerBase
{
    private readonly UserService _userService = userService;
    private readonly IMapper _mapper = mapper;

    [HttpGet("GetUserById")]
    public async Task<IActionResult> GetUserById([FromQuery] int id)
    {
        User? user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound($"User with id {id} not found");
        UserView userView = _mapper.Map<UserView>(user);
        return Ok(userView);
    }
    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        IEnumerable<User> users = await _userService.GetAllUsersAsync();
        IEnumerable<UserView> userViews = _mapper.Map<IEnumerable<UserView>>(users);
        return Ok(userViews);
    }
    [HttpPost]
    public async Task<ActionResult> CreateUser(UserView userView)
    {
        User newUser = _mapper.Map<User>(userView);
        await _userService.AddUserAsync(newUser);
        return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, _mapper.Map<UserView>(newUser));
    }
}
