using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.ViewModels.Common;

namespace Imigration.Domains.ViewModels.Account
{
    public class ForgotPasswordViewModel : GoogleRecaptchaViewModel
    {
        public string Email { get; set; }

    }
}
