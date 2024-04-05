using AutoMapper;
using CleanIsClean.Application.Services;
using CleanIsClean.Application.ViewModels;
using CleanIsClean.Infrastructure.Models;
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
    [HttpPost]
    public async Task<ActionResult> CreateUser(UserView userView)
    {
        User newUser = _mapper.Map<User>(userView);
        await _userService.AddUserAsync(newUser);
        return Ok("User created successfully");
    }
}
