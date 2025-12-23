using System.Security.Claims;

namespace PersonalPermission.WebUI.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string UserId(this ClaimsPrincipal user)
    => user.FindFirst("UserId")?.Value;

        public static string UserGuid(this ClaimsPrincipal user)
            => user.FindFirst("UserGuid")?.Value;

        public static string Username(this ClaimsPrincipal user)
            => user.FindFirst("Username")?.Value;

        public static string FirstName(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.GivenName)?.Value;

        public static string LastName(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.Surname)?.Value;

        public static string FullName(this ClaimsPrincipal user)
            => user.Identity?.Name;

        public static string Department(this ClaimsPrincipal user)
            => user.FindFirst("Department")?.Value;

        public static string Title(this ClaimsPrincipal user)
            => user.FindFirst("Title")?.Value;

        public static bool IsAdmin(this ClaimsPrincipal user)
            => user.IsInRole("Admin");
    }
}
