using BigSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigSchool.ViewModels
{
    public class CourseOl
    {
        public int Id { get; set; }

        public bool IsCanceled { get; set; }

        public String LecturerId { get; set; }

        public ApplicationUser Lecturer { get; set; }

        public String Place { get; set; }

        public DateTime DateTime { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string UserName { get; set; }

        public bool isShowGoing { get; set; }

        public bool isShowFollow { get; set; }
    }
}