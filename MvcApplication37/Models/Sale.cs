using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcApplication37.Models
{
    public class Sale
    {
        // Продажа
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public virtual Product Product { get; set; }
        public virtual Client Client { get; set; }
        public DateTime Date { get; set; }
    }
}