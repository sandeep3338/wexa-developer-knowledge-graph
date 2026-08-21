using Neo4j.Driver;
using Wexa.Graph.Api.Models;
using Wexa.Graph.Api.Repositories;

namespace Wexa.Graph.Api.Services;

public class DeveloperService
{
    private readonly GraphRepository _repository;

    public DeveloperService(GraphRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<DeveloperRecommendation>>
        FindDevelopersForProjectAsync(string projectId)
    {
        const string query = """
            MATCH (p:Project {id: $projectId})-[:USES]->(t:Technology)
            MATCH (t)-[:RELATED_TO*0..2]->(related:Technology)
            MATCH (d:Developer)-[:WORKED_ON]->(:Project)-[:USES]->(related)

            RETURN DISTINCT
                d.id AS developerId,
                d.name AS developerName,
                d.role AS role,
                d.experienceYears AS experienceYears,
                collect(DISTINCT related.name) AS matchingTechnologies

            ORDER BY experienceYears DESC
            """;

        return await _repository.ExecuteAsync(
            query,
            record => new DeveloperRecommendation
            {
                DeveloperId =
    record.Get<string>("developerId"),

                DeveloperName =
    record.Get<string>("developerName"),

                Role =
    record.Get<string>("role"),

                ExperienceYears =
    record.Get<int>("experienceYears"),

                MatchingTechnologies =
    record.Get<List<string>>("matchingTechnologies")
            },
            new { projectId });
    }
    public async Task<DeveloperProfile?> GetDeveloperProfileAsync(
    string developerId)
    {
        const string query = """
        MATCH (d:Developer {id: $developerId})

        OPTIONAL MATCH (d)-[:HAS_SKILL]->(skill:Skill)

        OPTIONAL MATCH (d)-[:WORKED_ON]->(project:Project)

        OPTIONAL MATCH (project)-[:USES]->(technology:Technology)

        OPTIONAL MATCH (d)-[:WORKED_AT]->(company:Company)

        RETURN
            d.id AS id,
            d.name AS name,
            d.role AS role,
            d.experienceYears AS experienceYears,
            collect(DISTINCT skill.name) AS skills,
            collect(DISTINCT project.name) AS projects,
            collect(DISTINCT technology.name) AS technologies,
            collect(DISTINCT company.name) AS companies
        """;

        var results = await _repository.ExecuteAsync(
            query,
            record => new DeveloperProfile
            {
                Id = record.Get<string>("id"),
                Name = record.Get<string>("name"),
                Role = record.Get<string>("role"),
                ExperienceYears =
                    record.Get<int>("experienceYears"),
                Skills =
                    record.Get<List<string>>("skills"),
                Projects =
                    record.Get<List<string>>("projects"),
                Technologies =
                    record.Get<List<string>>("technologies"),
                Companies =
                    record.Get<List<string>>("companies")
            },
            new { developerId });

        return results.FirstOrDefault();
    }
    public async Task<IReadOnlyList<Developer>> GetDevelopersAsync()
    {
        const string query = """
        MATCH (d:Developer)
        RETURN d.id AS id,
               d.name AS name,
               d.role AS role,
               d.experienceYears AS experienceYears
        ORDER BY d.name
        """;

        return await _repository.ExecuteAsync(
            query,
            record => new Developer
            {
                Id = record.Get<string>("id"),
                Name = record.Get<string>("name"),
                Role = record.Get<string>("role"),
                ExperienceYears =
                    record.Get<int>("experienceYears")
            });
    }
    public async Task<IReadOnlyList<Project>> GetProjectsAsync()
    {
        const string query = """
        MATCH (p:Project)
        RETURN p.id AS id,
               p.name AS name,
               p.description AS description
        ORDER BY p.name
        """;

        return await _repository.ExecuteAsync(
            query,
            record => new Project
            {
                Id = record.Get<string>("id"),
                Name = record.Get<string>("name"),
                Description = record.Get<string>("description")
            });
    }

    public async Task<DeveloperGraph?> GetDeveloperGraphAsync(
    string developerId)
    {
        const string query = """
        MATCH (d:Developer {id: $developerId})

        OPTIONAL MATCH (d)-[r]-(connected)

        RETURN
            d,
            collect(DISTINCT connected) AS nodes,
            collect(DISTINCT r) AS relationships
        """;

        var results = await _repository.ExecuteAsync(
            query,
            record =>
            {
                var developerNode =
                    record.Get<INode>("d");

                var connectedNodes =
                    record.Get<List<INode>>("nodes");

                var relationships =
                    record.Get<List<IRelationship>>("relationships");

                var nodes = new List<GraphNode>
                {
                new()
                {
                    Id = developerNode.ElementId,
                    Label = developerNode
                        .Properties
                        .GetValueOrDefault("name")
                        ?.ToString() ?? developerId,
                    Type = "Developer"
                }
                };

                nodes.AddRange(
                    connectedNodes.Select(node =>
                        new GraphNode
                        {
                            Id = node.ElementId,
                            Label = node.Properties
                                .GetValueOrDefault("name")
                                ?.ToString() ?? node.ElementId,
                            Type = node.Labels.FirstOrDefault()
                                   ?? "Node"
                        }));

                var graphRelationships =
                    relationships.Select(r =>
                        new GraphRelationship
                        {
                            Source = r.StartNodeElementId,
                            Target = r.EndNodeElementId,
                            Type = r.Type
                        })
                        .ToList();

                return new DeveloperGraph
                {
                    Developer = new Developer
                    {
                        Id = developerNode
                            .Properties["id"]
                            .As<string>(),

                        Name = developerNode
                            .Properties["name"]
                            .As<string>(),

                        Role = developerNode
                            .Properties["role"]
                            .As<string>(),

                        ExperienceYears =
                            developerNode
                                .Properties["experienceYears"]
                                .As<int>()
                    },

                    Nodes = nodes,
                    Relationships = graphRelationships
                };
            },
            new { developerId });

        return results.FirstOrDefault();
    }
}