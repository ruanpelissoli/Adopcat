﻿using MeAdota.API.Filters;
using MeAdota.API.Mapping;
using MeAdota.Model.DTO;
using MeAdota.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace MeAdota.API.Controllers
{
    [CustomAuthorize]
    [RoutePrefix("api/poster")]
    public class PosterController : BaseApiController
    {
        private IPosterService _posterService;
        private IUserService _userService;

        public PosterController(IPosterService posterService, IUserService userService)
        {
            _posterService = posterService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            var poster = await _posterService.GetAsync(id).ConfigureAwait(false);

            if (poster == null) return BadRequest();
            return Ok(MappingConfig.Mapper().Map<PosterOutputDTO>(poster));
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllPosters()
        {
            var user = await _userService.GetByToken(Token);
            if (user == null) return Unauthorized();

            var posters = await _posterService.GetAllPostersAsync(user.Id).ConfigureAwait(false);
            return Ok(MappingConfig.Mapper().Map<List<PosterOutputDTO>>(posters));
        }

        [HttpGet]
        [Route("filter")]
        public async Task<IHttpActionResult> GetAllPosters(string city, int? petType, bool? castrated, bool? dewormed)
        {
            var user = await _userService.GetByToken(Token);
            if (user == null) return Unauthorized();

            var filter = new FilterDTO
            {
                PetType = petType,
                Castrated = castrated,
                City = city
            };
            
            var posters = await _posterService.GetAllPostersAsync(user.Id, filter).ConfigureAwait(false);
            return Ok(MappingConfig.Mapper().Map<List<PosterOutputDTO>>(posters));
        }

        [HttpGet]
        [Route("my")]
        public async Task<IHttpActionResult> GetMyPosters()
        {
            var user = await _userService.GetByToken(Token);
            if (user == null) return Unauthorized();

            var posters = await _posterService.GetByUserIdAsync(user.Id).ConfigureAwait(false);
            return Ok(MappingConfig.Mapper().Map<List<PosterOutputDTO>>(posters));
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(PosterInputDTO model)
        {
            var poster = await _posterService.CreateAsync(model).ConfigureAwait(false);
            return Ok(MappingConfig.Mapper().Map<PosterOutputDTO>(poster));
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put(PosterInputDTO model)
        {
            await _posterService.UpdateAsync(model).ConfigureAwait(false);
            return Ok();
        }
    }
}
