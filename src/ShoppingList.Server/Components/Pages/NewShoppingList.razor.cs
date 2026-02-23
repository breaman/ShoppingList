using Microsoft.AspNetCore.Components;
using ShoppingList.Data.Models;
using ShoppingList.Shared.DTOs;

namespace ShoppingList.Server.Components.Pages;

public partial class NewShoppingList : ComponentBase
{
    [SupplyParameterFromForm] private ShoppingListDto ShoppingList { get; set; } = default!;
    
    
    [Inject] private ApplicationDbContext DbContext { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private ILogger<NewShoppingList> Logger { get; set; } = default!;

    protected override void OnInitialized()
    {
        ShoppingList ??= new();
    }

    private async Task HandleValidSubmit()
    {
        Logger.LogInformation("Saving new shopping list with name: {Name}", ShoppingList.Name);
        var newShoppingList = new ShoppingList.Data.Models.ShoppingList
        {
            Name = ShoppingList.Name
        };
        DbContext.ShoppingLists.Add(newShoppingList);
        await DbContext.SaveChangesAsync();
        Logger.LogInformation("New shopping list saved with ID: {Id}", newShoppingList.Id);
        NavigationManager.NavigateTo($"/shopping-list/{newShoppingList.Id}");
    }
}