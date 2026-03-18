using Microsoft.EntityFrameworkCore;
using ShoppingList.Data.Models;
using ShoppingList.Shared.DTOs;
using ShoppingList.Shared.Services;

namespace ShoppingList.Server.Services;

public class ShoppingListService(ApplicationDbContext dbContext) : IShoppingListService
{
    public async Task<List<ShoppingListDto>> GetShoppingListsAsync()
    {
        return await dbContext.ShoppingLists
            .AsNoTracking()
            .OrderByDescending(sl => sl.CreatedOn)
            .Select(sl => new ShoppingListDto
            {
                ShoppingListId = sl.Id,
                Name = sl.Name,
                CreatedOn = sl.CreatedOn,
                TotalItems = sl.Items.Count(i => !i.IsDeleted),
                RemainingItems = sl.Items.Count(i => !i.IsDeleted && !i.IsPicked)
            })
            .ToListAsync();
    }
    
    public async Task<ShoppingListDto?> GetShoppingListByIdAsync(int id)
    {
        return (await dbContext.ShoppingLists.SingleOrDefaultAsync(sl => sl.Id == id))?.ToDto();
    }

    public async Task<ShoppingListDto?> UpdateShoppingListAsync(ShoppingListDto shoppingListDto)
    {
        var shoppingList = await dbContext.ShoppingLists.FindAsync(shoppingListDto.ShoppingListId);
        if (shoppingList is null)
        {
            return null;
        }

        shoppingList.Name = shoppingListDto.Name;
        await dbContext.SaveChangesAsync();

        return shoppingList.ToDto();
    }

    public async Task DeleteShoppingListAsync(int id)
    {
        var shoppingList = await dbContext.ShoppingLists.FindAsync(id);
        if (shoppingList is not null)
        {
            dbContext.ShoppingLists.Remove(shoppingList);
            await dbContext.SaveChangesAsync();
        }
    }
}