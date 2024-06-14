using Delivery.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Controllers;

public class EateryController : Controller
{
    private DeliveryDb _db;
    private UserManager<User> _userManager;
    private IWebHostEnvironment _environment;

    public EateryController(DeliveryDb db, UserManager<User> userManager, IWebHostEnvironment environment)
    {
        _db = db;
        _userManager = userManager;
        _environment = environment;
    }
    
    
    [HttpGet]
    public async Task<IActionResult> DishDetails(int? id)
    {
        Dish? dish = await _db.Dishes.Include(e => e.Eatery).FirstOrDefaultAsync(d => d.Id == id);
        if (dish != null)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var isAdminUser = await _userManager.IsInRoleAsync(currentUser, "admin") 
                              || await _userManager.IsInRoleAsync(currentUser, "manager");
            ViewBag.EditDeleteAccess = isAdminUser;
            return View(dish);
        }
        return NotFound();
    }

    [HttpPost]
    [Authorize]
    [Authorize(Roles = "admin,manager")]
    public async Task<IActionResult> DeleteDish(int id)
    {
        Dish? dish = await _db.Dishes.FirstOrDefaultAsync(u => u.Id == id);
        var currentUser = await _userManager.GetUserAsync(User);
        var adminUser = await _userManager.IsInRoleAsync(currentUser, "admin") 
                        || await _userManager.IsInRoleAsync(currentUser, "manager");
        Eatery? eatery = await _db.Eateries.Include(d => d.Dishes).FirstOrDefaultAsync(e => e.Id == dish.EateryId);
        if (dish!=null && adminUser)
        {
            _db.Dishes.Remove(dish);
            eatery.Dishes.Remove(dish);
            _db.Eateries.Update(eatery);
            int n = await _db.SaveChangesAsync();
            if (n>0)
                return Ok(true);
        }
        return Ok(false);
    }
    
    [HttpPost]
    [Authorize]
    [Authorize(Roles = "admin,manager")]
    public async Task<IActionResult> EditDish(Dish model, int dishId, IFormFile? uploadedFile)
    {
        Dish? dish = await _db.Dishes.FirstOrDefaultAsync(u => u.Id == dishId);
        if (dish != null)
        {
            string? path = null;
            if (uploadedFile!=null)
            {
                var buffer = dish.Avatar.Split('=');
                var buffer2 = buffer[buffer.Length - 1].Split('.');
                string newFileName = Path.ChangeExtension($"{model.Name.Trim()}-ProfileN={int.Parse(buffer2[0])+1}", Path.GetExtension(uploadedFile.FileName));
                path= $"/userImages/" + newFileName.Trim();
                using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
            }
            dish.Name = model.Name;
            dish.Description = model.Description;
            dish.Avatar = path != null ? path : dish.Avatar;
            dish.Price = model.Price;
            _db.Dishes.Update(dish);
            int n = await _db.SaveChangesAsync();
            if (n>0)
                return new ObjectResult(dish);
            else
                return BadRequest();
        }
        ModelState.AddModelError("","Заведение не найдено!");
        return NotFound();
    }
    
    public async Task<IActionResult> Index()
    {
        var eateries = _db.Eateries.ToList();
        return View(eateries);
    }

    
    [Authorize]
    [Authorize(Roles = "admin,manager")]
    [HttpGet]
    public async Task<IActionResult> CreateDish(int? eateryId)
    {
        if (_db.Eateries.Any(e=>e.Id == eateryId))
        {
            ViewBag.EateryId = eateryId;
            return View();
        }
        return BadRequest();
    }

    [Authorize]
    [Authorize(Roles = "admin,manager")]
    [HttpPost]
    public async Task<IActionResult> CreateDish(Dish model, IFormFile uploadedFile)
    {
        if (ModelState.IsValid)
        {
            Eatery? eatery = await _db.Eateries.FirstOrDefaultAsync(e => e.Id == model.EateryId);
            string newFileName = Path.ChangeExtension($"{model.Name.Trim()}-EateryN=1", Path.GetExtension(uploadedFile.FileName));
            string path= $"/userImages/" + newFileName.Trim();
            using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }
            model.Avatar = path;
            _db.Dishes.Add(model);
            eatery.Dishes.Add(model);
            _db.Eateries.Update(eatery);
            int n = await _db.SaveChangesAsync();
            if (n > 0)
                return RedirectToAction("Details", "Eatery", new {id = eatery.Id});
        }
        ModelState.AddModelError("", "Произошла ошибка при создании блюда!");
        return View(model);
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        Eatery? eatery = await _db.Eateries.Include(d => d.Dishes).FirstOrDefaultAsync(e => e.Id == id);
        if (eatery!=null)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var isAdminUser = await _userManager.IsInRoleAsync(currentUser, "admin") 
                              || await _userManager.IsInRoleAsync(currentUser, "manager");
            ViewBag.EditDeleteAccess = isAdminUser;
            return View(eatery);
        }
        return NotFound();
    }

    [Authorize]
    [Authorize(Roles = "admin,manager")]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        Eatery? eatery = await _db.Eateries.FirstOrDefaultAsync(u => u.Id == id);
        var currentUser = await _userManager.GetUserAsync(User);
        var adminUser = await _userManager.IsInRoleAsync(currentUser, "admin") 
                        || await _userManager.IsInRoleAsync(currentUser, "manager");
        if (eatery!=null && adminUser)
        {
            _db.Eateries.Remove(eatery);
            int n = await _db.SaveChangesAsync();
            if (n>0)
                return Ok(true);
        }
        return Ok(false);
    }

    [Authorize]
    [Authorize(Roles = "admin,manager")]
    [HttpPost]
    public async Task<IActionResult> Edit(Eatery? model, int eateryId, IFormFile? uploadedFile)
    {
        Eatery? eatery = await _db.Eateries.FirstOrDefaultAsync(u => u.Id == eateryId);
        if (eatery != null)
        {
            string? path = null;
            if (uploadedFile!=null)
            {
                var buffer = eatery.Avatar.Split('=');
                var buffer2 = buffer[buffer.Length - 1].Split('.');
                string newFileName = Path.ChangeExtension($"{model.Name.Trim()}-ProfileN={int.Parse(buffer2[0])+1}", Path.GetExtension(uploadedFile.FileName));
                path= $"/userImages/" + newFileName.Trim();
                using (var fileStream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
            }
            eatery.Name = model.Name;
            eatery.Description = model.Description;
            eatery.Avatar = path != null ? path : eatery.Avatar;
            _db.Eateries.Update(eatery);
            int n = await _db.SaveChangesAsync();
            if (n>0)
                return new ObjectResult(eatery);
            else
                return BadRequest();
        }
        ModelState.AddModelError("","Заведение не найдено!");
        return NotFound();
    }
}