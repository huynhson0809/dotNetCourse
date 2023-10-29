using System.ComponentModel.DataAnnotations;

namespace UdemyDotNet.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(4, ErrorMessage = "Min length > 4 characters")]
        public string Name { get; set; }
        public string Code { get; set; }
        [MaxLength(100, ErrorMessage = "Max length < 100 characters")]
        public string? RegionImageUrl { get; set; }
    }
}
