using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imigration.Domains.Enums
{
    public enum UserEnums
    {

        [Display(Name = "برنز")] Bronze,

        [Display(Name = "نقره")] Silver,

        [Display(Name = "طلا")] Gold
    }
}
