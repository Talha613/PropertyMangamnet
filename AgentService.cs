using PropertyManagement.Core.DTOs;
using PropertyManagement.Core.DTOs.Shared;
using PropertyManagement.Core.DTOs.Sps;
using PropertyManagement.Core.Models;
using PropertyManagement.Data.Repositories;
using System.Text.RegularExpressions;
namespace PropertyManagement.Business.Services;
public interface IAgentService
{
    Task<ApiResponse<GetAgentsListPaginatedDto>> GetAgentsList(GetAgentsListRequest request);
    Task<ApiResponse<string>> InsertUpdateAgentUserRequest(InsertUpdateAgentUserRequest request);
    Task<ApiResponse<GetAgentUsersPaginatedDto>> GetAgentUsers(PaginatedRequest request);

}

public class AgentService : IAgentService
{
    private readonly IAgentRepository _agentRepository;


    public AgentService(IAgentRepository agentRepository)
    {
        _agentRepository = agentRepository;
    }

    private GetAgentsListPaginatedDto? GroupAndMapProperties(List<GetAgentsList> properties, int currentPage, int pageSize)
    {

        var groupedProperties = properties
            .GroupBy(p => p.UserId)
            .Select(group => new GetAgentsListPaginatedDto
            {

                Pagination = new PaginationDto
                {
                    TotalPages = group.FirstOrDefault()?.TotalPages ?? 0,
                    TotalRecords = group.FirstOrDefault()?.TotalRecords ?? 0,
                    CurrentPage = currentPage,
                    PageSize = pageSize

                },

                Data = group.Select(p => new GetAgentsListDto
                {
                    UserId = p.UserId,
                    AgentName = p.AgentName,
                    AgentEmail = p.AgentEmail,
                    Experience = p.Experience,
                    Phone = p.Phone,
                    RealEstateBrokerId = p.RealEstateBrokerId,
                    ProfilePicture = p.ProfilePicture,
                    WhatsApp = p.WhatsApp

                }).ToList()
            })
            .ToList();

        return groupedProperties.SingleOrDefault();
    }



    private GetAgentUsersPaginatedDto GroupAndMapAgentUsers(
        List<GetAgentUsers> agentUsers,
        int currentPage,
        int pageSize)
    {
        var paginatedData = agentUsers.Select(a => new GetAgentUsersDto
        {
            UserId = a.UserId,
            Name = a.Name,
            Email = a.Email,
            Experience = a.Experience,
            ProfilePicture = a.ProfilePicture,
            Phone = a.Phone,
            WhatsApp = a.WhatsApp,
            RealEstateBrokerId = a.RealEstateBrokerId,
            IsActive = a.IsActive,
            CreateBy = a.CreateBy,
            CreatedAt = a.CreatedAt,
            UpdateBy = a.UpdateBy,
            UpdatedAt = a.UpdatedAt
        }).ToList();

        return new GetAgentUsersPaginatedDto
        {
            Pagination = new PaginationDto
            {
                TotalPages = agentUsers.FirstOrDefault()?.TotalPages ?? 0,
                TotalRecords = agentUsers.FirstOrDefault()?.RecordsFiltered ?? 0,
                FilteredRecords = agentUsers.FirstOrDefault()?.RecordsFiltered ?? 0,
                CurrentPage = currentPage,
                PageSize = pageSize
            },
            Data = paginatedData
        };
    }




    public async Task<ApiResponse<GetAgentsListPaginatedDto>> GetAgentsList(GetAgentsListRequest request)
    {
        if (request.PageNumber <= 0)
            return new ApiResponse<GetAgentsListPaginatedDto>(false, "Page Number is required.", null);
        if (request.PageSize <= 0)
            return new ApiResponse<GetAgentsListPaginatedDto>(false, "Page Number is required.", null);

        var data = await _agentRepository.GetAgentsList(request.BrokerId, request.PageNumber, request.PageSize);

        return new ApiResponse<GetAgentsListPaginatedDto>(true, "Data has been retrieved . ", GroupAndMapProperties(data, request.PageNumber, request.PageSize));
    }

    public async Task<ApiResponse<string>> InsertUpdateAgentUserRequest(InsertUpdateAgentUserRequest request)
    {
        // Validate required fields
        if (string.IsNullOrEmpty(request.Name))
            return new ApiResponse<string>(false, "Name is required.", null);
        if (string.IsNullOrEmpty(request.Email))
            return new ApiResponse<string>(false, "Email is required.", null);
        if (string.IsNullOrEmpty(request.Phone))
            return new ApiResponse<string>(false, "Phone is required.", null);
        if (string.IsNullOrEmpty(request.WhatsApp))
            return new ApiResponse<string>(false, "WhatsApp number is required.", null);
        if (request.RealEstateBrokerId < 0)
            return new ApiResponse<string>(false, "Real Estate Broker ID must be a valid positive integer.", null);


        if (request.RealEstateBrokerId != 0 && string.IsNullOrEmpty(request.Password))
            return new ApiResponse<string>(false, "Real Estate Broker ID must be a valid positive integer.", null);

        // Validate email format (basic regex check)
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if (!emailRegex.IsMatch(request.Email))
            return new ApiResponse<string>(false, "Invalid email format.", null);

        // Validate phone number (basic check, you can improve this if needed)
        if (string.IsNullOrEmpty(request.Phone))
            return new ApiResponse<string>(false, "Invalid phone number.", null);

        // Validate WhatsApp number (basic check, you can improve this if needed)
        if (string.IsNullOrEmpty(request.WhatsApp))
            return new ApiResponse<string>(false, "Invalid WhatsApp number.", null);

        // Validate experience (must be positive)
        if (request.Experience <= 0)
            return new ApiResponse<string>(false, "Experience must be a valid positive integer.", null);

        var result = await _agentRepository.InsertUpdateAgentUser(request.UserId, request.Name, request.Email,
            request.Password, request.Experience, request.ProfilePicture, request.Phone, request.WhatsApp,
            request.RealEstateBrokerId, request.IsActive, Convert.ToInt32(request.CreateBy), request.UpdateBy!);
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }

    public async Task<ApiResponse<GetAgentUsersPaginatedDto>> GetAgentUsers(PaginatedRequest request)
    {
        if (request.PageNumber <= 0)
            return new ApiResponse<GetAgentUsersPaginatedDto>(false, "Page Number is required.", null);
        if (request.PageSize <= 0)
            return new ApiResponse<GetAgentUsersPaginatedDto>(false, "Page Number is required.", null);
        var result = await _agentRepository.GetAgentUsers(request.PageNumber,
            request.PageSize, request.Search, request.OrderColumnIndex, request.OrderDirection);
        return new ApiResponse<GetAgentUsersPaginatedDto>(true, "Data has been retrieved. ", GroupAndMapAgentUsers(result, request.PageNumber, request.PageSize));
    }




}


public record GetAgentsListRequest(
    int? BrokerId,
    int PageNumber,
    int PageSize
);


public class InsertUpdateAgentUserRequest
{
    public int UserId { get; set; } 
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; } 
    public int Experience { get; set; }
    public string? ProfilePicture { get; set; }
    public string? Phone { get; set; }
    public string? WhatsApp { get; set; }
    public int RealEstateBrokerId { get; set; }
    public bool IsActive { get; set; }
    public string CreateBy { get; set; } = string.Empty;
    public string? UpdateBy { get; set; } = string.Empty;
}
