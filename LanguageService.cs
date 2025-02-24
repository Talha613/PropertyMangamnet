using PropertyManagement.Core.DTOs;
using PropertyManagement.Core.DTOs.Shared;
using PropertyManagement.Core.DTOs.Sps;
using PropertyManagement.Core.Models;
using PropertyManagement.Data.Repositories;

namespace PropertyManagement.Business.Services;

public interface ILanguageService
{
    Task<ApiResponse<string>> InsertUpdateLanguage(InsertUpdateLanguageRequest request);
    Task<ApiResponse<GetLanguagesPaginatedDto>> GetLanguages(PaginatedRequest request);
}
public class LanguageService : ILanguageService
{
    private readonly ILanguageRepository _languageRepository;

    public LanguageService(ILanguageRepository languageRepository)
    {
        _languageRepository = languageRepository;
    }

    public async Task<ApiResponse<GetLanguagesPaginatedDto>> GetLanguages(PaginatedRequest request)
    {
        if (request.PageNumber <= 0)
            return new ApiResponse<GetLanguagesPaginatedDto>(false, "Page Number is required.", null);
        if (request.PageSize <= 0)
            return new ApiResponse<GetLanguagesPaginatedDto>(false, "Page Size is required.", null);

        var result = await _languageRepository.GetLanguages(request.PageNumber,
            request.PageSize, request.Search, request.OrderColumnIndex, request.OrderDirection);

        return new ApiResponse<GetLanguagesPaginatedDto>(true, "Data has been retrieved.", GroupAndMapLanguages(result, request.PageNumber, request.PageSize));
    }
    private GetLanguagesPaginatedDto GroupAndMapLanguages(
        List<GetLanguages> languages,
        int currentPage,
        int pageSize)
    {
        var paginatedData = languages.Select(l => new GetLanguagesDto
        {
            LanguageId = l.LanguageId,
            LanguageCode = l.LanguageCode,
            LanguageName = l.LanguageName,
            IsActive = l.IsActive,
            CreatedAt = l.CreatedAt,
            UpdatedAt = l.UpdatedAt
        }).ToList();

        return new GetLanguagesPaginatedDto
        {
            Pagination = new PaginationDto
            {
                TotalPages = languages.FirstOrDefault()?.TotalPages ?? 0,
                TotalRecords = languages.FirstOrDefault()?.RecordsFiltered ?? 0,
                FilteredRecords = languages.FirstOrDefault()?.RecordsFiltered ?? 0,
                CurrentPage = currentPage,
                PageSize = pageSize
            },
            Data = paginatedData
        };
    }

    public async Task<ApiResponse<string>> InsertUpdateLanguage(InsertUpdateLanguageRequest request)
    {
        // Validation for LanguageName
        if (string.IsNullOrEmpty(request.LanguageName))
            return new ApiResponse<string>(false, "LanguageName is required.", null);

        // Validation for LanguageCode
        if (string.IsNullOrEmpty(request.LanguageCode))
            return new ApiResponse<string>(false, "LanguageCode is required.", null);

        // Validation for LanguageId
        if (request.LanguageId < 0)
            return new ApiResponse<string>(false, "LanguageId must be a valid positive integer.", null);

        // Execute repository method
        var result = await _languageRepository.InsertUpdateLanguage(
            request.LanguageId,
            request.LanguageCode,
            request.LanguageName,
            request.IsActive,
            request.CreateBy,
            request.UpdateBy
        );

        // Return response
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }
}

public class InsertUpdateLanguageRequest
{
    public int LanguageId { get; set; }
    public string? LanguageCode { get; set; }
    public string? LanguageName { get; set; }
    public bool IsActive { get; set; }
    public int CreateBy { get; set; }
    public int UpdateBy { get; set; }
}