using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int stockqty { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}