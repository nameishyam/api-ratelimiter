import { useEffect, useState } from "react";
import * as api from "./";

function App() {
  const [name, setName] = useState("");
  const [clients, setClients] = useState<api.Client[]>([]);
  const [selectedClient, setSelectedClient] = useState("");
  const [limit, setLimit] = useState(10);
  const [logs, setLogs] = useState<
    { status: number; text: string; time: string }[]
  >([]);

  async function loadClients() {
    try {
      const data = await api.getClients();
      setClients(data);
    } catch (err) {
      console.error("Failed to load clients", err);
    }
  }

  useEffect(() => {
    loadClients();
  }, []);

  async function handleCreateClient() {
    if (!name) return;
    await api.createClient(name);
    setName("");
    loadClients();
  }

  async function handleSetLimit() {
    if (!selectedClient) return;
    await api.setRateLimit(selectedClient, limit);
    alert("Rate limit updated successfully!");
  }

  async function handleTest(apiKey: string) {
    const res = await api.callTest(apiKey);
    setLogs((prev) =>
      [{ ...res, time: new Date().toLocaleTimeString() }, ...prev].slice(0, 5),
    );
  }

  return (
    <div className="dashboard-wrapper">
      <header className="main-header">
        <div className="brand">
          <div className="logo-icon">⚡</div>
          <h1>
            Limitless<span>Dash</span>
          </h1>
        </div>
        <div className="status-indicator">System Online</div>
      </header>

      <main className="dashboard-grid">
        {/* LEFT COLUMN: Management */}
        <div className="column management">
          <section className="card">
            <h3>Register New Client</h3>
            <div className="input-group">
              <input
                type="text"
                placeholder="e.g. Mobile App"
                value={name}
                onChange={(e) => setName(e.target.value)}
              />
              <button className="btn-primary" onClick={handleCreateClient}>
                Create
              </button>
            </div>
          </section>

          <section className="card">
            <h3>Throttling Configuration</h3>
            <div className="form-stack">
              <label>Target Client</label>
              <select
                value={selectedClient}
                onChange={(e) => setSelectedClient(e.target.value)}
              >
                <option value="">Select a client...</option>
                {clients.map((c) => (
                  <option key={c.id} value={c.id}>
                    {c.name}
                  </option>
                ))}
              </select>

              <label>Requests Per Minute</label>
              <input
                type="number"
                value={limit}
                onChange={(e) => setLimit(parseInt(e.target.value))}
              />
              <button className="btn-secondary" onClick={handleSetLimit}>
                Update Policy
              </button>
            </div>
          </section>
        </div>

        {/* RIGHT COLUMN: Monitoring */}
        <div className="column monitoring">
          <section className="card">
            <h3>Active Clients</h3>
            <div className="client-grid">
              {clients.map((c) => (
                <div className="client-card" key={c.id}>
                  <div className="client-info">
                    <strong>{c.name}</strong>
                    <code>{c.apiKey}</code>
                  </div>
                  <button
                    className="btn-outline"
                    onClick={() => handleTest(c.apiKey)}
                  >
                    Fire Request
                  </button>
                </div>
              ))}
            </div>
          </section>

          <section className="card log-section">
            <h3>Request Logs</h3>
            <div className="log-container">
              {logs.length === 0 && (
                <p className="empty-state">No requests sent yet.</p>
              )}
              {logs.map((log, i) => (
                <div
                  key={i}
                  className={`log-entry ${log.status === 429 ? "error" : "success"}`}
                >
                  <span className="log-time">{log.time}</span>
                  <span className="log-status">{log.status}</span>
                  <span className="log-msg">{log.text}</span>
                </div>
              ))}
            </div>
          </section>
        </div>
      </main>
    </div>
  );
}

export default App;
