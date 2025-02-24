using PropertyManagement.Business.Services.External;
using PropertyManagement.Core.DTOs;
using PropertyManagement.Data.Repositories;
namespace PropertyManagement.Business.Services;
public interface IUserAccountService
{
    Task<ApiResponse<DatabaseResponse>> RegisterUser(RegisterUserRequest request);
    Task<ApiResponse<DatabaseResponse>> UpdateUserPassword(UpdateUserPasswordRequest request);
    Task<ApiResponse<DatabaseResponse>> UserLogin(UserLoginRequest request);
}

public class UserAccountService : IUserAccountService
{
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IVerifyGoogleAccessTokenService _verifyGoogleAccessTokenService;
    private readonly IUserTokenService _userTokenService;

    public UserAccountService(IUserAccountRepository userAccountRepository, IVerifyGoogleAccessTokenService verifyGoogleAccessTokenService,IUserTokenService userTokenService)
    {
        _userAccountRepository = userAccountRepository;
        _verifyGoogleAccessTokenService = verifyGoogleAccessTokenService;
        _userTokenService = userTokenService;
    }

    public async Task<ApiResponse<DatabaseResponse>> RegisterUser(RegisterUserRequest request)
    {
        if (string.IsNullOrEmpty(request.FirstName))
            return new ApiResponse<DatabaseResponse>(false, "First name is required.", null);
        if (string.IsNullOrEmpty(request.LastName))
            return new ApiResponse<DatabaseResponse>(false, "Last name is required.", null);
        if (string.IsNullOrEmpty(request.Email))
            return new ApiResponse<DatabaseResponse>(false, "Email is required.", null);
        if (string.IsNullOrEmpty(request.Password))
            return new ApiResponse<DatabaseResponse>(false, "Password is required.", null);

        var result = await _userAccountRepository.RegisterUser(request.FirstName, request.LastName, request.Email, request.Password);
        return new ApiResponse<DatabaseResponse>(result.Status, result.Message, null);
    }

    public async Task<ApiResponse<DatabaseResponse>> UpdateUserPassword(UpdateUserPasswordRequest request)
    {

        if (string.IsNullOrEmpty(request.Email))
            return new ApiResponse<DatabaseResponse>(false, "Email is required.", null);
        if (string.IsNullOrEmpty(request.Password))
            return new ApiResponse<DatabaseResponse>(false, "Password is required.", null);

        var result = await _userAccountRepository.UpdateUserPassword(request.Email, request.Password);
        return new ApiResponse<DatabaseResponse>(result.Status, result.Message, null);
    }

    public async Task<ApiResponse<DatabaseResponse>> UserLogin(UserLoginRequest request)
    {
         string email = request.Email;

        if (string.IsNullOrEmpty(request.Email))
            return new ApiResponse<DatabaseResponse>(false, "Email is required.", null);

        if (string.IsNullOrEmpty(request.AccessToken))
        {
            if (string.IsNullOrEmpty(request.Password))
                return new ApiResponse<DatabaseResponse>(false, "Password is required when AccessToken is not provided.", null);
        }
        else
        {
           var data =  await _verifyGoogleAccessTokenService.GetUserInfoGoogleAsync(request.AccessToken);
           email = data.Email;
        }

        var result = await _userAccountRepository.UserLogin(email, request.Password,request.AccessToken); 
        return result.Status ? new ApiResponse<DatabaseResponse>(result.Status, result.Message, null, _userTokenService.GenerateToken(email, email, email,result.Data)) 
            : new ApiResponse<DatabaseResponse>(result.Status, result.Message, null);
    }
}


public record RegisterUserRequest(string FirstName = "", string LastName = "" , string Email = "", string Password = "");
public record UpdateUserPasswordRequest(string Email = "", string Password = "");
public record UserLoginRequest(string Email = "", string Password = "",string AccessToken = "");
