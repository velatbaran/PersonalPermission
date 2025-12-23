using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using PersonalPermission.Core.Entities;
using PersonalPermission.Data;
using PersonalPermission.Service.Concrete;
using PersonalPermission.Service.IService;

namespace PersonalPermission.WebUI.Controllers
{
    [Authorize]
    [Authorize(Policy = "AdminPolicy")]
    public class UsersController : Controller
    {
        private readonly IService<User> _serviceUser;
        private readonly IService<PermissionState> _servicePermissionState;
        private readonly IService<UsedAdministrivePermission> _serviceUsedAdministrivePermission;
        private readonly IService<UsedYearlyPermission> _serviceUsedYearlyPermission;
        private readonly IService<Department> _serviceDepartment;
        private readonly IService<Title> _serviceTitle;
        private readonly IToastNotification _toastNotification;

        public UsersController(IService<User> serviceUser, IService<PermissionState> servicePermissionState, IService<Department> serviceDepartment, IToastNotification toastNotification, IService<UsedAdministrivePermission> serviceUsedAdministrivePermission, IService<UsedYearlyPermission> serviceUsedYearlyPermission, IService<Title> serviceTitle)
        {
            _serviceUser = serviceUser;
            _servicePermissionState = servicePermissionState;
            _serviceDepartment = serviceDepartment;
            _toastNotification = toastNotification;
            _serviceUsedAdministrivePermission = serviceUsedAdministrivePermission;
            _serviceUsedYearlyPermission = serviceUsedYearlyPermission;
            _serviceTitle = serviceTitle;
        }

        // GET: Users
        [Route("kullanicilar")]
        public async Task<IActionResult> Index()
        {
            User user = await _serviceUser.GetQueryable().Include(x => x.Department).Include(x => x.Title).Where(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString()).FirstOrDefaultAsync();
            if (user != null)
            {
                if (user.Department.Name == "BİLGİ TEKNOLOJİLERİ ŞUBE MÜDÜRLÜĞÜ")
                    return View(await _serviceUser.GetQueryable().Include(x => x.Title).Include(x => x.Department).OrderByDescending(x => x.CreatedDate).ToListAsync());
                else
                    return View(await _serviceUser.GetQueryable().Include(x => x.Title).Include(x => x.Department).Where(x => x.Department.Name == user.Department.Name).OrderByDescending(x => x.CreatedDate).ToListAsync());
            }
            else
            {
                return View();
            }

        }

        // GET: Users/Details/5
        [Route("kullanicidetay/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _serviceUser.GetAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        [Route("kullaniciekle")]
        public async Task<IActionResult> Create()
        {
            User user = await _serviceUser.GetQueryable().Include(x => x.Department).Include(x => x.Title).Where(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString()).FirstOrDefaultAsync();
            if (user != null)
            {
                if (user.Department.Name == "BİLGİ TEKNOLOJİLERİ ŞUBE MÜDÜRLÜĞÜ")
                    ViewData["DepartmentId"] = new SelectList(_serviceDepartment.GetAll(), "Id", "Name");
                else
                    ViewData["DepartmentId"] = new SelectList(_serviceDepartment.GetAll(x => x.Name == user.Department.Name), "Id", "Name");
            }
            ViewData["TitleId"] = new SelectList(_serviceTitle.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Users/Create
        [Route("kullaniciekle")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            // user.CreatedDate = DateTime.Now;
            User __user = await _serviceUser.GetQueryable().Include(x => x.Department).Include(x => x.Title).Where(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString()).FirstOrDefaultAsync();
            if (ModelState.IsValid)
            {
                if (await _serviceUser.AnyAsync(x => x.Username == user.Username))
                {
                    _toastNotification.AddErrorToastMessage("Aynı kullanıcı adı sistemde kayıtlı. Lütfen başka bir kullanıcı adı giriniz!", new ToastrOptions { Title = "Hata" });
                    if (user.Department.Name == "BİLGİ TEKNOLOJİLERİ ŞUBE MÜDÜRLÜĞÜ")
                        ViewData["DepartmentId"] = new SelectList(_serviceDepartment.GetAll(), "Id", "Name");
                    else
                        ViewData["DepartmentId"] = new SelectList(_serviceDepartment.GetAll(x => x.Name == __user.Department.Name), "Id", "Name");
                    ViewData["TitleId"] = new SelectList(_serviceTitle.GetAll(), "Id", "Name");
                    return View(user);

                }

                //var _userServiceTime = new UserServiceTime();
                //var result = _userServiceTime.CalculatingServiceTime(user.StartingWorkDate);
                int gainedYearlyPermission = 0;
                //     int gainedAdministrativePermission = 0;
                int years = DateTime.Now.Year - user.StartingWorkDate.Year;
                int months = DateTime.Now.Month - user.StartingWorkDate.Month;
                int days = DateTime.Now.Day - user.StartingWorkDate.Day;

                if (days < 0)
                {
                    months--;
                    days += DateTime.DaysInMonth(user.StartingWorkDate.Year, user.StartingWorkDate.Month);
                }

                if (months < 0)
                {
                    years--;
                    months += 12;
                }

                //   user.UserGuid = Guid.NewGuid();
                user.ServiceTimeDay = days;
                user.ServiceTimeMonth = months;
                user.ServiceTimeYear = years;

                await _serviceUser.AddAsync(user);
                await _serviceUser.SaveChangesAsync();


                var title = await _serviceTitle.GetAsync(x => x.Id == user.TitleId);

                if (title.Name != "İşçi")
                {
                    if (user.ServiceTimeYear < 1)
                    {
                        gainedYearlyPermission = 0;
                        // gainedAdministrativePermission = 0;
                        PermissionState permissionState = new PermissionState()
                        {
                            UserId = user.Id,
                            UserGuid = user.UserGuid,
                            GainedYearlyPermission = gainedYearlyPermission,
                            //     GainedAdministrativePermission = gainedAdministrativePermission,
                            RemainYearlyPermission = gainedYearlyPermission,
                            //   RemainAdministrativePermission = gainedAdministrativePermission,
                            UsedYearlyPermission = 0,
                            UsedAdministrativePermission = 0,
                            WhichYears = user.StartingWorkDate.Year,
                            IsActive = true,
                            AddedDate = DateTime.Now,
                        };
                        await _servicePermissionState.AddAsync(permissionState);
                        await _servicePermissionState.SaveChangesAsync();
                        _toastNotification.AddSuccessToastMessage("Kayıt işlemi başarılı", new ToastrOptions { Title = "Başarılı" });
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        if (user.ServiceTimeYear >= 1 && user.ServiceTimeYear < 10)
                        {
                            gainedYearlyPermission = 20;
                            //     gainedAdministrativePermission = 0;
                        }
                        if (user.ServiceTimeYear >= 10)
                        {
                            gainedYearlyPermission = 30;
                            //  gainedAdministrativePermission = 0;
                        }

                        bool isActive;
                        for (int i = years; i >= 0; i--)
                        {
                            if (i >= 2) isActive = false;
                            else isActive = true;
                            PermissionState permissionState = new PermissionState()
                            {
                                UserId = user.Id,
                                UserGuid = user.UserGuid,
                                GainedYearlyPermission = gainedYearlyPermission,
                                //          GainedAdministrativePermission = gainedAdministrativePermission,
                                RemainYearlyPermission = gainedYearlyPermission,
                                //         RemainAdministrativePermission = gainedAdministrativePermission,
                                UsedYearlyPermission = 0,
                                UsedAdministrativePermission = 0,
                                WhichYears = DateTime.Now.Year - i,
                                IsActive = isActive,
                                AddedDate = DateTime.Now,
                            };
                            await _servicePermissionState.AddAsync(permissionState);
                        }
                        await _servicePermissionState.SaveChangesAsync();
                        _toastNotification.AddSuccessToastMessage("Kayıt işlemi başarılı", new ToastrOptions { Title = "Başarılı" });
                        return RedirectToAction(nameof(Index));

                    }
                }
                else
                {
                    if (user.ServiceTimeYear < 1)
                    {
                        gainedYearlyPermission = 0;
                        // gainedAdministrativePermission = 0;
                        PermissionState permissionState = new PermissionState()
                        {
                            UserId = user.Id,
                            UserGuid = user.UserGuid,
                            GainedYearlyPermission = gainedYearlyPermission,
                            //     GainedAdministrativePermission = gainedAdministrativePermission,
                            RemainYearlyPermission = gainedYearlyPermission,
                            //   RemainAdministrativePermission = gainedAdministrativePermission,
                            UsedYearlyPermission = 0,
                            UsedAdministrativePermission = 0,
                            WhichYears = user.StartingWorkDate.Year,
                            IsActive = true,
                            AddedDate = DateTime.Now,
                        };
                        await _servicePermissionState.AddAsync(permissionState);
                        await _servicePermissionState.SaveChangesAsync();
                        _toastNotification.AddSuccessToastMessage("Kayıt işlemi başarılı", new ToastrOptions { Title = "Başarılı" });
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        if (user.ServiceTimeYear >= 1 && user.ServiceTimeYear < 5)
                        {
                            gainedYearlyPermission = 18;
                            //     gainedAdministrativePermission = 0;
                        }
                        if (user.ServiceTimeYear >= 5 && user.ServiceTimeYear < 10)
                        {
                            gainedYearlyPermission = 25;
                            //  gainedAdministrativePermission = 0;
                        }
                        if (user.ServiceTimeYear >= 10)
                        {
                            gainedYearlyPermission = 30;
                            //  gainedAdministrativePermission = 0;
                        }

                        for (int i = years; i >= 0; i--)
                        {
                            PermissionState permissionState = new PermissionState()
                            {
                                UserId = user.Id,
                                UserGuid = user.UserGuid,
                                GainedYearlyPermission = gainedYearlyPermission,
                                //          GainedAdministrativePermission = gainedAdministrativePermission,
                                RemainYearlyPermission = gainedYearlyPermission,
                                //         RemainAdministrativePermission = gainedAdministrativePermission,
                                UsedYearlyPermission = 0,
                                UsedAdministrativePermission = 0,
                                WhichYears = DateTime.Now.Year - i,
                                IsActive = true,
                                AddedDate = DateTime.Now,
                            };
                            await _servicePermissionState.AddAsync(permissionState);
                        }
                        await _servicePermissionState.SaveChangesAsync();
                        _toastNotification.AddSuccessToastMessage("Kayıt işlemi başarılı", new ToastrOptions { Title = "Başarılı" });
                        return RedirectToAction(nameof(Index));

                    }
                }

            }
            if (user.Department.Name == "BİLGİ TEKNOLOJİLERİ ŞUBE MÜDÜRLÜĞÜ")
                ViewData["DepartmentId"] = new SelectList(_serviceDepartment.GetAll(), "Id", "Name");
            else
                ViewData["DepartmentId"] = new SelectList(_serviceDepartment.GetAll(x => x.Name == __user.Department.Name), "Id", "Name");
            ViewData["TitleId"] = new SelectList(_serviceTitle.GetAll(), "Id", "Name", user.TitleId);
            return View(user);
        }

        // GET: Users/Edit/5
        [Route("kullaniciguncelle/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var user = await _serviceUser.FindAsync(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            if (await _serviceUsedYearlyPermission.AnyAsync(x => x.UserId == user.Id))
            {
                _toastNotification.AddWarningToastMessage("Daha önce yıllık izin işlemi yapıldığı için güncelleme yapılamaz. ", new ToastrOptions { Title = "Uyarı" });
                return View(user);
            }
            User __user = await _serviceUser.GetQueryable().Include(x => x.Department).Include(x => x.Title).Where(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString()).FirstOrDefaultAsync();
            if (__user.Department.Name == "BİLGİ TEKNOLOJİLERİ ŞUBE MÜDÜRLÜĞÜ")
                ViewData["DepartmentId"] = new SelectList(_serviceDepartment.GetAll(), "Id", "Name");
            else
                ViewData["DepartmentId"] = new SelectList(_serviceDepartment.GetAll(x => x.Name == __user.Department.Name), "Id", "Name");
            //  ViewData["DepartmentId"] = new SelectList(_serviceDepartment.GetAll(), "Id", "Name", user.DepartmentId);
            ViewData["TitleId"] = new SelectList(_serviceTitle.GetAll(), "Id", "Name", user.TitleId);
            return View(user);
        }

        // POST: Users/Edit/5
        [Route("kullaniciguncelle/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            User __user = await _serviceUser.GetQueryable().Include(x => x.Department).Include(x => x.Title).Where(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString()).FirstOrDefaultAsync();
            if (ModelState.IsValid)
            {
                if (await _serviceUser.AnyAsync(x => x.Username == user.Username && x.Id != user.Id))
                {
                    _toastNotification.AddErrorToastMessage("Aynı kullanıcı adı sistemde kayıtlı. Lütfen başka bir kullanıcı adı giriniz!", new ToastrOptions { Title = "Hata" });
                    if (__user.Department.Name == "BİLGİ TEKNOLOJİLERİ ŞUBE MÜDÜRLÜĞÜ")
                        ViewData["DepartmentId"] = new SelectList(_serviceDepartment.GetAll(), "Id", "Name");
                    else
                        ViewData["DepartmentId"] = new SelectList(_serviceDepartment.GetAll(x => x.Name == __user.Department.Name), "Id", "Name");
                    ViewData["TitleId"] = new SelectList(_serviceTitle.GetAll(), "Id", "Name");
                    return View(user);

                }
                try
                {
                    var _user = await _serviceUser.FindAsync(id);

                    int gainedYearlyPermission = 0;
                    //  int gainedAdministrativePermission = 0;
                    int years = DateTime.Now.Year - user.StartingWorkDate.Year;
                    int months = DateTime.Now.Month - user.StartingWorkDate.Month;
                    int days = DateTime.Now.Day - user.StartingWorkDate.Day;

                    if (days < 0)
                    {
                        months--;
                        days += DateTime.DaysInMonth(user.StartingWorkDate.Year, user.StartingWorkDate.Month);
                    }

                    if (months < 0)
                    {
                        years--;
                        months += 12;
                    }

                    user.ServiceTimeDay = days;
                    user.ServiceTimeMonth = months;
                    user.ServiceTimeYear = years;

                    _user.Name = user.Name;
                    _user.Surname = user.Surname;
                    _user.Username = user.Username;
                    _user.RegistryNo = user.RegistryNo;
                    _user.TitleId = user.TitleId;
                    _user.DepartmentId = user.DepartmentId;
                    _user.StartingWorkDate = user.StartingWorkDate;
                    _user.Password = user.Password;
                    _user.ServiceTimeDay = user.ServiceTimeDay;
                    _user.ServiceTimeMonth = user.ServiceTimeMonth;
                    _user.ServiceTimeYear = user.ServiceTimeYear;
                    _user.IsAdmin = user.IsAdmin;

                    var userPermissionState = await _servicePermissionState.GetAllAsync(x => x.UserId == _user.Id);
                    _servicePermissionState.Delete(userPermissionState);
                    await _servicePermissionState.SaveChangesAsync();

                    _serviceUser.Update(_user);
                    await _serviceUser.SaveChangesAsync();

                    var title = await _serviceTitle.GetAsync(x => x.Id == user.TitleId);
                    if (title.Name != "İşçi")
                    {
                        if (user.ServiceTimeYear < 1)
                        {
                            gainedYearlyPermission = 0;
                            // gainedAdministrativePermission = 0;
                            PermissionState permissionState = new PermissionState()
                            {
                                UserId = user.Id,
                                UserGuid = user.UserGuid,
                                GainedYearlyPermission = gainedYearlyPermission,
                                //     GainedAdministrativePermission = gainedAdministrativePermission,
                                RemainYearlyPermission = gainedYearlyPermission,
                                //   RemainAdministrativePermission = gainedAdministrativePermission,
                                UsedYearlyPermission = 0,
                                UsedAdministrativePermission = 0,
                                WhichYears = user.StartingWorkDate.Year,
                                IsActive = true,
                                AddedDate = DateTime.Now,
                            };
                            await _servicePermissionState.AddAsync(permissionState);
                            await _servicePermissionState.SaveChangesAsync();
                            _toastNotification.AddSuccessToastMessage("Kayıt işlemi başarılı", new ToastrOptions { Title = "Başarılı" });
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            if (user.ServiceTimeYear >= 1 && user.ServiceTimeYear < 10)
                            {
                                gainedYearlyPermission = 20;
                                //     gainedAdministrativePermission = 0;
                            }
                            if (user.ServiceTimeYear >= 10)
                            {
                                gainedYearlyPermission = 30;
                                //  gainedAdministrativePermission = 0;
                            }

                            bool isActive;
                            for (int i = years; i >= 0; i--)
                            {
                                if (i >= 2) isActive = false;
                                else isActive = true;
                                PermissionState permissionState = new PermissionState()
                                {
                                    UserId = user.Id,
                                    UserGuid = user.UserGuid,
                                    GainedYearlyPermission = gainedYearlyPermission,
                                    //          GainedAdministrativePermission = gainedAdministrativePermission,
                                    RemainYearlyPermission = gainedYearlyPermission,
                                    //         RemainAdministrativePermission = gainedAdministrativePermission,
                                    UsedYearlyPermission = 0,
                                    UsedAdministrativePermission = 0,
                                    WhichYears = DateTime.Now.Year - i,
                                    IsActive = isActive,
                                    AddedDate = DateTime.Now,
                                };
                                await _servicePermissionState.AddAsync(permissionState);
                            }
                            await _servicePermissionState.SaveChangesAsync();
                            _toastNotification.AddSuccessToastMessage("Kayıt işlemi başarılı", new ToastrOptions { Title = "Başarılı" });
                            return RedirectToAction(nameof(Index));

                        }
                    }
                    else
                    {
                        if (user.ServiceTimeYear < 1)
                        {
                            gainedYearlyPermission = 0;
                            // gainedAdministrativePermission = 0;
                            PermissionState permissionState = new PermissionState()
                            {
                                UserId = user.Id,
                                UserGuid = user.UserGuid,
                                GainedYearlyPermission = gainedYearlyPermission,
                                //     GainedAdministrativePermission = gainedAdministrativePermission,
                                RemainYearlyPermission = gainedYearlyPermission,
                                //   RemainAdministrativePermission = gainedAdministrativePermission,
                                UsedYearlyPermission = 0,
                                UsedAdministrativePermission = 0,
                                WhichYears = user.StartingWorkDate.Year,
                                IsActive = true,
                                AddedDate = DateTime.Now,
                            };
                            await _servicePermissionState.AddAsync(permissionState);
                            await _servicePermissionState.SaveChangesAsync();
                            _toastNotification.AddSuccessToastMessage("Kayıt işlemi başarılı", new ToastrOptions { Title = "Başarılı" });
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            if (user.ServiceTimeYear >= 1 && user.ServiceTimeYear < 5)
                            {
                                gainedYearlyPermission = 18;
                                //     gainedAdministrativePermission = 0;
                            }
                            if (user.ServiceTimeYear >= 5 && user.ServiceTimeYear < 10)
                            {
                                gainedYearlyPermission = 25;
                                //  gainedAdministrativePermission = 0;
                            }
                            if (user.ServiceTimeYear >= 10)
                            {
                                gainedYearlyPermission = 30;
                                //  gainedAdministrativePermission = 0;
                            }

                            for (int i = years; i >= 0; i--)
                            {
                                PermissionState permissionState = new PermissionState()
                                {
                                    UserId = user.Id,
                                    UserGuid = user.UserGuid,
                                    GainedYearlyPermission = gainedYearlyPermission,
                                    //          GainedAdministrativePermission = gainedAdministrativePermission,
                                    RemainYearlyPermission = gainedYearlyPermission,
                                    //         RemainAdministrativePermission = gainedAdministrativePermission,
                                    UsedYearlyPermission = 0,
                                    UsedAdministrativePermission = 0,
                                    WhichYears = DateTime.Now.Year - i,
                                    IsActive = true,
                                    AddedDate = DateTime.Now,
                                };
                                await _servicePermissionState.AddAsync(permissionState);
                            }
                            await _servicePermissionState.SaveChangesAsync();
                            _toastNotification.AddSuccessToastMessage("Kayıt işlemi başarılı", new ToastrOptions { Title = "Başarılı" });
                            return RedirectToAction(nameof(Index));

                        }
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _toastNotification.AddErrorToastMessage(ex.Message, new ToastrOptions { Title = "Hata" });
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            if (__user.Department.Name == "BİLGİ TEKNOLOJİLERİ ŞUBE MÜDÜRLÜĞÜ")
                ViewData["DepartmentId"] = new SelectList(_serviceDepartment.GetAll(), "Id", "Name");
            else
                ViewData["DepartmentId"] = new SelectList(_serviceDepartment.GetAll(x => x.Name == __user.Department.Name), "Id", "Name");
            ViewData["TitleId"] = new SelectList(_serviceTitle.GetAll(), "Id", "Name", user.TitleId);
            return View(user);
        }

        // GET: Users/Delete/5
        [Route("kullanicisil/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _serviceUser.GetAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [Route("kullanicisil/{id?}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _serviceUser.FindAsync(id);
            var userUsedAdministrative = await _serviceUsedAdministrivePermission.GetAsync(x => x.UserGuid == user.UserGuid);
            var userUsedYears = await _serviceUsedYearlyPermission.GetAsync(x => x.UserGuid == user.UserGuid);
            var userPermissionState = await _servicePermissionState.GetAsync(x => x.UserGuid == user.UserGuid);
            if (userUsedAdministrative != null)
            {
                _serviceUsedAdministrivePermission.Delete(userUsedAdministrative);
            }
            if (userUsedYears != null)
            {
                _serviceUsedYearlyPermission.Delete(userUsedYears);
            }
            if (userPermissionState != null)
            {
                _servicePermissionState.Delete(userPermissionState);
            }
            if (user != null)
            {
                _serviceUser.Delete(user);
            }
            await _serviceUsedAdministrivePermission.SaveChangesAsync();
            await _serviceUsedYearlyPermission.SaveChangesAsync();
            await _servicePermissionState.SaveChangesAsync();
            await _serviceUser.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            var user = _serviceUser.GetAsync(e => e.Id == id);
            if (user == null)
            {
                return false;
            }
            return true;
        }
    }
}
