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

        //  Constructor expects an instance of the context
        public ImagesService(ImagesContext context)
        {
            this.context = context;          
        }

        // Get all Images
        public List<Image> GetImages()
        {
            return context.Images.ToList();
        }

        // Add New Image
        public int AddNewImage(Image image)
        {
            // Check if image exist,change path
            image = CheckDbIfExist(image);

         
            context.Images.Add(image);
            context.SaveChanges();

             return image.Id;
        }

        // Delete Image
        public void DeleteImage(int id)
        {
            //Find the selected Image
            Image selectedImage = context.Images.Where(x => x.Id == id).FirstOrDefault();

            //Remove
            context.Images.Remove(selectedImage);
            context.SaveChanges();
        }

        private Image CheckDbIfExist(Image image)
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