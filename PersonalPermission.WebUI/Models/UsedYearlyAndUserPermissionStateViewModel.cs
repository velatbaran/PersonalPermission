using PersonalPermission.Core.Entities;

namespace PersonalPermission.WebUI.Models
{
    public class UsedYearlyAndUserPermissionStateViewModel
    {
        public PermissionState PermissionState { get; set; }
        public UsedYearlyPermission UsedYearlyPermission { get; set; }
    }
}
