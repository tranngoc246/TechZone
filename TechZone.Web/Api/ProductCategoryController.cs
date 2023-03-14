using AutoMapper;
using System.Collections.Generic;
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
        private IProductCategoryService _productCategoryService;

        private IMappingService _mappingService;
        public ProductCategoryController(IErrorService errorService, IProductCategoryService productCategoryService, IMappingService mappingService)
            : base(errorService)
        {
            this._productCategoryService = productCategoryService;
            this._mappingService = mappingService;
        }

        [Route("getall")]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productCategoryService.GetAll();
                var listProductCategoryVm = _mappingService.Mapper.Map<List<ProductCategoryViewModel>>(model);

                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var content = new ObjectContent<List<ProductCategoryViewModel>>(listProductCategoryVm, new JsonMediaTypeFormatter(), "application/json");
                response.Content = content;

                return response;
            });
        }
    }
}