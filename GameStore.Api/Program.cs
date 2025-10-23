using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
    new(
        1,
        "Elden Ring",
        "Action RPG",
        59.99m,
        new DateOnly(2022, 2, 25)),
    new(
        2,
        "Hollow Knight",
        "Metroidvania",
        14.99m,
        new DateOnly(2017, 2, 24)),
    new(
        3,
        "God of War Ragnarök",
        "Action Adventure",
        69.99m,
        new DateOnly(2022, 11, 9))
];

// GET /games
app.MapGet("games", () => games);

// GET /games/1
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id)).WithName(GetGameEndpointName);


// POST /games
app.MapPost("games", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

app.Run();