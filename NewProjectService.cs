using PropertyManagement.Core.DTOs;
using PropertyManagement.Core.DTOs.Shared;
using PropertyManagement.Core.DTOs.Sps;
using PropertyManagement.Core.Models;
using PropertyManagement.Data.Repositories;

namespace PropertyManagement.Business.Services;
public interface INewProjectService
{

    Task<ApiResponse<NewProjectsPaginatedDto>> GetNewProjects(GetNewProjectsRequest request);
    Task<ApiResponse<GetNewProjectDetailsDto>> GetNewProjectDetails(int projectId, string language);
    Task<ApiResponse<string>> InsertUpdateNewProject(InsertUpdateNewProjectRequest request);
    Task<ApiResponse<string>> InsertNewProjectMedia(InsertNewProjectMediaRequest request);
    Task<ApiResponse<string>> InsertNewProjectsDetailMap(InsertNewProjectsDetailMapRequest request);
    Task<ApiResponse<string>> DeleteNewProject(int projectId);
    Task<ApiResponse<string>> InsertUpdatePaymentPlan(InsertUpdatePaymentPlanRequest request);
    Task<ApiResponse<string>> InsertUpdateProjectPaymentPlanDetails(InsertUpdateProjectPaymentPlanDetailRequest request);

    Task<ApiResponse<string>> InsertUpdateProjectTimeline(InsertUpdateProjectTimelineRequest request);
    Task<ApiResponse<string>> InsertUpdateProjectUnit(InsertUpdateProjectUnitRequest request);

    Task<ApiResponse<List<GetMediaByProjectIdForAdminPanelDto>>> GetMediaByProjectIdForAdminPanel(int projectId);
    Task<ApiResponse<List<GetUnitByProjectIdForAdminDto>>> GetUnitByProjectIdForAdmin(int projectId);
    Task<ApiResponse<List<GetProjectTimelineByProjectIdForAdminDto>>> GetProjectTimelineByProjectIdForAdmin(int projectId);


    Task<ApiResponse<GetProjectsPaginatedDto>> GetProjects(PaginatedRequest request);
}

public class NewProjectService : INewProjectService
{
    private readonly INewProjectRepository _newProjectRepository;


    public NewProjectService(INewProjectRepository newProjectRepository)
    {
        _newProjectRepository = newProjectRepository;
    }



    #region DataExtractMethods
    private NewProjectsPaginatedDto? GroupAndMapProperties(List<GetNewProjects> properties, int currentPage, int pageSize)
    {



        // Group the properties by BuyPropertyId
        var groupedProperties = properties
        .GroupBy(p => p.ProjectId)
        .Select(group => new NewProjectsPaginatedDto
        {
            // Assuming TotalPages and TotalRecords are the same for all properties in the group

            Pagination = new PaginationDto
            {
                TotalPages = group.FirstOrDefault()?.TotalPages ?? 0,
                TotalRecords = group.FirstOrDefault()?.TotalRecords ?? 0,
                CurrentPage = currentPage,
                PageSize = pageSize

            },

            // Mapping Data as a list of BuyPropertyDto for each group (for each unique BuyPropertyId)
            Data = group.Select(p => new GetNewProjectsDto()
            {
                ProjectId = group.First().ProjectId,
                ProjectDetails = new GetNewProjectsDetailsDto()
                {
                    ProjectStatus = group.First().ProjectStatus,
                    ShortDescription = group.First().ShortDescription,
                    LaunchPrice = group.First().LaunchPrice,
                    DeliveryDate = group.First().DeliveryDate,
                    DeveloperName = group.First().DeveloperName,
                    Location = group.First().Location,

                },
                // Mapping MediaInfo for each individual property
                MediaInfo = group.Where(m => m.ProjectId == p.ProjectId)  // Filter media for this specific property
                    .Select(m => new MediaInfoDto
                    {
                        MediaId = 0,
                        MediaType = m.MediaType,
                        MediaUrl = m.MediaUrl,
                        MediaMenu = m.MediaMenu
                    }).ToList()
            }).DistinctBy(x => x.ProjectId).ToList()
        })
        .ToList();

        // Return the first grouped element (or adjust this logic to return all groups, depending on the scenario)
        return groupedProperties.DistinctBy(x => x.Data).SingleOrDefault();  // You can modify this based on pagination or other logic
    }
    private GetNewProjectDetailsDto? GroupAndMapPropertyDetails(List<GetNewProjectDetails> properties, List<GetProjectPaymentPLan> plan, List<GetProjectTimeline> timeline, List<GetProjectUnit> units)
    {
        // Group the properties by BuyPropertyId
        var groupedProperties = properties
            .GroupBy(p => p.ProjectId)
            .Select(group => new GetNewProjectDetailsDto
            {
                ProjectId = group.Key, // group.Key represents the BuyPropertyId

                // Mapping PropertyDetails (taking the first item from the group)
                UnitInfo = units
                    .Select(p => new GetProjectUnitDto()
                    {
                        PropertyType = p.PropertyType,
                        BedType = p.BedType,
                        FloorPlanUrl = p.FloorPlanUrl,
                        LayoutType = p.LayoutType,
                        Price = p.Price,
                        Sqft = p.Sqft,
                        ProjectUnitId = p.ProjectUnitId
                    }).ToList(),
                TimeLineInfo = timeline
                    .Select(p => new GetProjectTimelineDto()
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Date = p.Date
                    }).ToList(),
                PaymentPlanInfo = plan
                    .Select(p => new GetProjectPaymentPLanDto
                    {
                        Id = p.Id,
                        PaymentPlanName = p.PaymentPlanName,
                        PlanDescription = p.PlanDescription,
                        PlanSubDescription = p.PlanSubDescription,
                        PlanPercentage = p.PlanPercentage
                    }).ToList(),
                DeveloperInfo = new DeveloperDto()
                {
                    DeveloperId = group.First().DeveloperId,
                    DeveloperName = group.First().DeveloperName,
                    DeveloperLogo = group.First().DeveloperLogo
                },
                ProjectDetails = new GetNewProjectDetailOfDetailsDto()
                {
                    PropertyTypes = group.First().PropertyTypes,
                    ProjectStatus = group.First().ProjectStatus,
                    ShortDescription = group.First().ShortDescription,
                    LongDescription = group.First().LongDescription,
                    Location = group.First().Location,
                    DeliveryDate = group.First().DeliveryDate,
                    GovFee = group.First().GovFee,
                    LaunchPrice = group.First().LaunchPrice,
                    NumberOfBuildings = group.First().NumberOfBuildings,
                    SalesStart = group.First().SalesStart
                },
                Amenities = group.Select(m => new KeyValueString()
                {
                    Key = m.Value,
                    Value = m.ImageUrl,
                }).DistinctBy(x => x.Key).ToList(),
                MediaInfo = group.Select(m => new MediaInfoDto
                {
                    MediaId = m.NewProjectMediaId,
                    MediaType = m.MediaType,
                    MediaUrl = m.MediaUrl,
                    MediaMenu = m.MediaMenu
                }).DistinctBy(x => x.MediaId).ToList()
            })
            .ToList();

        return groupedProperties.SingleOrDefault();
    }

    private GetProjectsPaginatedDto GroupAndMapProjects(List<GetProjects> projects, int currentPage, int pageSize)
    {
        var paginatedData = projects.Select(p => new GetProjectsDto
        {
            ProjectId = p.ProjectId,
            ProjectStatus = p.ProjectStatus,
            DeveloperId = p.DeveloperId,
            AreaId = p.AreaId,
            CommunityName = p.CommunityName,
            GovFee = p.GovFee,
            ShortDescription = p.ShortDescription,
            LongDescription = p.LongDescription,
            LaunchPrice = p.LaunchPrice,
            DeliveryDate = p.DeliveryDate,
            SalesStart = p.SalesStart,
            NumberOfBuildings = p.NumberOfBuildings,
            PropertyTypeId = p.PropertyTypeId,
            CreateBy = p.CreateBy,
            CreateAt = p.CreateAt,
            UpdateBy = p.UpdateBy,
            UpdateAt = p.UpdateAt
        }).ToList();

        return new GetProjectsPaginatedDto
        {
            Pagination = new PaginationDto
            {
                TotalPages = projects.FirstOrDefault()?.TotalPages ?? 0,
                TotalRecords = projects.FirstOrDefault()?.RecordsFiltered ?? 0,
                FilteredRecords = projects.FirstOrDefault()?.RecordsFiltered ?? 0,
                CurrentPage = currentPage,
                PageSize = pageSize
            },
            Data = paginatedData
        };
    }



    private List<GetMediaByProjectIdForAdminPanelDto> GroupAndMapNewProjectMediaForAdminPanel(List<GetMediaByProjectIdForAdminPanel> mediaList)
    {
        var paginatedData = mediaList.Select(m => new GetMediaByProjectIdForAdminPanelDto
        {
            NewProjectMediaId = m.NewProjectMediaId,
            NewProjectId = m.NewProjectId,
            MediaMenuId = m.MediaMenuId,
            MediaUrl = m.MediaUrl,
            MediaDescription = m.MediaDescription,
            MediaType = m.MediaType,
            CreateBy = m.CreateBy,
            CreatedAt = m.CreatedAt
        }).ToList();

        return paginatedData;
    }


    private List<GetProjectTimelineByProjectIdForAdminDto> GroupAndMapProjectTimeline(List<GetProjectTimelineByProjectIdForAdmin> timelineList)
    {
        var mappedTimelineData = timelineList.Select(t => new GetProjectTimelineByProjectIdForAdminDto()
        {
            ProjectTimelineId = t.ProjectTimelineId,
            ProjectId = t.ProjectId,
            TimeLineConfigId = t.TimeLineConfigId,
            Date = t.Date,
            CreatedAt = t.CreatedAt,
            CreatedBy = t.CreatedBy,
            UpdatedAt = t.UpdatedAt,
            UpdatedBy = t.UpdatedBy
        }).ToList();

        return mappedTimelineData;
    }


    private List<GetUnitByProjectIdForAdminDto> GroupAndMapProjectUnitsForAdminPanel(List<GetUnitByProjectIdForAdmin> unitList)
    {
        var mappedData = unitList.Select(u => new GetUnitByProjectIdForAdminDto()
        {
            ProjectUnitId = u.ProjectUnitId,
            ProjectId = u.ProjectId,
            PropertyTypeId = u.PropertyTypeId,
            Bed = u.Bed,
            LayoutType = u.LayoutType,
            Price = u.Price,
            Sqft = u.Sqft,
            FloorPlanUrl = u.FloorPlanUrl,
            CreateAt = u.CreateAt,
            CreateBy = u.CreateBy,
            UpdateAt = u.UpdateAt,
            UpdateBy = u.UpdateBy
        }).ToList();

        return mappedData;
    }


    #endregion







    public async Task<ApiResponse<NewProjectsPaginatedDto>> GetNewProjects(GetNewProjectsRequest request)
    {
        if (request.PageNumber <= 0)
            return new ApiResponse<NewProjectsPaginatedDto>(false, "Page Number is required.", null);
        if (request.PageSize <= 0)
            return new ApiResponse<NewProjectsPaginatedDto>(false, "Page Number is required.", null);

        var data = await _newProjectRepository.GetNewProjects(request.Search, request.PropertyTypeId,
            request.Bedrooms, request.Bathrooms, request.MinPrice, request.MaxPrice, request.Keywords, request.MinArea,
            request.MaxArea, request.AmenitiesIDs, request.UserId, request.Language, request.PageNumber, request.PageSize);


        return new ApiResponse<NewProjectsPaginatedDto>(true, "Data has been retrieved . ", GroupAndMapProperties(data, request.PageNumber, request.PageSize));
    }


    public async Task<ApiResponse<GetNewProjectDetailsDto>> GetNewProjectDetails(int projectId, string language)
    {
        if (projectId <= 0)
            return new ApiResponse<GetNewProjectDetailsDto>(false, "Property Id is required.", null);
        var data = await _newProjectRepository.GetNewProjectDetails(projectId, language);
        var paymentPlan = await _newProjectRepository.GetProjectPaymentPLan(projectId);
        var timeLine = await _newProjectRepository.GetProjectTimeline(projectId);
        var units = await _newProjectRepository.GetProjectUnit(projectId);



        return new ApiResponse<GetNewProjectDetailsDto>(true, "Data has been retrieved . ", GroupAndMapPropertyDetails(data, paymentPlan, timeLine, units));

    }






    public async Task<ApiResponse<string>> InsertUpdateNewProject(InsertUpdateNewProjectRequest request)
    {
        if (request.ProjectId < 0)
            return new ApiResponse<string>(false, "Invalid Project ID.", null);
        if (string.IsNullOrEmpty(request.CommunityName))
            return new ApiResponse<string>(false, "CommunityName Required.", null);
        var result = await _newProjectRepository.InsertUpdateNewProject(
            request.ProjectId,
            request.ProjectStatus,
            request.DeveloperId,
            request.AreaId,
            request.CommunityName,
            request.GovFee,
            request.ShortDescription,
            request.LongDescription,
            request.LaunchPrice,
            request.DeliveryDate,
            request.SalesStart,
            request.NumberOfBuildings,
            request.PropertyTypeId,
            request.CreateBy,
            request.UpdateBy
        );

        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }



    public async Task<ApiResponse<string>> InsertNewProjectMedia(InsertNewProjectMediaRequest request)
    {
        // Validation
        if (request.NewProjectId < 0)
            return new ApiResponse<string>(false, "Invalid Project ID.", null);
        if (request.MediaMenuId < 0)
            return new ApiResponse<string>(false, "Invalid Media Menu ID.", null);
        if (string.IsNullOrEmpty(request.MediaUrl))
            return new ApiResponse<string>(false, "Media URL is required.", null);
        if (string.IsNullOrEmpty(request.MediaType))
            return new ApiResponse<string>(false, "Media Type is required.", null);

        // Calling the repository method
        var result = await _newProjectRepository.InsertNewProjectMedia(
            request.NewProjectId,
            request.MediaMenuId,
            request.MediaUrl,
            request.MediaDescription,
            request.MediaType,
            request.CreateBy,
            request.UpdateBy
        );

        // Return response based on result
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }


    public async Task<ApiResponse<string>> InsertNewProjectsDetailMap(InsertNewProjectsDetailMapRequest request)
    {
        // Validate required parameters
        if (request.ProjectId <= 0)
            return new ApiResponse<string>(false, "Invalid Project ID.", null);

        if (string.IsNullOrEmpty(request.ConfigDetailsList))
            return new ApiResponse<string>(false, "ConfigDetailsList is required.", null);

        // Call repository method
        var result = await _newProjectRepository.InsertNewProjectsDetailMap(
            request.ProjectId,
            request.ConfigDetailsList,
            request.FirstValue,
            request.SecondValue,
            request.CreateBy
        );
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }

    public async Task<ApiResponse<string>> DeleteNewProject(int projectId)
    {
        // Validate required parameters
        if (projectId <= 0)
            return new ApiResponse<string>(false, "Invalid Project ID.", null);
        var result = await _newProjectRepository.DeleteNewProject(projectId);
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }

    public async Task<ApiResponse<string>> InsertUpdatePaymentPlan(InsertUpdatePaymentPlanRequest request)
    {
        // Validate required parameters
        if (request.ProjectId <= 0)
            return new ApiResponse<string>(false, "Invalid Project ID.", null);

        if (string.IsNullOrEmpty(request.PaymentPlanName))
            return new ApiResponse<string>(false, "Payment Plan Name is required.", null);

        // Call the repository method
        var result = await _newProjectRepository.InsertUpdatePaymentPlan(
            request.PaymentPlanId,
            request.ProjectId,
            request.PaymentPlanName,
            request.CreateBy,
            request.UpdateBy
        );
        return new ApiResponse<string>(result.Status, result.Message, result.Data);

    }

    public async Task<ApiResponse<string>> InsertUpdateProjectTimeline(InsertUpdateProjectTimelineRequest request)
    {
        // Validation
        if (request.ProjectId <= 0)
            return new ApiResponse<string>(false, "Invalid Project ID.", null);

        if (request.TimeLineConfigId <= 0)
            return new ApiResponse<string>(false, "Invalid TimeLine Config ID.", null);
        if (request.TimeLineConfigId <= 0)
            return new ApiResponse<string>(false, "Invalid TimeLine Config ID.", null);
        if (request.CreateBy <= 0)
            return new ApiResponse<string>(false, "Invalid Creator ID.", null);


        // Call the repository method for Insert/Update
        var result = await _newProjectRepository.InsertUpdateProjectTimeline(
            request.ProjectTimelineId,
            request.ProjectId,
            request.TimeLineConfigId,
            request.Date,
            request.CreateBy,
            request.UpdateBy
        );

        // Return the result
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }

    public async Task<ApiResponse<string>> InsertUpdateProjectPaymentPlanDetails(
        InsertUpdateProjectPaymentPlanDetailRequest request)
    {
        // Validation
        if (request.PaymentPlanId <= 0)
            return new ApiResponse<string>(false, "Invalid Payment Plan ID.", null);

        if (string.IsNullOrEmpty(request.PlanDescription))
            return new ApiResponse<string>(false, "Plan Description is required.", null);

        if (request.PlanPercentage <= 0 || request.PlanPercentage > 100)
            return new ApiResponse<string>(false, "Invalid Plan Percentage.", null);

        // Call the repository method
        var result = await _newProjectRepository.InsertUpdateProjectPaymentPlanDetail(
            request.ProjectPaymentPlanDetailId,
            request.PaymentPlanId,
            request.PlanDescription,
            request.PlanPercentage,
            request.PlanSubDescription,
            request.CreateBy,
            request.UpdateBy
        );
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }


    public async Task<ApiResponse<string>> InsertUpdateProjectUnit(InsertUpdateProjectUnitRequest request)
    {
        // Validation
        if (request.ProjectId <= 0)
            return new ApiResponse<string>(false, "Invalid Project ID.", null);

        if (request.PropertyTypeId <= 0)
            return new ApiResponse<string>(false, "Invalid Property Type ID.", null);

        if (request.Bed < 0)
            return new ApiResponse<string>(false, "Invalid Bed value.", null);

        if (request.Sqft <= 0)
            return new ApiResponse<string>(false, "Invalid Square Footage.", null);

        if (request.CreateBy <= 0)
            return new ApiResponse<string>(false, "Invalid Creator ID.", null);

        // Call the repository method for Insert/Update
        var result = await _newProjectRepository.InsertUpdateProjectUnit(
            request.ProjectUnitId,
            request.ProjectId,
            request.PropertyTypeId,
            request.Bed,
            request.LayoutType,
            request.Price,
            request.Sqft,
            request.FloorPlanUrl,
            request.CreateBy,
            request.UpdateBy
        );

        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }

    public async Task<ApiResponse<List<GetMediaByProjectIdForAdminPanelDto>>> GetMediaByProjectIdForAdminPanel(int projectId)
    {
        if (projectId <= 0)
            return new ApiResponse<List<GetMediaByProjectIdForAdminPanelDto>>(false, "projectId is required.", null);

        var result = await _newProjectRepository.GetMediaByProjectIdForAdminPanel(projectId);
        return new ApiResponse<List<GetMediaByProjectIdForAdminPanelDto>>(true, "Data has been retrieved. ", GroupAndMapNewProjectMediaForAdminPanel(result));
        
    }

    public async Task<ApiResponse<List<GetUnitByProjectIdForAdminDto>>> GetUnitByProjectIdForAdmin(int projectId) 
    {
        if (projectId <= 0)
            return new ApiResponse<List<GetUnitByProjectIdForAdminDto>>(false, "projectId is required.", null);

        var result = await _newProjectRepository.GetUnitByProjectIdForAdmin(projectId);
        return new ApiResponse<List<GetUnitByProjectIdForAdminDto>>(true, "Data has been retrieved. ", GroupAndMapProjectUnitsForAdminPanel(result));
    }

    public async Task<ApiResponse<List<GetProjectTimelineByProjectIdForAdminDto>>> GetProjectTimelineByProjectIdForAdmin(int projectId)
    {
        if (projectId <= 0)
            return new ApiResponse<List<GetProjectTimelineByProjectIdForAdminDto>>(false, "projectId is required.", null);

        var result = await _newProjectRepository.GetProjectTimelineByProjectIdForAdmin(projectId);
        return new ApiResponse<List<GetProjectTimelineByProjectIdForAdminDto>>(true, "Data has been retrieved. ", GroupAndMapProjectTimeline(result));
    }

    public async Task<ApiResponse<GetProjectsPaginatedDto>> GetProjects(PaginatedRequest request)
    {
        if (request.PageNumber <= 0)
            return new ApiResponse<GetProjectsPaginatedDto>(false, "Page Number is required.", null);
        if (request.PageSize <= 0)
            return new ApiResponse<GetProjectsPaginatedDto>(false, "Page Number is required.", null);
        var result = await _newProjectRepository.GetProjects(request.PageNumber,
            request.PageSize, request.Search, request.OrderColumnIndex, request.OrderDirection);
        return new ApiResponse<GetProjectsPaginatedDto>(true, "Data has been retrieved. ", GroupAndMapProjects(result, request.PageNumber, request.PageSize));
    }
}


public record GetNewProjectsRequest(
    string? Search,
    int? PropertyTypeId,
    int? Bedrooms,
    int? Bathrooms,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? Keywords,
    int? MinArea,
    int? MaxArea,
    string? AmenitiesIDs,
    int? UserId,
    int PageNumber,
    int PageSize,
    string Language = "en"
);

public class InsertUpdateProjectTimelineRequest
{
    public int ProjectTimelineId { get; set; }
    public int ProjectId { get; set; }
    public int TimeLineConfigId { get; set; }
    public DateTime? Date { get; set; }
    public int CreateBy { get; set; }
    public int? UpdateBy { get; set; }
}
public class InsertUpdateNewProjectRequest
{
    public int ProjectId { get; set; }
    public int ProjectStatus { get; set; }
    public int DeveloperId { get; set; }
    public int AreaId { get; set; }
    public string? CommunityName { get; set; }
    public decimal? GovFee { get; set; }
    public string? ShortDescription { get; set; } 
    public string? LongDescription { get; set; } 
    public decimal? LaunchPrice { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? SalesStart { get; set; }
    public int? NumberOfBuildings { get; set; }
    public int? PropertyTypeId { get; set; }
    public int CreateBy { get; set; }
    public int? UpdateBy { get; set; }
}


public class InsertNewProjectMediaRequest
{
    public int NewProjectId { get; set; }
    public int MediaMenuId { get; set; }
    public string? MediaUrl { get; set; }
    public string? MediaDescription { get; set; }
    public string? MediaType { get; set; }
    public int CreateBy { get; set; }
    public int? UpdateBy { get; set; }
}


public class InsertNewProjectsDetailMapRequest
{
    public int ProjectId { get; set; }
    public string? ConfigDetailsList { get; set; }
    public string? FirstValue { get; set; }
    public string? SecondValue { get; set; }
    public int CreateBy { get; set; }
}

public class InsertUpdatePaymentPlanRequest
{
    public int PaymentPlanId { get; set; }
    public int ProjectId { get; set; }
    public string? PaymentPlanName { get; set; }
    public int CreateBy { get; set; }
    public int? UpdateBy { get; set; }
}

public class InsertUpdateProjectPaymentPlanDetailRequest
{
    public int ProjectPaymentPlanDetailId { get; set; }
    public int PaymentPlanId { get; set; }
    public string? PlanDescription { get; set; }
    public decimal PlanPercentage { get; set; }
    public string? PlanSubDescription { get; set; }
    public int CreateBy { get; set; }
    public int? UpdateBy { get; set; }
}




public class InsertUpdateProjectUnitRequest
{
    public int ProjectUnitId { get; set; }
    public int ProjectId { get; set; }
    public int PropertyTypeId { get; set; }
    public int Bed { get; set; }
    public string? LayoutType { get; set; }
    public decimal? Price { get; set; }
    public int Sqft { get; set; }
    public string? FloorPlanUrl { get; set; }
    public int CreateBy { get; set; }
    public int? UpdateBy { get; set; }
}
