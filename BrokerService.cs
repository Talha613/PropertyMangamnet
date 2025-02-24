using PropertyManagement.Core.DTOs;
using PropertyManagement.Core.DTOs.Shared;
using PropertyManagement.Core.DTOs.Sps;
using PropertyManagement.Core.Models;
using PropertyManagement.Data.Repositories;
namespace PropertyManagement.Business.Services;
public interface IBrokerService
{
    Task<ApiResponse<GetEstateBrokersWithAgentCountDtoPaginatedDto>> GetEstateBrokersWithAgentCounts(GetEstateBrokersWithAgentCountRequest request);
    Task<ApiResponse<string>> InsertUpdateRealEstateBroker(InsertUpdateRealEstateBrokerRequest request);

    Task<ApiResponse<GetRealEstateBrokersPaginatedDto>>GetRealEstateBrokers(PaginatedRequest request);
}

public class BrokerService : IBrokerService
{
    private readonly IBrokerRepository _brokerRepository;


    public BrokerService(IBrokerRepository brokerRepository)
    {
        _brokerRepository = brokerRepository;
    }

    private GetEstateBrokersWithAgentCountDtoPaginatedDto? GroupAndMapProperties(List<GetEstateBrokersWithAgentCounts> properties, int currentPage, int pageSize)
    {
        var groupedProperties = properties
            .GroupBy(p => p.RealEstateBrokerId)
            .Select(group => new GetEstateBrokersWithAgentCountDtoPaginatedDto
            {
                Pagination = new PaginationDto
                {
                    TotalPages = group.FirstOrDefault()?.TotalPages ?? 0,
                    TotalRecords = group.FirstOrDefault()?.TotalRecords ?? 0,
                    CurrentPage = currentPage,
                    PageSize = pageSize
                },

                Data = group.Select(p => new GetEstateBrokersWithAgentCountDto
                {
                    RealEstateBrokerId = p.RealEstateBrokerId,
                    BrokerName = p.BrokerName,
                    BrokerEmail = p.BrokerEmail,
                    BrokerPhone = p.BrokerPhone,
                    Logo = p.Logo,
                    About = p.About,
                    Orn = p.Orn,
                    Address = p.Address,
                    AgentCount = p.AgentCount
                }).ToList()
            })
            .ToList();

        // If you expect only one group, return the first one.
        return groupedProperties.FirstOrDefault();
    }





    private GetRealEstateBrokersPaginatedDto GroupAndMapBroker(
        List<GetRealEstateBrokers> brokers,
        int currentPage,
        int pageSize)
    {
        var paginatedData = brokers.Select(b => new GetRealEstateBrokersDto
        {
            RealEstateBrokerId = b.RealEstateBrokerId, // Corrected the property name
            Name = b.Name,
            Orn = b.Orn,
            Office = b.Office,
            Building = b.Building,
            AreaId = b.AreaId,
            Email = b.Email,
            Phone = b.Phone,
            Logo = b.Logo,
            About = b.About,
            CreatedAt = b.CreatedAt,
            CreateBy = b.CreateBy,
            UpdatedAt = b.UpdatedAt,
            UpdateBy = b.UpdateBy
        }).ToList();

        return new GetRealEstateBrokersPaginatedDto
        {
            Pagination = new PaginationDto
            {
                TotalPages = brokers.FirstOrDefault()?.TotalPages ?? 0,
                TotalRecords = brokers.FirstOrDefault()?.RecordsFiltered ?? 0,
                FilteredRecords = brokers.FirstOrDefault()?.RecordsFiltered ?? 0,
                CurrentPage = currentPage,
                PageSize = pageSize
            },
            Data = paginatedData
        };
    }



    public async Task<ApiResponse<GetEstateBrokersWithAgentCountDtoPaginatedDto>> GetEstateBrokersWithAgentCounts(GetEstateBrokersWithAgentCountRequest request)
    {
        if (request.PageNumber <= 0)
            return new ApiResponse<GetEstateBrokersWithAgentCountDtoPaginatedDto>(false, "Page Number is required.", null);
        if (request.PageSize <= 0)
            return new ApiResponse<GetEstateBrokersWithAgentCountDtoPaginatedDto>(false, "Page Number is required.", null);

        var data = await _brokerRepository.GetEstateBrokersWithAgentCounts( request.PageNumber, request.PageSize);


        return new ApiResponse<GetEstateBrokersWithAgentCountDtoPaginatedDto>(true, "Data has been retrieved . ", GroupAndMapProperties(data, request.PageNumber, request.PageSize));
    }

    public async Task<ApiResponse<string>> InsertUpdateRealEstateBroker(InsertUpdateRealEstateBrokerRequest request)
    {



        // Validate required fields
        if (string.IsNullOrEmpty(request.Name))
            return new ApiResponse<string>(false, "Name is required.", null);

        if (string.IsNullOrEmpty(request.Orn))
            return new ApiResponse<string>(false, "ORN is required.", null);

        if (string.IsNullOrEmpty(request.Office))
            return new ApiResponse<string>(false, "Office is required.", null);

        if (string.IsNullOrEmpty(request.Building))
            return new ApiResponse<string>(false, "Building is required.", null);

        if (request.AreaId <= 0)
            return new ApiResponse<string>(false, "Area ID must be a valid positive integer.", null);

        if (string.IsNullOrEmpty(request.Email))
            return new ApiResponse<string>(false, "Email is required.", null);

        if (string.IsNullOrEmpty(request.Phone))
            return new ApiResponse<string>(false, "Phone is required.", null);

        if (request.RealEstateBrokerId < 0)
            return new ApiResponse<string>(false, "Real Estate Broker ID cannot be negative.", null);
        if (string.IsNullOrEmpty(request.About))
            return new ApiResponse<string>(false, "About is required.", null);

        var result = await _brokerRepository.InsertUpdateRealEstateBroker(request.RealEstateBrokerId, request.Name,
            request.Orn, request.Office, request.Building, request.AreaId, request.Email, request.Phone, request.Logo,
            request.About, Convert.ToInt32(request.CreateBy), request.UpdateBy);

        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }

    public async Task<ApiResponse<GetRealEstateBrokersPaginatedDto>> GetRealEstateBrokers(PaginatedRequest request)
    {
        if (request.PageNumber <= 0)
            return new ApiResponse<GetRealEstateBrokersPaginatedDto>(false, "Page Number is required.", null);
        if (request.PageSize <= 0)
            return new ApiResponse<GetRealEstateBrokersPaginatedDto>(false, "Page Number is required.", null);
        var result = await _brokerRepository.GetRealEstateBrokers(request.PageNumber,
            request.PageSize, request.Search, request.OrderColumnIndex, request.OrderDirection);
        return new ApiResponse<GetRealEstateBrokersPaginatedDto>(true, "Data has been retrieved. ", GroupAndMapBroker(result, request.PageNumber, request.PageSize));
    }
}


public record GetEstateBrokersWithAgentCountRequest(
    int PageNumber,
    int PageSize
);

public class InsertUpdateRealEstateBrokerRequest
{
    public int RealEstateBrokerId { get; set; } // Pass 0 for INSERT, or the existing ID for UPDATE
    public string? Name { get; set; }
    public string? Orn { get; set; }
    public string? Office { get; set; }
    public string? Building { get; set; }
    public int AreaId { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Logo { get; set; }
    public string? About { get; set; }
    public string? CreateBy { get; set; } // Optional, can be null
    public string? UpdateBy { get; set; } // Optional, can be null
}

