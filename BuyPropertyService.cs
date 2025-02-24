using PropertyManagement.Core.DTOs;
using PropertyManagement.Core.DTOs.Shared;
using PropertyManagement.Core.DTOs.Sps;
using PropertyManagement.Core.Models;
using PropertyManagement.Data.Repositories;
namespace PropertyManagement.Business.Services;
public interface IBuyPropertyService
{

    Task<ApiResponse<BuyPropertyPaginatedDto>> GetBuyProperties(GetBuyPropertiesRequest request);
    Task<ApiResponse<GetBuyPropertyDetailDto>> GetBuyPropertyDetails(int buyPropertyId, string language);
    Task<ApiResponse<string>> InsertUpdateBuyProperty(BuyPropertyRequest request);
    Task<ApiResponse<string>> InsertBuyPropertyDetailMap(InsertBuyPropertyDetailMapRequest request);
    Task<ApiResponse<string>> InsertBuyPropertyMedia(InsertBuyPropertyMediaRequest request);
    Task<ApiResponse<string>> InsertPropertyPurchaseMortgageCosts(PropertyPurchaseMortgageCostRequest request);
    Task<ApiResponse<string>> InsertPropertyPurchaseCostsCash(PropertyPurchaseCostsCashRequest request);
    Task<ApiResponse<GetBuyPropertiesForPanelPaginatedDto>> GetBuyPropertiesForPanel(PaginatedRequest request);

    Task<ApiResponse<List<GetMediaByBuyPropertyIdForAdminPanelDto>>> GetMediaByBuyPropertyIdForAdminPanel(int propertyId);

    Task<ApiResponse<List<GetPropertyPurchaseCostsCashByBuyPropertyIdForPanelDto>>> GetPropertyPurchaseCostsCashByBuyPropertyId(int propertyId);

    Task<ApiResponse<List<GetPropertyPurchaseMortgageCostsByBuyPropertyIdForPanelDto>>> GetPropertyPurchaseMortgageCostsByBuyPropertyIdForPanel(int propertyId);
}

public class BuyPropertyService : IBuyPropertyService
{
    private readonly IBuyPropertyRepository _buyPropertyRepository;


    public BuyPropertyService(IBuyPropertyRepository buyPropertyRepository)
    {
        _buyPropertyRepository = buyPropertyRepository;
    }


    private GetBuyPropertiesForPanelPaginatedDto GroupAndMapBuyProperties(List<GetBuyPropertiesForPanel> properties, int currentPage, int pageSize)
    {
        var paginatedData = properties.Select(p => new GetBuyPropertiesForPanelDto
        {
            BuyPropertyId = p.BuyPropertyId,
            ShortDescription = p.ShortDescription,
            LongDescription = p.LongDescription,
            BuildingName = p.BuildingName,
            AreaId = p.AreaId,
            Longitude = p.Longitude,
            Latitude = p.Latitude,
            SquareFeet = p.SquareFeet,
            SquareMeter = p.SquareMeter,
            BedRooms = p.BedRooms,
            HaveMadeRoom = p.HaveMadeRoom,
            Bathrooms = p.Bathrooms,
            PropertyTypeId = p.PropertyTypeId,
            IsActive = p.IsActive,
            IsApproved = p.IsApproved,
            ApprovedBy = p.ApprovedBy,
            CreateBy = p.CreateBy,
            CreatedAt = p.CreatedAt,
            UpdateBy = p.UpdateBy,
            UpdatedAt = p.UpdatedAt
        }).ToList();

        return new GetBuyPropertiesForPanelPaginatedDto
        {
            Pagination = new PaginationDto
            {
                TotalPages = properties.FirstOrDefault()?.TotalPages ?? 0,
                TotalRecords = properties.FirstOrDefault()?.RecordsFiltered ?? 0,
                FilteredRecords = properties.FirstOrDefault()?.RecordsFiltered ?? 0,
                CurrentPage = currentPage,
                PageSize = pageSize
            },
            Data = paginatedData
        };
    }


    private BuyPropertyPaginatedDto? GroupAndMapProperties(List<GetBuyProperties> properties, List<GetBuyPropertiesAssociatedCounts> counts, int currentPage, int pageSize)
    {

        var type = new List<GetBuyPropertiesAssociatedCountsDto>();
        var cities = new List<GetBuyPropertiesAssociatedCountsDto>();


        foreach (var item in counts)
        {
            if (item.Type == "property_type")
                type.Add(new GetBuyPropertiesAssociatedCountsDto
                {
                    Value = item.Value,
                    Id = item.Id,
                    Count = item.Count
                });
            else
                cities.Add(new GetBuyPropertiesAssociatedCountsDto
                {
                    Value = item.Value,
                    Id = item.Id,
                    Count = item.Count
                });
        }


        // Group the properties by BuyPropertyId
        var groupedProperties = properties
        .GroupBy(p => p.BuyPropertyId)
        .Select(group => new BuyPropertyPaginatedDto
        {
            // Assuming TotalPages and TotalRecords are the same for all properties in the group

            PropertyTypeCounts = type,
            CityCounts = cities,
            Pagination = new PaginationDto
            {
                TotalPages = group.FirstOrDefault()?.TotalPages ?? 0,
                TotalRecords = group.FirstOrDefault()?.TotalRecords ?? 0,
                CurrentPage = currentPage,
                PageSize = pageSize

            },

            // Mapping Data as a list of BuyPropertyDto for each group (for each unique BuyPropertyId)
            Data = group.Select(p => new BuyPropertyDto
            {
                BuyPropertyId = p.BuyPropertyId,
                // Mapping PropertyDetails
                PropertyDetails = new BuyPropertyDetailsDto
                {

                    PropertyType = p.PropertyType,
                    Price = p.Price,
                    ShortDescription = p.ShortDescription,
                    Location = p.Location,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,
                    SquareFeet = p.SquareFeet,
                    SquareMeter = p.SquareMeter,
                    Listed = p.Listed
                },
                // Mapping BrokerInfo for each property
                BrokerInfo = new BrokerInfoDto
                {
                    BrokerName = p.BrokerName,
                    BrokerLogo = p.BrokerLogo,
                    Phone = p.Phone,
                    WhatsApp = p.WhatsApp,
                    Email = p.Email
                },
                // Mapping MediaInfo for each individual property
                MediaInfo = group.Where(m => m.BuyPropertyId == p.BuyPropertyId)  // Filter media for this specific property
                    .Select(m => new MediaInfoDto
                    {
                        MediaId = m.BuyPropertyMediaId,
                        MediaType = m.MediaType,
                        MediaUrl = m.MediaUrl,
                        MediaMenu = m.MediaMenu
                    }).ToList()
            }).DistinctBy(x => x.BuyPropertyId).ToList()
        })
        .ToList();

        // Return the first grouped element (or adjust this logic to return all groups, depending on the scenario)
        return groupedProperties.DistinctBy(x => x.Data).SingleOrDefault();  // You can modify this based on pagination or other logic
    }


    private GetBuyPropertyDetailDto? GroupAndMapPropertyDetails(List<GetBuyPropertyDetails> properties, List<GetBuyPropertyPurchaseCostsCash> cash, List<GetBuyPropertyPurchaseMortgageCosts> mortgage)
    {
        // Group the properties by BuyPropertyId
        var groupedProperties = properties
            .GroupBy(p => p.BuyPropertyId)
            .Select(group => new GetBuyPropertyDetailDto
            {
                BuyPropertyId = group.Key, // group.Key represents the BuyPropertyId

                // Mapping PropertyDetails (taking the first item from the group)
                PropertyDetails = new GetBuyPropertyDetailOfDetailsDto()
                {
                    PropertyType = group.First().PropertyType,
                    ShortDescription = group.First().ShortDescription,
                    LongDescription = group.First().LongDescription,
                    Location = group.First().Location,
                    Bedrooms = group.First().Bedrooms,
                    Bathrooms = group.First().Bathrooms,
                    SquareFeet = group.First().SquareFeet,
                    SquareMeter = group.First().SquareMeter,
                    Listed = group.First().Listed
                },
                Amenities = group.Select(m => new KeyValueString()
                {
                    Key = m.Value,
                    Value = m.ImageUrl,
                }).DistinctBy(x => x.Key).ToList(),
                BuyPropertyPurchaseCostsCashDto = new GetBuyPropertyPurchaseCostsCashDto()
                {
                    AgencyFeePercentage = cash.First().AgencyFeePercentage,
                    AgencyFeeVatPercentage = cash.First().AgencyFeeVatPercentage,
                    BuyPropertyId = cash.First().BuyPropertyId,
                    CashCostId = cash.First().CashCostId,
                    ConveyancerFee = cash.First().ConveyancerFee,
                    LandDeptFeePercentage = cash.First().LandDeptFeePercentage,
                    PurchasePrice = cash.First().PurchasePrice,
                    TotalCost = cash.First().TotalCost,
                    TrusteeFee = cash.First().TrusteeFee
                },
                BuyPropertyPurchaseMortgageCostsDto = new GetBuyPropertyPurchaseMortgageCostsDto()
                {
                    AgencyFee = mortgage.First().AgencyFee,
                    AmountRequiredUpfront = mortgage.First().AmountRequiredUpfront,
                    BankArrangementFee = mortgage.First().BankArrangementFee,
                    BuyPropertyId = mortgage.First().BuyPropertyId,
                    DownPayment = mortgage.First().DownPayment,
                    LandDeptFee = mortgage.First().LandDeptFee,
                    MortgageCostId = mortgage.First().MortgageCostId,
                    MortgageRegistrationFee = mortgage.First().MortgageRegistrationFee,
                    PurchasePrice = mortgage.First().PurchasePrice,
                    TrusteeFee = mortgage.First().TrusteeFee,
                    ValuationFee = mortgage.First().ValuationFee

                },
                // Mapping BrokerInfo (taking the first item from the group)
                BrokerInfo = new BrokerInfoDto
                {
                    BrokerName = group.First().BrokerName,
                    BrokerLogo = group.First().BrokerLogo,
                    Phone = group.First().Phone,
                    WhatsApp = group.First().WhatsApp,
                    Email = group.First().Email
                },
                AgentInfoDto = new AgentInfoDto()
                {
                    Name = group.First().Name,
                    ProfilePicture = group.First().ProfilePicture,
                    Phone = group.First().Phone,
                    WhatsApp = group.First().WhatsApp,
                    Email = group.First().Email
                },
                // Mapping MediaInfo (selecting all media for this group)
                MediaInfo = group.Select(m => new MediaInfoDto
                {
                    MediaId = m.BuyPropertyMediaId,
                    MediaType = m.MediaType,
                    MediaUrl = m.MediaUrl,
                    MediaMenu = m.MediaMenu
                }).DistinctBy(x => x.MediaId).ToList()
            })
            .ToList();

        return groupedProperties.SingleOrDefault();
    }

    private List<GetMediaByBuyPropertyIdForAdminPanelDto> GroupAndMapBuyPropertyMediaForAdminPanel(List<GetMediaByBuyPropertyIdForAdminPanel> mediaList)
    {
        var paginatedData = mediaList.Select(m => new GetMediaByBuyPropertyIdForAdminPanelDto
        {
            BuyPropertyMediaId = m.BuyPropertyMediaId,
            BuyPropertyId = m.BuyPropertyId,
            MediaMenuId = m.MediaMenuId,
            MediaUrl = m.MediaUrl,
            MediaType = m.MediaType,
            CreateBy = m.CreateBy,
            CreatedAt = m.CreatedAt
        }).ToList();

        return paginatedData;
    }

    private List<GetPropertyPurchaseMortgageCostsByBuyPropertyIdForPanelDto> MapMortgageCosts(List<GetPropertyPurchaseMortgageCostsByBuyPropertyIdForPanel> mortgageCostsList)
    {
        return mortgageCostsList.Select(m => new GetPropertyPurchaseMortgageCostsByBuyPropertyIdForPanelDto
        {
            MortgageCostId = m.MortgageCostId,
            BuyPropertyId = m.BuyPropertyId,
            PurchasePrice = m.PurchasePrice,
            DownPayment = m.DownPayment,
            LandDeptFee = m.LandDeptFee,
            TrusteeFee = m.TrusteeFee,
            MortgageRegistrationFee = m.MortgageRegistrationFee,
            AgencyFee = m.AgencyFee,
            BankArrangementFee = m.BankArrangementFee,
            ValuationFee = m.ValuationFee,
            AmountRequiredUpfront = m.AmountRequiredUpfront,
            CreatedBy = m.CreatedBy,
            CreatedAt = m.CreatedAt,
            UpdatedBy = m.UpdatedBy,
            UpdatedAt = m.UpdatedAt
        }).ToList();
    }


    private List<GetPropertyPurchaseCostsCashByBuyPropertyIdForPanelDto> GroupAndMapPropertyPurchaseCostsCash(List<GetPropertyPurchaseCostsCashByBuyPropertyIdForPanel> cashCostList)
    {
        var mappedData = cashCostList.Select(c => new GetPropertyPurchaseCostsCashByBuyPropertyIdForPanelDto
        {
            CashCostId = c.CashCostId,
            BuyPropertyId = c.BuyPropertyId,
            PurchasePrice = c.PurchasePrice,
            LandDeptFeePercentage = c.LandDeptFeePercentage,
            AgencyFeePercentage = c.AgencyFeePercentage,
            AgencyFeeVatPercentage = c.AgencyFeeVatPercentage,
            TrusteeFee = c.TrusteeFee,
            ConveyancerFee = c.ConveyancerFee,
            TotalCost = c.TotalCost,
            CreatedBy = c.CreatedBy,
            CreatedAt = c.CreatedAt,
            UpdatedBy = c.UpdatedBy,
            UpdatedAt = c.UpdatedAt
        }).ToList();

        return mappedData;
    }



    public async Task<ApiResponse<List<GetPropertyPurchaseCostsCashByBuyPropertyIdForPanelDto>>> GetPropertyPurchaseCostsCashByBuyPropertyId(int propertyId)
    {
        if (propertyId <= 0)
            return new ApiResponse<List<GetPropertyPurchaseCostsCashByBuyPropertyIdForPanelDto>>(false, "projectId is required.", null);

        var result = await _buyPropertyRepository.GetPropertyPurchaseCostsCashByBuyPropertyIdForPanel(propertyId);
        return new ApiResponse<List<GetPropertyPurchaseCostsCashByBuyPropertyIdForPanelDto>>(true, "Data has been retrieved. ", GroupAndMapPropertyPurchaseCostsCash(result));

    }

    public async Task<ApiResponse<List<GetPropertyPurchaseMortgageCostsByBuyPropertyIdForPanelDto>>> GetPropertyPurchaseMortgageCostsByBuyPropertyIdForPanel(int propertyId)
    {
        if (propertyId <= 0)
            return new ApiResponse<List<GetPropertyPurchaseMortgageCostsByBuyPropertyIdForPanelDto>>(false, "projectId is required.", null);

        var result = await _buyPropertyRepository.GetPropertyPurchaseMortgageCostsByBuyPropertyIdForPanel(propertyId);
        return new ApiResponse<List<GetPropertyPurchaseMortgageCostsByBuyPropertyIdForPanelDto>>(true, "Data has been retrieved. ", MapMortgageCosts(result));

    }
    

    public async Task<ApiResponse<List<GetMediaByBuyPropertyIdForAdminPanelDto>>> GetMediaByBuyPropertyIdForAdminPanel(int propertyId)
    {
        if (propertyId <= 0)
            return new ApiResponse<List<GetMediaByBuyPropertyIdForAdminPanelDto>>(false, "projectId is required.", null);

        var result = await _buyPropertyRepository.GetMediaByBuyPropertyIdForAdminPanel(propertyId);
        return new ApiResponse<List<GetMediaByBuyPropertyIdForAdminPanelDto>>(true, "Data has been retrieved. ", GroupAndMapBuyPropertyMediaForAdminPanel(result));

    }

    public async Task<ApiResponse<BuyPropertyPaginatedDto>> GetBuyProperties(GetBuyPropertiesRequest request)
    {
        if (request.PageNumber <= 0)
            return new ApiResponse<BuyPropertyPaginatedDto>(false, "Page Number is required.", null);
        if (request.PageSize <= 0)
            return new ApiResponse<BuyPropertyPaginatedDto>(false, "Page Number is required.", null);

        var data = await _buyPropertyRepository.GetBuyProperties(request.Search, request.PropertyTypeId,
            request.Bedrooms, request.Bathrooms, request.MinPrice, request.MaxPrice, request.Keywords, request.MinArea,
            request.MaxArea, request.AmenitiesIDs, request.UserId, request.Language, request.PageNumber, request.PageSize);

        var counts = await _buyPropertyRepository.GetBuyPropertiesAssociatedCounts(request.Search, request.PropertyTypeId,
            request.Bedrooms, request.Bathrooms, request.MinPrice, request.MaxPrice, request.Keywords, request.MinArea,
            request.MaxArea, request.AmenitiesIDs, request.Language);

        return new ApiResponse<BuyPropertyPaginatedDto>(true, "Data has been retrieved . ", GroupAndMapProperties(data, counts, request.PageNumber, request.PageSize));
    }


    public async Task<ApiResponse<string>> InsertUpdateBuyProperty(BuyPropertyRequest request)
    {
        // Validation
        if (request.BuyPropertyId < 0)
            return new ApiResponse<string>(false, "Invalid Buy Property ID.", null);
        if (string.IsNullOrEmpty(request.ShortDescription))
            return new ApiResponse<string>(false, "Short Description is required.", null);
        if (string.IsNullOrEmpty(request.LongDescription))
            return new ApiResponse<string>(false, "Long Description is required.", null);
        if (string.IsNullOrEmpty(request.BuildingName))
            return new ApiResponse<string>(false, "Building Name is required.", null);
        if (request.AreaId <= 0)
            return new ApiResponse<string>(false, "Invalid Area ID.", null);
        if (request.SquareFeet <= 0)
            return new ApiResponse<string>(false, "Square Feet must be greater than 0.", null);
        if (request.SquareMeter <= 0)
            return new ApiResponse<string>(false, "Square Meter must be greater than 0.", null);
        if (request.Bedrooms <= 0)
            return new ApiResponse<string>(false, "Bedrooms must be greater than 0.", null);
        if (request.Bathrooms <= 0)
            return new ApiResponse<string>(false, "Bathrooms must be greater than 0.", null);
        if (request.PropertyTypeId <= 0)
            return new ApiResponse<string>(false, "Invalid Property Type ID.", null);

        // Call Repository to execute stored procedure
        var result = await _buyPropertyRepository.InsertUpdateBuyProperty(
            request.BuyPropertyId,
            request.ShortDescription,
            request.LongDescription,
            request.BuildingName,
            request.AreaId,
            request.Longitude,
            request.Latitude,
            request.SquareFeet,
            request.SquareMeter,
            request.Bedrooms,
            request.HaveMadeRoom,
            request.Bathrooms,
            request.PropertyTypeId,
            request.IsActive,
            request.CreateBy,
            request.UpdateBy
        );

        // Return API response
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }



    public async Task<ApiResponse<GetBuyPropertyDetailDto>> GetBuyPropertyDetails(int buyPropertyId, string language)
    {
        if (buyPropertyId <= 0)
            return new ApiResponse<GetBuyPropertyDetailDto>(false, "Property Id is required.", null);
        var data = await _buyPropertyRepository.GetBuyPropertyDetails(buyPropertyId, language);

        var cash = await _buyPropertyRepository.GetBuyPropertyPurchaseCostsCash(buyPropertyId);
        var mortgage = await _buyPropertyRepository.GetBuyPropertyPurchaseMortgageCosts(buyPropertyId);

        var result = GroupAndMapPropertyDetails(data, cash, mortgage);
        return new ApiResponse<GetBuyPropertyDetailDto>(true, "Data has been retrieved . ", result);
    }


    public async Task<ApiResponse<string>> InsertBuyPropertyDetailMap(InsertBuyPropertyDetailMapRequest request)
    {
        // Validate required parameters
        if (request.BuyPropertyId <= 0)
            return new ApiResponse<string>(false, "Invalid BuyPropertyId ID.", null);

        if (string.IsNullOrEmpty(request.ConfigDetailsList))
            return new ApiResponse<string>(false, "ConfigDetailsList is required.", null);

        // Call repository method
        var result = await _buyPropertyRepository.InsertBuyPropertyDetailMap(
            request.BuyPropertyId,
            request.ConfigDetailsList,
            request.FirstValue,
            request.SecondValue,
            request.CreateBy
        );
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }


    public async Task<ApiResponse<string>> InsertPropertyPurchaseMortgageCosts(PropertyPurchaseMortgageCostRequest request)
    {
        // Validation
        if (request.BuyPropertyId <= 0)
            return new ApiResponse<string>(false, "Invalid Buy Property ID.", null);
        if (request.PurchasePrice <= 0)
            return new ApiResponse<string>(false, "Purchase Price must be greater than 0.", null);
        if (request.DownPayment < 0)
            return new ApiResponse<string>(false, "Down Payment cannot be negative.", null);
        if (request.LandDeptFee < 0)
            return new ApiResponse<string>(false, "Land Department Fee cannot be negative.", null);
        if (request.TrusteeFee < 0)
            return new ApiResponse<string>(false, "Trustee Fee cannot be negative.", null);
        if (request.MortgageRegistrationFee < 0)
            return new ApiResponse<string>(false, "Mortgage Registration Fee cannot be negative.", null);
        if (request.AgencyFee < 0)
            return new ApiResponse<string>(false, "Agency Fee cannot be negative.", null);
        if (request.BankArrangementFee < 0)
            return new ApiResponse<string>(false, "Bank Arrangement Fee cannot be negative.", null);
        if (request.ValuationFee < 0)
            return new ApiResponse<string>(false, "Valuation Fee cannot be negative.", null);
        if (request.AmountRequiredUpfront < 0)
            return new ApiResponse<string>(false, "Amount Required Upfront cannot be negative.", null);

        // Call repository method
        var result = await _buyPropertyRepository.InsertPropertyPurchaseMortgageCosts(
            request.BuyPropertyId,
            request.PurchasePrice,
            request.DownPayment,
            request.LandDeptFee,
            request.TrusteeFee,
            request.MortgageRegistrationFee,
            request.AgencyFee,
            request.BankArrangementFee,
            request.ValuationFee,
            request.AmountRequiredUpfront,
            request.CreatedBy
        );

        // Return response
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }



    public async Task<ApiResponse<string>> InsertBuyPropertyMedia(InsertBuyPropertyMediaRequest request)
    {
        // Validation
        if (request.BuyPropertyId < 0)
            return new ApiResponse<string>(false, "Invalid Buy Project ID.", null);
        if (request.MediaMenuId < 0)
            return new ApiResponse<string>(false, "Invalid Media Menu ID.", null);
        if (string.IsNullOrEmpty(request.MediaUrl))
            return new ApiResponse<string>(false, "Media URL is required.", null);
        if (string.IsNullOrEmpty(request.MediaType))
            return new ApiResponse<string>(false, "Media Type is required.", null);

        // Calling the repository method
        var result = await _buyPropertyRepository.InsertBuyPropertyMedia(
            request.BuyPropertyId,
            request.MediaMenuId,
            request.MediaUrl,
            request.MediaType,
            request.CreateBy
        );


        // Return response based on result
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }



    public async Task<ApiResponse<string>> InsertPropertyPurchaseCostsCash(PropertyPurchaseCostsCashRequest request)
    {
        // Validation
        if (request.BuyPropertyId <= 0)
            return new ApiResponse<string>(false, "Invalid Buy Property ID.", null);
        if (request.PurchasePrice <= 0)
            return new ApiResponse<string>(false, "Purchase Price must be greater than 0.", null);
        if (request.LandDeptFeePercentage < 0)
            return new ApiResponse<string>(false, "Land Department Fee Percentage cannot be negative.", null);
        if (request.AgencyFeePercentage < 0)
            return new ApiResponse<string>(false, "Agency Fee Percentage cannot be negative.", null);
        if (request.AgencyFeeVatPercentage < 0)
            return new ApiResponse<string>(false, "Agency Fee VAT Percentage cannot be negative.", null);
        if (request.TrusteeFee < 0)
            return new ApiResponse<string>(false, "Trustee Fee cannot be negative.", null);
        if (request.ConveyancerFee < 0)
            return new ApiResponse<string>(false, "Conveyancer Fee cannot be negative.", null);

        // Call repository method
        var result = await _buyPropertyRepository.InsertPropertyPurchaseCostsCash(
            request.BuyPropertyId,
            request.PurchasePrice,
            request.LandDeptFeePercentage,
            request.AgencyFeePercentage,
            request.AgencyFeeVatPercentage,
            request.TrusteeFee,
            request.ConveyancerFee,
            request.CreatedBy
        );

        // Return response
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }


    public async Task<ApiResponse<GetBuyPropertiesForPanelPaginatedDto>> GetBuyPropertiesForPanel(PaginatedRequest request)
    {
        if (request.PageNumber <= 0)
            return new ApiResponse<GetBuyPropertiesForPanelPaginatedDto>(false, "Page Number is required.", null);
        if (request.PageSize <= 0)
            return new ApiResponse<GetBuyPropertiesForPanelPaginatedDto>(false, "Page Number is required.", null);
        var result = await _buyPropertyRepository.GetBuyPropertiesForPanel(request.PageNumber,
            request.PageSize, request.Search, request.OrderColumnIndex, request.OrderDirection);
  
        return new ApiResponse<GetBuyPropertiesForPanelPaginatedDto>(true, "Data has been retrieved. ", GroupAndMapBuyProperties(result, request.PageNumber, request.PageSize));
    }

}

public class BuyPropertyRequest
{
    public int BuyPropertyId { get; set; }
    public string ShortDescription { get; set; } = string.Empty;
    public string LongDescription { get; set; } = string.Empty;
    public string BuildingName { get; set; } = string.Empty;
    public int AreaId { get; set; }
    public decimal? Longitude { get; set; }
    public decimal? Latitude { get; set; }
    public int SquareFeet { get; set; }
    public int SquareMeter { get; set; }
    public int Bedrooms { get; set; }
    public bool HaveMadeRoom { get; set; }
    public int Bathrooms { get; set; }
    public int PropertyTypeId { get; set; }
    public bool IsActive { get; set; }
    public int CreateBy { get; set; }
    public int? UpdateBy { get; set; }
}
public record GetBuyPropertiesRequest(
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



public class InsertBuyPropertyDetailMapRequest
{
    public int BuyPropertyId { get; set; }
    public string? ConfigDetailsList { get; set; }
    public string? FirstValue { get; set; }
    public string? SecondValue { get; set; }
    public int CreateBy { get; set; }
}


public class InsertBuyPropertyMediaRequest
{
    public int BuyPropertyId { get; set; }
    public int MediaMenuId { get; set; }
    public string? MediaUrl { get; set; }
    public string? MediaType { get; set; }
    public int CreateBy { get; set; }
}



public class PropertyPurchaseMortgageCostRequest
{
    public int BuyPropertyId { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal DownPayment { get; set; }
    public decimal LandDeptFee { get; set; }
    public decimal TrusteeFee { get; set; }
    public decimal MortgageRegistrationFee { get; set; }
    public decimal AgencyFee { get; set; }
    public decimal BankArrangementFee { get; set; }
    public decimal ValuationFee { get; set; }
    public decimal AmountRequiredUpfront { get; set; }
    public int CreatedBy { get; set; }
}
public class PropertyPurchaseCostsCashRequest
{
    public int BuyPropertyId { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal LandDeptFeePercentage { get; set; }
    public decimal AgencyFeePercentage { get; set; }
    public decimal AgencyFeeVatPercentage { get; set; }
    public decimal TrusteeFee { get; set; }
    public decimal ConveyancerFee { get; set; }
    public int CreatedBy { get; set; }
}
