using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string categoryname { get; set; }
    }
}