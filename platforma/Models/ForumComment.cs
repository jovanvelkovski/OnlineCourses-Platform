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
        //public int whoId { get; set; }
        public string Comment { get; set; }
        public DateTime DateComment { get; set; }
        public string User { get; set; }
        public int ForumId { get; set; }
    }
}