using AutoMapper;
using TvMaze.Data.Entities;
using TvMaze.Scraper.Models;

namespace TvMaze.Scraper.Mapping;

public class TvMazeProfile: Profile
{
    public TvMazeProfile()
    {
        CreateMap<TvMazeApiShowModel, Show>().IncludeMembers(s => s.EmbeddedCast)
            .ForMember(d => d.ShowId, opt => opt.MapFrom(s => s.Id));
        CreateMap<TvMazeApiEmbeddedCastModel, Show>().IncludeMembers(s => s.Actors);
        CreateMap<ICollection<TvMazeApiCastMemberModel>, Show>();
        CreateMap<TvMazeApiCastMemberModel, Actor>().IncludeMembers(s => s.Person);
        CreateMap<TvMazeApiPersonModel, Actor>()
            .ForMember(d => d.ActorId, opt => opt.MapFrom(s => s.Id));
    }
}