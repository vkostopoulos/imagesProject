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


        public string UploadImage(HttpPostedFileBase InputImage,Image image)
        {
            string AzurePath = "";

            // Retrieve reference to a blob named as image.
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(image.Name + Path.GetExtension(InputImage.FileName));

            // Create or overwrite the ImageName blob with contents from a local file.
            using (var fileStream = System.IO.File.OpenRead(Path.GetFullPath(InputImage.FileName)))
            {
                blockBlob.UploadFromStream(fileStream);
            }

            //Get blob url
            AzurePath = blockBlob.Uri.ToString();

            return AzurePath;
        }
        public bool DeleteImage(string Imagename)
        {
            try
            {
                // Retrieve reference to a blob named "myblob.txt".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(Imagename);

                // Delete the blob.
                blockBlob.Delete();

                return true;
            }
            catch { return false; }
        }


        // Check if blob with same Name exist. If yes change name to ImageName _ Number and check it again.
        // Find the first available Name and return the changed image
        public Image CheckIfImageExist(HttpPostedFileBase InputImage, Image image)
        {
            string AzurePath = "";

            int number = 1;
            string FinalImageName = image.Name;
            // Retrieve reference to a blob named as image.
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(image.Name + Path.GetExtension(InputImage.FileName));

            //If image name already exist, change name to ImageName _ (Number)

            while (blockBlob.Exists())
            {              
                number += 1;
                blockBlob = container.GetBlockBlobReference(image.Name + "_" + number + Path.GetExtension(InputImage.FileName));
                FinalImageName = image.Name + "_" + number;
            }

            //Get blob url
            AzurePath = blockBlob.Uri.ToString();

            image.ImagePath = AzurePath;
            image.Name = FinalImageName;

            return image;
        }
    }
}