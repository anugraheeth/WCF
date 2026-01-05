using ATLSCANService.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ATLSCANService.Logger;

namespace ATLSCANService
{
    public class ZipService : IZipService
    {
        private string source = @"D:\Cognizant Project\ZipSystem\SourceZips";
        private string dest = @"D:\Cognizant Project\ZipSystem\Destination";

        public string ProcessZipManual(string zipFileName)
        {
            try
            {
                // Ensure .zip extension
                if (!zipFileName.EndsWith(".zip"))
                    zipFileName += ".zip";

                string zipPath = Path.Combine(source, zipFileName);

                if (!File.Exists(zipPath))
                    throw new FileNotFoundException($"ZIP not found: {zipPath}");

                ZipProcessor.Process(zipPath, dest);
                return "ZIP processed successfully";
            }
            catch (Exception ex)
            {
                Log(ex);
                throw new FaultException<ServiceFault>(
                    new ServiceFault { Message = "Manual processing failed", Details = ex.Message }
                );
            }
        }

        public string ProcessZipBatch()
        {
            int success = 0;
            var zips = Directory.GetFiles(source, "*.zip");

            Parallel.ForEach(zips, zip =>
            {
                ZipProcessor.Process(zip, dest);
                Interlocked.Increment(ref success);
            });

            return $"Processed {success} ZIP files";
        }

        private readonly string DestinationRoot =
        @"D:\Cognizant Project\ZipSystem\Destination";

        public List<string> SearchFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return new List<string>();

            if (!Directory.Exists(DestinationRoot))
                return new List<string>();

            // Search recursively
            var files = Directory
                .GetFiles(DestinationRoot, "*", SearchOption.AllDirectories)
                .Where(f => Path.GetFileName(f)
                    .IndexOf(fileName, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(f => GetRelativePath(f))
                .ToList();

            return files;
        }

        public byte[] DownloadFile(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new FaultException("Invalid file path");

            relativePath = relativePath.Replace("/", "\\");

            var fullPath = Path.Combine(DestinationRoot, relativePath);

            if (!File.Exists(fullPath))
                throw new FaultException("File not found");

            return File.ReadAllBytes(fullPath);
        }


        private string GetRelativePath(string fullPath)
        {
            return fullPath
                .Replace(DestinationRoot, "")
                .TrimStart('\\')
                .Replace("\\", "/");
        }

    }

}
