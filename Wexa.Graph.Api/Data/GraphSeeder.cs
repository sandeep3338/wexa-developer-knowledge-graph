using Neo4j.Driver;

namespace Wexa.Graph.Api.Data;

public static class GraphSeeder
{
    public static async Task SeedAsync(IDriver driver)
    {
        await using var session = driver.AsyncSession();

        await CreateConstraintsAsync(session);
        await CreateDevelopersAsync(session);
        await CreateSkillsAsync(session);
        await CreateTechnologiesAsync(session);
        await CreateCompaniesAsync(session);
        await CreateProjectsAsync(session);
        await CreateDeveloperSkillsAsync(session);
        await CreateDeveloperProjectsAsync(session);
        await CreateDeveloperCompaniesAsync(session);
        await CreateProjectTechnologiesAsync(session);
        await CreateProjectCompaniesAsync(session);
        await CreateTechnologyRelationshipsAsync(session);
    }

    private static async Task CreateConstraintsAsync(IAsyncSession session)
    {
        var queries = new[]
        {
            """
            CREATE CONSTRAINT developer_id_unique IF NOT EXISTS
            FOR (d:Developer)
            REQUIRE d.id IS UNIQUE
            """,

            """
            CREATE CONSTRAINT skill_id_unique IF NOT EXISTS
            FOR (s:Skill)
            REQUIRE s.id IS UNIQUE
            """,

            """
            CREATE CONSTRAINT technology_id_unique IF NOT EXISTS
            FOR (t:Technology)
            REQUIRE t.id IS UNIQUE
            """,

            """
            CREATE CONSTRAINT company_id_unique IF NOT EXISTS
            FOR (c:Company)
            REQUIRE c.id IS UNIQUE
            """,

            """
            CREATE CONSTRAINT project_id_unique IF NOT EXISTS
            FOR (p:Project)
            REQUIRE p.id IS UNIQUE
            """
        };

        foreach (var query in queries)
        {
            await session.RunAsync(query);
        }
    }

    private static async Task CreateDevelopersAsync(IAsyncSession session)
    {
        var developers = new[]
        {
            new { Id = "dev-001", Name = "Arjun Sharma", Role = "Backend Developer", Experience = 5 },
            new { Id = "dev-002", Name = "Priya Nair", Role = "Full Stack Developer", Experience = 6 },
            new { Id = "dev-003", Name = "Rahul Mehta", Role = "Software Engineer", Experience = 4 },
            new { Id = "dev-004", Name = "Sneha Rao", Role = "Frontend Developer", Experience = 3 },
            new { Id = "dev-005", Name = "Vikram Singh", Role = "Backend Developer", Experience = 7 },
            new { Id = "dev-006", Name = "Ananya Iyer", Role = "Cloud Engineer", Experience = 5 },
            new { Id = "dev-007", Name = "Kiran Reddy", Role = "Software Engineer", Experience = 4 },
            new { Id = "dev-008", Name = "Meera Kapoor", Role = "Full Stack Developer", Experience = 6 },
            new { Id = "dev-009", Name = "Rohit Verma", Role = "Backend Developer", Experience = 8 },
            new { Id = "dev-010", Name = "Sandeep Kumar", Role = "Software Engineer", Experience = 3 }
        };

        const string query = """
            UNWIND $developers AS developer
            MERGE (d:Developer {id: developer.Id})
            SET d.name = developer.Name,
                d.role = developer.Role,
                d.experienceYears = developer.Experience
            """;

        await session.RunAsync(query, new { developers });
    }

    private static async Task CreateSkillsAsync(IAsyncSession session)
    {
        var skills = new[]
        {
            new { Id = "skill-001", Name = "Backend Development" },
            new { Id = "skill-002", Name = "API Development" },
            new { Id = "skill-003", Name = "Database Design" },
            new { Id = "skill-004", Name = "Cloud Computing" },
            new { Id = "skill-005", Name = "Frontend Development" },
            new { Id = "skill-006", Name = "DevOps" },
            new { Id = "skill-007", Name = "Graph Databases" },
            new { Id = "skill-008", Name = "Automation Testing" }
        };

        const string query = """
            UNWIND $skills AS skill
            MERGE (s:Skill {id: skill.Id})
            SET s.name = skill.Name
            """;

        await session.RunAsync(query, new { skills });
    }

    private static async Task CreateTechnologiesAsync(IAsyncSession session)
    {
        var technologies = new[]
        {
            new { Id = "tech-001", Name = "C#", Category = "Programming Language" },
            new { Id = "tech-002", Name = ".NET", Category = "Framework" },
            new { Id = "tech-003", Name = "ASP.NET Core", Category = "Framework" },
            new { Id = "tech-004", Name = "Angular", Category = "Frontend Framework" },
            new { Id = "tech-005", Name = "SQL Server", Category = "Database" },
            new { Id = "tech-006", Name = "Azure", Category = "Cloud" },
            new { Id = "tech-007", Name = "Docker", Category = "DevOps" },
            new { Id = "tech-008", Name = "Redis", Category = "Database" },
            new { Id = "tech-009", Name = "Python", Category = "Programming Language" },
            new { Id = "tech-010", Name = "Java", Category = "Programming Language" },
            new { Id = "tech-011", Name = "React", Category = "Frontend Framework" },
            new { Id = "tech-012", Name = "Git", Category = "Development Tool" }
        };

        const string query = """
            UNWIND $technologies AS technology
            MERGE (t:Technology {id: technology.Id})
            SET t.name = technology.Name,
                t.category = technology.Category
            """;

        await session.RunAsync(query, new { technologies });
    }

    private static async Task CreateCompaniesAsync(IAsyncSession session)
    {
        var companies = new[]
        {
            new { Id = "company-001", Name = "TechNova", Industry = "Software" },
            new { Id = "company-002", Name = "CloudWorks", Industry = "Cloud Services" },
            new { Id = "company-003", Name = "DataSphere", Industry = "Data Analytics" },
            new { Id = "company-004", Name = "InnovateLabs", Industry = "Technology" },
            new { Id = "company-005", Name = "NextGen Systems", Industry = "Enterprise Software" }
        };

        const string query = """
            UNWIND $companies AS company
            MERGE (c:Company {id: company.Id})
            SET c.name = company.Name,
                c.industry = company.Industry
            """;

        await session.RunAsync(query, new { companies });
    }

    private static async Task CreateProjectsAsync(IAsyncSession session)
    {
        var projects = new[]
        {
            new { Id = "project-001", Name = "PLM Platform", Description = "Product lifecycle management platform" },
            new { Id = "project-002", Name = "E-Commerce API", Description = "Scalable commerce backend" },
            new { Id = "project-003", Name = "Inventory Management", Description = "Real-time inventory management system" },
            new { Id = "project-004", Name = "Payment Gateway", Description = "Secure payment processing platform" },
            new { Id = "project-005", Name = "Analytics Platform", Description = "Business analytics and reporting platform" },
            new { Id = "project-006", Name = "HR Management", Description = "Employee management application" },
            new { Id = "project-007", Name = "Logistics System", Description = "Shipment and logistics management system" },
            new { Id = "project-008", Name = "Automation Framework", Description = "UI and API test automation framework" }
        };

        const string query = """
            UNWIND $projects AS project
            MERGE (p:Project {id: project.Id})
            SET p.name = project.Name,
                p.description = project.Description
            """;

        await session.RunAsync(query, new { projects });
    }

    private static async Task CreateDeveloperSkillsAsync(IAsyncSession session)
    {
        const string query = """
    UNWIND $relationships AS relationship
    MATCH (d:Developer {id: relationship.DeveloperId})
    MATCH (s:Skill {id: relationship.SkillId})
    MERGE (d)-[r:HAS_SKILL]->(s)
    SET r.level = relationship.Level
    """;

        var relationships = new[]
        {
            new { DeveloperId = "dev-001", SkillId = "skill-001", Level = "Advanced" },
            new { DeveloperId = "dev-001", SkillId = "skill-002", Level = "Advanced" },
            new { DeveloperId = "dev-001", SkillId = "skill-003", Level = "Intermediate" },

            new { DeveloperId = "dev-002", SkillId = "skill-001", Level = "Advanced" },
            new { DeveloperId = "dev-002", SkillId = "skill-005", Level = "Advanced" },

            new { DeveloperId = "dev-003", SkillId = "skill-002", Level = "Advanced" },
            new { DeveloperId = "dev-003", SkillId = "skill-006", Level = "Intermediate" },

            new { DeveloperId = "dev-004", SkillId = "skill-005", Level = "Advanced" },

            new { DeveloperId = "dev-005", SkillId = "skill-001", Level = "Expert" },
            new { DeveloperId = "dev-005", SkillId = "skill-003", Level = "Advanced" },

            new { DeveloperId = "dev-006", SkillId = "skill-004", Level = "Expert" },
            new { DeveloperId = "dev-006", SkillId = "skill-006", Level = "Advanced" },

            new { DeveloperId = "dev-007", SkillId = "skill-002", Level = "Advanced" },
            new { DeveloperId = "dev-007", SkillId = "skill-008", Level = "Advanced" },

            new { DeveloperId = "dev-008", SkillId = "skill-001", Level = "Advanced" },
            new { DeveloperId = "dev-008", SkillId = "skill-005", Level = "Advanced" },

            new { DeveloperId = "dev-009", SkillId = "skill-001", Level = "Expert" },
            new { DeveloperId = "dev-009", SkillId = "skill-007", Level = "Intermediate" },

            new { DeveloperId = "dev-010", SkillId = "skill-008", Level = "Advanced" }
        };

        await session.RunAsync(query, new { relationships });
    }

    private static async Task CreateDeveloperProjectsAsync(IAsyncSession session)
    {
        const string query = """
    UNWIND $relationships AS relationship
    MATCH (d:Developer {id: relationship.DeveloperId})
    MATCH (p:Project {id: relationship.ProjectId})
    MERGE (d)-[r:WORKED_ON]->(p)
    SET r.role = relationship.Role
    """;

        var relationships = new[]
        {
            new { DeveloperId = "dev-001", ProjectId = "project-001", Role = "Backend Developer" },
            new { DeveloperId = "dev-001", ProjectId = "project-008", Role = "Automation Developer" },

            new { DeveloperId = "dev-002", ProjectId = "project-002", Role = "Full Stack Developer" },
            new { DeveloperId = "dev-002", ProjectId = "project-006", Role = "Full Stack Developer" },

            new { DeveloperId = "dev-003", ProjectId = "project-003", Role = "Backend Developer" },
            new { DeveloperId = "dev-003", ProjectId = "project-007", Role = "Software Engineer" },

            new { DeveloperId = "dev-004", ProjectId = "project-002", Role = "Frontend Developer" },

            new { DeveloperId = "dev-005", ProjectId = "project-001", Role = "Technical Lead" },
            new { DeveloperId = "dev-005", ProjectId = "project-004", Role = "Backend Developer" },

            new { DeveloperId = "dev-006", ProjectId = "project-005", Role = "Cloud Engineer" },

            new { DeveloperId = "dev-007", ProjectId = "project-008", Role = "QA Automation Engineer" },

            new { DeveloperId = "dev-008", ProjectId = "project-005", Role = "Full Stack Developer" },
            new { DeveloperId = "dev-008", ProjectId = "project-006", Role = "Full Stack Developer" },

            new { DeveloperId = "dev-009", ProjectId = "project-004", Role = "Technical Lead" },
            new { DeveloperId = "dev-009", ProjectId = "project-007", Role = "Backend Developer" },

            new { DeveloperId = "dev-010", ProjectId = "project-008", Role = "Automation Engineer" }
        };

        await session.RunAsync(query, new { relationships });
    }

    private static async Task CreateDeveloperCompaniesAsync(IAsyncSession session)
    {
        const string query = """
    UNWIND $relationships AS relationship
    MATCH (d:Developer {id: relationship.DeveloperId})
    MATCH (c:Company {id: relationship.CompanyId})
    MERGE (d)-[r:WORKED_AT]->(c)
    SET r.fromYear = relationship.FromYear,
        r.toYear = relationship.ToYear
    """;

        var relationships = new[]
        {
            new { DeveloperId = "dev-001", CompanyId = "company-001", FromYear = 2022, ToYear = 2025 },
            new { DeveloperId = "dev-002", CompanyId = "company-002", FromYear = 2021, ToYear = 2026 },
            new { DeveloperId = "dev-003", CompanyId = "company-003", FromYear = 2023, ToYear = 2026 },
            new { DeveloperId = "dev-004", CompanyId = "company-001", FromYear = 2024, ToYear = 2026 },
            new { DeveloperId = "dev-005", CompanyId = "company-004", FromYear = 2019, ToYear = 2026 },
            new { DeveloperId = "dev-006", CompanyId = "company-002", FromYear = 2021, ToYear = 2026 },
            new { DeveloperId = "dev-007", CompanyId = "company-005", FromYear = 2022, ToYear = 2026 },
            new { DeveloperId = "dev-008", CompanyId = "company-003", FromYear = 2020, ToYear = 2026 },
            new { DeveloperId = "dev-009", CompanyId = "company-004", FromYear = 2018, ToYear = 2026 },
            new { DeveloperId = "dev-010", CompanyId = "company-005", FromYear = 2023, ToYear = 2026 }
        };

        await session.RunAsync(query, new { relationships });
    }

    private static async Task CreateProjectTechnologiesAsync(IAsyncSession session)
    {
        const string query = """
    UNWIND $relationships AS relationship
    MATCH (p:Project {id: relationship.ProjectId})
    MATCH (t:Technology {id: relationship.TechnologyId})
    MERGE (p)-[r:USES]->(t)
    SET r.criticality = relationship.Criticality
    """;

        var relationships = new[]
        {
            new { ProjectId = "project-001", TechnologyId = "tech-001", Criticality = "Core" },
            new { ProjectId = "project-001", TechnologyId = "tech-002", Criticality = "Core" },
            new { ProjectId = "project-001", TechnologyId = "tech-003", Criticality = "Core" },
            new { ProjectId = "project-001", TechnologyId = "tech-005", Criticality = "Core" },
            new { ProjectId = "project-001", TechnologyId = "tech-012", Criticality = "Supporting" },

            new { ProjectId = "project-002", TechnologyId = "tech-001", Criticality = "Core" },
            new { ProjectId = "project-002", TechnologyId = "tech-003", Criticality = "Core" },
            new { ProjectId = "project-002", TechnologyId = "tech-004", Criticality = "Core" },
            new { ProjectId = "project-002", TechnologyId = "tech-005", Criticality = "Core" },

            new { ProjectId = "project-003", TechnologyId = "tech-002", Criticality = "Core" },
            new { ProjectId = "project-003", TechnologyId = "tech-005", Criticality = "Core" },
            new { ProjectId = "project-003", TechnologyId = "tech-008", Criticality = "Supporting" },

            new { ProjectId = "project-004", TechnologyId = "tech-001", Criticality = "Core" },
            new { ProjectId = "project-004", TechnologyId = "tech-003", Criticality = "Core" },
            new { ProjectId = "project-004", TechnologyId = "tech-006", Criticality = "Core" },
            new { ProjectId = "project-004", TechnologyId = "tech-008", Criticality = "Supporting" },

            new { ProjectId = "project-005", TechnologyId = "tech-009", Criticality = "Core" },
            new { ProjectId = "project-005", TechnologyId = "tech-004", Criticality = "Core" },
            new { ProjectId = "project-005", TechnologyId = "tech-006", Criticality = "Core" },

            new { ProjectId = "project-006", TechnologyId = "tech-010", Criticality = "Core" },
            new { ProjectId = "project-006", TechnologyId = "tech-011", Criticality = "Core" },
            new { ProjectId = "project-006", TechnologyId = "tech-005", Criticality = "Core" },

            new { ProjectId = "project-007", TechnologyId = "tech-001", Criticality = "Core" },
            new { ProjectId = "project-007", TechnologyId = "tech-002", Criticality = "Core" },
            new { ProjectId = "project-007", TechnologyId = "tech-006", Criticality = "Core" },

            new { ProjectId = "project-008", TechnologyId = "tech-001", Criticality = "Core" },
            new { ProjectId = "project-008", TechnologyId = "tech-009", Criticality = "Supporting" },
            new { ProjectId = "project-008", TechnologyId = "tech-012", Criticality = "Core" }
        };

        await session.RunAsync(query, new { relationships });
    }

    private static async Task CreateProjectCompaniesAsync(IAsyncSession session)
    {
        const string query = """
    UNWIND $relationships AS relationship
    MATCH (p:Project {id: relationship.ProjectId})
    MATCH (c:Company {id: relationship.CompanyId})
    MERGE (p)-[:BELONGS_TO]->(c)
    """;

        var relationships = new[]
        {
            new { ProjectId = "project-001", CompanyId = "company-001" },
            new { ProjectId = "project-002", CompanyId = "company-002" },
            new { ProjectId = "project-003", CompanyId = "company-003" },
            new { ProjectId = "project-004", CompanyId = "company-004" },
            new { ProjectId = "project-005", CompanyId = "company-003" },
            new { ProjectId = "project-006", CompanyId = "company-005" },
            new { ProjectId = "project-007", CompanyId = "company-004" },
            new { ProjectId = "project-008", CompanyId = "company-005" }
        };

        await session.RunAsync(query, new { relationships });
    }

    private static async Task CreateTechnologyRelationshipsAsync(IAsyncSession session)
    {
        const string query = """
    UNWIND $relationships AS relationship
    MATCH (source:Technology {id: relationship.SourceId})
    MATCH (target:Technology {id: relationship.TargetId})
    MERGE (source)-[:RELATED_TO]->(target)
    """;

        var relationships = new[]
        {
            new { SourceId = "tech-001", TargetId = "tech-002" },
            new { SourceId = "tech-002", TargetId = "tech-003" },
            new { SourceId = "tech-002", TargetId = "tech-005" },
            new { SourceId = "tech-003", TargetId = "tech-004" },
            new { SourceId = "tech-005", TargetId = "tech-008" },
            new { SourceId = "tech-006", TargetId = "tech-007" },
            new { SourceId = "tech-009", TargetId = "tech-004" },
            new { SourceId = "tech-010", TargetId = "tech-011" }
        };

        await session.RunAsync(query, new { relationships });
    }
}