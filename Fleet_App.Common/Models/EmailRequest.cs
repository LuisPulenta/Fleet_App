using System.ComponentModel.DataAnnotations;

namespace Fleet_App.Common.Models
{
    public class EmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}