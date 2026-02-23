using ShoppingList.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ShoppingList.Data.Models;

public class ApplicationDbContext : AuthDbContext
{
    public DbSet<ShoppingList> ShoppingLists { get; set; }
    public DbSet<ShoppingListItem> ShoppingListItems { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IUserService userService) :
        base(options, userService)
    {
    }
}