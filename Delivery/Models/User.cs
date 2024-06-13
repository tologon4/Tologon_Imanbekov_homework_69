using Microsoft.AspNetCore.Identity;

namespace Delivery.Models;

public class User : IdentityUser<int>
{
    public string Avatar { get; set; }
    
}