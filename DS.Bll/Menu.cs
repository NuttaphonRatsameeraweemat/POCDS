using DS.Bll.Context;
using DS.Bll.Interfaces;
using DS.Bll.Models;
using DS.Data.Pocos;
using DS.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;

namespace DS.Bll
{
    public class Menu : IMenu
    {

        #region Fields

        /// <summary>
        /// The utilities unit of work for manipulating utilities data in database.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu" /> class.
        /// </summary>
        /// <param name="unitOfWork">The utilities unit of work.</param>
        public Menu(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Methods

        public List<MenuViewModel> GenerateMenu(string username)
        {
            var rolelist = GetRole(username);
            var menu = GetMenu(rolelist);

            return menu;
        }

        private List<MenuViewModel> GetMenu(List<AppSingleRole> rolelist)
        {
            List<MenuViewModel> result = new List<MenuViewModel>();

            List<AppMenu> userMenuList = GetSideMenu(rolelist);

            string parentMenuCode = ConstantValue.RootMenuCode;

            var menuList = userMenuList.Where(a => a.ParentMenuCode.Equals(parentMenuCode, StringComparison.OrdinalIgnoreCase)).OrderBy(a => a.Sequence).ToList();

            foreach (var item in menuList)
            {
                result.AddRange(GetMenuItem(userMenuList, item));
            }

            return result;
        }

        private List<MenuViewModel> GetMenuItem(List<AppMenu> userMenuList, AppMenu menu)
        {
            List<MenuViewModel> result = new List<MenuViewModel>();
            ResourceManager rm = new ResourceManager(typeof(AppText));

            string menuText = rm.GetString(menu.ResourceName);

            if (menu.MenuType.Equals("GROUP", StringComparison.OrdinalIgnoreCase))
            {
                List<AppMenu> childMenuList = userMenuList.Where(a => a.ParentMenuCode.Equals(menu.MenuCode, StringComparison.OrdinalIgnoreCase)).OrderBy(a => a.Sequence).ToList();

                MenuViewModel mItem = new MenuViewModel
                {
                    Name = menuText,
                    Children = new List<MenuViewModel>()
                };

                foreach (var item in childMenuList)
                {
                    mItem.Children.AddRange(GetMenuItem(userMenuList, item));
                }

                result.Add(mItem);
            }
            else if (menu.MenuType.Equals("ITEM", StringComparison.OrdinalIgnoreCase))
            {
                MenuViewModel sItem = new MenuViewModel
                {
                    Name = menuText,
                    Url = string.Format("{0}/{1}/{2}", menu.Area, menu.Controller, menu.Action),
                    Children = null,
                };
                result.Add(sItem);
            }

            return result;

        }

        private List<AppMenu> GetSideMenu(List<AppSingleRole> rolelist)
        {
            List<AppMenu> userMenuList = new List<AppMenu>();

            var appMenus = _unitOfWork.GetRepository<AppMenu>().GetCache();

            //Check menu which user have role to display
            var roleMenuList = appMenus.Where(b => rolelist.Any(a => a.RoleId.Equals(b.RoleForDisplay, StringComparison.OrdinalIgnoreCase)) ||
                                                   rolelist.Any(a => a.RoleId.Equals(b.RoleForManage, StringComparison.OrdinalIgnoreCase))).ToList();

            var groupMenuList = appMenus.Where(b => b.MenuType.Equals("GROUP", StringComparison.OrdinalIgnoreCase)).ToList();

            //Find parent menu
            int maxParent = 10; //Prevent infinity loop
            foreach (var item in roleMenuList)
            {
                bool findParent = true;
                int countParent = 0;
                var parentMenuCode = item.ParentMenuCode;

                userMenuList.Add(item);

                while (findParent && !string.IsNullOrEmpty(parentMenuCode) && countParent < maxParent)
                {
                    AppMenu parentMenu = appMenus.FirstOrDefault(b => b.MenuCode.Equals(parentMenuCode, StringComparison.OrdinalIgnoreCase));

                    if (!parentMenu.MenuCode.Equals(ConstantValue.RootMenuCode, StringComparison.OrdinalIgnoreCase))
                    {
                        findParent = true;
                        parentMenuCode = parentMenu.ParentMenuCode;
                        userMenuList.Add(parentMenu);
                    }
                    else
                    {
                        findParent = false;
                    }

                    countParent++;
                }
            }

            //Remove duplicate menu
            userMenuList = userMenuList.Distinct().ToList();

            return userMenuList;
        }

        private List<AppSingleRole> GetRole(string username)
        {
            List<AppSingleRole> roleList = new List<AppSingleRole>();
            AppSingleRole role = new AppSingleRole();

#if DEBUG  
            //For test, add super admin role
            role = new AppSingleRole
            {
                RoleId = ConstantValue.RoleSuperAdmin
            };
            roleList.Add(role);

            roleList = GetSuperAdminSingleRole();
#endif

            var user = _unitOfWork.GetRepository<UserRole>().GetCache(x => x.Username == username).FirstOrDefault();
            if (user != null)
            {
                var userCompRoleList = _unitOfWork.GetRepository<AppCompositeRole>().GetCache(
                                                          x => x.Status == ConstantValue.ConfigStatusActive &&
                                                          x.Id == user.CompositeRoleId).ToList();

                if (userCompRoleList.Any())
                {
                    foreach (var compRole in userCompRoleList)
                    {
                        var compRoleItemList = _unitOfWork.GetRepository<AppCompositeRoleItem>().GetCache(
                                                         m => m.CompositeRoleId == user.CompositeRoleId).ToList();

                        foreach (var compRoleItem in compRoleItemList)
                        {
                            role = new AppSingleRole();
                            if (!string.IsNullOrEmpty(compRoleItem.RoleId))
                            {
                                role.RoleId = compRoleItem.RoleId;
                                roleList.Add(role);
                            }
                        }
                    }
                }
            }


            return roleList;
        }

        private List<AppSingleRole> GetSuperAdminSingleRole()
        {
            List<AppSingleRole> roleList = new List<AppSingleRole>();
            roleList = _unitOfWork.GetRepository<AppSingleRole>().GetCache().ToList();
            var role = new AppSingleRole
            {
                RoleId = ConstantValue.RoleSuperAdmin
            };
            roleList.Add(role);

            return roleList;
        }

        #endregion

    }
}
