using imagesAzureProject.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ImagesAzureProject.Repositories
{
    //Implement IAzureStorage Interface
    public class AzureStorage : IAzureStorage
    {
        CloudBlobContainer container;
        public AzureStorage()
        {
        // Retrieve storage account from connection string
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

        // Create blob client
        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        // Retrieve reference to a previously created container
        container = blobClient.GetContainerReference("images");

        // Create the container if it doesn't already exist.
        container.CreateIfNotExists();

            // Add Perimissions
            container.SetPermissions(
                new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

        }


        //upload image and return the image with blob path
        public Image UploadImage(HttpPostedFileBase InputImage,Image image)
        {
    
            // Retrieve reference to a blob named as image.
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(Guid.NewGuid().ToString()+"_"+image.Name);
            
            // Create the ImageName blob with contents from a local file.
            using (var fileStream = System.IO.File.OpenRead(Path.GetFullPath(InputImage.FileName)))
            {
                blockBlob.UploadFromStream(fileStream);
            }

            //Get blob url
            image.ImagePath = blockBlob.Uri.ToString();

            return image;
        }
        public bool DeleteImage(Image image)
        {
            try
            {
                // Retrieve reference to a blob named "myblob.txt".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(Path.GetFileName(image.ImagePath));

                // Delete the blob.
                blockBlob.Delete();

                return true;
            }
            catch { return false; }
        }
    
    }
}