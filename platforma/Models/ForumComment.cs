using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace platforma.Models
{
    public class ForumComment
    {
        [Key]
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime DateComment { get; set; }
        public string User { get; set; }

    }
}