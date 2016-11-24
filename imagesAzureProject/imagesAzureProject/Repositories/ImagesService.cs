using imagesAzureProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace ImagesAzureProject.Repositories
{

    //Implement IImageService Interface
    public class ImagesService : IImagesService
    {
       
        //  Define database context in a class variable
        private ImagesContext context;

        private IAzureStorage AzureStorage;

        //  Constructor expects an instance of the context
        // Injected the Repository
        public ImagesService(ImagesContext context,IAzureStorage AzureStorage)
        {
            this.context = context;
            this.AzureStorage = AzureStorage;
        }

        // Get all Images
        public List<Image> GetImages()
        {
            return context.Images.ToList();
        }

        // Add New Image
        public int AddNewImage(Image image, HttpPostedFileBase PostedImage)
        {
            // Check if image exist,change path
            image = CheckDbIfExist(image, PostedImage);

            //upload it                   
            Image UploadedImage = AzureStorage.UploadImage(PostedImage, image);

            context.Images.Add(UploadedImage);
            context.SaveChanges();

             return image.Id;
        }

        // Delete Image
        public void DeleteImage(int id)
        {
            //Find the selected Image
            Image selectedImage = context.Images.Where(x => x.Id == id).FirstOrDefault();

            //Delete from Azure Storage
            bool Deleted = AzureStorage.DeleteImage(selectedImage.Name);
            //Remove
            context.Images.Remove(selectedImage);
            context.SaveChanges();
        }

        private Image CheckDbIfExist(Image image,HttpPostedFileBase PostedImage)
        {
            string ImageName = "";

            int number = 1;

            int ImageExist = context.Images.Where(x => x.Name == image.Name).Count();
            ImageName = image.Name;

            // Check if exist
            while (ImageExist>0)
            {
                number += 1;
                ImageName = image.Name + "_" + number;
                ImageExist = context.Images.Where(x => x.Name == ImageName).Count();
            }

            // Change image name to imageName + _ + number
            image.Name = ImageName;

            return image;
        }
     
    }
}