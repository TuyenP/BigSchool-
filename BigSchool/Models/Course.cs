using System;
using System.ComponentModel.DataAnnotations;

namespace BigSchool.Models
{
    public class Course
    {
        public int Id { get; set; }

        public bool IsCanceled { get; set; }

        [Required]
        public String LecturerId { get; set; }

        public ApplicationUser Lecturer { get; set; }

        [Required]
        [StringLength(255)]
        public String Place { get; set; }

        [Display(Name = "Thời gian")]
        public DateTime DateTime { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}