using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalPermission.Core.Entities
{
    public class UsedAdministrivePermission : CommonEntity
    {
        [DisplayName("Kullanıcı")]
        public Guid? UserGuid { get; set; }

        [DisplayName("İzin Tarihi"), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public DateTime StartingDate { get; set; }

        [DisplayName("Başlangıç Saati"), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public int StartingTime { get; set; }

        [DisplayName("Bitiş Saati"), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public int ExpirationTime { get; set; }

        [DisplayName("Toplam Kullanılacak Saat"), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public int TotalTime { get; set; }

        //[DisplayName("Ait Olduğu Yıl")]
        //public DateTime? YearofBelong { get; set; }

        [DisplayName("Hangi Yıl")]
        public int WhichYears { get; set; }

        [DisplayName("İzindeki Adres"), StringLength(100)]
        public string? Address { get; set; }

        [DisplayName("Açıklama"), StringLength(150)]
        public string? Description { get; set; }

        [DisplayName("Kullanıcı")]
        public int? UserId { get; set; }
        [DisplayName("Kullanıcı")]
        public User? User { get; set; }
    }
}
