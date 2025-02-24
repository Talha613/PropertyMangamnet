using PropertyManagement.Business.Services.External;
using PropertyManagement.Core.DTOs;
using PropertyManagement.Data.Repositories;
namespace PropertyManagement.Business.Services;
public interface IAgentUserAccountService
{
    Task<ApiResponse<DatabaseResponse>> AgentUserUpdatePassword(UpdateAgentUserPasswordRequest request);
    Task<ApiResponse<DatabaseResponse>> AgentUserLogin(AgentUserLoginRequest request);
}

public class AgentUserAccountService : IAgentUserAccountService
{
    private readonly IAgentUserAccountRepository _agentUserAccountRepository;
    private readonly IAgentUserTokenService _agentUserTokenService;

    public AgentUserAccountService(IAgentUserAccountRepository agentUserAccountRepository, IAgentUserTokenService agentUserTokenService)
    {
        _agentUserAccountRepository = agentUserAccountRepository;
        _agentUserTokenService = agentUserTokenService;
    }


    public async Task<ApiResponse<DatabaseResponse>> AgentUserUpdatePassword(UpdateAgentUserPasswordRequest request)
    {

        if (string.IsNullOrEmpty(request.Email))
            return new ApiResponse<DatabaseResponse>(false, "Email is required.", null);
        if (string.IsNullOrEmpty(request.Password))
            return new ApiResponse<DatabaseResponse>(false, "Password is required.", null);

        var result = await _agentUserAccountRepository.UpdateAgentUserPassword(request.Email, request.Password);
        return new ApiResponse<DatabaseResponse>(result.Status, result.Message, null);
    }

    public async Task<ApiResponse<DatabaseResponse>> AgentUserLogin(AgentUserLoginRequest request)
    {
         string email = request.Email;

        if (string.IsNullOrEmpty(request.Email))
            return new ApiResponse<DatabaseResponse>(false, "Email is required.", null);

        if (string.IsNullOrEmpty(request.Password))
            return new ApiResponse<DatabaseResponse>(false, "Password is required when AccessToken is not provided.", null);
        

        var result = await _agentUserAccountRepository.AgentUserLogin(email, request.Password); 
        return result.Status ? new ApiResponse<DatabaseResponse>(result.Status, result.Message, null, _agentUserTokenService.GenerateToken(email,result.Data)) 
            : new ApiResponse<DatabaseResponse>(result.Status, result.Message, null);
    }
}


public record UpdateAgentUserPasswordRequest(string Email = "", string Password = "");
public record AgentUserLoginRequest(string Email = "", string Password = "");



