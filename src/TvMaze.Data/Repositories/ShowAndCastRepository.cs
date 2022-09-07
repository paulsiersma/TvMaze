using Microsoft.EntityFrameworkCore;
using TvMaze.Data.Entities;

namespace TvMaze.Data.Repositories;

public class ShowAndCastRepository : IShowAndCastRepository
{
    private readonly TvMazeDbContext _dbContext;
    private const int PageSize = 10;

    public ShowAndCastRepository(TvMazeDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<Show>> GetAll(int pageNumber)
    {
        var firstId = pageNumber * PageSize;
        var lastId = pageNumber * PageSize + PageSize;

        return await _dbContext.Shows
            .Where(s => s.ShowId >= firstId && s.ShowId < lastId)
            .Include(s => s.Actors.OrderByDescending(a => a.Birthday))
            .ToListAsync();
    }

    public async Task<int> GetMaxShowId()
    {
        if (await _dbContext.Shows.CountAsync() == 0)
        {
            return 0;
        }

        return await _dbContext.Shows.MaxAsync(s => s.ShowId);
    }

    public async Task AddOrUpdateShows(IEnumerable<Show> shows)
    {
        foreach (var show in shows.DistinctBy(s => s.ShowId))
        {
            var actors = new List<Actor>(show.Actors.DistinctBy(a => a.ActorId));
            show.Actors = new List<Actor>();

            var existingShow = _dbContext.Shows.SingleOrDefault(s => s.ShowId == show.ShowId);
            if (existingShow == null)
            {
                _dbContext.Shows.Add(show);
            }

            foreach (var actor in actors)
            {
                var existingActor = _dbContext.Actors.SingleOrDefault(a => a.ActorId == actor.ActorId);
                if (existingActor == null)
                {
                    actor.Shows.Add(existingShow ?? show);
                    _dbContext.Actors.Add(actor);
                }
                else
                {
                    if (existingActor.Shows.All(s => s.ShowId != show.ShowId))
                    {
                        existingActor.Shows.Add(existingShow ?? show);
                    }
                }
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}

public interface IShowAndCastRepository
{
    public Task<int> GetMaxShowId();
    Task<IEnumerable<Show>> GetAll(int pageNumber);
    public Task AddOrUpdateShows(IEnumerable<Show> shows);
}