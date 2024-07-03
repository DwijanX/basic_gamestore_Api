using refreshProjectDotNet.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpoint="GetGame";

List<GameDto> games=[ //m is to declare that the number is a decimal
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

// Get /games
app.MapGet("/games", () => games);

// GET /games/1
app.MapGet("/games/{id}", (int id) => games.Find(game=>game.Id==id))
    .WithName(GetGameEndpoint);

// POST /games
app.MapPost("/games", (CreateGameDto newGame)=>{
    var gameDto=new GameDto(games.Max(game=>game.Id)+1,newGame.Name,newGame.Genre,newGame.Price,newGame.ReleaseDate);
    games.Add(gameDto);
    return Results.CreatedAtRoute(GetGameEndpoint,new {id=gameDto.Id},gameDto);
});

// PUT /games

app.MapPut("games/{id}",(int id,UpdateGameDto updatedGame)=>{
    var index=games.FindIndex(game=>game.Id==id);
    games[index]=new GameDto(id,updatedGame.Name,updatedGame.Genre,updatedGame.Price,updatedGame.ReleaseDate);
    return Results.NoContent();
});

app.Run();
