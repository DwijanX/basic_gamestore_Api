using refreshProjectDotNet.Data;
using refreshProjectDotNet.Dtos;
using refreshProjectDotNet.Entities;
using refreshProjectDotNet.Mapping;

namespace refreshProjectDotNet.Endpoints;

public static class GamesEndpoint
{
    const string GetGameEndpoint="GetGame";

    private static readonly List<GameSummaryDto> games=[ //m is to declare that the number is a decimal
        new GameSummaryDto(1,"GTA V","Action",20.5m,new DateOnly(2013,9,17)),
        new GameSummaryDto(2,"FIFA 22","Sport",60.5m,new DateOnly(2021,10,1)),
        new GameSummaryDto(3,"Cyberpunk 2077","RPG",40.5m,new DateOnly(2020,12,10)),
        new GameSummaryDto(4,"The Witcher 3","RPG",30.5m,new DateOnly(2015,5,19)),
        new GameSummaryDto(5,"FIFA 21","Sport",50.5m,new DateOnly(2020,10,6)),
        new GameSummaryDto(6,"FIFA 20","Sport",40.5m,new DateOnly(2019,9,24)),
        new GameSummaryDto(7,"FIFA 19","Sport",30.5m,new DateOnly(2018,9,28)),
        new GameSummaryDto(8,"FIFA 18","Sport",20.5m,new DateOnly(2017,9,29)),
        new GameSummaryDto(9,"FIFA 17","Sport",10.5m,new DateOnly(2016,9,27)),
        new GameSummaryDto(10,"FIFA 16","Sport",5.5m,new DateOnly(2015,9,22))

    ];

    //this makes the method an extension method, extension classes are always static
    
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app){

        var group=app.MapGroup("games").WithParameterValidation(); //this is a middleware that will add /games to all the routes
        
        
        // Get /games
        group.MapGet("/", (GameStoreContext dbContext) => dbContext.Games);

        // GET /games/1
        group.MapGet("/{id}", (int id,GameStoreContext dbContext) => {
            Game game=dbContext.Games.Find(id);
            return game is null?Results.NotFound():Results.Ok(game.ToDetailsDto());
        })
        .WithName(GetGameEndpoint);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame,GameStoreContext dbContext)=>{
            
            Game game=newGame.ToEntity();
            
            
            dbContext.Games.Add(game);
            dbContext.SaveChanges();
            return Results.CreatedAtRoute(GetGameEndpoint,new {id=game.Id},game.ToDetailsDto());
        });

        // PUT /games

        group.MapPut("/{id}",(int id,UpdateGameDto updatedGame,GameStoreContext dbContext)=>{
            var existingGame=dbContext.Games.Find(id);
            if(existingGame is null)
                return Results.NotFound();
            dbContext.Entry(existingGame).CurrentValues.SetValues(updatedGame.ToEntity(id));
            dbContext.SaveChanges();
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
