using H5ServersideProgrammering.Components;
using H5ServersideProgrammering.Components.Account;
using H5ServersideProgrammering.Data;
using H5ServersideProgrammering.Handlers;
using H5ServersideProgrammering.Repository;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace H5ServersideProgrammering
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();

            var connectionStringIdentity = builder.Configuration.GetConnectionString("DefaultConnectionIdentity") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            var connectionStringData = builder.Configuration.GetConnectionString("DefaultConnectionData") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionStringIdentity));
            builder.Services.AddDbContext<AppDataContext>(options =>
                options.UseSqlite(connectionStringData));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddTransient<IUserCPRRepository, UserCPRRepository>();
            builder.Services.AddTransient<ITodoRepository, TodoRepository>();
            //builder.Services.AddSingleton<SymmetricEncryptionHandler>();

            // We are only using asymmetric encryption
            builder.Services.AddSingleton<AsymmetricEncryptionHandler>();
            builder.Services.AddSingleton<HashingHandler>();
            

            builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AuthenticatedUser", Policy =>
                {
                    Policy.RequireAuthenticatedUser();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            app.Run();
        }
    }
}
