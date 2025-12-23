using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalPermission.Core.Entities
{
    public class Department : CommonEntity
    {
        [DisplayName("Adı"), StringLength(50), Required(ErrorMessage = "{0} alanı boş geçilemez")]
        public string Name { get; set; }

        public IList<User>? Users { get; set; }

        public Department()
        {
            Users = new List<User>();
        }
    }
}
