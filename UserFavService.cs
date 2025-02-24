using PropertyManagement.Core.DTOs;
using PropertyManagement.Data.Repositories;
namespace PropertyManagement.Business.Services;
public interface IUserFavService
{
    Task<ApiResponse<string>> InsertBuyPropertyUserFav(int buyPropertyId,int userId);
    Task<ApiResponse<string>> RemoveBuyPropertyUserFav(int id);
    Task<ApiResponse<string>> InsertRentPropertyUserFav(int rentPropertyId, int userId);
    Task<ApiResponse<string>> RemoveRentPropertyUserFav(int id);

    Task<ApiResponse<string>> InsertNewProjectUserFav(int projectId, int userId);
    Task<ApiResponse<string>> RemoveNewProjectUserFav(int id);
}

public class UserFavService : IUserFavService
{
    private readonly IUserFavRepository _userFavRepository;


    public UserFavService(IUserFavRepository userFavRepository)
    {
        _userFavRepository = userFavRepository;
    }


    public async Task<ApiResponse<string>> InsertBuyPropertyUserFav(int buyPropertyId, int userId)
    {
        var result = await _userFavRepository.InsertBuyPropertyUserFav(buyPropertyId, userId);
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }

    public async Task<ApiResponse<string>> RemoveBuyPropertyUserFav(int id)
    {
        var result = await _userFavRepository.RemoveBuyPropertyUserFav(id);
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }

    public async Task<ApiResponse<string>> InsertRentPropertyUserFav(int rentPropertyId, int userId)
    {
        var result = await _userFavRepository.InsertRentPropertyUserFav(rentPropertyId, userId);
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }

    public async Task<ApiResponse<string>> RemoveRentPropertyUserFav(int id)
    {
        var result = await _userFavRepository.RemoveRentPropertyUserFav(id);
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }

    public async Task<ApiResponse<string>> InsertNewProjectUserFav(int projectId, int userId)
    {
        var result = await _userFavRepository.InsertNewProjectUserFav(projectId, userId);
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }

    public async Task<ApiResponse<string>> RemoveNewProjectUserFav(int id)
    {
        var result = await _userFavRepository.RemoveNewProjectUserFav(id);
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }
}



