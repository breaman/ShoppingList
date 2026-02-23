using Microsoft.EntityFrameworkCore;
using ShoppingList.Data.Models;
using ShoppingList.Shared.DTOs;
using ShoppingList.Shared.Services;

namespace ShoppingList.Server.Services;

public class ShoppingListService(ApplicationDbContext dbContext) : IShoppingListService
{
    public async Task<List<ShoppingListDto>> GetShoppingListsAsync()
    {
        var shoppingLists = await dbContext.ShoppingLists.AsNoTracking().OrderByDescending(sl => sl.CreatedOn)
            .ToListAsync();
        
        return shoppingLists.Select(sl => sl.ToDto()).ToList();
    }
    
    public async Task<ShoppingListDto?> GetShoppingListByIdAsync(int id)
    {
        return (await dbContext.ShoppingLists.SingleOrDefaultAsync(sl => sl.Id == id))?.ToDto();
    }
}