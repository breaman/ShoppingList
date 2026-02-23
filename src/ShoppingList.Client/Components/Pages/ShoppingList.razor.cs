using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using ShoppingList.Shared.DTOs;
using ShoppingList.Shared.Services;

namespace ShoppingList.Client.Components.Pages;

public partial class ShoppingList : ComponentBase
{
    private HubConnection? _hubConnection;
    
    [Parameter]
    public int Id { get; set; }
    public string? ItemName { get; set; }
    
    [PersistentState]
    public List<ShoppingListItemDto>? ShoppingListItems { get; set; }
    [PersistentState]
    public ShoppingListDto? ShoppingListDto { get; set; }
    
    [Inject] private IShoppingListItemService ShoppingListItemService { get; set; } = default!;
    [Inject] private IShoppingListService ShoppingListService { get; set; } = default!;
    [Inject] private ILogger<ShoppingList> Logger { get; set; } = default!;
    [Inject] private HttpClient HttpClient { get; set; } = default!;
    
    protected override async Task OnInitializedAsync()
    {
        ShoppingListDto ??= await ShoppingListService.GetShoppingListByIdAsync(Id);
        ShoppingListItems ??= await ShoppingListItemService.GetShoppingListItemsAsync(Id);
        
        if (RendererInfo.IsInteractive)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(new Uri($"{HttpClient.BaseAddress}shoppingListHub"))
                .Build();

            _hubConnection.On<ShoppingListItemDto>("shoppingListItemAdded", (shoppingListItem) =>
            {
                ShoppingListItems.Add(shoppingListItem);
                InvokeAsync(StateHasChanged);
            });

            _hubConnection.On<ShoppingListItemDto>("shoppingListItemUpdated", (shoppingListItem) =>
            {
                // find the item in the list and update it
                var item = ShoppingListItems.FirstOrDefault(x =>
                    x.ShoppingListItemId == shoppingListItem.ShoppingListItemId);
                if (item is not null)
                {
                    item.Name = shoppingListItem.Name;
                    item.IsPicked = shoppingListItem.IsPicked;
                    item.IsDeleted = shoppingListItem.IsDeleted;
                }

                InvokeAsync(StateHasChanged);
            });

            await _hubConnection.StartAsync();
            await _hubConnection.InvokeAsync("JoinGroup", Id);
        }
    }
    
    private async Task ToggleItemAsync(ShoppingListItemDto item, ChangeEventArgs value)
    {
        item.IsPicked = (bool)(value.Value ?? false);
        await HttpClient.PutAsJsonAsync($"/api/ShoppingListItems/{Id}", item);
    }
    
    private async Task HandleItemNameKeyUpAsync(KeyboardEventArgs args)
    {
        if (string.Equals(args.Key, "Enter", StringComparison.Ordinal))
        {
            await AddItemAsync();
        }
    }
    
    private async Task HandleItemKeyUpAsync(ShoppingListItemDto item, KeyboardEventArgs args)
    {
        if (string.Equals(args.Key, "Backspace", StringComparison.Ordinal) || string.Equals(args.Key, "Delete", StringComparison.Ordinal))
        {
            await DeleteItemAsync(item);
        }
    }
    
    private async Task AddItemAsync()
    {
        if (!string.IsNullOrWhiteSpace(ItemName))
        {
            Logger.LogInformation("Adding new shopping list item");
            var newItem = new ShoppingListItemDto()
            {
                ShoppingListItemId = Id,
                Name = ItemName
            };

            ItemName = null;
            await HttpClient.PostAsJsonAsync($"/api/ShoppingListItems/{Id}", newItem);
        }
    }

    private async Task DeleteItemAsync(ShoppingListItemDto item)
    {
        await HttpClient.DeleteAsync($"/api/ShoppingListItems/{item.ShoppingListItemId}");
    }
}