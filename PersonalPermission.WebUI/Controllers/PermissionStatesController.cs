using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalPermission.Core.Entities;
using PersonalPermission.Data;
using PersonalPermission.Service.IService;

namespace PersonalPermission.WebUI.Controllers
{
    [Authorize]
    public class PermissionStatesController : Controller
    {
        private readonly IService<PermissionState> _servicePermissionState;
        private readonly IService<UsedYearlyPermission> _serviceUsedYearlyPermission;
        private readonly IService<UsedAdministrivePermission> _serviceUsedAdministrivePermission;
        private readonly IService<User> _serviceUser;

        public PermissionStatesController(IService<PermissionState> servicePermissionState, IService<UsedYearlyPermission> serviceUsedYearlyPermission, IService<UsedAdministrivePermission> serviceUsedAdministrivePermission, IService<User> serviceUser)
        {
            _servicePermissionState = servicePermissionState;
            _serviceUsedYearlyPermission = serviceUsedYearlyPermission;
            _serviceUsedAdministrivePermission = serviceUsedAdministrivePermission;
            _serviceUser = serviceUser;
        }

        [Route("izindurumum")]
        public async Task<IActionResult> Index()
        {
            //   var user = await _context.Users.FindAsync(
            return View(await _servicePermissionState.GetQueryable().Include(u => u.User).Where(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString() && x.IsActive).ToListAsync());
        }

        [Route("tumpersonellerinizindurumu")]
        [Authorize(Policy = "AdminPolicy")]
        // all permission states
        public async Task<IActionResult> Index2()
        {
            var user = await _serviceUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString());
            var persmissionStates = await _servicePermissionState.GetQueryable().Include(u => u.User).Where(x => x.IsActive && x.User.DepartmentId == user.DepartmentId).ToListAsync();
            return View(persmissionStates);
        }

        [Route("tumpersonellerinyıllıkizinleri")]
        [Authorize(Policy = "AdminPolicy")]
        // all permissions administrative.StartingDate,administrative.StartingTime,administrative.ExpirationTime,administrative.TotalTime
        public async Task<IActionResult> Index3()
        {
            //var query = from user in _context.Users.ToList()
            //            join yearly in _context.UsedYearlyPermissions.ToList() on user.Id equals yearly.UserId
            //            select new 
            //            {
            //                user.Name,user.Surname,yearly.WhichYears,yearly.StartingDate,yearly.ExpirationDate,yearly.NumberOfDay,yearly.Address,yearly.Description,user.CreatedDate
            //            };
            //query = query.OrderByDescending(x=>x.CreatedDate);
            var user = await _serviceUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString());
            return View(await _serviceUsedYearlyPermission.GetQueryable().Include(u=>u.User).Where(x=>x.User.DepartmentId == user.DepartmentId).ToListAsync());
        }

        [Route("tumpersonellerinidariizinleri")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Index4()
        {
            //var query = from user in _context.Users.ToList()
            //            join administrative in _context.UsedAdministrivePermissions.ToList() on user.Id equals administrative.UserId
            //            select new
            //            {
            //                user.Name,
            //                user.Surname,
            //                administrative.WhichYears,
            //                administrative.StartingDate,
            //                administrative.StartingTime,
            //                administrative.ExpirationTime,
            //                administrative.TotalTime,
            //                administrative.Address,
            //                administrative.Description,
            //                user.CreatedDate
            //            };
            //query = query.OrderByDescending(m => m.CreatedDate);

            var user = await _serviceUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString());
            return View(await _serviceUsedAdministrivePermission.GetQueryable().Include(u => u.User).Where(x => x.User.DepartmentId == user.DepartmentId).ToListAsync());
        }
    }
}
