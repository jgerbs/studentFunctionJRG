using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class GamesFunctions
{
    private readonly GamesContext _context;

    public GamesFunctions(GamesContext context)
    {
        _context = context;
    }

    // Get all games
    [Function("GetAllGames")]
    public async Task<HttpResponseData> GetAllGames(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "games")] HttpRequestData req,
        FunctionContext executionContext)
    {
        var games = await _context.Games.ToListAsync();
        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteAsJsonAsync(games);
        return response;
    }

    // Get a game by ID
    [Function("GetGameById")]
    public async Task<HttpResponseData> GetGameById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "games/{id}")] HttpRequestData req,
        int id,
        FunctionContext executionContext)
    {
        var game = await _context.Games.FindAsync(id);
        var response = req.CreateResponse(game != null ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.NotFound);
        if (game != null)
        {
            await response.WriteAsJsonAsync(game);
        }
        return response;
    }

    // Create a new game (POST)
    [Function("CreateGame")]
    public async Task<HttpResponseData> CreateGame(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "games")] HttpRequestData req,
        FunctionContext executionContext)
    {
        var game = await req.ReadFromJsonAsync<Game>();
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        var response = req.CreateResponse(System.Net.HttpStatusCode.Created);
        return response;
    }

    // Update an existing game (PUT)
    [Function("UpdateGame")]
    public async Task<HttpResponseData> UpdateGame(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "games/{id}")] HttpRequestData req,
        int id,
        FunctionContext executionContext)
    {
        var game = await _context.Games.FindAsync(id);

        if (game == null)
        {
            return req.CreateResponse(System.Net.HttpStatusCode.NotFound);
        }

        var updatedGame = await req.ReadFromJsonAsync<Game>();
        game.Year = updatedGame.Year;
        game.Gender = updatedGame.Gender;
        game.City = updatedGame.City;
        game.Country = updatedGame.Country;
        game.Continent = updatedGame.Continent;
        game.Winner = updatedGame.Winner;

        await _context.SaveChangesAsync();
        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        return response;
    }

    // Delete a game (DELETE)
    [Function("DeleteGame")]
    public async Task<HttpResponseData> DeleteGame(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "games/{id}")] HttpRequestData req,
        int id,
        FunctionContext executionContext)
    {
        var game = await _context.Games.FindAsync(id);

        if (game == null)
        {
            return req.CreateResponse(System.Net.HttpStatusCode.NotFound);
        }

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();
        return req.CreateResponse(System.Net.HttpStatusCode.NoContent);
    }
}
