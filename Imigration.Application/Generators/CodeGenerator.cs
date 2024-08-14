using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imigration.Application.Generators
{
    public static class CodeGenerator
    {
        public static string CreateActivationCode()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
