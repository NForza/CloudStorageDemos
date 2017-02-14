using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("images");

            container.CreateIfNotExists();
            
            //Default permissions for container
            container.SetPermissions(
                new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            UploadFile(container, "example.jpg");
        }

        private static void UploadFile(CloudBlobContainer container, string fileName)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName); 
                      
            using (var fileStream = System.IO.File.OpenRead(fileName))
            {
                blockBlob.UploadFromStream(fileStream);
            }
        }
    }
}
