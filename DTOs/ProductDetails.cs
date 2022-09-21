using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ProductDetails
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int stockqty { get; set; }
        public string imageUrl { get; set; }
        public int categoryid { get; set; }
    }
}