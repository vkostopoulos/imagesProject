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
            image.Name = ImageNameAllreadyExist(image.Name);

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

        // Check if exist image with this Name. If yes return new fileName
        private string ImageNameAllreadyExist(string ImageName)
        {
            int number = 1;
            string tempFileName = ImageName;

            //If image name already exist, change name to ImageName _ (Number)
            while (context.Images.Any(x => x.Name == tempFileName))
            {
                    tempFileName = ImageName;
                    number += 1;
                    tempFileName = tempFileName + "_" + number;
            }
            
            ImageName = tempFileName;

            return ImageName;
        }
     
    }
}