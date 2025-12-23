using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PersonalPermission.Core.Entities;

namespace PersonalPermission.WebUI.Models
{
    public class MyProfileViewModel
    {

        [DisplayName("Adı"), StringLength(50), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string Name { get; set; }

        [DisplayName("Soyadı"), StringLength(50), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı Adı"), StringLength(50)]
        public string? Username { get; set; }
        [DisplayName("Unvanı"), StringLength(50), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string Title { get; set; }

        [DisplayName("Birim")]
        public string Department { get; set; }
    }
}
