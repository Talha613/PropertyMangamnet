using PropertyManagement.Core.DTOs;
using PropertyManagement.Core.DTOs.Shared;
using PropertyManagement.Core.DTOs.Sps;
using PropertyManagement.Core.Models;
using PropertyManagement.Data.Repositories;
namespace PropertyManagement.Business.Services;
public interface IPropertyConfigurationService
{
    Task<ApiResponse<string>> InsertUpdatePropertyConfigurationDetails(InsertUpdatePropertyConfigurationDetailsRequest request);
    Task<ApiResponse<GetPropertyConfigurationDetailPaginatedDto>> GetPropertyConfigurationDetail(PaginatedRequest request);
}

public class PropertyConfigurationService : IPropertyConfigurationService
{
    private readonly IPropertyConfigurationRepository _propertyConfigurationRepository;


    public PropertyConfigurationService(IPropertyConfigurationRepository propertyConfigurationRepository)
    {
        _propertyConfigurationRepository = propertyConfigurationRepository;
    }

    private GetPropertyConfigurationDetailPaginatedDto GroupAndMapProperties(
        List<GetPropertyConfigurationDetail> properties,
        int currentPage,
        int pageSize)
    {
        // Mapping the data from the list of properties to DTO
        var paginatedData = properties.Select(p => new GetPropertyConfigurationDetailDto
        {
            ConfigDetailId = p.ConfigDetailId,
            ConfigMasterId = p.ConfigMasterId,
            Value = p.Value,
            SecondValue = p.SecondValue,
            ImageUrl = p.ImageUrl,
            IsActive = p.IsActive,
            CreateBy = p.CreateBy,
            CreatedAt = p.CreatedAt,
            UpdateBy = p.UpdateBy,
            UpdatedAt = p.UpdatedAt
        }).ToList();

        // Creating and returning the paginated DTO with pagination metadata
        return new GetPropertyConfigurationDetailPaginatedDto
        {
            Pagination = new PaginationDto
            {
                TotalPages = properties[0].TotalPages,           // From your DB result
                TotalRecords = properties[0].RecordsFiltered,       // From your DB result
                FilteredRecords = properties[0].RecordsFiltered, // From your DB result
                CurrentPage = currentPage,         // The current page requested
                PageSize = pageSize                // Number of records per page
            },
            Data = paginatedData
        };
    }

    public async Task<ApiResponse<string>> InsertUpdatePropertyConfigurationDetails(InsertUpdatePropertyConfigurationDetailsRequest request)
    {
        if (request.MasterId <= 0)
            return new ApiResponse<string>(false, "Master Id is required.", null);
        if (string.IsNullOrEmpty(request.Value))
            return new ApiResponse<string>(false, "Value is required.", null);


        var result = await _propertyConfigurationRepository.InsertUpdatePropertyConfigurationDetails(request.DetailId,
            request.MasterId, request.Value, request.SecondValue, request.ImageUrl, Convert.ToInt32(request.By));

        return new ApiResponse<string>(result.Status, result.Message, result.Data);

    }

    public async Task<ApiResponse<GetPropertyConfigurationDetailPaginatedDto>> GetPropertyConfigurationDetail(PaginatedRequest request)
    {
        if (request.PageNumber <= 0)
            return new ApiResponse<GetPropertyConfigurationDetailPaginatedDto>(false, "Page Number is required.", null);
        if (request.PageSize <= 0)
            return new ApiResponse<GetPropertyConfigurationDetailPaginatedDto>(false, "Page Number is required.", null);
        var result = await _propertyConfigurationRepository.GetPropertyConfigurationDetail(request.PageNumber,
            request.PageSize, request.Search, request.OrderColumnIndex, request.OrderDirection);
        return new ApiResponse<GetPropertyConfigurationDetailPaginatedDto>(true, "Data has been retrieved. ", GroupAndMapProperties(result,request.PageNumber,request.PageSize));
    }
}

public class InsertUpdatePropertyConfigurationDetailsRequest
{
    public int DetailId { get; set; }
    public int MasterId { get; set; }
    public string Value { get; set; } = string.Empty;
    public string? SecondValue { get; set; }
    public string? ImageUrl { get; set; }
    public string By { get; set; } = string.Empty;
}




