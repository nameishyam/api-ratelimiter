import { useEffect, useState } from "react";
import {
  createClient,
  getClients,
  setRateLimit,
  callTest,
  type Client,
} from "./index";

function App() {
  const [name, setName] = useState<string>("");
  const [clients, setClients] = useState<Client[]>([]);
  const [selectedClient, setSelectedClient] = useState<string>("");
  const [limit, setLimit] = useState<number>(10);
  const [response, setResponse] = useState<string>("");

  async function loadClients() {
    const data = await getClients();
    setClients(data);
  }

  useEffect(() => {
    loadClients();
  }, []);

  async function handleCreateClient() {
    if (!name) return;

    await createClient(name);
    setName("");
    loadClients();
  }

  async function handleSetLimit() {
    if (!selectedClient) return;

    await setRateLimit(selectedClient, limit);
  }

  async function handleTest(apiKey: string) {
    const res = await callTest(apiKey);
    setResponse(res);
  }

  return (
    <div className="container">
      <h1>Distributed Rate Limiter Dashboard</h1>

      <section>
        <h2>Create Client</h2>

        <input
          type="text"
          placeholder="Client Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />

        <button onClick={handleCreateClient}>Create</button>
      </section>

      <section>
        <h2>Configure Rate Limit</h2>

        <select
          value={selectedClient}
          onChange={(e) => setSelectedClient(e.target.value)}
        >
          <option value="">Select Client</option>

          {clients.map((c) => (
            <option key={c.id} value={c.id}>
              {c.name}
            </option>
          ))}
        </select>

        <input
          type="number"
          value={limit}
          onChange={(e) => setLimit(parseInt(e.target.value))}
        />

        <button onClick={handleSetLimit}>Set Limit</button>
      </section>

      <section>
        <h2>Test Requests</h2>

        {clients.map((c) => (
          <div className="client-card" key={c.id}>
            <div>
              <b>{c.name}</b>
            </div>

            <div className="apikey">{c.apiKey}</div>

            <button onClick={() => handleTest(c.apiKey)}>Send Request</button>
          </div>
        ))}

        <div className="response">{response}</div>
      </section>
    </div>
  );
}

export default App;
