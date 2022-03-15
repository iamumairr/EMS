using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Models
{
    public class Student
    {
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "{0} can not be less than {1} and greater than {2}", MinimumLength = 5)]
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public string? StudentName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Of Birth")]
        public DateTime DOB { get; set; }

        [StringLength(254)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? StudentEmail { get; set; }
        [Display(Name ="Profile Picture")]
        public string? ProfilePicture { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        [Display(Name = "Picture")]
        public IFormFile? PictureUpload { get; set; }
    }
}