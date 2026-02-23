using Microsoft.AspNetCore.Http.HttpResults;
using ShoppingList.Shared.DTOs;
using ShoppingList.Shared.Services;

namespace ShoppingList.Server.APIs;

public static class ShoppingListApis
{
    public static RouteGroupBuilder MapShoppingListApi(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/ShoppingLists").WithTags("ShoppingLists");

        group.MapGet("/", async Task<Ok<List<ShoppingListDto>>> (IShoppingListService shoppingListService) =>
        {
            var shoppingLists = await shoppingListService.GetShoppingListsAsync();
            return TypedResults.Ok(shoppingLists);
        });
        
        group.MapGet("/{id}", async Task<Results<Ok<ShoppingListDto>, NotFound>> (int id, IShoppingListService shoppingListService) =>
        {
            var shoppingList = await shoppingListService.GetShoppingListByIdAsync(id);
            return shoppingList is not null ? TypedResults.Ok(shoppingList) : TypedResults.NotFound();
        });

        return group;
    }
}