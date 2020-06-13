using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace platforma.Models
{
    [Table("courses", Schema = "platforma")]
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public int InstructorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DefaultValue(0)]
        public int Price { get; set; }
        public List<Video> Videos { get; set; }
        public List<Document> Documents { get; set; }
        [DefaultValue(0)]
        public float AvgGrade { get; set; }
        public string Categories { get; set; }
    }
}