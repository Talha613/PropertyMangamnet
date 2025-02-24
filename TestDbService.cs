
using PropertyManagement.Core.DTOs;
using PropertyManagement.Data.Repositories;

namespace PropertyManagement.Business.Services;
public interface ITestDbService
{
    Task<ApiResponse<object>> GetAllAsync();
}


public class TestDbService : ITestDbService
{

    private readonly ITestDbRepo _proRepository;

    public TestDbService(ITestDbRepo proRepository)
    {
        _proRepository = proRepository;
    }

    public async Task<ApiResponse<object>> GetAllAsync()
    {
        var properties = await _proRepository.GetPropertiesAsync();
        return new ApiResponse<object>(false, "hi", properties);
    }

}

