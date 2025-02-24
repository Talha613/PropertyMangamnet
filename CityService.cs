using PropertyManagement.Core.DTOs;
using PropertyManagement.Core.DTOs.Shared;
using PropertyManagement.Core.DTOs.Sps;
using PropertyManagement.Core.Models;
using PropertyManagement.Data.Repositories;
namespace PropertyManagement.Business.Services;
public interface ICityService
{
    Task<ApiResponse<string>> InsertUpdateCity(InsertUpdateCityRequest request);
    Task<ApiResponse<GetCitiesPaginatedDto>> GetCities(PaginatedRequest request);
}

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;


    public CityService(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    private GetCitiesPaginatedDto GroupAndMapCities(
        List<GetCities> cities,
        int currentPage,
        int pageSize)
    {
        var paginatedData = cities.Select(c => new GetCitiesDto
        {
            CityId = c.CityId,
            CountryId = c.CountryId,
            CityName = c.CityName,
            IsActive = c.IsActive,
            CreateBy = c.CreateBy,
            CreatedAt = c.CreatedAt,
            UpdateBy = c.UpdateBy,
            UpdatedAt = c.UpdatedAt
        }).ToList();

        return new GetCitiesPaginatedDto
        {
            Pagination = new PaginationDto
            {
                TotalPages = cities.FirstOrDefault()?.TotalPages ?? 0,
                TotalRecords = cities.FirstOrDefault()?.RecordsFiltered ?? 0,
                FilteredRecords = cities.FirstOrDefault()?.RecordsFiltered ?? 0,
                CurrentPage = currentPage,
                PageSize = pageSize
            },
            Data = paginatedData
        };
    }
    public async Task<ApiResponse<GetCitiesPaginatedDto>> GetCities(PaginatedRequest request)
    {
        if (request.PageNumber <= 0)
            return new ApiResponse<GetCitiesPaginatedDto>(false, "Page Number is required.", null);
        if (request.PageSize <= 0)
            return new ApiResponse<GetCitiesPaginatedDto>(false, "Page Size is required.", null);

        var result = await _cityRepository.GetCities(request.PageNumber,
            request.PageSize, request.Search, request.OrderColumnIndex, request.OrderDirection);

        return new ApiResponse<GetCitiesPaginatedDto>(true, "Data has been retrieved.", GroupAndMapCities(result, request.PageNumber, request.PageSize));
    }
    public async Task<ApiResponse<string>> InsertUpdateCity(InsertUpdateCityRequest request)
    {
        if (string.IsNullOrEmpty(request.CityName))
            return new ApiResponse<string>(false, "CityName is required.", null);
        if (request.CityId < 0)
            return new ApiResponse<string>(false, "CityId must be a valid positive integer.", null);
        if (request.CountryId <= 0)
            return new ApiResponse<string>(false, "CountryId must be a valid positive integer.", null);


        var result = await _cityRepository.InsertUpdateCity(request.CityId, request.CountryId, request.CityName,
            request.IsActive, request.CreateBy, request.UpdateBy);
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }
}

public class InsertUpdateCityRequest
{
    public int CityId { get; set; }
    public int CountryId { get; set; }
    public string? CityName { get; set; }
    public bool IsActive { get; set; }
    public int CreateBy { get; set; }
    public int UpdateBy { get; set; }


}

