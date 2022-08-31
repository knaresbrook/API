using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly DataContext _context;
        public ProductController(DataContext context)
        {
            _context = context;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("products/{id}")]
        public async Task<Product> GetById(int id)
        {
            return await _context.Products.FindAsync(id);
        }
    }
}