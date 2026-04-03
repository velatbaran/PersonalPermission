using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalPermission.Core.Entities
{
    public class User : CommonEntity
    {
        [DisplayName("Adı"), StringLength(50), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string Name { get; set; }

        [DisplayName("Soyadı"), StringLength(50), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı Adı"), StringLength(50)]
        public string? Username { get; set; }

        [DisplayName("Sicil No"), StringLength(50), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string RegistryNo { get; set; }

        [DisplayName("Unvan")]
        public Title? Title { get; set; }
        [DisplayName("Unvan")]
        public int? TitleId { get; set; }

        [DisplayName("Şube")]
        public Department? Department { get; set; }
        [DisplayName("Şube")]
        public int? DepartmentId { get; set; }

        [DisplayName("İşe Giriş Tarihi"), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        //[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
        public DateTime StartingWorkDate { get; set; }

        [DisplayName("Hizmet Yılı")]
        [ScaffoldColumn(false)]
        public int ServiceTimeYear { get; set; }

        [DisplayName("Hizmet Ayı")]
        [ScaffoldColumn(false)]
        public int ServiceTimeMonth { get; set; }

        [DisplayName("Hizmet Günü")]
        [ScaffoldColumn(false)]
        public int ServiceTimeDay { get; set; }

        [DisplayName("Şifre"), StringLength(50), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string? Password { get; set; }

        [DisplayName("Admin Mi?")]
        public bool IsAdmin { get; set; } = false;

        //[DisplayName("Aktif Mi?")]
        //public bool IsActive { get; set; } = true;

        [DisplayName("Kullanıcı Guid"), ScaffoldColumn(false)]
        public Guid? UserGuid { get; set; } = Guid.NewGuid();

        public IList<PermissionState>? PermissionStates { get; set; }
        public IList<UsedAdministrivePermission>? UsedAdministrivePermissions { get; set; }
        public IList<UsedYearlyPermission>? UsedYearlyPermissions { get; set; }

        public User()
        {
            PermissionStates = new List<PermissionState>();
            UsedAdministrivePermissions = new List<UsedAdministrivePermission>();
            UsedYearlyPermissions = new List<UsedYearlyPermission>();
        }

    }
}
