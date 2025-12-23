using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalPermission.Core.Entities
{
    public class PermissionState : CommonEntity
    {
        [DisplayName("Kullanıcı")]
        public Guid? UserGuid { get; set; }

        [DisplayName("Hangi Yıl")]
        public int WhichYears { get; set; }

        //[DisplayName("Kazanılan İdari İzin")]
        //public int GainedAdministrativePermission { get; set; }

        [DisplayName("Kullanılan İdari İzin")]
        public int UsedAdministrativePermission { get; set; }

        //[DisplayName("Kalan İdari İzin")]
        //public int RemainAdministrativePermission { get; set; }

        [DisplayName("Kazanılan Yıllık İzin")]
        public int GainedYearlyPermission { get; set; }

        [DisplayName("Kullanılan Yıllık İzin")]
        public int UsedYearlyPermission { get; set; }

        [DisplayName("Kalan Yıllık İzin")]
        public int RemainYearlyPermission { get; set; }

        [DisplayName("Aktif Mi?")]
        public bool IsActive { get; set; }

        [DisplayName("Ekleme Tarihi")]
        public DateTime AddedDate { get; set; } = DateTime.Now;

        [DisplayName("Kullanıcı")]
        public int? UserId { get; set; }
        [DisplayName("Kullanıcı")]
        public User? User { get; set; }
    }
}
