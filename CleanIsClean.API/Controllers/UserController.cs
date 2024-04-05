using AutoMapper;
using CleanIsClean.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanIsClean.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController(UserService userService, IMapper mapper) : ControllerBase
{
    private readonly UserService _userService = userService;
    private readonly IMapper _mapper = mapper;
    [HttpGet("GetAllUsers")]
    [Authorize]
    public async Task<IActionResult> GetAllUsers()
    {
        IEnumerable<User> users = await _userService.GetAllUsersAsync();
        IEnumerable<UserView> userViews = _mapper.Map<IEnumerable<UserView>>(users);
        return Ok(userViews);
    }
    [HttpGet("GetUserById {id:int}")]
    [Authorize]
    public async Task<IActionResult> GetUserById(int id)
    {
        User? user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound($"User with id {id} not found");
        UserView userView = _mapper.Map<UserView>(user);
        return Ok(userView);
    }

}
