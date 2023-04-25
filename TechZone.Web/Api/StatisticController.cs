using Microsoft.AspNet.Identity;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using TechZone.Model.Models;
using TechZone.Service;
using TechZone.Web.Infrastructure.Core;
using TechZone.Web.Infrastructure.Extensions;
using TechZone.Web.Mappings;
using TechZone.Web.Models;
using Product = TechZone.Model.Models.Product;

namespace TechZone.Web.Api
{
    [RoutePrefix("api/statistic")]
    public class StatisticController : ApiControllerBase
    {
        private readonly IStatisticService _statisticService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IMappingService _mappingService;

        public StatisticController(IErrorService errorService,
            IStatisticService statisticService,
            IOrderService orderService,
            IProductService productService,
            IMappingService mappingService) : base(errorService)
        {
            _statisticService = statisticService;
            _orderService = orderService;
            _productService = productService;
            _mappingService = mappingService;
        }

        [Route("getrevenuebydate")]
        [HttpGet]
        public HttpResponseMessage GetRevenueStatisticByDate(HttpRequestMessage request, string fromDate, string toDate)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _statisticService.GetRevenueStatisticByDate(fromDate, toDate);
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

        [Route("getrevenuebymonth")]
        [HttpGet]
        public HttpResponseMessage GetRevenueStatisticByMonth(HttpRequestMessage request, string fromMonth, string toMonth)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _statisticService.GetRevenueStatisticByMonth(fromMonth, toMonth);
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, model);
                return response;
            });
        }

        [Route("getorder")]
        [HttpGet]
        public IHttpActionResult GetOrder(string keyword, int page, int pageSize = 20)
        {
            var model = _orderService.GetAllOrder(keyword);
            int totalRow = model.Count();
            var query = model.OrderByDescending(x => x.CreatedDate).Skip((page) * pageSize).Take(pageSize);

            var responseData = _mappingService.Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(query);

            var paginationSet = new PaginationSet<OrderViewModel>()
            {
                Items = responseData,
                Page = page,
                TotalCount = totalRow,
                TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
            };
            return Ok(paginationSet);
        }

        [HttpDelete]
        [Route("delete/order")]
        public HttpResponseMessage DeleteOrder(HttpRequestMessage request, int id)
        {
            var appGroup = _orderService.DeleteOrder(id);
            _orderService.Save();
            return request.CreateResponse(HttpStatusCode.OK, appGroup);
        }

        [Route("deletemulti/order")]
        [HttpDelete]
        public HttpResponseMessage DeleteMultiOrder(HttpRequestMessage request, string checkedList)
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
                    var listItem = new JavaScriptSerializer().Deserialize<List<int>>(checkedList);
                    foreach (var item in listItem)
                    {
                        _orderService.DeleteOrder(item);
                    }

                    _orderService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listItem.Count);
                }

                return response;
            });
        }


        [Route("update")]
        [HttpPut]
        public HttpResponseMessage UpdateOrderDetail(HttpRequestMessage request, OrderDetailViewModel orderDetailViewModel)
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

                    var dbOrderDetail = _orderService.GetOrderDetailByProductId(orderDetailViewModel.OrderID, orderDetailViewModel.ProductID);
                    UpdateOrder(orderDetailViewModel.OrderID);

                    dbOrderDetail.UpdateOrderDetail(orderDetailViewModel);
                    dbOrderDetail.DeliveryDate = DateTime.Now;

                    _orderService.UpdateOrderDetail(dbOrderDetail);
                    _orderService.Save();

                    response = request.CreateResponse(HttpStatusCode.Created);
                }

                return response;
            });
        }

        private void UpdateOrder(int id)
        {
            var dbOrder = _orderService.GetOrderById(id);
            dbOrder.Status = true;
            var listOrderDetail = _orderService.GetAllOrderDetail(id);

            foreach (var item in listOrderDetail)
            {
                if (!item.IsDelivery)
                {
                    dbOrder.Status = false;
                    break;
                }
            }
            _orderService.UpdateOrder(dbOrder);
        }

        [HttpDelete]
        [Route("delete/orderdetail")]
        public HttpResponseMessage DeleteOrderDetail(HttpRequestMessage request, int id)
        {
            var orderDetail = _orderService.DeleteOrderDetail(0,id);
            _orderService.Save();
            return request.CreateResponse(HttpStatusCode.OK, orderDetail);
        }

        [Route("deletemulti/orderdetail")]
        [HttpDelete]
        public HttpResponseMessage DeleteMultiOrderDetail(HttpRequestMessage request, string checkedList)
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
                    var listItem = new JavaScriptSerializer().Deserialize<List<int>>(checkedList);
                    foreach (var item in listItem)
                    {
                        _orderService.DeleteOrderDetail(0,item);
                    }

                    _orderService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listItem.Count);
                }

                return response;
            });
        }

        [Route("getorderdetail/{id:int}")]
        [HttpGet]
        public IHttpActionResult GetOrderDetail(int id)
        {
            var model = _orderService.GetAllOrderDetail(id);

            IEnumerable<OrderDetailViewModel> responseData = _mappingService.Mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailViewModel>>(model);
            List<OrderDetailViewModel> data = new List<OrderDetailViewModel>();
            foreach (var item in responseData)
            {
                if (item.IsOrder)
                {
                    var product = _productService.GetById(item.ProductID);
                    item.Product = _mappingService.Mapper.Map<Product, ProductViewModel>(product);
                    data.Add(item);
                }
            }

            return Ok(data);
        }
    }
}