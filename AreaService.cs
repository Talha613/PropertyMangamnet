using PropertyManagement.Core.DTOs;
using PropertyManagement.Core.DTOs.Shared;
using PropertyManagement.Core.DTOs.Sps;
using PropertyManagement.Core.Models;
using PropertyManagement.Data.Repositories;

namespace PropertyManagement.Business.Services;

public interface IAreaService
{
    Task<ApiResponse<string>> InsertUpdateArea(InsertUpdateAreaRequest request);
    Task<ApiResponse<GetAreasPaginatedDto>> GetAreas(PaginatedRequest request);
}

public class AreaService : IAreaService
{
    private readonly IAreaRepository _areaRepository;

    public AreaService(IAreaRepository areaRepository)
    {
        _areaRepository = areaRepository;
    }
    private GetAreasPaginatedDto GroupAndMapAreas(
        List<GetAreas> areas,
        int currentPage,
        int pageSize)
    {
        var paginatedData = areas.Select(a => new GetAreasDto
        {
            AreaId = a.AreaId,
            CityId = a.CityId,
            AreaName = a.AreaName,
            PostalCode = a.PostalCode,
            IsActive = a.IsActive,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt
        }).ToList();

        return new GetAreasPaginatedDto
        {
            Pagination = new PaginationDto
            {
                TotalPages = areas.FirstOrDefault()?.TotalPages ?? 0,
                TotalRecords = areas.FirstOrDefault()?.RecordsFiltered ?? 0,
                FilteredRecords = areas.FirstOrDefault()?.RecordsFiltered ?? 0,
                CurrentPage = currentPage,
                PageSize = pageSize
            },
            Data = paginatedData
        };
    }


    public async Task<ApiResponse<GetAreasPaginatedDto>> GetAreas(PaginatedRequest request)
    {
        if (request.PageNumber <= 0)
            return new ApiResponse<GetAreasPaginatedDto>(false, "Page Number is required.", null);
        if (request.PageSize <= 0)
            return new ApiResponse<GetAreasPaginatedDto>(false, "Page Size is required.", null);

        var result = await _areaRepository.GetAreas(request.PageNumber,
            request.PageSize, request.Search, request.OrderColumnIndex, request.OrderDirection);

        return new ApiResponse<GetAreasPaginatedDto>(true, "Data has been retrieved.", GroupAndMapAreas(result, request.PageNumber, request.PageSize));
    }

    public async Task<ApiResponse<string>> InsertUpdateArea(InsertUpdateAreaRequest request)
    {
        // Validation for AreaName
        if (string.IsNullOrEmpty(request.AreaName))
            return new ApiResponse<string>(false, "AreaName is required.", null);

        // Validation for AreaId
        if (request.AreaId < 0)
            return new ApiResponse<string>(false, "AreaId must be a valid positive integer.", null);

        // Validation for CityId
        if (request.CityId <= 0)
            return new ApiResponse<string>(false, "CityId must be a valid positive integer.", null);

        // Validation for PostalCode
        if (string.IsNullOrEmpty(request.PostalCode))
            return new ApiResponse<string>(false, "PostalCode is required.", null);

        // Execute repository method
        var result = await _areaRepository.InsertUpdateArea(
            request.AreaId,
            request.CityId,
            request.AreaName,
            request.PostalCode,
            request.IsActive,
            request.CreateBy,
            request.UpdateBy
        );

        // Return response
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }
}

public class InsertUpdateAreaRequest
{
    public int AreaId { get; set; }
    public int CityId { get; set; }
    public string? AreaName { get; set; }
    public string? PostalCode { get; set; }
    public bool IsActive { get; set; }
    public int CreateBy { get; set; }
    public int UpdateBy { get; set; }
}
