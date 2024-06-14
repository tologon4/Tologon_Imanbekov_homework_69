using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Delivery.Models;

public class DeliveryDb : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<User> Users { get; set; }
    public DbSet<Eatery> Eateries { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<Order> Orders { get; set; }
    
    
    public DeliveryDb(DbContextOptions<DeliveryDb> options) : base(options){}

}