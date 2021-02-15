using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class orders
    {
        public int ID { get; set; }
        public string Client { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public DateTime Date { get; set; }
        public string OrderShipped { get; set; }
    }
}