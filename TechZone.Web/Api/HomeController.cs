﻿using System.Web.Http;
using TechZone.Service;
using TechZone.Web.Infrastructure.Core;

namespace TechZone.Web.Api
{
    [RoutePrefix("api/home")]
    [Authorize]
    public class HomeController : ApiControllerBase
    {
        private IErrorService _errorService;

        public HomeController(IErrorService errorService) : base(errorService)
        {
            this._errorService = errorService;
        }

        [HttpGet]
        [Route("TestMethod")]
        public string TestMethod()
        {
            return "Hello, TECHZONE Member. ";
        }
    }
}