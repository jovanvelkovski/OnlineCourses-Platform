using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace platforma.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}