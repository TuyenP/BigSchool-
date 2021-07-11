using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigSchool.Models
{
    public class Attendance
    {
        [Key]
        [Column(Order = 1)]
        public int CourseId { get; set; }

        public Course Course { get; set; }

        [Key]
        [Column(Order = 2)]
        public string AttendeeId { get; set; }

        public ApplicationUser Attendee { get; set; }
    }
}