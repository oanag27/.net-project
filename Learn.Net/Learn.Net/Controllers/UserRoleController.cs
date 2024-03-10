using Learn.Net.Repository.Models;
using Learn.Net.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Learn.Net.Controllers
{
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService userRole;
        public UserRoleController(IUserRoleService roleService)
        {
            this.userRole = roleService;
        }

        [HttpPost("assignRolePermission")]
        public async Task<IActionResult> AssignRolePermission(List<TblRolepermission> rolePermissions)
        {
            var data = await this.userRole.AssignRolePermission(rolePermissions);
            return Ok(data);
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var data = await this.userRole.GetAllRoles();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpGet("GetAllMenus")]
        public async Task<IActionResult> GetAllMenus()
        {
            var data = await this.userRole.GetAllMenus();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpGet("GetAllMenusByRole")]
        public async Task<IActionResult> GetAllMenusByRole(string userrole)
        {
            var data = await this.userRole.GetAllMenuByRole(userrole);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpGet("GetMenuPermissionByRole")]
        public async Task<IActionResult> GetMenuPermissionByRole(string userrole, string menucode)
        {
            var data = await this.userRole.GetMenuPermissionByRole(userrole, menucode);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }


    }
}

