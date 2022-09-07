using AutoMapper;
using TvMaze.Data.Entities;
using TvMaze.Data.Repositories;
using TvMaze.Scraper.Services;

namespace TvMaze.Scraper
{
    public class Scraper : BackgroundService
    {
        private readonly ITvMazeApiClient _tvMazeApiClient;
        private readonly IShowAndCastRepository _showAndCastRepository;
        private readonly ILogger<Scraper> _logger;
        private readonly IMapper _mapper;

        public Scraper(ITvMazeApiClient tvMazeApiClient, IMapper mapper, IServiceScopeFactory factory, ILogger<Scraper> logger)
        {
            _tvMazeApiClient = tvMazeApiClient;
            _mapper = mapper; 
            _showAndCastRepository = factory.CreateScope().ServiceProvider.GetRequiredService<IShowAndCastRepository>();
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Get the max Show Id currently in our database
            var maxShowId = await _showAndCastRepository.GetMaxShowId();

            while (!stoppingToken.IsCancellationRequested)
            {
                var tvMazeApiShowModels = await _tvMazeApiClient.ScrapeShows(maxShowId);

                var shows = _mapper.Map<IEnumerable<Show>>(tvMazeApiShowModels);
                if (shows == null) continue;

                var showsList = shows.ToList();
                await _showAndCastRepository.AddOrUpdateShows(showsList);
                maxShowId = showsList.Max(s => s.ShowId);
            }
        }
    }
}