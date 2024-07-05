using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MoveMentor.Models
{
    public class Trening
    {
        public int Id { get; set; }
        [Display(Name = "Start Date")]
        public DateTime DateTimeStart { get; set; }
        [Display(Name = "End Date")]
        [ValidateEndDateTime(ErrorMessage = "End Date cannot be earlier than Start Date.")]
        public DateTime DateTimeEnd { get; set; }

        [Display(Name = "Activity/Discipline Name")]
        public int SportTypeId { get; set; }
        [Display(Name = "Activity/Discipline Name")]
        public virtual SportType? SportType { get; set; }
        public string? UserId { get; set; }
        public virtual IdentityUser? User { get; set; }

        [Display(Name = "Comment")]
        public string Comment { get; set; }
    }
    public class ValidateEndDateTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var trening = (Trening)validationContext.ObjectInstance;

            if (trening.DateTimeEnd < trening.DateTimeStart)
            {
                return new ValidationResult(ErrorMessage);
            }

            if (trening.DateTimeStart < DateTime.Now)
            {
                return new ValidationResult("Start Date cannot be earlier than current date.");
            }

            return ValidationResult.Success;
        }
    }
}
