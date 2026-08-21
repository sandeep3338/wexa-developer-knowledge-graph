using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
using Wexa.Graph.Api.Data;

namespace Wexa.Graph.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly IDriver _driver;

    public DatabaseController(IDriver driver)
    {
        _driver = driver;
    }

    [HttpPost("seed")]
    public async Task<IActionResult> Seed()
    {
        try
        {
            await GraphSeeder.SeedAsync(_driver);

            return Ok(new
            {
                message = "Graph database seeded successfully."
            });
        }
        catch (Exception)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new
                {
                    message = "Unable to seed the graph database."
                });
        }
    }
}