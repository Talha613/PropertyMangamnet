using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.Business.Services;
using PropertyManagement.Core.DTOs.Shared;

namespace PropertyManagement.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AreaController : ControllerBase
{
    private readonly IAreaService _areaService;
    public AreaController(IAreaService areaService)
    {
        _areaService = areaService;
    }



    [HttpPost("create_update_area")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUpdateArea(InsertUpdateAreaRequest request)
    {
        var id = User.FindFirst(ClaimTypeDto.Id)?.Value;
        request.CreateBy = Convert.ToInt32(id);
        request.UpdateBy = Convert.ToInt32(id);
        var data = await _areaService.InsertUpdateArea(request);
        return Ok(data);
    }

    [HttpPost("get_areas")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAreas(PaginatedRequest request)
    {
        var data = await _areaService.GetAreas(request);
        return Ok(data);
    }


}

