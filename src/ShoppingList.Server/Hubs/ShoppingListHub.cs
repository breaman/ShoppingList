using Microsoft.AspNetCore.SignalR;
using ShoppingList.Shared.DTOs;

namespace ShoppingList.Server.Hubs;

public class ShoppingListHub(ILogger<ShoppingListHub> logger) : Hub
{
    public async Task JoinGroup(int shoppingListId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, shoppingListId.ToString());
        // await Clients.Group(groupName).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined the group {groupName}.");
    }

    public async Task ProcessMessage(ShoppingListItemDto itemDto)
    {
        logger.LogInformation("Here is where we would call the database to save off the item information: {shoppingListItem}", itemDto);
        await Clients.Groups(itemDto.ShoppingListItemId.ToString()).SendAsync("ShoppingListUpdate", itemDto);
    }
}