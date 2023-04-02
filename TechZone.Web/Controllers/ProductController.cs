﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TechZone.Common;
using TechZone.Model.Models;
using TechZone.Service;
using TechZone.Web.Infrastructure.Core;
using TechZone.Web.Mappings;
using TechZone.Web.Models;

namespace TechZone.Web.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productService;
        private IProductCategoryService _productCategoryService;
        private IMappingService _mappingService;

        public ProductController(IProductService productService,
            IProductCategoryService productCategoryService,
            IMappingService mappingService)
        {
            this._productService = productService;
            this._productCategoryService = productCategoryService;
            _mappingService = mappingService;
        }

        public ActionResult Detail(int productId)
        {
            return View();
        }

        public ActionResult Category(int id, int page = 1)
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;
            var productModel = _productService.GetListProductByCategoryIdPaging(id, page, pageSize, out totalRow);
            var productViewModel = _mappingService.Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            var category = _productCategoryService.GetById(id);
            ViewBag.Category = _mappingService.Mapper.Map<ProductCategory, ProductCategoryViewModel>(category);
            var paginationSet = new PaginationSet<ProductViewModel>()
            {
                Items = productViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            return View(paginationSet);
        }
    }
}