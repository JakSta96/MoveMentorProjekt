using System.ComponentModel.DataAnnotations;

namespace MoveMentor.Models
{
	public class SportType
	{
		public int Id { get; set; }
		[Display(Name = "Activity/discipline name")]
        [StringLength(50, ErrorMessage = "The name cannot exceed 50 characters.")]
        public string Name { get; set; }
	}
}
