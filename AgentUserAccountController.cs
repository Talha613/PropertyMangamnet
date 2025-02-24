using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.Business.Services;
using System.Security.Claims;

namespace PropertyManagement.API.Controllers;
    [Route("api/[controller]")]
    [ApiController]
    public class AgentUserAccount : ControllerBase
    {
        private readonly IAgentUserAccountService _userAccountService;
        public AgentUserAccount(IAgentUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
    }

    [HttpPost("login_agent_user")]
    public async Task<IActionResult> Login(AgentUserLoginRequest request)
    {
        var data = await _userAccountService.AgentUserLogin(request);
        return Ok(data);
    }
    [HttpPost("change_agent_user_password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(string password)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        UpdateAgentUserPasswordRequest request = new UpdateAgentUserPasswordRequest(email!,password);
        var data = await _userAccountService.AgentUserUpdatePassword(request);
        return Ok(data);
    }
}

