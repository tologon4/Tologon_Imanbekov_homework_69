using Delivery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Controllers;

public class ValidationController : Controller
{
    private DeliveryDb _db;
    private UserManager<User> _userManager;

    public ValidationController(DeliveryDb context, UserManager<User> userManager)
    {
        _db = context;
        _userManager = userManager;
    }
    
    [AcceptVerbs("GET", "POST")]
    public bool CheckEmail(string Email)
    {
        return !_db.Users.Any(u => u.Email.ToLower().Trim() == Email.ToLower().Trim());
    }
    
    [AcceptVerbs("GET", "POST")]
    public bool CheckUsername(string UserName)
    {
        return !_db.Users.Any(u => u.UserName.ToLower().Trim() == UserName.ToLower().Trim());
    }
    
    [AcceptVerbs("GET", "POST")]
    public bool CheckEateryName(string Name)
    {
        return !_db.Eateries.Any(u => u.Name.ToLower().Trim() == Name.ToLower().Trim());
    }
    
    [HttpGet]
    public async Task<IActionResult> EditCheckEateryName(string name, int id)
    {
        Eatery? eatery = await _db.Eateries.FirstOrDefaultAsync(e => e.Id == id);
        bool result = true;
        if (_db.Eateries.Any(e => e.Name.ToLower().Trim()==name.ToLower().Trim()))
            if (eatery.Name.ToLower().Trim()==name.ToLower().Trim())
                result = true;
            else
                result = false;
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> EditCheckUsernameEmail(string value, int? id)
    {
        User? adminUsr = await _userManager.GetUserAsync(User);
        User? user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        bool result = true;
        bool userName = value.ToLower().Trim() == adminUsr.UserName.ToLower().Trim() 
                        || value.ToLower().Trim() == user.UserName.ToLower().Trim();
        bool email = value.ToLower().Trim() == adminUsr.Email.ToLower().Trim() 
                     || value.ToLower().Trim() == user.Email.ToLower().Trim();
        if (_db.Users.Any(u => u.UserName.ToLower().Trim() == value.ToLower().Trim()) 
            || _db.Users.Any(u => u.Email.ToLower().Trim() == value.ToLower().Trim()))
        {
            if (userName || email )
                result = true;
            else
                result = false;
        }
        return Ok(result);
    }
}