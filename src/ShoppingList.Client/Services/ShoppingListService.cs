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

    public async Task<ShoppingListDto?> UpdateShoppingListAsync(ShoppingListDto shoppingListDto)
    {
        var response = await httpClient.PutAsJsonAsync($"/api/ShoppingLists/{shoppingListDto.ShoppingListId}", shoppingListDto);
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<ShoppingListDto>()
            : null;
    }

    public async Task DeleteShoppingListAsync(int id)
    {
        await httpClient.DeleteAsync($"/api/ShoppingLists/{id}");
    }
}