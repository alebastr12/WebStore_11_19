using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.DAL.Data;
using WebStore.Domain.Entities;

namespace WebStore.Services.Database
{
    public class WebStoreContextInitializer
    {
        private readonly WebStoreContext _db;
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly ILogger<WebStoreContextInitializer> _Logger;

        public WebStoreContextInitializer(
            WebStoreContext db, 
            UserManager<User> UserManager, 
            RoleManager<IdentityRole> RoleManager,
            ILogger<WebStoreContextInitializer> Logger)
        {
            _db = db;

            _UserManager = UserManager;
            _RoleManager = RoleManager;
            _Logger = Logger;
        }

        public async Task InitializeAsync()
        {
            //_db.Database.EnsureCreated();
            await _db.Database.MigrateAsync();

            await InitializeIdentityAsync();

            if (await _db.Products.AnyAsync())
                return;

            using (var transaction = _db.Database.BeginTransaction())
            {
                await _db.Sections.AddRangeAsync(TestData.Sections);

                await _db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Sections] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF");

                transaction.Commit();
            }

            using (var transaction = _db.Database.BeginTransaction())
            {
                await _db.Brands.AddRangeAsync(TestData.Brands);

                await _db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Brands] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF");

                transaction.Commit();
            }

            using (var transaction = _db.Database.BeginTransaction())
            {
                await _db.Products.AddRangeAsync(TestData.Products);

                await _db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Products] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT [dbo].[Products] OFF");

                transaction.Commit();
            }

        }

        private async Task InitializeIdentityAsync()
        {
            if (!await _RoleManager.RoleExistsAsync(User.RoleUser))
            {
                _Logger.LogInformation("Роль пользователя отсутствует в БД");
                var result = await _RoleManager.CreateAsync(new IdentityRole(User.RoleUser));
                if(result.Succeeded)
                    _Logger.LogInformation("Роль пользователя успешно добавлена в БД");
                else
                {
                    var msg = string.Join(", ", result.Errors.Select(e => e.Description));
                    _Logger.LogError("Ошибка при добавлении роли пользователя {0}", msg);
                    throw new InvalidOperationException($"Ошибка при инициализации роли пользователя: {msg}");
                }
            }

            if (!await _RoleManager.RoleExistsAsync(User.RoleAdmin))
            {
                _Logger.LogInformation("Роль администратора отсутствует в БД");
                var result = await _RoleManager.CreateAsync(new IdentityRole(User.RoleAdmin));
                if (result.Succeeded)
                    _Logger.LogInformation("Роль администратора успешно добавлена в БД");
                else
                {
                    var msg = string.Join(", ", result.Errors.Select(e => e.Description));
                    _Logger.LogError("Ошибка при добавлении роли администратора {0}", msg);
                    throw new InvalidOperationException($"Ошибка при инициализации роли администратора: {msg}");
                }
            }

            if (await _UserManager.FindByNameAsync(User.AdminUserName) == null)
            {
                var admin = new User
                {
                    UserName = User.AdminUserName,
                    Email = $"{User.AdminUserName}@server.ru"
                };

                var creation_result = await _UserManager.CreateAsync(admin, User.DefaultAdminPassword);

                if (creation_result.Succeeded)
                    await _UserManager.AddToRoleAsync(admin, User.RoleAdmin);
            }
        }
    }
}
