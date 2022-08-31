using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class CategoryController : BaseApiController
    {
        private readonly DataContext _context;
        public CategoryController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("categories")]
        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        [HttpGet("categories/{id}")]
        public async Task<Category> GetById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
    }
}