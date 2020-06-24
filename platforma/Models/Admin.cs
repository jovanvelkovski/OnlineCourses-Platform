using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace platforma.Models
{
    [Table("admins", Schema = "platforma")]
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}