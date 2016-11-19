using System.ComponentModel.DataAnnotations;

namespace imagesAzureProject.Models
{
    public class Image {
        /// <summary>
        /// The Id of the entity
        /// </summary>
        /// 
        public int Id { get; set; }

        /// <summary>
        /// The name of the image. It can be different than actual file name.
        /// </summary>

        [Required(ErrorMessage = "Please enter Image Name.")]
        [DataType(DataType.Text,ErrorMessage ="Please enter a valid Text")]
        [RegularExpression(@"^[_a-zA-Z0-9]*$", ErrorMessage = "Please enter a valid Name.Only alphanumeric Characters or _ !")]
        public string Name { get; set; }

        /// <summary>
        /// The description of the image
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The path the actual image is stored (normally the blob storage reference)
        /// </summary>
        /// 
       
        public string ImagePath { get; set; }
    }
}
