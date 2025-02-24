
using System.ComponentModel.DataAnnotations;

namespace PropertyManagement.Core.Models;
public class GetPropertiesByPriceRange
{

    
        [Key]
        public Guid PropertyId { get; set; }

        public string PropertyName { get; set; }
        public decimal PropertyPrice { get; set; }
        public string PropertyStatus { get; set; }
        public string PropertyCity { get; set; }
    }

