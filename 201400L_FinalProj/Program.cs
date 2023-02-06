using _201400L_FinalProj.models;
using _201400L_FinalProj.Pages;
using authentication.models;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDataProtection();
builder.Services.AddDbContext<AuthDbContext>();
SignInManager<ApplicationUser> _signInManager;
UserManager<ApplicationUser> _signManager;
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(1));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache(); //save session in memory

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromSeconds(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = ".ASProj.Session";
    options.Cookie.IsEssential = true;
    
});

//builder.Services.AddAuthentication("mycookieauth").AddCookie("mycookieauth", options
//=>
//{
//    options.cookie.name = "mycookieauth";
//    options.accessdeniedpath = "/account/accessdenied";
//});

//builder.Services.ConfigureApplicationCookie(options =>
//{
//	options.Cookie.Name = ".AspNetCore.Identity.Application";
//	options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
//	options.SlidingExpiration = true;
//});

builder.Services.Configure<IdentityOptions>(opts =>
{
	opts.Lockout.AllowedForNewUsers = true;
	opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
	opts.Lockout.MaxFailedAccessAttempts = 3;
    opts.SignIn.RequireConfirmedEmail = true;

});
builder.Services.AddControllersWithViews();
builder.Services.ConfigureApplicationCookie(Config =>
{
    Config.LoginPath = "/Login";
});
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.UseStatusCodePagesWithRedirects("/errors/Error404");
app.MapRazorPages();

app.Run();
