using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using TechZone.Model.Models;
using TechZone.Service;
using TechZone.Web.Infrastructure.Core;
using TechZone.Web.Mappings;
using TechZone.Web.Models;

namespace TechZone.Web.Api
{
    [RoutePrefix("api/productcategory")]
    public class ProductCategoryController : ApiControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;

        private readonly IMappingService _mappingService;
        public ProductCategoryController(IErrorService errorService, IProductCategoryService productCategoryService, IMappingService mappingService)
            : base(errorService)
        {
            this._productCategoryService = productCategoryService;
            this._mappingService = mappingService;
        }

        [Route("getall")]
        public IHttpActionResult GetAll(string keyword, int page = 0, int pageSize = 20)
        {
            int totalRow = 0;
            var model = _productCategoryService.GetAll(keyword);

            totalRow = model.Count();
            var query = model.OrderByDescending(x => x.CreatedDate).Skip((page) * pageSize).Take(pageSize);

            var responseData = _mappingService.Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(query);

            var paginationSet = new PaginationSet<ProductCategoryViewModel>()
            {
                Items = responseData,
                Page = page,
                TotalCount = totalRow,
                TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
            };
            return Ok(paginationSet);
        }
    }
}