using Webapplication.Models;
using Microsoft.EntityFrameworkCore;

namespace Webapplication.Data;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    
    public DbSet<Employees> ReimbursementDetails{get; set;}
    public DbSet<Reviews> RatingDetails{get; set;}
}