namespace Wexa.Graph.Api.Models;

public class DeveloperRecommendation
{
    public string DeveloperId { get; set; } = string.Empty;

    public string DeveloperName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public int ExperienceYears { get; set; }

    public List<string> MatchingTechnologies { get; set; } = [];
}