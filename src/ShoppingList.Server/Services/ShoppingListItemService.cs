using Microsoft.EntityFrameworkCore;
using ShoppingList.Data.Models;
using ShoppingList.Shared.DTOs;
using ShoppingList.Shared.Services;

namespace ShoppingList.Server.Services;

public class ShoppingListItemService(ApplicationDbContext dbContext) : IShoppingListItemService
{
    public async Task<List<ShoppingListItemDto>> GetShoppingListItemsAsync(int shoppingListId)
    {
        var shoppingListItems = await dbContext.ShoppingListItems.AsNoTracking()
            .Where(s => s.ShoppingListId == shoppingListId)
            .ToListAsync();

        return shoppingListItems.Select(sli => sli.ToDto()).ToList();
    }
}