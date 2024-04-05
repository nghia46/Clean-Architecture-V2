using AutoMapper;
using CleanIsClean.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CleanIsClean.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IMapper _mapper;

    public UserController(UserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<IActionResult> GetUserById(int id)
    {
        User user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound("User not found");
        UserView userView = _mapper.Map<UserView>(user);
        return Ok(userView);
    }
    [HttpPost]
    public async Task<ActionResult> CreateUser(UserView userView)
    {
        User newUser = _mapper.Map<User>(userView);
        await _userService.AddUserAsync(newUser);
        return Ok("User created successfully");
    }

}
