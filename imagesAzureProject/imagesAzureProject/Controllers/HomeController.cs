using imagesAzureProject.Models;
using ImagesAzureProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace imagesAzureProject.Controllers
{
    public class HomeController : Controller
    {

        // Declare a variable for an object that implements the IImagesService interface
        private IImagesService imageRepository;

        // Allows the caller pass a context instance
        public HomeController(IImagesService ImageRepository)
        {
            this.imageRepository = ImageRepository;
        }

        // GET: Home
        public ActionResult Index()
        {         
            return View(imageRepository.GetImages());
        }
    }
}