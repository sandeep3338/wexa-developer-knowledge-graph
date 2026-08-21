namespace Wexa.Graph.Api.Models;

public class DeveloperProfile
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public int ExperienceYears { get; set; }

    public List<string> Skills { get; set; } = [];

    public List<string> Projects { get; set; } = [];

    public List<string> Technologies { get; set; } = [];

    public List<string> Companies { get; set; } = [];
}