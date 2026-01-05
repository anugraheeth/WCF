using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ALTSCANUI.AtlscanRef;

namespace ALTSCANUI.Services
{
    public class ServiceClient
    {
            private readonly ZipServiceClient _client;

            public  ServiceClient()
            {
                _client = new ZipServiceClient();
            }

            public List<string> SearchFile(string fileName)
            {
                return _client.SearchFile(fileName).ToList();
            }

            public byte[] DownloadFile(string relativePath)
            {
                return _client.DownloadFile(relativePath);
            }
        }

}