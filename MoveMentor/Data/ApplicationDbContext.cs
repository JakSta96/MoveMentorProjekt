using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoveMentor.Models;

namespace MoveMentor.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		public DbSet<SportType> SportType { get; set; }
		public DbSet<Trening> Trening { get; set; }
	    public DbSet<Contacts> Contacts { get; set; }

    }
}