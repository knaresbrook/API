using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class CartItems
    {
        [Key]
        public int Id { get; set; }
        public int productid { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public string cartid { get; set; }
        public ShoppingCart shoppingcart { get; set; }
    }
}