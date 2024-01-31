using ContactAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ContactAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PutAndPatchController : Controller
    {
        private readonly List<Product> _products = new List<Product>
       {
        new Product { Id = 1, Name = "Product 1", Price = 10 },
        new Product { Id = 2, Name = "Product 2", Price = 20 }
        // Existing products...
       };

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product updatedProduct)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = updatedProduct.Name;
            existingProduct.Price = updatedProduct.Price;
            // Update other properties...

            return Ok(existingProduct);
        }
        //    [HttpPatch("{id}")]
        //    public IActionResult Patch(int id, [FromBody] JsonPatchDocument<Product> patchDoc)
        //    {
        //        var productToUpdate = _products.FirstOrDefault(p => p.Id == id);
        //        if (productToUpdate == null)
        //        {
        //            return NotFound();
        //        }

        //        patchDoc.ApplyTo(productToUpdate, ModelState, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)patchDoc);

        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        return Ok(productToUpdate);
        //    }
        //}
    }
}
