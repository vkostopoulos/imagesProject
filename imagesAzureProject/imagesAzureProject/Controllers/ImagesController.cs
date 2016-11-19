using imagesAzureProject.Models;
using ImagesAzureProject.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace imagesAzureProject.Controllers
{
    public class ImagesController : Controller
    {

        // Declare a variable for an object that implements the IImagesService interface
        private IImagesService imageRepository;

        // The Default Constructor create a new context instance
        public ImagesController()
        {
            this.imageRepository = new ImageRepository(new ImagesContext());
        }

        // Allows the caller pass a context instance
        public ImagesController(IImagesService ImageRepository)
        {
            this.imageRepository = ImageRepository;
        }

        // GET: Images
        public ActionResult AddImage()
        {
            return View();
        }


        //Get Name and Description
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddImage([Bind(Include = "Name,Description")] Image image)
        {
            //Validate Model 
            if (ModelState.IsValid)
            {
                    // Get first file
                    var newImage = Request.Files[0];

                    string errorMsg = "";

                //Check if file is Valid Image
                if (isValidImage(newImage, ref errorMsg))
                {
                    //Set ImagePath to image
                    image.ImagePath = Path.GetFullPath(newImage.FileName);

                    //Add Image to DB
                    int id = imageRepository.AddNewImage(image);

                    TempData["Success"] = "The image added successfully";
                }
                else
                    ModelState.AddModelError("ImagePath", errorMsg);                          
            }
            return View();
        }



        // Check if is a Valid Image
        private bool isValidImage(HttpPostedFileBase InputFile,ref string errorMsg)
        {
            // Check if is Null or 0 Kb
            if (InputFile == null || InputFile.ContentLength == 0)
            {
                //Add error to Model
                errorMsg = "Error! Please select Image file !";
                return false;
            }

            // Get FilePath
            string InputSource = Path.GetFullPath(InputFile.FileName);

            try
            {
                // Cehck if file is .png or .jpg
                if (InputSource.EndsWith(".png") || InputSource.EndsWith(".jpg"))
                {
                    // Check if file is Image File
                    System.Drawing.Image imgInput = System.Drawing.Image.FromFile(InputSource);
                    System.Drawing.Imaging.ImageFormat thisFormat = imgInput.RawFormat;
                }
                else
                {
                    errorMsg = "Error! Accept only .jpg and .png files!";
                    return false;
                }
            }
            catch
            {
                errorMsg = "Error! This file is not Image!";
                return false;
            }

            return true;

            }
    }
}