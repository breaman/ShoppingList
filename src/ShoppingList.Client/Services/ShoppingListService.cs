using System.Net.Http.Json;
using ShoppingList.Shared.DTOs;
using ShoppingList.Shared.Services;

namespace ShoppingList.Client.Services;

public class ShoppingListService(HttpClient httpClient) : IShoppingListService
{
    public async Task<List<ShoppingListDto>> GetShoppingListsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<ShoppingListDto>>("/api/ShoppingLists") ?? [];
    }

    public async Task<ShoppingListDto?> GetShoppingListByIdAsync(int id)
    {
        return await httpClient.GetFromJsonAsync<ShoppingListDto>($"/api/ShoppingLists/{id}");
    }
}