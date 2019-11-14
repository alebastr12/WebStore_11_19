using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;

namespace WebStore.ServiceHosting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserStore<User> _UserStore;

        public UsersController(WebStoreContext db)
        {
            _UserStore = new UserStore<User>(db) { AutoSaveChanges = true };
        }

    }
}