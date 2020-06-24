using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace platforma.Models
{
    public class Model
    {
        [Key]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int Price { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}