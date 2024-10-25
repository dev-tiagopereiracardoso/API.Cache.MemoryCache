using API.Cache.MemoryCache.Domain.Implementation.Interfaces;
using API.Cache.MemoryCache.Models.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace API.Cache.MemoryCache.Service.Controllers
{
    [Route("v1/cache")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "cache")]
    public class CacheController : ControllerBase
    {
        private readonly IMemoryService _memoryService;

        public CacheController(
                IMemoryService memoryService
            )
        {
            _memoryService = memoryService;
        }

        [HttpGet("{key}"), Authorize]
        [SwaggerOperation(Summary = "")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(string))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status417ExpectationFailed)]
        public IActionResult Search(string key)
        {
            var Data = _memoryService.Get(key);
            return StatusCode((int)HttpStatusCode.OK, Data);
        }

        [HttpPost("{key}"), Authorize]
        [SwaggerOperation(Summary = "")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(bool))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status417ExpectationFailed)]
        public IActionResult Search(string key, [FromBody] CacheInput cacheInput)
        {
            var Data = _memoryService.Set(key, cacheInput.Value);
            return StatusCode(Data ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest);
        }

        [HttpDelete(), Authorize]
        [SwaggerOperation(Summary = "")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(bool))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status417ExpectationFailed)]
        public IActionResult Delete()
        {
            _memoryService.Delete();
            return StatusCode((int)HttpStatusCode.OK);
        }
    }
}
