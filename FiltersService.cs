using PropertyManagement.Core.DTOs;
using PropertyManagement.Core.DTOs.Shared;
using PropertyManagement.Core.DTOs.Sps;
using PropertyManagement.Data.Repositories;
namespace PropertyManagement.Business.Services;
public interface IFiltersService
{
    Task<ApiResponse<GetFiltersDto>> GetFilters(string language = "en");
}

public class FiltersService : IFiltersService
{
    private readonly IFiltersRepository _filtersRepository;


    public FiltersService(IFiltersRepository filtersRepository)
    {
        _filtersRepository = filtersRepository;
    }

    
    public async Task<ApiResponse<GetFiltersDto>> GetFilters(string language = "en")
    {
        GetFiltersDto dto = new GetFiltersDto
        {
            PropertyType = new List<KeyValue>(),
            Amenities = new List<KeyValue>(),
            Furnishing = new List<KeyValue>(),
            CompletionStatus = new List<KeyValue>()
        };

        var data = await _filtersRepository.GetFilters(language);
        foreach (var item in data)
        {

                switch (item.ConfigKey)
                {
                    case "property_type":
                        dto.PropertyType?.Add(new KeyValue
                        {
                            Key = item.Id,
                            Value = item.Value,
                        });
                    break;
                case "amenities":
                    dto.Amenities?.Add(new KeyValue
                    {
                        Key = item.Id,
                        Value = item.Value,
                    });
                    break;
                case "furnishing":
                    dto.Furnishing?.Add(new KeyValue
                    {
                        Key = item.Id,
                        Value = item.Value,
                    });
                    break;
                case "completion_status":
                    dto.CompletionStatus?.Add(new KeyValue
                    {
                        Key = item.Id,
                        Value = item.Value,
                    });
                    break;
                }

        }
        
        return new ApiResponse<GetFiltersDto>(true, "Data has been retrieved . ", dto);
    }
}






