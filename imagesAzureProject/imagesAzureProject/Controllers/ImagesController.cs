using imagesAzureProject.Models;
using ImagesAzureProject.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            this.imageRepository = new ImagesService(new ImagesContext());
        }

        // Allows the caller pass a context instance
        public ImagesController(IImagesService ImageRepository)
        {
            this.imageRepository = ImageRepository;
        }

        public ActionResult AddImage()
        {
            return View();
        }

        public ActionResult DeleteImage(int? id)
        {          
            //Find the Image
            Image currentImage = imageRepository.GetImages().Where(x => x.Id == id).FirstOrDefault();
            
            //Check if Exist
            if (currentImage ==null)
                 return RedirectToAction("Index", "Home");


            return View(currentImage);
        }

  
        public ActionResult Details(int? id)
        {
            //Find the Image
            Image currentImage = imageRepository.GetImages().Where(x => x.Id == id).FirstOrDefault();

            //Check if Exist
            if (currentImage == null)
                return RedirectToAction("Index", "Home");


            return View(currentImage);
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

                if (!isValidName(image, ref errorMsg))
                {
                    ModelState.AddModelError("Name", errorMsg);
                    return View();
                }


                    //Check if file is Valid Image
                    if (isValidImage(newImage, ref errorMsg))
                {                
                    //Add Image to DB
                    int id = imageRepository.AddNewImage(image,newImage);
                  
                    TempData["Success"] = "The image was added successfully";
                }
                else
                    ModelState.AddModelError("ImagePath", errorMsg);                          
            }
            return View();
        }

        //Get Image and Delete it
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteImage([Bind(Include = "Id")] int Id)
        {
            string currentImageName = imageRepository.GetImages().Where(x => x.Id == Id).FirstOrDefault().Name;

            // Delete current Image
            imageRepository.DeleteImage(Id);

            TempData["SuccessfulDelete"] = "Successfully Deleted";
            return RedirectToAction("Index", "Home"); ;
        }


        // Validate Name
        private bool isValidName(Image image, ref string errorMsg)
        {
            if (image.Name == null)
            {
                errorMsg = "Please enter Image Name.";
                return false;
            }
          

            Regex r = new Regex(@"^[_a-zA-Z0-9]*$");
            Match match = r.Match(image.Name);
            if (!match.Success)
            {
                errorMsg = "Please enter a valid Name.Only alphanumeric Characters or _ !";
                return false;
            }

            return true;
        }



    // Check if is a Valid Image
    private bool isValidImage(HttpPostedFileBase InputFile, ref string errorMsg)
        {
            // Check if is Null or 0 Kb
            if (InputFile == null || InputFile.ContentLength == 0)
            {
                //Add error to Model
                errorMsg = "Error! Please select Image file !";
                return false;
            }

            try
            {
                //Images extensions . We could add every extension we want 
                string[] imagesExtensions = { ".jpg", ".png" };

                // Cehck if file is .png or .jpg
                if (imagesExtensions.Contains(Path.GetExtension(InputFile.FileName)))
                {
                    // Check if file is Image File
                    System.Drawing.Image imgInput = System.Drawing.Image.FromFile(InputFile.FileName);
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