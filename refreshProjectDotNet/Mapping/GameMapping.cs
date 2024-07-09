using refreshProjectDotNet.Dtos;
using refreshProjectDotNet.Entities;

namespace refreshProjectDotNet.Mapping;

public static class GameMapping
{
    public static Game ToEntity(this CreateGameDto dto)
    {
        return new Game
        {
            Name = dto.Name,
            GenreId = dto.GenreId,
            Price = dto.Price,
            ReleaseDate = dto.ReleaseDate
        };
    }
    public static GameSummaryDto ToSummaryDto(this Game entity)
    {
        return new(
        
            entity.Id,
            entity.Name,
            entity.Genre!.Name,
            entity.Price,
            entity.ReleaseDate
        );
    }
    public static GameDetailsDto ToDetailsDto(this Game entity)
    {
        return new(
            entity.Id,
            entity.Name,
            entity.GenreId,
            entity.Price,
            entity.ReleaseDate
        );
    }
}
