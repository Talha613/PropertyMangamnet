namespace PropertyManagement.Core.Models;

public class GetMediaByBuyPropertyIdForAdminPanel
{
    public int BuyPropertyMediaId { get; set; }

    public int BuyPropertyId { get; set; }

    public int MediaMenuId { get; set; }

    public string MediaUrl { get; set; } = string.Empty;

    public string MediaType { get; set; } = string.Empty;

    public int CreateBy { get; set; }

    public DateTime CreatedAt { get; set; }
}