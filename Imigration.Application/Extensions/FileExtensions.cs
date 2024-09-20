using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;
using Imigration.Application.Statics;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

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

        public static void DeleteFile( this string fileName , string path)
        {
            var finalPath = path + fileName;

            if (File.Exists(finalPath))
            {
                File.Delete(finalPath);
            }
        }
        public static List<string> GetSrcValue(this string text)
        {
            List<string> imgScrs = new List<string>();

            HtmlDocument doc = new HtmlDocument();

            doc.LoadHtml(text);

            var nodes = doc.DocumentNode.SelectNodes(@"//img[@src]");

            if (nodes != null && nodes.Any())
            {
                foreach (var img in nodes)
                {
                    HtmlAttribute att = img.Attributes["src"];
                    imgScrs.Add(att.Value.Split("/").Last());
                }
            }

            return imgScrs;
        }
        //moqayeste jdid aks ya barreesi hamegi
        public static void ManageEditorImages(  string currenText , string newText , string path)
        {
            var currentSrcs = currenText.GetSrcValue();

            var newSrcs = newText.GetSrcValue();

            if (currentSrcs.Count == 0) return;

            if (newSrcs.Count == 0)
            {
                foreach (var img in currentSrcs)
                {
                    img.DeleteFile(path);
                }
            }
            foreach (var img in currentSrcs)
            {
                if (newSrcs.All(s => s != img))
                {
                    img.DeleteFile(path);
                }
            }

        }
    }
}
