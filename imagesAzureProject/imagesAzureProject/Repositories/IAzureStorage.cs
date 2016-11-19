using imagesAzureProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImagesAzureProject.Repositories
{
    public interface IAzureStorage
    {

        //Upload Image to Storage and return the Path
        string UploadImage(HttpPostedFileBase InputImage,Image image);

        //Delete Image from Storage .Return true - false
        bool DeleteImage(string ImageName);

    }
}