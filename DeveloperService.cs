using PropertyManagement.Core.DTOs;
using PropertyManagement.Core.DTOs.Shared;
using PropertyManagement.Core.DTOs.Sps;
using PropertyManagement.Core.Models;
using PropertyManagement.Data.Repositories;
namespace PropertyManagement.Business.Services;
public interface IDeveloperService
{
    Task<ApiResponse<string>> InsertUpdateDeveloper(InsertUpdateDeveloperRequest request);
    Task<ApiResponse<GetDevelopersPaginatedDto>> GetDevelopers(PaginatedRequest request);
}

public class DeveloperService : IDeveloperService
{
    private readonly IDeveloperRepository _developerRepository;


    public DeveloperService(IDeveloperRepository developerRepository)
    {
        _developerRepository = developerRepository;
    }


    private GetDevelopersPaginatedDto GroupAndMapDevelopers(
        List<GetDevelopers> developers,
        int currentPage,
        int pageSize)
    {
        var paginatedData = developers.Select(d => new GetDevelopersDto
        {
            DeveloperId = d.DeveloperId,    
            DeveloperName = d.DeveloperName,    
            DeveloperLogo = d.DeveloperLogo,            
            IsActive = d.IsActive
        }).ToList();

        return new GetDevelopersPaginatedDto
        {
            Pagination = new PaginationDto
            {
                TotalPages = developers.FirstOrDefault()?.TotalPages ?? 0,            
                TotalRecords = developers.FirstOrDefault()?.RecordsFiltered ?? 0,     
                FilteredRecords = developers.FirstOrDefault()?.RecordsFiltered ?? 0,  
                CurrentPage = currentPage,        
                PageSize = pageSize                
            },
            Data = paginatedData
        };
    }




    public async Task<ApiResponse<string>> InsertUpdateDeveloper(InsertUpdateDeveloperRequest request)
    {

        if (string.IsNullOrEmpty(request.Name))
            return new ApiResponse<string>(false, "Name is required.", null);
        if (string.IsNullOrEmpty(request.Logo))
            return new ApiResponse<string>(false, "Logo is required.", null);

        var result =
            await _developerRepository.InsertUpdateDeveloper(request.Id, request.Name, request.Logo, request.IsActive);

        return new ApiResponse<string>(result.Status, result.Message, result.Data);

    }

    public async Task<ApiResponse<GetDevelopersPaginatedDto>> GetDevelopers(PaginatedRequest request)
    {
        if (request.PageNumber <= 0)
            return new ApiResponse<GetDevelopersPaginatedDto>(false, "Page Number is required.", null);
        if (request.PageSize <= 0)
            return new ApiResponse<GetDevelopersPaginatedDto>(false, "Page Number is required.", null);
        var result = await _developerRepository.GetDevelopers(request.PageNumber,
            request.PageSize, request.Search, request.OrderColumnIndex, request.OrderDirection);
        return new ApiResponse<GetDevelopersPaginatedDto>(true, "Data has been retrieved. ", GroupAndMapDevelopers(result, request.PageNumber, request.PageSize));
    }
}

public class InsertUpdateDeveloperRequest
{
    public int Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? Logo { get; set; } = string.Empty;
    public bool IsActive { get; set; }

}




