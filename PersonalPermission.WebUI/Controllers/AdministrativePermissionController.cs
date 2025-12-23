using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalPermission.Core.Entities;
using PersonalPermission.Service.IService;

namespace PersonalPermission.WebUI.Controllers
{
    [Authorize]
    public class AdministrativePermissionController : Controller
    {
        private readonly IService<PermissionState> _servicePermissionState;
        private readonly IService<UsedAdministrivePermission> _serviceUsedAdministrivePermission;
        private readonly IService<User> _serviceUser;
        private readonly string guid = "f31604a2-e868-4665-8320-dd80e3af7e23";

        public AdministrativePermissionController(IService<PermissionState> servicePermissionState, IService<UsedAdministrivePermission> serviceUsedAdministrivePermission, IService<User> serviceUser)
        {
            _servicePermissionState = servicePermissionState;
            _serviceUsedAdministrivePermission = serviceUsedAdministrivePermission;
            _serviceUser = serviceUser;
        }

        [Route("idariizinlerim")]
        public async Task<IActionResult> IndexAsync()
        {
            return View(await _serviceUsedAdministrivePermission.GetQueryable().Include(u => u.User).Where(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString()).ToListAsync());
        }

        [Route("idariizinekle")]
        [HttpGet]
        public IActionResult Create()
        {
            //var userPermissionState = _servicePermissionState.GetQueryable().Include(u => u.User).Where(x=>x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString() && x.RemainAdministrativePermission != 0).ToList().OrderBy(x => x.WhichYears).Take(1).FirstOrDefault();

           // ViewData["WhichYears"] = userPermissionState.WhichYears;
            //ViewData["RemainHours"] = userPermissionState.RemainAdministrativePermission;

            return View();
        }

        [Route("idariizinekle")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(UsedAdministrivePermission model)
        {
            //var _userPermissionState = _servicePermissionState.GetQueryable().Include(u => u.User).Where(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString() && x.RemainYearlyPermission != 0 && x.IsActive).ToList().OrderBy(x => x.WhichYears).Take(1).FirstOrDefault();
            var _userPermissionState = _servicePermissionState.GetQueryable().Include(u => u.User).Where(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString() && x.WhichYears == DateTime.Now.Year && x.IsActive).FirstOrDefault();
            var user = _serviceUser.GetQueryable().Where(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString()).FirstOrDefault();
            if (ModelState.IsValid)
            {
                int hours = model.ExpirationTime - model.StartingTime;
                if ((hours != model.TotalTime) /*|| (_userPermissionState.RemainAdministrativePermission < model.TotalTime)*/)
                {
                    //if (_userPermissionState.RemainAdministrativePermission < model.TotalTime)
                    //{
                    //    ModelState.AddModelError("", "Saat sayısı talep edilen yılın kalan saat sayısından büyük olamaz.");
                    //}

                    if (hours != model.TotalTime)
                    {
                        ModelState.AddModelError("", "Saat sayısı Başlama ve Bitiş saatleri farkından küçük yada büyük olamaz.");
                    }

                    //ViewData["WhichYears"] = _userPermissionState.WhichYears;
                    //ViewData["RemainHours"] = _userPermissionState.RemainAdministrativePermission;
                    return View(model);
                }

                else
                {
                    //_userPermissionState.RemainAdministrativePermission -= model.TotalTime;
                    _userPermissionState.UsedAdministrativePermission += model.TotalTime;
                    UsedAdministrivePermission _usedAdministrivePermission = new UsedAdministrivePermission()
                    {
                        UserId = user.Id, // // ToDo List : Authentication part
                        UserGuid = user.UserGuid, // ToDo List : Authentication part
                        StartingDate = model.StartingDate,
                        StartingTime = model.StartingTime,
                        ExpirationTime = model.ExpirationTime,
                        TotalTime = model.TotalTime,
                        Description = model.Description,
                        Address = model.Address,
                        WhichYears = model.WhichYears
                    };
                    _servicePermissionState.Update(_userPermissionState);
                    await _serviceUsedAdministrivePermission.AddAsync(_usedAdministrivePermission);
                    await _serviceUsedAdministrivePermission.SaveChangesAsync();
                    await _servicePermissionState.SaveChangesAsync();

                    return RedirectToAction("Index");
                }

            }
            //     var userPermissionState = _servicePermissionState.GetQueryable().Include(u => u.User).Where(x => x.IsActive == true && x.User.IsActive == true && x.UserGuid.ToString() == guid && x.RemainYearlyPermission != 0).ToList().OrderBy(x => x.WhichYears).Take(1).FirstOrDefault();
            ViewData["WhichYears"] = _userPermissionState.WhichYears;
           // ViewData["RemainHours"] = _userPermissionState.RemainAdministrativePermission;
            return View(model);
        }

        [Route("idariizinguncelle/{id?}")]
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var userUsedAdministrivePermission = _serviceUsedAdministrivePermission.Find(id.Value);
            //     var userPermissionStateNew = _servicePermissionState.GetQueryable().Include(u => u.User).Where(x => x.IsActive == true && x.User.IsActive == true && x.UserId == 31 && x.RemainYearlyPermission != 0).ToList().OrderBy(x => x.WhichYears).Take(1).FirstOrDefault();
        //    userPermissionStateOld.RemainAdministrativePermission += userUsedAdministrivePermission.TotalTime;

           // ViewData["WhichYears"] = userPermissionStateOld.WhichYears;
           // ViewData["RemainHours"] = userPermissionStateOld.RemainAdministrativePermission;

            return View(userUsedAdministrivePermission);
        }

        [Route("idariizinguncelle/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, UsedAdministrivePermission model)
        {
            var userUsedAdministrivePermission = _serviceUsedAdministrivePermission.Find(model.Id);
            //     var userPermissionStateNew = _servicePermissionState.GetQueryable().Include(u => u.User).Where(x => x.IsActive == true && x.User.IsActive == true && x.UserId == 31 && x.RemainYearlyPermission != 0).ToList().OrderBy(x => x.WhichYears).Take(1).FirstOrDefault();
            var _userPermissionStateOld = _servicePermissionState.GetQueryable().Include(u => u.User).Where(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString() && x.WhichYears == model.WhichYears).FirstOrDefault();
         //   _userPermissionStateOld.RemainAdministrativePermission += userUsedAdministrivePermission.TotalTime;
            _userPermissionStateOld.UsedAdministrativePermission -= userUsedAdministrivePermission.TotalTime;

            if (ModelState.IsValid)
            {
                int hours = model.ExpirationTime - model.StartingTime;
                if ((hours != model.TotalTime) /*|| (_userPermissionStateOld.RemainAdministrativePermission < model.TotalTime)*/)
                {
                    //if (_userPermissionStateOld.RemainAdministrativePermission < model.TotalTime)
                    //{
                    //    ModelState.AddModelError("", "Saat sayısı talep edilen yılın kalan saat sayısından büyük olamaz.");
                    //}

                    if (hours != model.TotalTime)
                    {
                        ModelState.AddModelError("", "Saat sayısı Başlama ve Bitiş saatleri farkından küçük yada büyük olamaz.");
                    }

                  //  ViewData["WhichYears"] = _userPermissionStateOld.WhichYears;
              //      ViewData["RemainHours"] = _userPermissionStateOld.RemainAdministrativePermission;
                    return View(model);
                }
                else
                {
                 //   _userPermissionStateOld.RemainAdministrativePermission -= model.TotalTime;
                    _userPermissionStateOld.UsedAdministrativePermission += model.TotalTime;

                    userUsedAdministrivePermission.UserId = _userPermissionStateOld.UserId; // // ToDo List : Authentication part
                    userUsedAdministrivePermission.UserGuid = _userPermissionStateOld.UserGuid; // ToDo List : Authentication part
                    userUsedAdministrivePermission.StartingDate = model.StartingDate;
                    userUsedAdministrivePermission.StartingTime = model.StartingTime;
                    userUsedAdministrivePermission.ExpirationTime = model.ExpirationTime;
                    userUsedAdministrivePermission.TotalTime = model.TotalTime;
                    userUsedAdministrivePermission.Description = model.Description;
                    userUsedAdministrivePermission.Address = model.Address;
                    userUsedAdministrivePermission.WhichYears = model.WhichYears;
                    _servicePermissionState.Update(_userPermissionStateOld);
                    _serviceUsedAdministrivePermission.Update(userUsedAdministrivePermission);
                    await _serviceUsedAdministrivePermission.SaveChangesAsync();
                    await _servicePermissionState.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }

       //     ViewData["WhichYears"] = _userPermissionStateOld.WhichYears;
         //   ViewData["RemainHours"] = _userPermissionStateOld.RemainAdministrativePermission;

            return View(model);
        }

        [Route("idariizinsil/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userUsedAdministrativePermission = _serviceUsedAdministrivePermission.GetQueryable().Include(u => u.User).Where(x => x.Id == id.Value).FirstOrDefault();
            if (userUsedAdministrativePermission == null)
            {
                return NotFound();
            }
            return View(userUsedAdministrativePermission);
        }

        [Route("idariizinsil/{id?}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userUsedAdministrativePermission = _serviceUsedAdministrivePermission.Find(id);
            if (userUsedAdministrativePermission != null)
            {
                var _userPermissionState = _servicePermissionState.GetQueryable().Include(u => u.User).Where(x => x.UserGuid == userUsedAdministrativePermission.UserGuid && x.WhichYears == userUsedAdministrativePermission.WhichYears).FirstOrDefault();
            //    _userPermissionState.RemainAdministrativePermission += userUsedAdministrativePermission.TotalTime;
                _userPermissionState.UsedAdministrativePermission -= userUsedAdministrativePermission.TotalTime;
                _serviceUsedAdministrivePermission.Delete(userUsedAdministrativePermission);
                _servicePermissionState.Update(_userPermissionState);
            }
            await _serviceUsedAdministrivePermission.SaveChangesAsync();
            await _servicePermissionState.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Route("idariizindetay/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userUsedAdministrativePermission = _serviceUsedAdministrivePermission.GetQueryable().Include(u => u.User).Where(x => x.Id == id.Value).FirstOrDefault();
            if (userUsedAdministrativePermission == null)
            {
                return NotFound();
            }
            return View(userUsedAdministrativePermission);
        }
    }
}
