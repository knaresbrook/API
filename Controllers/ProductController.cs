using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
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

        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("products")]

        public async Task<IEnumerable<ProductDto>> GetAll()
        {

            var result = (from p in _context.Products
                          join c in _context.Categories on p.CategoryId equals c.Id
                          select new ProductDto
                          {
                              id = p.Id,
                              code = p.code,
                              description = p.description,
                              price = p.price,
                              stockqty = p.stockqty,
                              ImageUrl = p.ImageUrl,
                              categoryid = c.Id,
                              categoryname = c.categoryname
                          }).ToListAsync();
            return await result;
        }

        [HttpGet("products/{id}")]
        public async Task<ProductDto> GetById(int id)
        {
            var result = (from p in _context.Products
                          join c in _context.Categories on p.CategoryId equals c.Id
                          where p.Id == id
                          select new ProductDto
                          {
                              id = p.Id,
                              code = p.code,
                              description = p.description,
                              price = p.price,
                              stockqty = p.stockqty,
                              ImageUrl = p.ImageUrl,
                              categoryid = c.Id,
                              categoryname = c.categoryname
                          }).FirstOrDefaultAsync();
            return await result;
        }

        [HttpPost("addproduct")]
        public async Task<ActionResult> InsertProduct(Product product)
        {
            if (product == null) return BadRequest("Empty Record");
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("updateproduct/{id}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            var results = await _context.Products.FindAsync(id);
            if (results == null) return BadRequest("Invalid Product...!");

            results.code = product.code;
            results.description = product.description;
            results.price = product.price;
            results.stockqty = product.stockqty;
            results.ImageUrl = product.ImageUrl;
            results.CategoryId = product.CategoryId;

            _context.Products.Update(results);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("addtocart")]
        public async Task<ActionResult> AddToShoppingCart(ShoppingCart shoppingCart)
        {
            var shopping = new ShoppingCart();

            shopping.Id = shoppingCart.Id;
            shopping.userName = shoppingCart.userName;
            shopping.dateCreation = shoppingCart.dateCreation;


            List<CartItems> citems = new List<CartItems>();

            foreach (var item in shoppingCart.CartItems)
            {
                CartItems c = new CartItems();
                c.productid = item.productid;
                c.description = item.description;
                c.quantity = item.quantity;
                c.price = item.price;
                c.cartid = shopping.Id;
                citems.Add(c);
            }
            shopping.CartItems = citems;

            await _context.AddAsync(shopping);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}