using AutoMapper;
using CleanIsClean.Application.ViewModels;
using CleanIsClean.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CleanIsClean.Application.Tools;

namespace CleanIsClean.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, IMapper mapper) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly IMapper _mapper = mapper;
    [HttpGet("GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        IEnumerable<User> users = await _userService.GetAllUsersAsync();
        IEnumerable<UserView> userViews = users.Select(u =>
        {
            UserView userView = _mapper.Map<UserView>(u);
            userView.RoleName = _userService.GetUserRoleNameById(u.Id).Result ?? "";
            return userView;
        });

        return Ok(userViews);
    }
    [HttpGet("GetUserById/{id:int}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        User? user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound($"User with id {id} not found");
        UserView userView = _mapper.Map<UserView>(user);
        userView.RoleName = await _userService.GetUserRoleNameById(user.Id) ?? "";
        return Ok(userView);
    }
    [HttpPut("UpdateUser/{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, UserView userView)
    {
        User? user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound($"User with id {id} not found");
        _mapper.Map(userView, user);
        await _userService.UpdateUserAsync(user);
        return Ok();
    }
    [HttpDelete("DeleteUser/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        User? user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound($"User with id {id} not found");
        await _userService.DeleteUserAsync(id);
        return Ok();
    }
    [HttpGet("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword()
    {
        string? userId = await Authentication.GetUserIdFromHttpContext(HttpContext);
        return Ok(userId);
    }
}
