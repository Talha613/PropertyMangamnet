using PropertyManagement.Core.DTOs;
using PropertyManagement.Core.DTOs.Shared;
using PropertyManagement.Core.DTOs.Sps;
using PropertyManagement.Core.Models;
using PropertyManagement.Data.Repositories;
namespace PropertyManagement.Business.Services;
public interface ICountryService
{
    Task<ApiResponse<string>> InsertUpdateCountry(InsertUpdateCountryRequest request);
    Task<ApiResponse<GetCountriesPaginatedDto>> GetCountries(PaginatedRequest request);
}

public class CountryService : ICountryService
{
    private readonly ICountryRepository _countryRepository;


    public CountryService(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    private GetCountriesPaginatedDto GroupAndMapCountries(
        List<GetCountries> countries,
        int currentPage,
        int pageSize)
    {
        var paginatedData = countries.Select(c => new GetCountriesDto
        {
            CountryId = c.CountryId,
            CountryName = c.CountryName,
            CountryCode = c.CountryCode,
            IsActive = c.IsActive,
            CreateBy = c.CreateBy,
            CreatedAt = c.CreatedAt,
            UpdateBy = c.UpdateBy,
            UpdatedAt = c.UpdatedAt
        }).ToList();

        return new GetCountriesPaginatedDto
        {
            Pagination = new PaginationDto
            {
                TotalPages = countries.FirstOrDefault()?.TotalPages ?? 0,
                TotalRecords = countries.FirstOrDefault()?.RecordsFiltered ?? 0,
                FilteredRecords = countries.FirstOrDefault()?.RecordsFiltered ?? 0,
                CurrentPage = currentPage,
                PageSize = pageSize
            },
            Data = paginatedData
        };
    }
    public async Task<ApiResponse<string>> InsertUpdateCountry(InsertUpdateCountryRequest request)
    {
        if (string.IsNullOrEmpty(request.CountryName))
            return new ApiResponse<string>(false, "CountryName is required.", null);
        if (string.IsNullOrEmpty(request.CountryCode))
            return new ApiResponse<string>(false, "CountryCode is required.", null);
        if (request.CountryId < 0)
            return new ApiResponse<string>(false, "CountryId must be a valid positive integer.", null);

        var result = await _countryRepository.InsertUpdateCountry(request.CountryId, request.CountryName,
            request.CountryCode, request.IsActive, request.CreateBy, request.UpdateBy);
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }

    public async Task<ApiResponse<GetCountriesPaginatedDto>> GetCountries(PaginatedRequest request)
    {
        if (request.PageNumber <= 0)
            return new ApiResponse<GetCountriesPaginatedDto>(false, "Page Number is required.", null);
        if (request.PageSize <= 0)
            return new ApiResponse<GetCountriesPaginatedDto>(false, "Page Size is required.", null);

        var result = await _countryRepository.GetCountries(request.PageNumber,
            request.PageSize, request.Search, request.OrderColumnIndex, request.OrderDirection);

        return new ApiResponse<GetCountriesPaginatedDto>(true, "Data has been retrieved.", GroupAndMapCountries(result, request.PageNumber, request.PageSize));
    }


}

public class InsertUpdateCountryRequest
{
    public int CountryId { get; set; }
    public string? CountryName { get; set; }
    public string? CountryCode { get; set; }

    public bool IsActive { get; set; }
    public int CreateBy { get; set; }
    public int UpdateBy { get; set; }


}

