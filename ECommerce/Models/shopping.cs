using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class shopping
    {
        public int ID { get; set; }
        public string Product { get; set; }
        public int ProductID { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}")]
        public int Cost { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
}