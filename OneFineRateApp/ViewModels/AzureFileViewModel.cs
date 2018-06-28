using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateApp.ViewModels
{
   
    public class CloudFile
    {
        public string FileName { get; set; }
        public string URL { get; set; }
        public long Size { get; set; }
        public long BlockCount { get; set; }
        public CloudBlockBlob BlockBlob { get; set; }
        public DateTime StartTime { get; set; }
        public string UploadStatusMessage { get; set; }
        public bool IsUploadCompleted { get; set; }
        public string FileKey { get; set; }
        public int FileIndex { get; set; }
        public int PropertyId { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public static CloudFile CreateFromIListBlobItem(IListBlobItem item)
        {
            if (item is CloudBlockBlob)
            {
                var blob = (CloudBlockBlob)item;
                return new CloudFile
                {
                    FileName = blob.Name,
                    URL = blob.Uri.ToString(),
                    Size = blob.Properties.Length
                };
            }
            return null;
        }
    }

    public class CloudFilesModel
    {
        public CloudFilesModel()
            : this(null)
        {
            Files = new List<CloudFile>();
        }

        public CloudFilesModel(IEnumerable<IListBlobItem> list)
        {
            Files = new List<CloudFile>();
            try
            {
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        CloudFile info = CloudFile.CreateFromIListBlobItem(item);
                        if (info != null)
                        {
                            Files.Add(info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Ignore Errors when empty
            }
        }
        public List<CloudFile> Files { get; set; }
    }
}