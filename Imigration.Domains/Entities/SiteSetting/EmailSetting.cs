﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imigration.Domains.Entities.Common;

namespace Imigration.Domains.Entities.SiteSetting
{
    public class EmailSetting : BaseEntity
    {
        public string From { get; set; }

        public string Password { get; set; }

        public string SMTP { get; set; }

        public bool EnableSSL { get; set; }

        public string DisplayName { get; set; }

        public int Port { get; set; }

        public bool IsDefault { get; set; }
    }
}
