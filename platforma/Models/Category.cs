using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace platforma.Models
{
    [Table("categories", Schema="platforma")]
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Course> Courses { get; set; }
    }
}