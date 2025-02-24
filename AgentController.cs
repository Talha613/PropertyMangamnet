using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.Business.Services;
using PropertyManagement.Core.DTOs.Shared;
using System.Security.Claims;

namespace PropertyManagement.API.Controllers;
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _agentService;
        public AgentController(IAgentService agentService)
        {
            _agentService = agentService;
    }


    [HttpPost("get_agents_list")]
    public async Task<IActionResult> GetAgentList(GetAgentsListRequest request)
    {
        var data = await _agentService.GetAgentsList(request);
        return Ok(data);
    }

    [HttpPost("create_update_agent")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUpdateAgent(InsertUpdateAgentUserRequest request)
    {
        var id = User.FindFirst(ClaimTypeDto.Id)?.Value;
        request.CreateBy = id!;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        request.UpdateBy = email;
        var data = await _agentService.InsertUpdateAgentUserRequest(request);
        return Ok(data);
    }

    [HttpPost("get_agent_users")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAgentUsers(PaginatedRequest request)
    {
        var data = await _agentService.GetAgentUsers(request);
        return Ok(data);
    }
}

