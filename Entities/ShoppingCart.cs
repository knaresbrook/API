using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class ShoppingCart
    {
        [Key]
        public string Id { get; set; }
        public string userName { get; set; }
        public DateTime dateCreation { get; set; }
        [ForeignKey("cartid")]
        public List<CartItems> CartItems { get; set; }
    }
}