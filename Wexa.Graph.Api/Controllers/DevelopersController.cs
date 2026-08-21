using Microsoft.AspNetCore.Mvc;
using Wexa.Graph.Api.Services;

namespace Wexa.Graph.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevelopersController : ControllerBase
{
    private readonly DeveloperService _developerService;

    public DevelopersController(DeveloperService developerService)
    {
        _developerService = developerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDevelopers()
    {
        try
        {
            var developers =
                await _developerService.GetDevelopersAsync();

            return Ok(developers);
        }
        catch (Exception)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new
                {
                    message =
                        "Unable to retrieve developers from the graph database."
                });
        }
    }

    [HttpGet("recommendations")]
    public async Task<IActionResult> GetRecommendations(
        [FromQuery] string projectId)
    {
        if (string.IsNullOrWhiteSpace(projectId))
        {
            return BadRequest(new
            {
                message = "projectId is required."
            });
        }

        try
        {
            var recommendations =
                await _developerService
                    .FindDevelopersForProjectAsync(projectId);

            return Ok(recommendations);
        }
        catch (Exception)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new
                {
                    message =
                        "Unable to retrieve developer recommendations."
                });
        }
    }

    [HttpGet("{developerId}")]
    public async Task<IActionResult> GetDeveloper(
        string developerId)
    {
        if (string.IsNullOrWhiteSpace(developerId))
        {
            return BadRequest(new
            {
                message = "developerId is required."
            });
        }

        try
        {
            var developer =
                await _developerService
                    .GetDeveloperProfileAsync(developerId);

            if (developer == null)
            {
                return NotFound(new
                {
                    message = "Developer not found."
                });
            }

            return Ok(developer);
        }
        catch (Exception)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new
                {
                    message =
                        "Unable to retrieve developer profile."
                });
        }
    }
    [HttpGet("~/api/projects")]
    public async Task<IActionResult> GetProjects()
    {
        try
        {
            var projects =
                await _developerService.GetProjectsAsync();

            return Ok(projects);
        }
        catch (Exception)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new
                {
                    message =
                        "Unable to retrieve projects from the graph database."
                });
        }
    }
    [HttpGet("{developerId}/graph")]
    public async Task<IActionResult> GetDeveloperGraph(
    string developerId)
    {
        if (string.IsNullOrWhiteSpace(developerId))
        {
            return BadRequest(new
            {
                message = "developerId is required."
            });
        }

        try
        {
            var graph =
                await _developerService
                    .GetDeveloperGraphAsync(developerId);

            if (graph == null)
            {
                return NotFound(new
                {
                    message = "Developer not found."
                });
            }

            return Ok(graph);
        }
        catch (Exception)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new
                {
                    message =
                        "Unable to retrieve developer graph."
                });
        }
    }
}