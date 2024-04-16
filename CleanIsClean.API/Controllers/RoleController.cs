using AutoMapper;
using CleanIsClean.Application.Services;
using CleanIsClean.Domain.Interfaces;
using CleanIsClean.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CleanIsClean.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController(IRoleService roleService,IMapper mapper) : ControllerBase
{
    private readonly IMapper _mapper = mapper;
    private readonly IRoleService _roleService = roleService;

    [HttpGet("GetAllRoles")]
    public async Task<IActionResult> GetAllRoles()
    {
        IEnumerable<Role> roles = await _roleService.GetAllRoles();
        IEnumerable<RoleView> roleViews = _mapper.Map<IEnumerable<RoleView>>(roles);
        return Ok(roleViews);
    }
    [HttpGet("GetRoleById/{id:guid}")]
    public async Task<IActionResult> GetRoleById(Guid id)
    {
        var role = await _roleService.GetRoleById(id);
        if (role == null)
        {
            return NotFound();
        }
        RoleView roleView = _mapper.Map<RoleView>(role);
        return Ok(roleView);
    }

    [HttpPost("AddRole")]
    public async Task<IActionResult> CreateRole(RoleView role)
    {
        await _roleService.CreateRole(role);
        return Ok($"Role {role.RoleName} successfully");
    }
}