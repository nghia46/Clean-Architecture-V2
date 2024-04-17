using AutoMapper;
using CleanIsClean.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CleanIsClean.Application.Tools;
using CleanIsClean.Domain.ViewModels;
using CleanIsClean.Domain.Models;

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
            userView.RoleName = _userService.GetUserRoleNameByUserId(u.Id).Result ?? "";
            return userView;
        });

        return Ok(userViews);
    }
    [HttpGet("GetUserById/{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        User? user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound($"User with id {id} not found");
        UserView userView = _mapper.Map<UserView>(user);
        userView.RoleName = await _userService.GetUserRoleNameByUserId(user.Id) ?? "";
        return Ok(userView);
    }
    [HttpPut("UpdateUser/{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, UserView userView)
    {
        User? user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound($"User with id {id} not found");
        _mapper.Map(userView, user);
        await _userService.UpdateUserAsync(user);
        return Ok();
    }
    [HttpDelete("DeleteUser/{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        User? user = await _userService.GetUserByIdAsync(id);
        if (user == null) return NotFound($"User with id {id} not found");
        await _userService.DeleteUserByIdAsync(id);
        return Ok();
    }
    [HttpGet("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword()
    {
        string? userId = await Authentication.GetUserIdFromHttpContext(HttpContext);
        return Ok(userId);
    }
}
