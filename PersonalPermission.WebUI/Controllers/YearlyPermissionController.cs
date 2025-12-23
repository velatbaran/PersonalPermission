using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalPermission.Core.Entities;
using PersonalPermission.Service.IService;
using PersonalPermission.WebUI.Models;

namespace PersonalPermission.WebUI.Controllers
{
    [Authorize]
    public class YearlyPermissionController : Controller
    {
        private readonly IService<PermissionState> _servicePermissionState;
        private readonly IService<UsedYearlyPermission> _serviceUsedYearlyPermission;
        private readonly IService<User> _serviceUser;
     //   private readonly string guid = "f31604a2-e868-4665-8320-dd80e3af7e23";

        public YearlyPermissionController(IService<PermissionState> servicePermissionState, IService<UsedYearlyPermission> serviceUsedYearlyPermission, IService<User> serviceUser)
        {
            _servicePermissionState = servicePermissionState;
            _serviceUsedYearlyPermission = serviceUsedYearlyPermission;
            _serviceUser = serviceUser;
        }

        [Route("yillikizinlerim")]
        public async Task<IActionResult> IndexAsync()
        {
            //  var user = _serviceUser.FindAsync(UserGuid)
            return View(await _serviceUsedYearlyPermission.GetQueryable().Include(u => u.User).Where(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString()).ToListAsync());
        }

        [Route("yillikizinekle")]
        [HttpGet]
        public IActionResult Create()
        {
            var userPermissionState = _servicePermissionState.GetQueryable().Include(u => u.User).Where(x =>x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString() && x.RemainYearlyPermission != 0 && x.IsActive).ToList().OrderBy(x => x.WhichYears).Take(1).FirstOrDefault();

            ViewData["WhichYears"] = userPermissionState.WhichYears;
            ViewData["RemainDays"] = userPermissionState.RemainYearlyPermission;

            return View();
        }

        [Route("yillikizinekle")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(UsedYearlyPermission model)
        {
            var _userPermissionState = _servicePermissionState.GetQueryable().Include(u => u.User).Where(x =>  x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString() && x.RemainYearlyPermission != 0 && x.IsActive).ToList().OrderBy(x => x.WhichYears).Take(1).FirstOrDefault();
            if (ModelState.IsValid)
            {

                int days = (model.ExpirationDate - model.StartingDate).Days;
                if ((days != model.NumberOfDay) || (_userPermissionState.RemainYearlyPermission < model.NumberOfDay))
                {
                    if (days != model.NumberOfDay)
                    {
                        ModelState.AddModelError("", "Gün sayısı Başlama ve Bitiş tarihleri farkından küçük yada büyük olamaz.");
                    }
                    if (_userPermissionState.RemainYearlyPermission < model.NumberOfDay)
                    {
                        ModelState.AddModelError("", "Gün sayısı talep edilen yılın kalan günün sayısından büyük olamaz.");
                    }

                    ViewData["WhichYears"] = _userPermissionState.WhichYears;
                    ViewData["RemainDays"] = _userPermissionState.RemainYearlyPermission;
                    return View(model);
                }
                else
                {
                    _userPermissionState.RemainYearlyPermission -= model.NumberOfDay;
                    _userPermissionState.UsedYearlyPermission += model.NumberOfDay;
                    UsedYearlyPermission _usedYearlyPermission = new UsedYearlyPermission()
                    {
                        UserId = _userPermissionState.UserId, // // ToDo List : Authentication part
                        UserGuid = _userPermissionState.UserGuid, // ToDo List : Authentication part
                        StartingDate = model.StartingDate,
                        ExpirationDate = model.ExpirationDate,
                        NumberOfDay = model.NumberOfDay,
                        Description = model.Description,
                        Address = model.Address,
                        WhichYears = model.WhichYears,
                    };
                    _servicePermissionState.Update(_userPermissionState);
                    await _serviceUsedYearlyPermission.AddAsync(_usedYearlyPermission);
                    await _serviceUsedYearlyPermission.SaveChangesAsync();
                    await _servicePermissionState.SaveChangesAsync();

                    return RedirectToAction("Index");
                }

            }


       //     var userPermissionState = _servicePermissionState.GetQueryable().Include(u => u.User).Where(x => x.IsActive == true && x.User.IsActive == true && x.UserGuid.ToString() == guid && x.RemainYearlyPermission != 0).ToList().OrderBy(x => x.WhichYears).Take(1).FirstOrDefault();
            ViewData["WhichYears"] = _userPermissionState.WhichYears;
            ViewData["RemainDays"] = _userPermissionState.RemainYearlyPermission;
            return View(model);
        }

        [Route("yillikizinguncelle/{id?}")]
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var userUsedYearlyPermission = _serviceUsedYearlyPermission.Find(id.Value);
       //     var userPermissionStateNew = _servicePermissionState.GetQueryable().Include(u => u.User).Where(x => x.IsActive == true && x.User.IsActive == true && x.UserId == 31 && x.RemainYearlyPermission != 0).ToList().OrderBy(x => x.WhichYears).Take(1).FirstOrDefault();
            var userPermissionStateOld = _servicePermissionState.GetQueryable().Include(u => u.User).Where(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString() && x.WhichYears == userUsedYearlyPermission.WhichYears && x.IsActive).FirstOrDefault();
            userPermissionStateOld.RemainYearlyPermission += userUsedYearlyPermission.NumberOfDay;

            ViewData["WhichYears"] = userPermissionStateOld.WhichYears;
            ViewData["RemainDays"] = userPermissionStateOld.RemainYearlyPermission;

            return View(userUsedYearlyPermission);
        }

        [Route("yillikizinguncelle/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id,UsedYearlyPermission model)
        {
            var userUsedYearlyPermission = _serviceUsedYearlyPermission.Find(model.Id);
            //     var userPermissionStateNew = _servicePermissionState.GetQueryable().Include(u => u.User).Where(x => x.IsActive == true && x.User.IsActive == true && x.UserId == 31 && x.RemainYearlyPermission != 0).ToList().OrderBy(x => x.WhichYears).Take(1).FirstOrDefault();
            var _userPermissionStateOld = _servicePermissionState.GetQueryable().Include(u => u.User).Where(x =>  x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString() && x.WhichYears == model.WhichYears && x.IsActive).FirstOrDefault();
            _userPermissionStateOld.RemainYearlyPermission += userUsedYearlyPermission.NumberOfDay;
            _userPermissionStateOld.UsedYearlyPermission -= userUsedYearlyPermission.NumberOfDay;

            if (ModelState.IsValid)
            {
                int days = (model.ExpirationDate - model.StartingDate).Days;
                if ((days != model.NumberOfDay) || (_userPermissionStateOld.RemainYearlyPermission < model.NumberOfDay))
                {

                    if (days != model.NumberOfDay)
                    {
                        ModelState.AddModelError("", "Gün sayısı Başlama ve Bitiş tarihleri farkından küçük yada büyük olamaz.");
                    }
                    if (_userPermissionStateOld.RemainYearlyPermission < model.NumberOfDay)
                    {
                        ModelState.AddModelError("", "Gün sayısı talep edilen yılın kalan günün sayısından büyük olamaz.");
                    }

                    ViewData["WhichYears"] = _userPermissionStateOld.WhichYears;
                    ViewData["RemainDays"] = _userPermissionStateOld.RemainYearlyPermission;
                    return View(model);
                }
                else
                {
                    _userPermissionStateOld.RemainYearlyPermission -= model.NumberOfDay;
                    _userPermissionStateOld.UsedYearlyPermission += model.NumberOfDay;

                    userUsedYearlyPermission.UserId = _userPermissionStateOld.UserId; // // ToDo List : Authentication part
                    userUsedYearlyPermission.UserGuid = _userPermissionStateOld.UserGuid; // ToDo List : Authentication part
                    userUsedYearlyPermission.StartingDate = model.StartingDate;
                    userUsedYearlyPermission.ExpirationDate = model.ExpirationDate;
                    userUsedYearlyPermission.NumberOfDay = model.NumberOfDay;
                    userUsedYearlyPermission.Description = model.Description;
                    userUsedYearlyPermission.Address = model.Address;
                    userUsedYearlyPermission.WhichYears = model.WhichYears;
                    _servicePermissionState.Update(_userPermissionStateOld);
                    _serviceUsedYearlyPermission.Update(userUsedYearlyPermission);
                    await _serviceUsedYearlyPermission.SaveChangesAsync();
                    await _servicePermissionState.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }


            ViewData["WhichYears"] = _userPermissionStateOld.WhichYears;
            ViewData["RemainDays"] = _userPermissionStateOld.RemainYearlyPermission;

            return View(model);
        }

        [Route("yillikizinsil/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userUsedYearlyPermission = _serviceUsedYearlyPermission.GetQueryable().Include(u => u.User).Where(x => x.Id == id.Value).FirstOrDefault();
            if (userUsedYearlyPermission == null)
            {
                return NotFound();
            }
            return View(userUsedYearlyPermission);
        }

        [Route("yillikizinsil/{id?}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userUsedYearlyPermission = _serviceUsedYearlyPermission.Find(id);
            if (userUsedYearlyPermission != null)
            {
                var _userPermissionState = _servicePermissionState.GetQueryable().Include(u => u.User).Where(x => x.UserGuid == userUsedYearlyPermission.UserGuid && x.WhichYears == userUsedYearlyPermission.WhichYears && x.IsActive).FirstOrDefault();
                _userPermissionState.RemainYearlyPermission += userUsedYearlyPermission.NumberOfDay;
                _userPermissionState.UsedYearlyPermission -= userUsedYearlyPermission.NumberOfDay;
                _serviceUsedYearlyPermission.Delete(userUsedYearlyPermission);
                _servicePermissionState.Update(_userPermissionState);
            }
            await _serviceUsedYearlyPermission.SaveChangesAsync();
            await _servicePermissionState.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Route("yillikizindetay/{id?}")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userUsedYearlyPermission = _serviceUsedYearlyPermission.GetQueryable().Include(u => u.User).Where(x => x.Id == id.Value).FirstOrDefault();
            if (userUsedYearlyPermission == null)
            {
                return NotFound();
            }

            return View(userUsedYearlyPermission);
        }
    }
}
