﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;

namespace WebStore.ServiceHosting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleStore<IdentityRole> _RoleStore;

        public RolesController(WebStoreContext db)
        {
            _RoleStore = new RoleStore<IdentityRole>(db) { AutoSaveChanges = true };
        }

        [HttpGet("AllRoles")]
        public async Task<IEnumerable<IdentityRole>> GetAllRoles() => await _RoleStore.Roles.ToArrayAsync();

        [HttpPost]
        public async Task<bool> CreateAsync(IdentityRole role) => (await _RoleStore.CreateAsync(role)).Succeeded;

        [HttpPut]
        public async Task<bool> UpdateAsync(IdentityRole role) => (await _RoleStore.UpdateAsync(role)).Succeeded;

        [HttpPost("Delete")]
        public async Task<bool> DeleteAsync(IdentityRole role) => (await _RoleStore.DeleteAsync(role)).Succeeded;

        [HttpPost("GetRoleId")]
        public async Task<string> GetRoleIdAsync(IdentityRole role) => await _RoleStore.GetRoleIdAsync(role);

        [HttpPost("GetRoleName")]
        public async Task<string> GetRoleNameAsync(IdentityRole role) => await _RoleStore.GetRoleNameAsync(role);

        [HttpPost("SetRoleName/{name}")]
        public async Task SetRoleNameAsync(IdentityRole role, string name) => await _RoleStore.SetRoleNameAsync(role, name);

        [HttpPost("GetNormalizedRoleName")]
        public async Task<string> GetNormalizedRoleNameAsync(IdentityRole role) => await _RoleStore.GetNormalizedRoleNameAsync(role);

        [HttpPost("SetNormalizedRoleName/{name}")]
        public async Task SetNormalizedRoleNameAsync(IdentityRole role, string name) => await _RoleStore.SetNormalizedRoleNameAsync(role, name);

        [HttpGet("FindById/{id}")]
        public async Task<IdentityRole> FindByIdAsync(string id) => await _RoleStore.FindByIdAsync(id);

        [HttpGet("FindByName/{name}")]
        public async Task<IdentityRole> FindByNameAsync(string name) => await _RoleStore.FindByNameAsync(name);
    }
}