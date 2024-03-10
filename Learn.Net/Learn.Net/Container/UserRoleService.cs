using Learn.Net.Helper;
using Learn.Net.Modal;
using Learn.Net.Repository;
using Learn.Net.Repository.Models;
using Learn.Net.Services;
using Microsoft.EntityFrameworkCore;

namespace Learn.Net.Container
{
    public class UserRoleService : IUserRoleService
    {
        private readonly Repository.LearnContext context;
        public UserRoleService(Repository.LearnContext learndata)
        {
            this.context = learndata;
        }
        public async Task<APIResponse> AssignRolePermission(List<TblRolepermission> _data)
        {
            APIResponse response = new APIResponse();
            int processcount = 0;
            try
            {
                using (var dbtransaction = await this.context.Database.BeginTransactionAsync())
                {
                    if (_data.Count > 0)
                    {
                        _data.ForEach(item =>
                        {
                            var userdata = this.context.TblRolepermissions.FirstOrDefault(item1 => item1.Userrole == item.Userrole &&
                            item1.Menucode == item.Menucode);
                            if (userdata != null)
                            {
                                userdata.Haveview = item.Haveview;
                                userdata.Haveadd = item.Haveadd;
                                userdata.Havedelete = item.Havedelete;
                                userdata.Haveedit = item.Haveedit;
                                processcount++;
                            }
                            else
                            {
                                this.context.TblRolepermissions.Add(item);
                                processcount++;

                            }

                        });

                        if (_data.Count == processcount)
                        {
                            await this.context.SaveChangesAsync();
                            await dbtransaction.CommitAsync();
                            response.Result = "pass";
                            response.ErrorMessage = "Saved successfully.";
                        }
                        else
                        {
                            await dbtransaction.RollbackAsync();
                        }

                    }
                    else
                    {
                        response.Result = "fail";
                        response.ErrorMessage = "Failed";
                    }
                }

            }
            catch (Exception ex)
            {
                response = new APIResponse();
            }

            return response;
        }

        public async Task<List<TblMenu>> GetAllMenus()
        {
            return await this.context.TblMenus.ToListAsync();
        }

        public async Task<List<TblRole>> GetAllRoles()
        {
            return await this.context.TblRoles.ToListAsync();
        }

        public async Task<List<AppMenu>> GetAllMenuByRole(string userrole)
        {
            List<AppMenu> appMenus = new List<AppMenu>();

            var accessData = (from menu in this.context.TblRolepermissions.Where(o => o.Userrole == userrole && o.Haveview)
                              join m in this.context.TblMenus on menu.Menucode equals m.Code into _jointable
                              from p in _jointable.DefaultIfEmpty()
                              select new { code = menu.Menucode, name = p.Name }).ToList();
            if (accessData.Any())
            {
                accessData.ForEach(item =>
                {
                    appMenus.Add(new AppMenu()
                    {
                        Code = item.code,
                        Name = item.name
                    });
                });
            }

            return appMenus;
        }

        public async Task<MenuPermission> GetMenuPermissionByRole(string userrole, string menucode)
        {
            MenuPermission menuPermission = new MenuPermission();
            var data = await this.context.TblRolepermissions.FirstOrDefaultAsync(i => i.Userrole == userrole && i.Haveview
            && i.Menucode == menucode);
            if (data != null)
            {
                menuPermission.Code = data.Menucode;
                menuPermission.HaveView = data.Haveview;
                menuPermission.HaveAdd = data.Haveadd;
                menuPermission.HaveEdit = data.Haveedit;
                menuPermission.HaveDelete = data.Havedelete;
            }
            return menuPermission;
        }
    }
}
