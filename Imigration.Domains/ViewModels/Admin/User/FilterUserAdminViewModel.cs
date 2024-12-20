﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.ViewModels.Common;

namespace Imigration.Domains.ViewModels.Admin.User
{
    public class FilterUserAdminViewModel : Paging<Entities.Account.User>
    {
        public FilterUserAdminViewModel()
        {
            ActivationStatus = AccountActivationStatus.All;
        }

        public string? UserSearch { get; set; }

        public AccountActivationStatus ActivationStatus { get; set; }
    }

    public enum AccountActivationStatus
    {
        [Display(Name = "همه")] All,
        [Display(Name = "حساب کاربری فعال")] IsActive,
        [Display(Name = "حساب کاربری غیرفعال")] NotActive
    }
}
