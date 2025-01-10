using System.Reflection.Emit;
using CustomIdentity.Data;
using CustomIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("default");


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(connectionString)
    .UseSeeding((context, _) =>
    {
        const string ADMIN_ROLE_ID = "eba739e9-5095-4642-a475-6dc0ab9dd632";
        const string ADMIN_USER_ID = "9a3f00d0-7311-41f8-a161-ba70f65198b9";

        const string USER_ROLE_ID = "v3dd2503-9d29-4021-af0d-45ba71853639";
        const string MANAGER_ROLE_ID = "a52c33c3-304a-4c0b-aa17-566ef3345959";

        // Check if roles exist before adding them
        if (!context.Set<IdentityRole>().Any(r => r.Id == ADMIN_ROLE_ID))
        {
            context.Set<IdentityRole>().Add(new IdentityRole
            {
                Id = ADMIN_ROLE_ID,
                Name = "Admin",
                NormalizedName = "ADMIN"
            });
        }

        if (!context.Set<IdentityRole>().Any(r => r.Id == USER_ROLE_ID))
        {
            context.Set<IdentityRole>().Add(new IdentityRole
            {
                Id = USER_ROLE_ID,
                Name = "User",
                NormalizedName = "USER"
            });
        }

        if (!context.Set<IdentityRole>().Any(r => r.Id == MANAGER_ROLE_ID))
        {
            context.Set<IdentityRole>().Add(new IdentityRole
            {
                Id = MANAGER_ROLE_ID,
                Name = "Manager",
                NormalizedName = "MANAGER"
            });
        }

        // Check if admin user exists before adding
        if (!context.Set<AppUser>().Any(u => u.Id == ADMIN_USER_ID))
        {
            var hasher = new PasswordHasher<AppUser>();
            context.Set<AppUser>().Add(new AppUser
            {
                Id = ADMIN_USER_ID,
                Name = "Admin",
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "admin@123"),
                SecurityStamp = string.Empty
            });
        }

        // Check if role assignments exist before adding
        if (!context.Set<IdentityUserRole<string>>()
            .Any(ur => ur.UserId == ADMIN_USER_ID && ur.RoleId == ADMIN_ROLE_ID))
        {
            context.Set<IdentityUserRole<string>>().Add(new IdentityUserRole<string>
            {
                RoleId = ADMIN_ROLE_ID,
                UserId = ADMIN_USER_ID
            });
        }

        if (!context.Set<IdentityUserRole<string>>()
            .Any(ur => ur.UserId == ADMIN_USER_ID && ur.RoleId == USER_ROLE_ID))
        {
            context.Set<IdentityUserRole<string>>().Add(new IdentityUserRole<string>
            {
                RoleId = USER_ROLE_ID,
                UserId = ADMIN_USER_ID
            });
        }

        if (!context.Set<IdentityUserRole<string>>()
            .Any(ur => ur.UserId == ADMIN_USER_ID && ur.RoleId == MANAGER_ROLE_ID))
        {
            context.Set<IdentityUserRole<string>>().Add(new IdentityUserRole<string>
            {
                RoleId = MANAGER_ROLE_ID,
                UserId = ADMIN_USER_ID
            });
        }

        context.SaveChanges();
    })
);


builder.Services.AddIdentity<AppUser, IdentityRole>(
    options =>
    {
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    //.WithStaticAssets();


app.Run();
