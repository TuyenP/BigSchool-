using BigSchool.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BigSchool.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [Display(Name = "Địa chỉ")]
        public String Place { get; set; }

        [Required(ErrorMessage = "Ngày không được để trống")]
        [Display(Name = "Ngày")]
        [FutureDate]
        public String Date { get; set; }

        [Required(ErrorMessage = "Thời gian không được để trống")]
        [Display(Name = "Thời gian")]
        [ValidTime]
        public String Time { get; set; }

        [Required(ErrorMessage = "Thể loại không được để trống")]
        [Display(Name = "Thể loại")]
        public int Category { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public string Heading { get; set; }

        public string Action { get { return (Id != 0) ? "Update" : "Create"; } }

        public DateTime GetDateTime()
        {
            return DateTime.Parse(string.Format("{0} {1}", Date, Time));
        }
    }
}