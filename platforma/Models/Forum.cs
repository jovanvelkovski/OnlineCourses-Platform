using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace platforma.Models
{
    [Table("forums", Schema = "platforma")]
    public class Forum
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<ForumComment> Comments { get; set; }
    }
}