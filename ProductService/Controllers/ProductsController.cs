using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<string> Products = new()
    {
        "Keyboard", "Mouse", "Monitor", "CPU"
    };

    [HttpGet]
    public IActionResult GetProducts()
    {
        return Ok(Products);
    }
}