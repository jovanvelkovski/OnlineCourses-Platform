using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace platforma.Models
{
    [Table("students", Schema = "platforma")]
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
        public int Tokens { get; set; }
    }
}