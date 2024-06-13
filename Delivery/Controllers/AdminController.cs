using Delivery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Controllers;

[Authorize(Roles = "admin")]
[Authorize]
public class AdminController : Controller
{
    private DeliveryDb _db;
    private UserManager<User> _userManager;
    private SignInManager<User> _signInManager;
    private IWebHostEnvironment _environment;

    public AdminController(DeliveryDb db, UserManager<User> userManager,
        SignInManager<User> signInManager, IWebHostEnvironment environment)
    {
        _db = db;
        _environment = environment;
        _userManager = userManager;
        _signInManager = signInManager;
    }


    [Authorize]
    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> DeleteUser(int? id)
    {
        User? user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        var currentUser = await _userManager.GetUserAsync(User);
        var adminUser = await _userManager.IsInRoleAsync(currentUser, "admin");
        if (user!=null && adminUser)
        {
            _db.Users.Remove(user);
            int n = await _db.SaveChangesAsync();
            if (n>0)
                return Ok(true);
        }
        return Ok(false);
    }
    
    [Authorize]
    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> MakeRole(int? id)
    {
        User? user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        var currentUser = await _userManager.GetUserAsync(User);
        var adminUser = await _userManager.IsInRoleAsync(currentUser, "admin");
        if (user!=null && adminUser)
        {
            var isUser = await _userManager.IsInRoleAsync(user, "user");
            string roleIdent = "";
            if (isUser)
            {
                await _userManager.RemoveFromRoleAsync(user, "user");
                await _userManager.AddToRoleAsync(user,"manager");
                roleIdent = "manager";
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(user, "manager");
                await _userManager.AddToRoleAsync(user, "user");
                roleIdent = "user";
            }
            return Ok(new {success = true, role = roleIdent});
        }
        return Ok(new {success = false});
    }
    
    
}