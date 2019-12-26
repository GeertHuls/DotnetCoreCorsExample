using System;
using System.Collections.Generic;
using Bogus;
using Globomantics.ProductsApi.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Globomantics.ProductsApi.Controllers
{
    [ApiController]
    // this will override the global GlobomanticsInteral policy
    [EnableCors("PublicApi")]
    [Route("api/public/[controller]")]
    public class PublicProductsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<ProductModel> Get()
        {
            var products = new Faker<ProductModel>()
                            .RuleFor(o => o.Id, f => Guid.NewGuid())
                            .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                            .RuleFor(o => o.Image, f => f.Image.PicsumUrl(200, 200))
                            .RuleFor(o => o.Price, f => f.Commerce.Price());

            return products.Generate(100);
        }
    }
}
