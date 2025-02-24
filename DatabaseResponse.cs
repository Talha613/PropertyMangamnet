using System.ComponentModel.DataAnnotations;
namespace PropertyManagement.Core.DTOs;
public class DatabaseResponse
{
        [Key]
        public required string Message { get; set; }
        public bool Status { get; set; }
        public required string Data { get; set; }
}
