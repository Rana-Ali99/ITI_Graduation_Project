using System.ComponentModel.DataAnnotations;

namespace ReadersClubApi.DTO
{
    public class RegiserForm
    {
        [Required]
        [MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string? PhoneNumber { get; set; }
        public bool IsAuthor { get; set; } 
    }
}
