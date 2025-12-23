using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using PersonalPermission.Core.Entities;
using PersonalPermission.Service.Concrete;
using PersonalPermission.Service.IService;
using PersonalPermission.WebUI.Models;
using System.Security.Claims;

namespace PersonalPermission.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IService<User> _serviceUser;
        private readonly IToastNotification _toastNotification;

        public AccountController(IService<User> serviceUser, IToastNotification toastNotification)
        {
            _serviceUser = serviceUser;
            _toastNotification = toastNotification;
        }

        //[Authorize]
        //public async Task<IActionResult> MyProfileAsync()
        //{
        //    User user = await _serviceUser.GetQueryable().Include(x=>x.Department).Include(x=>x.Title).Where(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value.ToString()).FirstOrDefaultAsync();
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    var model = new MyProfileViewModel()
        //    {
        //        Username = user.Username,
        //        Department = user.Department.Name,
        //        Name = user.Name,
        //        Surname = user.Surname,
        //        Title = user.Title.Name
        //    };
        //    return View(model);
        //}

        [AllowAnonymous]
        [Route("giris")]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("giris")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = await _serviceUser.GetQueryable().Include(x => x.Department).Include(x => x.Title).Where(x => x.Username == model.Username && x.Password == model.Password).FirstOrDefaultAsync();
                    if (account == null)
                    {

                        _toastNotification.AddErrorToastMessage("Kullanıcı adı veya şifre hatalı", new ToastrOptions { Title = "Hatalı" });
                    }
                    else
                    {
                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, $"{account.Name} {account.Surname}"),
                            new Claim(ClaimTypes.GivenName, account.Name),
                            new Claim(ClaimTypes.Surname, account.Surname),
                            new Claim(ClaimTypes.Role, account.IsAdmin ? "Admin" : "Standart"),

                            new Claim("Username", account.Username),
                            new Claim("Department", account.Department.Name),
                            new Claim("Title", account.Title.Name),
                            new Claim("UserId", account.Id.ToString()),
                            new Claim("UserGuid", account.UserGuid.ToString())
                        };

                        var userIdentity = new ClaimsIdentity(claims, "Login");
                        ClaimsPrincipal userPrincipal = new ClaimsPrincipal(userIdentity);
                        await HttpContext.SignInAsync(userPrincipal);
                        _toastNotification.AddSuccessToastMessage("Giriş işlemi başarılı", new ToastrOptions { Title = "Başarılı" });
                        return Redirect(string.IsNullOrEmpty(model.ReturnUrl) ? "/anasayfa" : model.ReturnUrl);
                    }
                }
                catch (Exception)
                {
                    // loglama
                    _toastNotification.AddErrorToastMessage("Lütfen kullanıcı bilgilerini kontrol ediniz", new ToastrOptions { Title = "Hatalı" });
                    return View(model);
                }
            }
            return View(model);
        }

        [Route("cikis")]
        public async Task<IActionResult> LogOutAsync()
        {
            await HttpContext.SignOutAsync();
            _toastNotification.AddSuccessToastMessage("Çıkış işlemi başarılı", new ToastrOptions { Title = "Başarılı" });
            return Redirect("/giris");
        }

        [Route("sifremiunuttum")]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [Route("sifremiunuttum")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPasswordAsync(ForgettPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = await _serviceUser.GetAsync(x => x.Username == model.Username);
                    if (user is null)
                    {
                        _toastNotification.AddErrorToastMessage("Sistemde kayıtlı böyle bir kullanıcı adı yok!", new ToastrOptions { Title = "Hata" });
                        return View(model);
                    }

                    user.Password = model.Password;
                    _serviceUser.Update(user);
                    var sonuc = await _serviceUser.SaveChangesAsync();
                    if (sonuc > 0)
                    {
                        _toastNotification.AddSuccessToastMessage("Şifreniz başarıyla değişti.", new ToastrOptions { Title = "Başarılı" });
                        return View(model);
                    }

                }
                catch (Exception)
                {
                    _toastNotification.AddErrorToastMessage("Şifreniz değiştirilirken hata oluştu!", new ToastrOptions { Title = "Hata" });
                }
            }
            return View(model);
        }

        [Route("sifredegistir")]
        [HttpGet, Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Route("sifredegistir")]
        [HttpPost, Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = await _serviceUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
                    user.Password = model.Password;
                    _serviceUser.Update(user);
                    await _serviceUser.SaveChangesAsync();
                    _toastNotification.AddSuccessToastMessage("Şifre değiştirme işlemi başarılı", new ToastrOptions { Title = "Başarılı" });
                    return View(model);
                }
                catch (Exception)
                {
                    // loglama
                    _toastNotification.AddErrorToastMessage("Lütfen şifre bilgilerini kontrol ediniz", new ToastrOptions { Title = "Hatalı" });
                    return View(model);
                }
            }
            return View(model);
        }

        [Route("erisimengellendi"), Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
