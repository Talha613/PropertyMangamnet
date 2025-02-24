using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.Business.Services;
using PropertyManagement.Core.DTOs.Shared;

namespace PropertyManagement.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BrokerController : ControllerBase
{
    private readonly IBrokerService _brokerService;
    public BrokerController(IBrokerService brokerService)
    {
        _brokerService = brokerService;
    }


    [HttpPost("get_broker_list")]
    public async Task<IActionResult> GetAgentList(GetEstateBrokersWithAgentCountRequest request)
    {
        var data = await _brokerService.GetEstateBrokersWithAgentCounts(request);
        return Ok(data);
    }

    [HttpPost("create_update_developer")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUpdateDeveloper(InsertUpdateRealEstateBrokerRequest request)
    {
        var id = User.FindFirst(ClaimTypeDto.Id)?.Value;
        request.CreateBy = id!;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        request.UpdateBy = email;
        var data = await _brokerService.InsertUpdateRealEstateBroker(request);
        return Ok(data);
    }


    [HttpPost("get_real_estate_brokers")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetRealEstateBrokers(PaginatedRequest request)
    {
        var data = await _brokerService.GetRealEstateBrokers(request);
        return Ok(data);
    }

}

