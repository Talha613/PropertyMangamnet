using PropertyManagement.Business.Services.External;
using PropertyManagement.Core.DTOs;
using PropertyManagement.Data.Repositories;
namespace PropertyManagement.Business.Services;
public interface IBackOfficeUserAccountService
{
    Task<ApiResponse<DatabaseResponse>> UpdateBackOfficeUserUpdatePassword(UpdateBackOfficeUserUpdatePasswordRequest request);
    Task<ApiResponse<DatabaseResponse>> BackOfficeUserLogin(BackOfficeUserLoginRequest request);
}

public class BackOfficeUserAccountService : IBackOfficeUserAccountService
{
    private readonly IBackOfficeUserAccountRepository _backOfficeUserAccountRepository;
    private readonly IBackOfficeTokenService _backOfficeTokenService;

    public BackOfficeUserAccountService(IBackOfficeUserAccountRepository backOfficeUserAccountRepository, IBackOfficeTokenService backOfficeTokenService)
    {
        _backOfficeUserAccountRepository = backOfficeUserAccountRepository;
        _backOfficeTokenService = backOfficeTokenService;
    }


    public async Task<ApiResponse<DatabaseResponse>> UpdateBackOfficeUserUpdatePassword(UpdateBackOfficeUserUpdatePasswordRequest request)
    {

        if (string.IsNullOrEmpty(request.Email))
            return new ApiResponse<DatabaseResponse>(false, "Email is required.", null);
        if (string.IsNullOrEmpty(request.Password))
            return new ApiResponse<DatabaseResponse>(false, "Password is required.", null);

        var result = await _backOfficeUserAccountRepository.UpdateBackOfficeUserPassword(request.Email, request.Password);
        return new ApiResponse<DatabaseResponse>(result.Status, result.Message, null);
    }

    public async Task<ApiResponse<DatabaseResponse>> BackOfficeUserLogin(BackOfficeUserLoginRequest request)
    {
         string email = request.Email;

        if (string.IsNullOrEmpty(request.Email))
            return new ApiResponse<DatabaseResponse>(false, "Email is required.", null);

        if (string.IsNullOrEmpty(request.Password))
            return new ApiResponse<DatabaseResponse>(false, "Password is required when AccessToken is not provided.", null);
        

        var result = await _backOfficeUserAccountRepository.BackOfficeUserLogin(email, request.Password); 
        return result.Status ? new ApiResponse<DatabaseResponse>(result.Status, result.Message, null, _backOfficeTokenService.GenerateToken(email, result.Data)) 
            : new ApiResponse<DatabaseResponse>(result.Status, result.Message, null);
    }
}


public record UpdateBackOfficeUserUpdatePasswordRequest(string Email = "", string Password = "");
public record BackOfficeUserLoginRequest(string Email = "", string Password = "");



