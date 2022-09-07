using Microsoft.AspNetCore.Mvc;
using TvMaze.Data.Entities;
using TvMaze.Data.Repositories;

namespace TvMaze.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly IShowAndCastRepository _showAndCastRepository;
        private readonly ILogger<ShowsController> _logger;

        public ShowsController(IShowAndCastRepository showAndCastRepository, ILogger<ShowsController> logger)
        {
            _showAndCastRepository = showAndCastRepository;
            _logger = logger;
        }

        [HttpGet(Name = "GetShowsAndCast")]
        public async Task<IEnumerable<Show>> Get([FromQuery]int pageNumber)
        {
            return await _showAndCastRepository.GetAll(pageNumber);
        }
    }
}