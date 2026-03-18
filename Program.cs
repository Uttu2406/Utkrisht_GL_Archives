using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransportManagement;
using TransportManagement.Data;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// 1. Connection Strings
var identityConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var transportConnectionString = builder.Configuration.GetConnectionString("Con")
    ?? throw new InvalidOperationException("Connection string 'Con' not found.");

// 2. Add DbContexts
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(identityConnectionString));
builder.Services.AddDbContext<TransportDbContext>(options => options.UseSqlServer(transportConnectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 3. Identity Configuration (Using ApplicationDbContext for Auth)
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// 4. Startup Tasks (Critical Section)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // FIRST: Seed Roles and make sure YOUR user is an Admin
    await SeedAdminUser(services);

    // SECOND: Run your Transport Logic
    var context = services.GetRequiredService<TransportDbContext>();
    var runner = new TransportManagementApp(context);
    runner.Run();
}

// 5. Middleware Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// DO NOT CHANGE THIS ORDER
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

// 6. Seed Function (Updated for Multiple Admins)
async Task SeedAdminUser(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // 1. Ensure the "Admin" role exists in the database
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // 2. List of emails you want to promote to Admin
    string[] adminEmails = {
        "bhandari.krish.kb@gmail.com",
        "rishi@gmail.com",
        "kumarisweety2277@gmail.com",
        "admin@gmail.com"
    };

    foreach (var email in adminEmails)
    {
        var user = await userManager.FindByEmailAsync(email);

        // If the user exists and isn't already an Admin, add them to the role
        if (user != null)
        {
            var isInRole = await userManager.IsInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}