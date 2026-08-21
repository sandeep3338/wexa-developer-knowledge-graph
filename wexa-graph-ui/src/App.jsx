import { useEffect, useState } from "react";
import "./App.css";

const API_BASE_URL = "https://localhost:7046/api";

function App() {
  const [developers, setDevelopers] = useState([]);
  const [selectedDeveloper, setSelectedDeveloper] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [projects, setProjects] = useState([]);
const [selectedProject, setSelectedProject] = useState(null);
const [recommendations, setRecommendations] = useState([]);
const [recommendationsLoading, setRecommendationsLoading] = useState(false);
const [developerGraph, setDeveloperGraph] = useState(null);
const [graphLoading, setGraphLoading] = useState(false);


  useEffect(() => {
    loadDevelopers();
  }, []);

  async function loadDevelopers() {
    try {
      setLoading(true);
      setError("");

      const response = await fetch(
        `${API_BASE_URL}/developers`
      );

      if (!response.ok) {
        throw new Error("Unable to load developers.");
      }

      const data = await response.json();

      setDevelopers(data);
    } catch (err) {
      setError(
        "Unable to connect to the Wexa Graph API."
      );
    } finally {
      setLoading(false);
    }
  }
 useEffect(() => {
  loadProjects();
}, []);

async function loadProjects() {
  try {
    const response = await fetch(
      `${API_BASE_URL}/projects`
    );

    if (!response.ok) {
      throw new Error("Unable to load projects.");
    }

    const data = await response.json();
    setProjects(data);
  } catch {
    setError("Unable to load projects.");
  }
}
async function loadDeveloperGraph(developerId) {
  try {
    setGraphLoading(true);

    const response = await fetch(
      `${API_BASE_URL}/developers/${developerId}/graph`
    );

    if (!response.ok) {
      throw new Error("Unable to load graph.");
    }

    const data = await response.json();

    setDeveloperGraph(data);
  } catch {
    setError("Unable to load developer graph.");
  } finally {
    setGraphLoading(false);
  }
}

async function loadRecommendations(projectId) {
  try {
    setRecommendationsLoading(true);
    setRecommendations([]);
    setError("");

    const response = await fetch(
      `${API_BASE_URL}/developers/recommendations?projectId=${encodeURIComponent(projectId)}`
    );

    if (!response.ok) {
      throw new Error("Unable to load recommendations.");
    }

    const data = await response.json();

    setRecommendations(data);
  } catch {
    setError("Unable to load developer recommendations.");
  } finally {
    setRecommendationsLoading(false);
  }
}
 async function loadDeveloper(developerId) {
  try {
    setError("");
    setDeveloperGraph(null);
    setGraphLoading(true);

    const response = await fetch(
      `${API_BASE_URL}/developers/${developerId}`
    );

    if (!response.ok) {
      throw new Error("Developer not found.");
    }

    const data = await response.json();

    setSelectedDeveloper(data);

    await loadDeveloperGraph(developerId);
  } catch {
    setError("Unable to load developer profile.");
  } finally {
    setGraphLoading(false);
  }
}



  return (
    <div className="app">
      <header className="topbar">
        <div>
          <div className="brand">WEXA</div>
          <div className="subtitle">
            Developer Knowledge Graph
          </div>
        </div>

        <div className="status">
          <span className="status-dot"></span>
          Graph Connected
        </div>
      </header>

      <main className="content">
        <section className="hero">
          <p className="eyebrow">KNOWLEDGE GRAPH</p>

          <h1>
            Discover developers through
            <span> connected experience.</span>
          </h1>

          <p className="hero-text">
            Explore skills, projects, technologies and
            companies connected through CognoDB.
          </p>
        </section>

        {error && (
          <div className="error">
            {error}
          </div>
        )}
          <section className="projects-section">
  <div className="section-header">
    <div>
      <p className="eyebrow">GRAPH EXPLORER</p>
      <h2>Find developers for a project</h2>
      <p>
        Recommendations are based on connected project
        and technology experience.
      </p>
    </div>
  </div>

  <div className="project-selector">
    {projects.map((project) => (
      <button
        key={project.id}
        className={`project-card ${
          selectedProject?.id === project.id
            ? "selected"
            : ""
        }`}
        onClick={() => {
          setSelectedProject(project);
          loadRecommendations(project.id);
        }}
      >
        <strong>{project.name}</strong>
        <span>{project.description}</span>
      </button>
    ))}
  </div>
</section>
<section className="recommendations-section">
  <div className="section-header">
    <div>
      <p className="eyebrow">GRAPH MATCH</p>

      <h2>
        {selectedProject
          ? `Recommended developers for ${selectedProject.name}`
          : "Developer recommendations"}
      </h2>
    </div>
  </div>

  {!selectedProject ? (
    <div className="profile-empty small">
      Select a project to discover connected developers.
    </div>
  ) : recommendationsLoading ? (
    <div className="loading">
      Exploring the graph...
    </div>
  ) : recommendations.length === 0 ? (
    <div className="empty">
      No matching developers found.
    </div>
  ) : (
    <div className="recommendation-grid">
      {recommendations.map((developer) => (
        <div
          className="recommendation-card"
          key={developer.developerId}
        >
          <div className="recommendation-header">
            <div className="avatar">
              {developer.developerName
                .split(" ")
                .map((name) => name[0])
                .join("")
                .slice(0, 2)}
            </div>

            <div>
              <strong>
                {developer.developerName}
              </strong>

              <span>
                {developer.role}
              </span>

              <small>
                {developer.experienceYears} years experience
              </small>
            </div>
          </div>

          <div className="matching">
            <span className="matching-label">
              Matching technologies
            </span>

            <div className="tags">
              {developer.matchingTechnologies.map(
                (technology) => (
                  <span
                    className="tag"
                    key={technology}
                  >
                    {technology}
                  </span>
                )
              )}
            </div>
          </div>
        </div>
      ))}
    </div>
  )}
</section>
        <section className="workspace">
          <div className="developer-panel">
            <div className="section-header">
              <div>
                <h2>Developers</h2>
                <p>
                  {developers.length} people in the graph
                </p>
              </div>
            </div>

            {loading ? (
              <div className="loading">
                Loading developers...
              </div>
            ) : developers.length === 0 ? (
              <div className="empty">
                No developers found.
              </div>
            ) : (
              <div className="developer-list">
                {developers.map((developer) => (
                  <button
                    key={developer.id}
                    className={`developer-card ${
                      selectedDeveloper?.id === developer.id
                        ? "selected"
                        : ""
                    }`}
                    onClick={() =>
                      loadDeveloper(developer.id)
                    }
                  >
                    <div className="avatar">
                      {developer.name
                        .split(" ")
                        .map((name) => name[0])
                        .join("")
                        .slice(0, 2)}
                    </div>

                    <div className="developer-info">
                      <strong>{developer.name}</strong>

                      <span>
                        {developer.role}
                      </span>

                      <small>
                        {developer.experienceYears} years
                        experience
                      </small>
                    </div>

                    <div className="arrow">→</div>
                  </button>
                ))}
              </div>
            )}
          </div>

          <div className="profile-panel">
            {!selectedDeveloper ? (
              <div className="profile-empty">
                <div className="graph-icon">◎</div>

                <h2>Select a developer</h2>

                <p>
                  Choose someone from the list to explore
                  their connected graph.
                </p>
              </div>
            ) : (
              <DeveloperProfile
  developer={selectedDeveloper}
  developerGraph={developerGraph}
  graphLoading={graphLoading}
/>
            )}
          </div>
        </section>
      </main>
    </div>
  );
}

function DeveloperProfile({
  developer,
  developerGraph,
  graphLoading
}) {
  return (
    <div className="profile">
      <div className="profile-header">
        <div className="large-avatar">
          {developer.name
            .split(" ")
            .map((name) => name[0])
            .join("")
            .slice(0, 2)}
        </div>

        <div>
          <p className="eyebrow">DEVELOPER</p>

          <h2>{developer.name}</h2>

          <p className="profile-role">
            {developer.role} ·{" "}
            {developer.experienceYears} years
          </p>
        </div>
      </div>

      <GraphSection
        title="Skills"
        items={developer.skills}
        type="tags"
      />

      <GraphSection
        title="Projects"
        items={developer.projects}
      />

      <GraphSection
        title="Technologies"
        items={developer.technologies}
        type="tags"
      />

      <GraphSection
        title="Companies"
        items={developer.companies}
      />

      <GraphExplorer
  graph={developerGraph}
  loading={graphLoading}
/>
    </div>
  );
}

function GraphSection({ title, items, type }) {
  return (
    <div className="graph-section">
      <h3>{title}</h3>

      {!items || items.length === 0 ? (
        <p className="muted">No connections found.</p>
      ) : type === "tags" ? (
        <div className="tags">
          {items.map((item) => (
            <span className="tag" key={item}>
              {item}
            </span>
          ))}
        </div>
      ) : (
        <div className="connections">
          {items.map((item) => (
            <div className="connection" key={item}>
              <span className="connection-dot"></span>
              {item}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
function GraphExplorer({ graph, loading }) {
  if (loading) {
    return (
      <div className="graph-loading">
        Exploring connections...
      </div>
    );
  }

  if (!graph) {
    return null;
  }

  const developer =
    graph.nodes.find(
      (node) => node.type === "Developer"
    );

  const connectedNodes =
    graph.nodes.filter(
      (node) => node.type !== "Developer"
    );

  return (
    <div className="graph-explorer">
      <div className="graph-title">
        <div>
          <p className="eyebrow">RELATIONSHIP MAP</p>
          <h3>Connected knowledge</h3>
        </div>

        <span>
          {connectedNodes.length} connections
        </span>
      </div>

      <div className="graph-center">
        <div className="graph-developer">
          <div className="large-avatar">
            {developer?.label
              .split(" ")
              .map((name) => name[0])
              .join("")
              .slice(0, 2)}
          </div>

          <strong>{developer?.label}</strong>
          <small>Developer</small>
        </div>
      </div>

      <div className="graph-connections">
        {connectedNodes.map((node) => {
          const relationship =
            graph.relationships.find(
              (rel) =>
                rel.source === developer?.id &&
                rel.target === node.id
            );

          return (
            <div
              className="graph-connection"
              key={node.id}
            >
              <div className="graph-line"></div>

              <div className={`graph-node ${node.type.toLowerCase()}`}>
                <span className="node-type">
                  {node.type}
                </span>

                <strong>{node.label}</strong>

                {relationship && (
                  <small>
                    {relationship.type}
                  </small>
                )}
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
}

export default App;