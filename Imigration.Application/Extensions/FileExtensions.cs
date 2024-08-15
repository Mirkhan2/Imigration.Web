using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Imigration.Application.Extensions
{
    public static class FileExtensions
    {
        public static bool UploadFile(this IFormFile file, string fileName, string path,
            List<string>? validFormats = null)
        {
            if (validFormats != null && validFormats.Any())
            {
                var fileFormat = Path.GetExtension(file.FileName);

                if (validFormats.All(s => s != fileFormat))
                {
                    return false;
                }
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var finalPath = path + fileName;

            using (var stream = new FileStream(finalPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return true;
        }
    }
}
