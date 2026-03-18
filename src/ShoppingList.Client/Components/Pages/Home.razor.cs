using Microsoft.AspNetCore.Components;
using ShoppingList.Shared.DTOs;
using ShoppingList.Shared.Services;

namespace ShoppingList.Client.Components.Pages;

public partial class Home : ComponentBase
{
    private List<ShoppingListDto>? _shoppingLists;
    private int? _editingListId;
    private string _editingName = string.Empty;
    private ShoppingListDto? _listPendingDelete;

    [Inject] private IShoppingListService ShoppingListService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _shoppingLists = await ShoppingListService.GetShoppingListsAsync();
    }

    private void BeginEdit(ShoppingListDto list)
    {
        _editingListId = list.ShoppingListId;
        _editingName = list.Name;
    }

    private void CancelEdit()
    {
        _editingListId = null;
        _editingName = string.Empty;
    }

    private async Task SaveEditAsync()
    {
        if (_editingListId is null || string.IsNullOrWhiteSpace(_editingName))
        {
            return;
        }

        var list = _shoppingLists?.FirstOrDefault(l => l.ShoppingListId == _editingListId);
        if (list is null)
        {
            return;
        }

        list.Name = _editingName;
        await ShoppingListService.UpdateShoppingListAsync(list);
        _editingListId = null;
        _editingName = string.Empty;
    }

    private void ConfirmDelete(ShoppingListDto list)
    {
        _listPendingDelete = list;
    }

    private void CancelDelete()
    {
        _listPendingDelete = null;
    }

    private async Task ExecuteDeleteAsync()
    {
        if (_listPendingDelete is null)
        {
            return;
        }

        await ShoppingListService.DeleteShoppingListAsync(_listPendingDelete.ShoppingListId);
        _shoppingLists?.Remove(_listPendingDelete);
        _listPendingDelete = null;
    }
}
