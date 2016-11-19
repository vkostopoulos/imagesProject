using imagesAzureProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ImagesAzureProject.Repositories
{

    //Implement IImageService Interface
    public class ImageRepository : IImagesService
    {
       
        //  Define database context in a class variable
        private ImagesContext context;

        //  Constructor expects an instance of the context
        public ImageRepository(ImagesContext context)
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
           int number = 0;

            //If image name already exist, change name to ImageName _ (Number)
           while (ImageNameAllreadyExist(image))
            {
                number += 1;
                image.Name = image.Name + "_" + number;
            }

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

        // Check if exist image with this Name
        private bool ImageNameAllreadyExist(Image image)
        {
            return context.Images.Any(x => x.Name == image.Name);
        }
    }
}