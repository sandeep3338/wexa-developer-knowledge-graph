namespace Wexa.Graph.Api.Models;

public class DeveloperGraph
{
    public Developer Developer { get; set; } = new();

    public List<GraphNode> Nodes { get; set; } = [];

    public List<GraphRelationship> Relationships { get; set; } = [];
}

public class GraphNode
{
    public string Id { get; set; } = string.Empty;

    public string Label { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;
}

public class GraphRelationship
{
    public string Source { get; set; } = string.Empty;

    public string Target { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;
}