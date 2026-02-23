using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Data.Models;
using ShoppingList.Shared.DTOs;

namespace ShoppingList.Server.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject] private ApplicationDbContext DbContext { get; set; } = default!;
    
    private List<ShoppingListDto>? _shoppingLists;
    
    protected override async Task OnInitializedAsync()
    {
        _shoppingLists = (await DbContext.ShoppingLists.ToListAsync())
            .Select(shoppingList => shoppingList.ToDto()).ToList();
    }
}