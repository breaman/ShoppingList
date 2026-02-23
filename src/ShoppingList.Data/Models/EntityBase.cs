using ShoppingList.Data.Interfaces;

namespace ShoppingList.Data.Models;

public abstract class EntityBase : IEntityBase
{
    public int Id { get; set; }
}