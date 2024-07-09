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
    public static GameDto ToDto(this Game entity)
    {
        return new(
        
            entity.Id,
            entity.Name,
            entity.Genre!.Name,
            entity.Price,
            entity.ReleaseDate
        );
    }
}
