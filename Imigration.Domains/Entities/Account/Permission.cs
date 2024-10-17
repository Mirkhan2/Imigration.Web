using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imigration.Domains.Entities.Account
{
    public class Permission 
    {
        #region Properties

        [Key]
        public long Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        #endregion
        public ICollection<UserPermission> UserPermission { get; set; }
        #region Relations

        #endregion

    }
}
