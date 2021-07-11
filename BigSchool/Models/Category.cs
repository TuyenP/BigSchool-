using System;
using System.ComponentModel.DataAnnotations;

namespace BigSchool.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public String Name { get; set; }
    }
}