using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SportTrack.Data;
using SportTrack.Models;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http;

namespace SportTrack.Tests.Domain
{
    public class LoginTests
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}"));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.AddLogging();
            services.AddHttpContextAccessor();           // accessor
            services.AddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();
            services.AddSingleton<IUserConfirmation<ApplicationUser>, DefaultUserConfirmation<ApplicationUser>>();

            var provider = services.BuildServiceProvider();

            // ⬇️  OBAVEZNO: osiguraj HttpContext
            var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
            httpContextAccessor.HttpContext = new DefaultHttpContext
            {
                RequestServices = provider
            };

            _userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            _signInManager = provider.GetRequiredService<SignInManager<ApplicationUser>>();

            SeedUser().Wait();
        }

        private async Task SeedUser()
        {
            var user = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@algebra.hr",
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(user, "P@ssw0rd!");
        }

        [Fact]
        public async Task Login_WithCorrectUserAndPassword_ShouldSucceed()
        {
            var result = await _signInManager.PasswordSignInAsync("admin", "P@ssw0rd!", false, false);
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task Login_WithCorrectUserAndWrongPassword_ShouldFail()
        {
            var result = await _signInManager.PasswordSignInAsync("admin", "pogresnaLozinka", false, false);
            Assert.False(result.Succeeded);
        }

        [Fact]
        public async Task Login_WithInvalidUsername_ShouldFail()
        {
            var result = await _signInManager.PasswordSignInAsync("nepoznat", "P@ssw0rd!", false, false);
            Assert.False(result.Succeeded);
        }
    }
}