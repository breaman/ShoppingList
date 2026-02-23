using ShoppingList.Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ShoppingList.Data.Models;

public class Role : IdentityRole<int>, IEntityBase
{
}