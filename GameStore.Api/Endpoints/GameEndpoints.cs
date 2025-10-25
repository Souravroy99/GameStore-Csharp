// Entity Framework Core ---> A lightweight, extensible, open source and cross-platform Object-Relational Mapper(ORM) for .NET.

using GameStore.Api.Dtos;
namespace GameStore.Api.Endpoints;

public static class GameEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = [
        new(
            1,
            "Elden Ring",
            "Action RPG",
            59.99m,
            new DateOnly(2022, 02, 25)), // Year, Month, Date
        new(
            2,
            "Hollow Knight",
            "Metroidvania",
            14.99m,
            new DateOnly(2017, 02, 24)),
        new(
            3,
            "God of War Ragnarök",
            "Action Adventure",
            69.99m,
            new DateOnly(2022, 11, 9))
    ];


    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games").WithParameterValidation();



        // GET /games
        group.MapGet("/", () => games);



        // GET /games/1
        group.MapGet("/{id}", (int id) =>
        {
            GameDto? game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetGameEndpointName);



        // POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
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
            /*
                Results.CreatedAtRoute(
                    string routeName,
                    object? routeValues,
                    object? value
                )
            */

        });
        


        // PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updateGame) =>
        {
            var index = games.FindIndex((game) => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new(
                id,
                updateGame.Name,
                updateGame.Genre,
                updateGame.Price,
                updateGame.ReleaseDate
            );

            return Results.NoContent();
        });



        // DELETE /games/1
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll((game) => game.Id == id);

            return Results.NoContent();
        });


        return group;
    }
}