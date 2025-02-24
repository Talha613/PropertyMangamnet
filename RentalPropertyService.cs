using PropertyManagement.Core.DTOs;
using PropertyManagement.Core.DTOs.Shared;
using PropertyManagement.Core.DTOs.Sps;
using PropertyManagement.Core.Models;
using PropertyManagement.Data.Repositories;
namespace PropertyManagement.Business.Services;
public interface IRentalPropertyService
{
    Task<ApiResponse<RentalPropertyPaginatedDto>> GetRentalProperties(GetRentalPropertiesRequest request);
    Task<ApiResponse<GetRentPropertyDetailDto>> GetRentalPropertyDetails(int rentalPropertyId, string language);
    Task<ApiResponse<string>> InsertUpdateRentalProperty(RentalPropertyRequest request);
    Task<ApiResponse<string>> InsertRentalPropertyMedia(InsertRentalPropertyMediaRequest request);
    Task<ApiResponse<string>> InsertRentalPropertyDetailMap(InsertRentalPropertyDetailMapRequest request);
    Task<ApiResponse<string>> InsertRentalPropertyCosts(InsertRentalPropertyCostsRequest request);
}

public class RentalPropertyService : IRentalPropertyService
{
    private readonly IRentalPropertyRepository _rentalPropertyRepository;


    public RentalPropertyService(IRentalPropertyRepository rentalPropertyRepository)
    {
        _rentalPropertyRepository = rentalPropertyRepository;
    }

    private RentalPropertyPaginatedDto? GroupAndMapProperties(List<GetRentalProperties> properties, List<GetRentalPropertiesAssociatedCounts> counts, int currentPage, int pageSize)
    {

        var type = new List<GetRentalPropertiesAssociatedCountsDto>();
        var cities = new List<GetRentalPropertiesAssociatedCountsDto>();

        foreach (var item in counts)
        {
            if (item.Type == "property_type")
                type.Add(new GetRentalPropertiesAssociatedCountsDto
                {
                    Value = item.Value,
                    Id = item.Id,
                    Count = item.Count
                });
            else
                cities.Add(new GetRentalPropertiesAssociatedCountsDto
                {
                    Value = item.Value,
                    Id = item.Id,
                    Count = item.Count
                });
        }
        // Group the properties by BuyPropertyId
        var groupedProperties = properties
        .GroupBy(p => p.RentalPropertyId)
        .Select(group => new RentalPropertyPaginatedDto
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
            Data = group.Select(p => new RentalPropertyDto
            {
                RentalPropertyId = p.RentalPropertyId,
                // Mapping PropertyDetails
                PropertyDetails = new RentalPropertyDetailsDto
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
                MediaInfo = group.Where(m => m.RentalPropertyId == p.RentalPropertyId)  // Filter media for this specific property
                    .Select(m => new MediaInfoDto
                    {
                        MediaId = m.RentalPropertyMediaId,
                        MediaType = m.MediaType,
                        MediaUrl = m.MediaUrl,
                        MediaMenu = m.MediaMenu
                    }).ToList()
            }).DistinctBy(x => x.RentalPropertyId).ToList()
        })
        .ToList();

        // Return the first grouped element (or adjust this logic to return all groups, depending on the scenario)
        return groupedProperties.DistinctBy(x => x.Data).SingleOrDefault();  // You can modify this based on pagination or other logic
    }




    private GetRentPropertyDetailDto? GroupAndMapPropertyDetails(List<GetRentalPropertyDetails> properties, List<GetRentalPropertyCosts> cost)
    {
        // Group the properties by BuyPropertyId
        var groupedProperties = properties
            .GroupBy(p => p.RentalPropertyId)
            .Select(group => new GetRentPropertyDetailDto
            {
                RentalPropertyId = group.Key, // group.Key represents the BuyPropertyId

                // Mapping PropertyDetails (taking the first item from the group)
                PropertyDetails = new RentPropertyDetailOfDetailsDto()
                {
                    PropertyType = group.First().PropertyType,
                    ShortDescription = group.First().ShortDescription,
                    LongDescription = group.First().LongDescription,
                    Location = group.First().Location,
                    AvailableFrom = group.First().AvailableFrom,
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
                GetRentalPropertyCosts = new RentalPropertyCostsDto()
                {
                    UpfrontCostId = cost.First().UpfrontCostId,
                    RentPropertyId = cost.First().RentPropertyId,
                    AnnualRent = cost.First().AnnualRent,
                    AgencyFeePercentage = cost.First().AgencyFeePercentage,
                    AgencyFeeVatPercentage = cost.First().AgencyFeeVatPercentage,
                    SecurityDeposit = cost.First().SecurityDeposit,
                    DewaDeposit = cost.First().DewaDeposit,
                    EjariFee = cost.First().EjariFee,
                    TotalUpfrontCosts = cost.First().TotalUpfrontCosts
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
                    MediaId = m.RentalPropertyMediaId,
                    MediaType = m.MediaType,
                    MediaUrl = m.MediaUrl,
                    MediaMenu = m.MediaMenu
                }).DistinctBy(x => x.MediaId).ToList()
            })
            .ToList();

        return groupedProperties.SingleOrDefault();
    }

    public async Task<ApiResponse<RentalPropertyPaginatedDto>> GetRentalProperties(GetRentalPropertiesRequest request)
    {
        if (request.PageNumber <= 0)
            return new ApiResponse<RentalPropertyPaginatedDto>(false, "Page Number is required.", null);
        if (request.PageSize <= 0)
            return new ApiResponse<RentalPropertyPaginatedDto>(false, "Page Number is required.", null);

        var data = await _rentalPropertyRepository.GetRentalProperties(request.Search, request.PropertyTypeId,
            request.Bedrooms, request.Bathrooms, request.MinPrice, request.MaxPrice, request.Keywords, request.MinArea,
            request.MaxArea, request.AmenitiesIDs, request.UserId, request.Language, request.PageNumber, request.PageSize);

        var counts = await _rentalPropertyRepository.GetRentalPropertiesAssociatedCounts(request.Search, request.PropertyTypeId,
            request.Bedrooms, request.Bathrooms, request.MinPrice, request.MaxPrice, request.Keywords, request.MinArea,
            request.MaxArea, request.AmenitiesIDs, request.Language);


        return new ApiResponse<RentalPropertyPaginatedDto>(true, "Data has been retrieved . ", GroupAndMapProperties(data, counts, request.PageNumber, request.PageSize));
    }

    public async Task<ApiResponse<GetRentPropertyDetailDto>> GetRentalPropertyDetails(int rentalPropertyId, string language)
    {
        if (rentalPropertyId <= 0)
            return new ApiResponse<GetRentPropertyDetailDto>(false, "Property Id is required.", null);
        var data = await _rentalPropertyRepository.GetRentalPropertyDetails(rentalPropertyId, language);

        var cost = await _rentalPropertyRepository.GetRentalPropertyCosts(rentalPropertyId);

        return new ApiResponse<GetRentPropertyDetailDto>(true, "Data has been retrieved . ", GroupAndMapPropertyDetails(data, cost));


    }



    public async Task<ApiResponse<string>> InsertUpdateRentalProperty(RentalPropertyRequest request)
    {
        // Validation
        if (request.RentalPropertyId < 0)
            return new ApiResponse<string>(false, "Invalid Rental Property ID.", null);
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
        var result = await _rentalPropertyRepository.InsertUpdateRentalProperty(
            request.RentalPropertyId,
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
            request.AvailableFrom,
            request.IsActive,
            request.CreateBy,
            request.UpdateBy
        );

        // Return API response
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }




    public async Task<ApiResponse<string>> InsertRentalPropertyMedia(InsertRentalPropertyMediaRequest request)
    {
        // Validation
        if (request.RentalPropertyId < 0)
            return new ApiResponse<string>(false, "Invalid Rental Project ID.", null);
        if (request.MediaMenuId < 0)
            return new ApiResponse<string>(false, "Invalid Media Menu ID.", null);
        if (string.IsNullOrEmpty(request.MediaUrl))
            return new ApiResponse<string>(false, "Media URL is required.", null);
        if (string.IsNullOrEmpty(request.MediaType))
            return new ApiResponse<string>(false, "Media Type is required.", null);

        // Calling the repository method
        var result = await _rentalPropertyRepository.InsertRentalPropertyMedia(
            request.RentalPropertyId,
            request.MediaMenuId,
            request.MediaUrl,
            request.MediaType,
            request.CreateBy
        );


        // Return response based on result
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }


    public async Task<ApiResponse<string>> InsertRentalPropertyDetailMap(InsertRentalPropertyDetailMapRequest request)
    {
        // Validate required parameters
        if (request.RentalPropertyId <= 0)
            return new ApiResponse<string>(false, "Invalid RentalProperty ID.", null);

        if (string.IsNullOrEmpty(request.ConfigDetailsList))
            return new ApiResponse<string>(false, "ConfigDetailsList is required.", null);

        // Call repository method
        var result = await _rentalPropertyRepository.InsertRentalPropertyDetailMap(
            request.RentalPropertyId,
            request.ConfigDetailsList,
            request.FirstValue,
            request.SecondValue,
            request.CreateBy
        );
        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }




    public async Task<ApiResponse<string>> InsertRentalPropertyCosts(InsertRentalPropertyCostsRequest request)
    {
        // Validation
        if (request.RentPropertyId <= 0)
        {
            return new ApiResponse<string>(false, "Invalid Rent Property ID.", null);
        }

        if (request.AnnualRent <= 0)
        {
            return new ApiResponse<string>(false, "Annual Rent must be greater than zero.", null);
        }

        if (request.AgencyFeePercentage < 0 || request.AgencyFeeVatPercentage < 0)
        {
            return new ApiResponse<string>(false, "Agency Fee and VAT Percentage cannot be negative.", null);
        }

        if (request.AgencyFeePercentage < 0 || request.AgencyFeeVatPercentage < 0 || request.EjariFee < 0)
        {
            return new ApiResponse<string>(false, "Deposit and fees must be non-negative.", null);
        }

        var result = await _rentalPropertyRepository.InsertRentalPropertyCosts(
            request.RentPropertyId,
            request.AnnualRent,
            request.AgencyFeePercentage,
            request.AgencyFeeVatPercentage,
            request.SecurityDeposit,
            request.DewADeposit,
            request.EjariFee,
            request.CreatedBy
        );



        return new ApiResponse<string>(result.Status, result.Message, result.Data);
    }




}
public class InsertRentalPropertyCostsRequest
{
    public int RentPropertyId { get; set; }
    public decimal AnnualRent { get; set; }
    public decimal AgencyFeePercentage { get; set; }
    public decimal AgencyFeeVatPercentage { get; set; }
    public decimal SecurityDeposit { get; set; }
    public decimal DewADeposit { get; set; }
    public decimal EjariFee { get; set; }
    public int CreatedBy { get; set; }
}

public class InsertRentalPropertyDetailMapRequest
{
    public int RentalPropertyId { get; set; }
    public string? ConfigDetailsList { get; set; }
    public string? FirstValue { get; set; }
    public string? SecondValue { get; set; }
    public int CreateBy { get; set; }
}
public class RentalPropertyRequest
{
    public int RentalPropertyId { get; set; }
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
    public DateTime? AvailableFrom { get; set; }
    public bool IsActive { get; set; }
    public int CreateBy { get; set; }
    public int? UpdateBy { get; set; }
}

public record GetRentalPropertiesRequest(
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



public class InsertRentalPropertyMediaRequest
{
    public int RentalPropertyId { get; set; }
    public int MediaMenuId { get; set; }
    public string? MediaUrl { get; set; }
    public string? MediaType { get; set; }
    public int CreateBy { get; set; }
}