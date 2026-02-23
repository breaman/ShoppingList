using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using ShoppingList.Data.Models;
using ShoppingList.Server.Hubs;
using ShoppingList.Shared.DTOs;
using ShoppingList.Shared.Services;

namespace ShoppingList.Server.APIs;

public static class ShoppingListItemApis
{
    public static RouteGroupBuilder MapShoppingListItemApi(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/ShoppingListItems").WithTags("ShoppingListItems");

        group.MapGet("/{shoppingListId:int}",
            async Task<Ok<List<ShoppingListItemDto>>> (int shoppingListId,
                IShoppingListItemService shoppingListItemService) =>
            {
                var shoppingLists = await shoppingListItemService.GetShoppingListItemsAsync(shoppingListId);
                return TypedResults.Ok(shoppingLists);
            });
        
        group.MapPost("/{shoppingListId:int}",
            async Task<Ok<ShoppingListItemDto>>(int shoppingListId, ShoppingListItemDto shoppingListItemDto,
                ApplicationDbContext dbContext, IHubContext<ShoppingListHub> hubContext) =>
            {
                var createdItem = new ShoppingListItem()
                {
                    Name = shoppingListItemDto.Name,
                    ShoppingListId = shoppingListId
                };
                
                dbContext.ShoppingListItems.Add(createdItem);
                await dbContext.SaveChangesAsync();
                
                var dto = createdItem.ToDto();
                await hubContext.Clients.Group(shoppingListId.ToString()).SendAsync("shoppingListItemAdded", dto);
                
                return TypedResults.Ok(dto);
            });
        
        group.MapPut("/{shoppingListId:int}",
            async Task<Results<Ok<ShoppingListItemDto>, NotFound>>(int shoppingListId, ShoppingListItemDto shoppingListItemDto,
                ApplicationDbContext dbContext, IHubContext<ShoppingListHub> hubContext) =>
            {
                var updatedItem = await dbContext.ShoppingListItems.FindAsync(shoppingListItemDto.ShoppingListItemId);
                if (updatedItem is null)
                {
                    return TypedResults.NotFound();
                }
                
                updatedItem.Name = shoppingListItemDto.Name;
                updatedItem.IsPicked = shoppingListItemDto.IsPicked;
                
                await dbContext.SaveChangesAsync();
                
                var dto = updatedItem.ToDto();
                await hubContext.Clients.Group(shoppingListId.ToString()).SendAsync("shoppingListItemUpdated", dto);
                
                return TypedResults.Ok(dto);
            });

        return group;
    }
}