using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;

namespace ATLSCANService
{
    public class ZipProcessor
    {
        public static void Process(string zipPath, string destinationRoot)
        {
            DateTime now = DateTime.Now;

            string folder = Path.Combine(
                destinationRoot,
                now.Year.ToString(),
                now.Month.ToString("D2"),
                now.Day.ToString("D2")
            );

            Directory.CreateDirectory(folder);

            string destZip = Path.Combine(folder, Path.GetFileName(zipPath));
            File.Copy(zipPath, destZip, true);

            ZipFile.ExtractToDirectory(destZip, folder);
        }
    }

}