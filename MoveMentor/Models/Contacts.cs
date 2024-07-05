using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MoveMentor.Models
{
    public class Contacts
    {
        public int Id { get; set; }
        [StringLength(80, ErrorMessage = "The name cannot exceed 50 characters.")]
        public string Name { get; set; }
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Phone number must be exactly 9 digits.")]
        public int Phone {  get; set; }
        [StringLength(50, ErrorMessage = "The name cannot exceed 50 characters.")]
        [Display(Name = "E-mail Address")]
        [Required(ErrorMessage = "Mail is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string mail { get; set; }

        public string? UserId { get; set; }
        public virtual IdentityUser? User { get; set; }

    }
}
