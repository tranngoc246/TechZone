using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using TechZone.Model.Models;
using TechZone.Service;
using TechZone.Web.Infrastructure.Core;
using TechZone.Web.Infrastructure.Extensions;
using TechZone.Web.Mappings;
using TechZone.Web.Models;

namespace TechZone.Web.Api
{
    [RoutePrefix("api/product")]
    [Authorize]
    public class ProductController : ApiControllerBase
    {
        #region Initialize
        private readonly IProductService _productService;
        private readonly IMappingService _mappingService;

        public ProductController(IErrorService errorService, IProductService productService, IMappingService mappingService)
            : base(errorService)
        {
            this._productService = productService;
            this._mappingService = mappingService;
        }

        #endregion

        [Route("getallparents")]
        [HttpGet]
        public IHttpActionResult GetAllParents()
        {
            var model = _productService.GetAll();
            var responseData = _mappingService.Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);
            return Ok(responseData);
        }

        [Route("getbyid/{id:int}")]
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            var model = _productService.GetById(id);
            var responseData = _mappingService.Mapper.Map<Product, ProductViewModel>(model);
            return Ok(responseData);
        }

        [Route("getall")]
        [HttpGet]
        public IHttpActionResult GetAll(string keyword, int page = 0, int pageSize = 20)
        {
            int totalRow = 0;
            var model = _productService.GetAll(keyword);

            totalRow = model.Count();
            var query = model.OrderByDescending(x => x.CreatedDate).Skip((page) * pageSize).Take(pageSize);

            var responseData = _mappingService.Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(query);

            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = responseData,
                Page = page,
                TotalCount = totalRow,
                TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
            };
            return Ok(paginationSet);
        }

        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductViewModel productVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var newProduct = new Product();
                    newProduct.UpdateProduct(productVm);
                    newProduct.CreatedDate = DateTime.Now;
                    newProduct.CreatedBy = User.Identity.Name;
                    _productService.Add(newProduct);
                    _productService.Save();

                    var responseData = _mappingService.Mapper.Map<Product, ProductViewModel>(newProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductViewModel productVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var dbProduct = _productService.GetById(productVm.ID);

                    dbProduct.UpdateProduct(productVm);
                    dbProduct.UpdatedDate = DateTime.Now;
                    dbProduct.UpdatedBy = User.Identity.Name;

                    _productService.Update(dbProduct);
                    _productService.Save();

                    var responseData = _mappingService.Mapper.Map<Product, ProductViewModel>(dbProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var oldProduct = _productService.Delete(id);
                    _productService.Save();

                    var responseData = _mappingService.Mapper.Map<Product, ProductViewModel>(oldProduct);
                    response = request.CreateResponse(HttpStatusCode.OK, responseData);
                }

                return response;
            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedProducts)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var listProduct = new JavaScriptSerializer().Deserialize<List<int>>(checkedProducts);
                    foreach (var item in listProduct)
                    {
                        _productService.Delete(item);
                    }
                    _productService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, true);
                }

                return response;
            });
        }
    }
}
