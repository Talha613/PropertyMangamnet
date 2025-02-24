using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyManagement.Business.Services;
using System.Security.Claims;

namespace PropertyManagement.API.Controllers;
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeUserAccount : ControllerBase
    {
        private readonly IBackOfficeUserAccountService _userAccountService;
        public BackOfficeUserAccount(IBackOfficeUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
    }

    [HttpPost("login_back_office_user")]
    public async Task<IActionResult> Login(BackOfficeUserLoginRequest request)
    {
        var data = await _userAccountService.BackOfficeUserLogin(request);
        return Ok(data);
    }
    [HttpPost("change_back_office_user_password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(string password)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        UpdateBackOfficeUserUpdatePasswordRequest request = new UpdateBackOfficeUserUpdatePasswordRequest(email!,password);
        var data = await _userAccountService.UpdateBackOfficeUserUpdatePassword(request);
        return Ok(data);
    }
}

