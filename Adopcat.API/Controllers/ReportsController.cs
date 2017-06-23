﻿using Adopcat.API.Filters;
using Adopcat.Model;
using Adopcat.Services.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace Adopcat.API.Controllers
{
    [CustomAuthorize]
    [RoutePrefix("api/reports")]
    public class ReportsController : BaseApiController
    {
        private IReportsService _reportsService;
        
        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(Reports model)
        {
            var poster = await _reportsService.CreateAsync(model).ConfigureAwait(false);
            return Ok(poster);
        }
    }
}
