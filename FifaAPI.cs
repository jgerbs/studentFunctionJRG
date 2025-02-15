using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StudentFunctions.Models.Game;
using Microsoft.EntityFrameworkCore; // For async methods

namespace Fifa.Function
{
    public class FifaAPI
    {
        private readonly ILogger<FifaAPI> _logger;
        private readonly FifaContext _context;

        public FifaAPI(ILogger<FifaAPI> logger, FifaContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Function("Welcome")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("Azure Functions FIFA API is running.");
            return new OkObjectResult("Welcome to the FIFA API!");
        }

        [Function("GetGames")]
        public async Task<HttpResponseData> GetGames(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "games")] HttpRequestData req)
        {
            _logger.LogInformation("Fetching all FIFA games.");

            var games = await _context.Games.ToArrayAsync(); // Async call

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");

            await response.WriteStringAsync(JsonConvert.SerializeObject(games)); // Use WriteStringAsync here

            return response;
        }

        [Function("GetGameById")]
        public async Task<HttpResponseData> GetGameById
        (
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "games/{id}")] HttpRequestData req,
            int id
        )
        {
            _logger.LogInformation($"Fetching game with ID: {id}");

            var game = await _context.Games.FindAsync(id); // Async call
            if (game == null)
            {
                var response = req.CreateResponse(HttpStatusCode.NotFound);
                response.Headers.Add("Content-Type", "application/json");
                await response.WriteStringAsync("Not Found"); // Use WriteStringAsync here
                return response;
            }

            var response2 = req.CreateResponse(HttpStatusCode.OK);
            response2.Headers.Add("Content-Type", "application/json");
            await response2.WriteStringAsync(JsonConvert.SerializeObject(game)); // Use WriteStringAsync here
            return response2;
        }

        [Function("CreateGame")]
        public async Task<HttpResponseData> CreateGame
        (
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "games")] HttpRequestData req
        )
        {
            _logger.LogInformation("Creating a new FIFA game.");

            var game = JsonConvert.DeserializeObject<Game>(await req.ReadAsStringAsync()); // Async read
            _context.Games.Add(game);
            await _context.SaveChangesAsync(); // Async call

            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(JsonConvert.SerializeObject(game)); // Use WriteStringAsync here

            return response;
        }

        [Function("UpdateGame")]
        public async Task<HttpResponseData> UpdateGame
        (
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "games/{id}")] HttpRequestData req,
            int id
        )
        {
            _logger.LogInformation($"Updating FIFA game with ID: {id}");

            var game = await _context.Games.FindAsync(id); // Async call
            if (game == null)
            {
                var response = req.CreateResponse(HttpStatusCode.NotFound);
                response.Headers.Add("Content-Type", "application/json");
                await response.WriteStringAsync("Not Found"); // Use WriteStringAsync here
                return response;
            }

            var updatedGame = JsonConvert.DeserializeObject<Game>(await req.ReadAsStringAsync()); // Async read
            game.Year = updatedGame.Year;
            game.Gender = updatedGame.Gender;
            game.City = updatedGame.City;
            game.Country = updatedGame.Country;
            game.Continent = updatedGame.Continent;
            game.Winner = updatedGame.Winner;

            await _context.SaveChangesAsync(); // Async call

            var response2 = req.CreateResponse(HttpStatusCode.OK);
            response2.Headers.Add("Content-Type", "application/json");
            await response2.WriteStringAsync(JsonConvert.SerializeObject(game)); // Use WriteStringAsync here
            return response2;
        }

        [Function("DeleteGame")]
        public async Task<HttpResponseData> DeleteGame
        (
          [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "games/{id}")] HttpRequestData req,
          int id
        )
        {
            _logger.LogInformation($"Deleting FIFA game with ID: {id}");

            var game = await _context.Games.FindAsync(id); // Async call
            if (game == null)
            {
                var response = req.CreateResponse(HttpStatusCode.NotFound);
                response.Headers.Add("Content-Type", "application/json");
                await response.WriteStringAsync("Not Found"); // Use WriteStringAsync here
                return response;
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync(); // Async call

            var response2 = req.CreateResponse(HttpStatusCode.OK);
            response2.Headers.Add("Content-Type", "application/json");
            await response2.WriteStringAsync(JsonConvert.SerializeObject(game)); // Use WriteStringAsync here

            return response2;
        }
    }
}
