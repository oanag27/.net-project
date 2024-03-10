using Learn.Net.Helper;
using Learn.Net.Modal;
using Learn.Net.Repository.Models;

namespace Learn.Net.Services
{
    public interface IUserRoleService
    {
        Task<APIResponse> AssignRolePermission(List<TblRolepermission> _data);
        Task<List<TblRole>> GetAllRoles();
        Task<List<TblMenu>> GetAllMenus();
        Task<List<AppMenu>> GetAllMenuByRole(string userrole);
        Task<MenuPermission> GetMenuPermissionByRole(string userrole, string menucode);
    }
}
