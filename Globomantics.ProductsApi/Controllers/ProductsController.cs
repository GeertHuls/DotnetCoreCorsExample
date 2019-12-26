using System;
using System.Collections.Generic;
using Bogus;
using Globomantics.ProductsApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace Globomantics.ProductsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        const string PageNo = "PageNo";
        const string PageSize = "PageSize";
        const string PageCount = "PageCount";
        const string PageTotalRecords = "PageTotalRecords";

        [HttpGet]
        public IActionResult Get()
        {
            SetPaginationHeader(10, 1, 100, 10000);
            var data = GetProducts();
            return Ok(data);
        }

        private IEnumerable<ProductModel> GetProducts()
        {
            var products = new Faker<ProductModel>()
                            .RuleFor(o => o.Id, f => Guid.NewGuid())
                            .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                            .RuleFor(o => o.Image, f => f.Image.PicsumUrl(200, 200))
                            .RuleFor(o => o.Price, f => f.Commerce.Price());

            return products.Generate(100);
        }

        private void SetPaginationHeader(int pageSize,int pageNo,int pageCount, int totalRecords)
        {
            HttpContext.Response.Headers.Add(PageNo, pageNo.ToString());
            HttpContext.Response.Headers.Add(PageSize, pageSize.ToString());
            HttpContext.Response.Headers.Add(PageCount, pageCount.ToString());
            HttpContext.Response.Headers.Add(PageTotalRecords, totalRecords.ToString());
        }
    }
}
