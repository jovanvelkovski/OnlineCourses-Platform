using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace platforma.Models
{
    [Table("prices", Schema = "platforma")]
    public class PriceCourse
    {
        [Key]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int Price { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}