using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication37.Models
{
    public class CartIndexViewModel
    {
        public Carts Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}