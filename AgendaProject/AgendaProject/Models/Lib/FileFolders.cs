using System.IO;
using System.Linq;
using System.Web;

namespace AgendaProject.Models.Lib
{
    public class FileFolders
    {
        public static void CreateAgendaFolder(string year, string monthDay)
        {
            var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Shoebox/" ), year, monthDay);

            Directory.CreateDirectory(path);

        }

        public static void DeleteAgendaFolder(string year, string monthDay)
        {
            var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Shoebox/" ), year, monthDay);
            Directory.Delete(path, true);
        }

        public static void RenameAgendaFolder(string year, string monthDay, string revision)
        {
            var oldPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Shoebox/" ),  year, monthDay);
            var newPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Shoebox/" ),  year, revision);
            Directory.Move(oldPath, newPath);
        }

        

        public static string SaveWorkItemAttachment(string year, string fileNumber, HttpPostedFileBase myFile)
        {
            var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Shoebox/"), year, fileNumber);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileCount =
                (from file in Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly) select file).Count();
            var fileName = "file_" + fileCount + ".pdf";
            
            myFile.SaveAs(Path.Combine(path, fileName));

            return (Path.Combine("~/Shoebox/", year, fileNumber, fileName).Replace("\\", "/"));

        }

        public static string SaveAgendaItemAttachment(string year, string monthDay, HttpPostedFileBase myFile)
        {
            var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Shoebox/"), year, monthDay);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileCount =
                (from file in Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly) select file).Count();
            var fileName = "file_" + fileCount + ".pdf";

            myFile.SaveAs(Path.Combine(path, fileName));

            return (Path.Combine("~/Shoebox/", year, monthDay, fileName).Replace("\\", "/"));
        }

        public static void DeleteAttachment(string filePath)
        {
            var path = HttpContext.Current.Server.MapPath(filePath);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}