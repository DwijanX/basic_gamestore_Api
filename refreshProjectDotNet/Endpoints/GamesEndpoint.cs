using refreshProjectDotNet.Data;
using refreshProjectDotNet.Dtos;
using refreshProjectDotNet.Entities;
using refreshProjectDotNet.Mapping;

namespace refreshProjectDotNet.Endpoints;

public static class GamesEndpoint
{
    const string GetGameEndpoint="GetGame";

    private static readonly List<GameDto> games=[ //m is to declare that the number is a decimal
        new GameDto(1,"GTA V","Action",20.5m,new DateOnly(2013,9,17)),
        new GameDto(2,"FIFA 22","Sport",60.5m,new DateOnly(2021,10,1)),
        new GameDto(3,"Cyberpunk 2077","RPG",40.5m,new DateOnly(2020,12,10)),
        new GameDto(4,"The Witcher 3","RPG",30.5m,new DateOnly(2015,5,19)),
        new GameDto(5,"FIFA 21","Sport",50.5m,new DateOnly(2020,10,6)),
        new GameDto(6,"FIFA 20","Sport",40.5m,new DateOnly(2019,9,24)),
        new GameDto(7,"FIFA 19","Sport",30.5m,new DateOnly(2018,9,28)),
        new GameDto(8,"FIFA 18","Sport",20.5m,new DateOnly(2017,9,29)),
        new GameDto(9,"FIFA 17","Sport",10.5m,new DateOnly(2016,9,27)),
        new GameDto(10,"FIFA 16","Sport",5.5m,new DateOnly(2015,9,22))

    ];

    //this makes the method an extension method, extension classes are always static
    
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app){

        var group=app.MapGroup("games").WithParameterValidation(); //this is a middleware that will add /games to all the routes
        
        
        // Get /games
        group.MapGet("/", () => games);

        // GET /games/1
        group.MapGet("/{id}", (int id) => {
            GameDto? game=games.Find(game=>game.Id==id);
            return game is null?Results.NotFound():Results.Ok(game);}).WithName(GetGameEndpoint);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame,GameStoreContext dbContext)=>{
            
            Game game=newGame.ToEntity();
            game.Genre=dbContext.Genres.Find(newGame.GenreId);
            
            
            dbContext.Games.Add(game);
            dbContext.SaveChanges();
            return Results.CreatedAtRoute(GetGameEndpoint,new {id=game.Id},game.ToDto());
        });

        // PUT /games

        group.MapPut("/{id}",(int id,UpdateGameDto updatedGame)=>{
            var index=games.FindIndex(game=>game.Id==id);
            if(index==-1) return Results.NotFound();
            games[index]=new GameDto(id,updatedGame.Name,updatedGame.Genre,updatedGame.Price,updatedGame.ReleaseDate);
            return Results.NoContent();
        });

        // Delete /games
        group.MapDelete("/{id}",(int id)=>{
            games.RemoveAll(game=>game.Id==id);
            return Results.NoContent();
        });

        return group;
    }
}
