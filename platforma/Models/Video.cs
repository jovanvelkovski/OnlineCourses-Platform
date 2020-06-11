using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace platforma.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string PathVideo { get; set; }
        public string PathImg { get; set; }
    }
}